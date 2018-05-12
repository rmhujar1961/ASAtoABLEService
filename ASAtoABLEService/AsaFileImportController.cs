using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;

namespace ASAtoABLEService
{
    public class AsaFileImportController
    {
          
        ILog logger;
        private string ArchivePath;

        public AsaFileImportController(ILog log )
        {
            logger = log;
            ArchivePath = ConfigurationManager.AppSettings["ArchivePath"];
            logger.Info("Archive Path is " + ArchivePath);
            ABLEController.SetLogger(log);
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void ProcessAsaFile(string filePath)
        {
            string  recordType;
            int recordCount = 0;
            List<AsaJobRecord> AsaJobRecordList = new List<AsaJobRecord>();
            List<AsaCustomerRecord> AsaCustomerRecordList = new List<AsaCustomerRecord>();
            List<AsaInvoiceRecord> AsaInvoiceRecordList = new List<AsaInvoiceRecord>();

            logger.Info("File path for Asa File is " + filePath);

            logger.Info("TEST - Wait 10 seconds before processing file");

            Thread.Sleep(10000);

            try
            {
                using (StreamReader streamReader = File.OpenText(filePath))
                {
                    string flatFileLine = String.Empty;
                    while ((flatFileLine = streamReader.ReadLine()) != null)
                    {
                        recordType = flatFileLine.Substring(0, AsaConstants.RECORD_TYPE_LENGTH);

                        switch (recordType)
                        {
                            case AsaConstants.JOB_RECORD_TYPE:
                                AsaJobRecord asaJobRecord = ProcessJobRecord(flatFileLine);
                                logger.Info("Job Record for Job: " + asaJobRecord.Job);
                                AsaJobRecordList.Add(asaJobRecord);
                                break;

                            case AsaConstants.CUSTOMER_RECORD_TYPE:
                                AsaCustomerRecord asaCustomerRecord = ProcessCustomerRecord(flatFileLine);
                                logger.Info("Customer Record for account: " + asaCustomerRecord.CustomerNumber);
                                AsaCustomerRecordList.Add(asaCustomerRecord);
                                break;

                            case AsaConstants.INVOICE_RECORD_TYPE:
                                AsaInvoiceRecord asaInvoiceRecord = ProcessInvoiceRecord(flatFileLine);
                                logger.Info("Invoice Record for invoice: " + asaInvoiceRecord.OEInvoiceNo);
                                AsaInvoiceRecordList.Add(asaInvoiceRecord);
                                break;

                            default:
                                logger.Info("Encountered Default case - Record type not defined or invalid");
                                break;
                        }
                        recordCount++;
                    }

                    logger.Info("Job record count: " + AsaJobRecordList.Count.ToString());
                    logger.Info("Customer record count: " + AsaCustomerRecordList.Count.ToString());
                    logger.Info("Invoice record count: " + AsaInvoiceRecordList.Count.ToString());
                    logger.Info("Total records processed: " + recordCount.ToString());
                    //Check for job records to process
                    if (AsaJobRecordList.Count > 0)
                    {
                        foreach (AsaJobRecord jobRecord in AsaJobRecordList)
                        {
                            logger.Info("Insert job record -> " + jobRecord.Job);
                            ABLEController.ABLEJobTableInsert(jobRecord.CustomerNumber, jobRecord.Job, jobRecord.AbleLotId,
                                                              jobRecord.AbleDateExpected, jobRecord.AbleTruckRoute, jobRecord.AbleShippingDay, jobRecord.WorkOrderNumber);
                        }
                    }
                    //Check for customer records to process
                    if (AsaCustomerRecordList.Count > 0)
                    {
                        foreach (AsaCustomerRecord customerRecord in AsaCustomerRecordList)
                        {

                            //Insert customer record   
                            if (customerRecord.StatusFlag == "A")
                            {
                                logger.Info("Insert customer record -> " + customerRecord.CustomerNumber);
                                ABLEController.ABLEAccountTableInsert(customerRecord.CustomerNumber, customerRecord.CustomerName,
                                                                        customerRecord.GetAddress(), customerRecord.CustomerContactName,
                                                                        customerRecord.GetPhoneNumber(), customerRecord.BinderID);
                            }
                            //Update customer record
                            else if (customerRecord.StatusFlag == "C")
                            {
                                logger.Info("Update customer record -> " + customerRecord.CustomerNumber);
                                ABLEController.ABLEAccountTableUpdate(customerRecord.CustomerNumber, customerRecord.CustomerName,
                                                                        customerRecord.GetAddress(), customerRecord.CustomerContactName,
                                                                        customerRecord.GetPhoneNumber(), customerRecord.BinderID);
                            }
                            //Delete customer record
                            else if (customerRecord.StatusFlag == "D")
                            {
                                logger.Info("Delete customer record -> " + customerRecord.CustomerNumber);
                                ABLEController.ABLEAccountTableDelete(customerRecord.CustomerNumber);
                            }
                        }

                    }
                    //Check for invoice records to process
                    if (AsaInvoiceRecordList.Count > 0)
                    {
                        foreach (AsaInvoiceRecord invoiceRecord in AsaInvoiceRecordList)
                        {
                            logger.Info("Insert Invoice record -> " + invoiceRecord.OEInvoiceNo);
                            //HAS MULTIPLE departments for each account string department = ABLEController.ABLEGetDepartmentForAccount(invoiceRecord.CustomerAccountNumber);

                            ABLEController.ABLEBillingTableInsert(invoiceRecord.CustomerAccountNumber, invoiceRecord.AbleLotID,
                                                                    invoiceRecord.InvoiceAmount, invoiceRecord.InvoiceAmount, "");
                        }
                    }
                }
                logger.Info("Done Processing Asa File " + filePath);
            }
            catch( Exception ex)
            {
                logger.Info("ProcessAsaFile exception --> " + ex.Message + "Stack Trace " + ex.StackTrace);
            }
             
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flatFileRecord"></param>
        /// <returns></returns>
        private AsaJobRecord ProcessJobRecord(string flatFileRecord)
        {
            int lineIndex = 0;
            int jobId = 0;
            int customerNo = 0;
            int workOrderNo = 0;


            AsaJobRecord asaJobRecord = new AsaJobRecord();

            lineIndex = AsaConstants.RECORD_TYPE_LENGTH;
            asaJobRecord.Job = flatFileRecord.Substring(lineIndex, AsaConstants.JOB_NUMBER_LENGTH).TrimEnd();

            //Remove leading zeros from string by converting to integer 
            if (Int32.TryParse(asaJobRecord.Job, out jobId))
            {
                logger.Debug("ProcessJobRecord: Job Id is " + jobId);
                asaJobRecord.Job = jobId.ToString();
            }
            else
            {
                logger.Debug("ProcessJobRecord: Job Id could not be parsed");
                asaJobRecord.Job = "0";
            }

            lineIndex += AsaConstants.JOB_NUMBER_LENGTH;
            asaJobRecord.WorkOrderNumber = flatFileRecord.Substring(lineIndex, AsaConstants.WORK_ORDER_NUMBER_LENGTH).TrimEnd();

            //Remove leading zeros from string by converting to integer
            if (Int32.TryParse(asaJobRecord.WorkOrderNumber, out workOrderNo))
            {
                logger.Debug("ProcessJobRecord: WorkOrderNumber is " + workOrderNo);
                asaJobRecord.WorkOrderNumber = workOrderNo.ToString();
            }
            else
            {
                logger.Debug("ProcessJobRecord: WorkOrderNumber could not be parsed");
                asaJobRecord.WorkOrderNumber = "0";
            }

            lineIndex += AsaConstants.WORK_ORDER_NUMBER_LENGTH;
            asaJobRecord.CustomerNumber = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_NUMBER_LENGTH).TrimEnd();

            //Remove leading zeros from string by converting to integer
            if (Int32.TryParse(asaJobRecord.CustomerNumber, out customerNo))
            {
                logger.Debug("ProcessJobRecord: Customer Number is " + customerNo);
                asaJobRecord.CustomerNumber = customerNo.ToString();
            }
            else
            {
                logger.Debug("ProcessJobRecord: Customer Number could not be parsed");
                asaJobRecord.CustomerNumber = "0";
            }

            lineIndex += AsaConstants.CUSTOMER_NUMBER_LENGTH;
            asaJobRecord.AbleDateExpected = flatFileRecord.Substring(lineIndex, AsaConstants.ABLE_DATE_EXPECTED).TrimEnd();
            lineIndex += AsaConstants.ABLE_DATE_EXPECTED;
            asaJobRecord.AbleTruckRoute = flatFileRecord.Substring(lineIndex, AsaConstants.ABLE_TRUCK_ROUTE_LENGTH).TrimEnd();
            lineIndex += AsaConstants.ABLE_TRUCK_ROUTE_LENGTH;
            asaJobRecord.AbleShippingDay = flatFileRecord.Substring(lineIndex, AsaConstants.ABLE_SHIPPING_DAY_LENGTH).TrimEnd();
            lineIndex += AsaConstants.ABLE_SHIPPING_DAY_LENGTH;
            asaJobRecord.AbleLotId = flatFileRecord.Substring(lineIndex, AsaConstants.ABLE_LOT_ID_LENGTH).TrimEnd();

            return asaJobRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flatFileRecord"></param>
        /// <returns></returns>
        private AsaCustomerRecord ProcessCustomerRecord(string flatFileRecord)
        {
            int lineIndex = 0;
            int customerNumber = 0;

            AsaCustomerRecord asaCustomerRecord = new AsaCustomerRecord();

            lineIndex = AsaConstants.RECORD_TYPE_LENGTH;
            asaCustomerRecord.CustomerNumber = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_ACCOUNT_NUMBER_LENGTH).TrimEnd();

            //Remove leading zeros from string by converting to integer 
            if (Int32.TryParse(asaCustomerRecord.CustomerNumber, out customerNumber))
            {
                logger.Debug("ProcessCustomerRecord Customer Number is " + customerNumber);
                asaCustomerRecord.CustomerNumber = customerNumber.ToString();
            }
            else
            {
                logger.Debug("ProcessCustomerRecord Customer Number could not be parsed");
                asaCustomerRecord.CustomerNumber = "0";
            }

            lineIndex += AsaConstants.CUSTOMER_ACCOUNT_NUMBER_LENGTH;
            asaCustomerRecord.StatusFlag = flatFileRecord.Substring(lineIndex, AsaConstants.STATUS_FLAG_LENGTH).TrimEnd();
            lineIndex += AsaConstants.STATUS_FLAG_LENGTH;
            asaCustomerRecord.CustomerName = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_NAME_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_NAME_LENGTH;
            asaCustomerRecord.CustomerAddress1 = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_ADDRESS1_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_ADDRESS1_LENGTH;
            asaCustomerRecord.CustomerAddress2 = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_ADDRESS2_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_ADDRESS2_LENGTH;
            asaCustomerRecord.CustomerCity = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_CITY_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_CITY_LENGTH;
            asaCustomerRecord.CustomerState = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_STATE_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_STATE_LENGTH;
            asaCustomerRecord.CustomerZipCode = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_ZIP_CODE_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_ZIP_CODE_LENGTH;
            asaCustomerRecord.CustomerPhoneNumber = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_PHONE_NUMBER_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_PHONE_NUMBER_LENGTH;
            asaCustomerRecord.CustomerContactName = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_CONTACT_NAME_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_CONTACT_NAME_LENGTH;
            asaCustomerRecord.BinderID = flatFileRecord.Substring(lineIndex, AsaConstants.BINDER_ID_LENGTH).TrimEnd();
            lineIndex += AsaConstants.BINDER_ID_LENGTH;
            asaCustomerRecord.UpchargeFlag = flatFileRecord.Substring(lineIndex, AsaConstants.UPCHARGE_FLAG_LENGTH).TrimEnd();

            return asaCustomerRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flatFileRecord"></param>
        /// <returns></returns>
        private AsaInvoiceRecord ProcessInvoiceRecord(string flatFileRecord)
        {
            int lineIndex = 0;
            int customerAccount = 0;
            int lotId = 0;
            int jobId = 0;

            AsaInvoiceRecord asaInvoiceRecord = new AsaInvoiceRecord();

            lineIndex = AsaConstants.RECORD_TYPE_LENGTH;
            asaInvoiceRecord.CustomerAccountNumber = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_ACCOUNT_NUMBER_LENGTH).TrimEnd();

            //Remove leading zeros from string by converting to integer 
            if (Int32.TryParse(asaInvoiceRecord.CustomerAccountNumber, out customerAccount))
            {
                logger.Debug("ProcessInvoiceRecord Customer Account is " + customerAccount);
                asaInvoiceRecord.CustomerAccountNumber = customerAccount.ToString();
            }
            else
            {
                logger.Debug("ProcessInvoiceRecord Customer Account could not be parsed");
                asaInvoiceRecord.CustomerAccountNumber = "0";
            }

            lineIndex += AsaConstants.CUSTOMER_ACCOUNT_NUMBER_LENGTH;
            asaInvoiceRecord.StatusFlag = flatFileRecord.Substring(lineIndex, AsaConstants.STATUS_FLAG_LENGTH).TrimEnd();
            lineIndex += AsaConstants.STATUS_FLAG_LENGTH;
            asaInvoiceRecord.AbleLotID = flatFileRecord.Substring(lineIndex, AsaConstants.ABLE_LOT_ID_LENGTH).TrimEnd();

            //Remove leading zeros from string by converting to integer
            if (Int32.TryParse(asaInvoiceRecord.AbleLotID, out lotId))
            {
                logger.Debug("ProcessInvoiceRecord: lot id is " + lotId);
                asaInvoiceRecord.AbleLotID = lotId.ToString();
            }
            else
            {
                logger.Debug("ProcessInvoiceRecord: lot id could not be parsed");
                asaInvoiceRecord.AbleLotID = "0";
            }

            lineIndex += AsaConstants.ABLE_LOT_ID_LENGTH;
            asaInvoiceRecord.OEInvoiceNo = flatFileRecord.Substring(lineIndex, AsaConstants.INVOICE_NUMBER_LENGTH).TrimEnd();
            lineIndex += AsaConstants.INVOICE_NUMBER_LENGTH;
            asaInvoiceRecord.InvoiceAmount = flatFileRecord.Substring(lineIndex, AsaConstants.INVOICE_AMOUNT_LENGTH).TrimEnd();
            lineIndex += AsaConstants.INVOICE_AMOUNT_LENGTH;
            asaInvoiceRecord.JobNumber = flatFileRecord.Substring(lineIndex, AsaConstants.JOB_NUMBER_LENGTH).TrimEnd();

            //Remove leading zeros from string by converting to integer
            if (Int32.TryParse(asaInvoiceRecord.JobNumber, out jobId))
            {
                logger.Debug("ProcessInvoiceRecord: Job Id is " + jobId);
                asaInvoiceRecord.JobNumber = jobId.ToString();
            }
            else
            {
                logger.Debug("ProcessInvoiceRecord: Job Id could not be parsed");
                asaInvoiceRecord.JobNumber = "0";
            }

            lineIndex += AsaConstants.JOB_NUMBER_LENGTH;
            asaInvoiceRecord.CustomerType = flatFileRecord.Substring(lineIndex, AsaConstants.CUSTOMER_TYPE_LENGTH).TrimEnd();
            lineIndex += AsaConstants.CUSTOMER_TYPE_LENGTH;
            asaInvoiceRecord.TotalOrderQty = flatFileRecord.Substring(lineIndex, AsaConstants.TOTAL_ORDER_QTY_LENGTH).TrimEnd();
            lineIndex += AsaConstants.TOTAL_ORDER_QTY_LENGTH;
            asaInvoiceRecord.TaxAmount = flatFileRecord.Substring(lineIndex, AsaConstants.TAX_AMOUNT_LENGTH).TrimEnd();
            lineIndex += AsaConstants.TAX_AMOUNT_LENGTH;
            asaInvoiceRecord.FreightAmount = flatFileRecord.Substring(lineIndex, AsaConstants.FREIGHT_AMOUNT_LENGTH).TrimEnd();

            return asaInvoiceRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void ArchiveAsaFile(string filePath)
        {
            int lastIndex = 0;
            string datePrefix = "";
            string fileName;
            string destination = "";

            lastIndex = filePath.LastIndexOf("\\");

            fileName = filePath.Substring(lastIndex + 1);
             
            DateTime currentDate = DateTime.Now;
            datePrefix = currentDate.ToString("MMMddyyyy");

            destination = ArchivePath + datePrefix + "_" + fileName;
            
            try
            {
                // Move the file.
                File.Move(filePath, destination);
            }
            catch (Exception ex)
            {
                 
            }
        }
    }

   
}
