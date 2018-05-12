using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using log4net;

namespace ASAtoABLEService
{
    class ABLEController
    {
        static ILog logger;

        public static void SetLogger(ILog log)
        {
            logger = log;
        }
        /// <summary>
        /// ABLE Job Table Insert
        /// </summary>
        /// <param name="job"></param>
        /// <param name="expected_return_date"></param>
        public static void ABLEJobTableInsert(string account, string job, string lot, string expected_return_date, string truck_route, string shipping_day, string WorkOrderNumber)
        {
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            logger.Info("ABLEJobTableInsert Sql Connection String: " + sqlConnectionString);
                  
            SqlCommand sqlCommand = new SqlCommand();

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                logger.Info("sp_AsaJobTableInsert parameters " + "Account " + account + " job " + job + " Expected return date " + expected_return_date +
                                                       " lot " + lot + " truck route " + truck_route + " shipping day " + shipping_day + " Work Order Number " + WorkOrderNumber);
                try
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_AsaJobTableInsert";
                    sqlCommand.Parameters.Add("@account", SqlDbType.VarChar).Value = account;
                    sqlCommand.Parameters.Add("@job", SqlDbType.VarChar).Value = job;
                    sqlCommand.Parameters.Add("@lot", SqlDbType.VarChar).Value = lot;
                    sqlCommand.Parameters.Add("@truck_route", SqlDbType.VarChar).Value = truck_route;
                    sqlCommand.Parameters.Add("@shipping_day", SqlDbType.VarChar).Value = shipping_day;
                    sqlCommand.Parameters.Add("@expected_return_date", SqlDbType.VarChar).Value = expected_return_date;
                    sqlCommand.Parameters.Add("@work_order_number", SqlDbType.VarChar).Value = WorkOrderNumber;

                    sqlCommand.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    logger.Error("Exception in sp_AsaJobTableInsert: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="library"></param>
        /// <param name="library_address"></param>
        /// <param name="contact"></param>
        /// <param name="phone"></param>
        /// <param name="binder"></param>
        public static void ABLEAccountTableInsert(string account, string library, string library_address, string contact, string phone, string binder)
        {
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            logger.Info("ABLEAccountTableInsert Sql Connection String: " + sqlConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                logger.Info("sp_AsaAccountTableInsert parameters " + "Account " + account + " library " + library + " library address " + library_address);
                try
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_AsaAccountTableInsert";
                    sqlCommand.Parameters.Add("@account", SqlDbType.VarChar).Value = account;
                    sqlCommand.Parameters.Add("@library", SqlDbType.VarChar).Value = library;
                    sqlCommand.Parameters.Add("@library_address", SqlDbType.VarChar).Value = library_address;
                    sqlCommand.Parameters.Add("@contact", SqlDbType.VarChar).Value = contact;
                    sqlCommand.Parameters.Add("@phone", SqlDbType.VarChar).Value = phone;
                    sqlCommand.Parameters.Add("@binder", SqlDbType.VarChar).Value = binder;

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.Error("Exception in sp_AsaAccountTableInsert: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="library"></param>
        /// <param name="library_address"></param>
        /// <param name="contact"></param>
        /// <param name="phone"></param>
        /// <param name="binder"></param>
        public static void ABLEAccountTableUpdate(string account, string library, string library_address, string contact, string phone, string binder)
        {
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            logger.Info("ABLEAccountTableUpdate Sql Connection String: " + sqlConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                logger.Info("sp_AsaAccountTableUpdate parameters " + "Account " + account + " library " + library + " library address " + library_address);
                try
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_AsaAccountTableUpdate";
                    sqlCommand.Parameters.Add("@account", SqlDbType.VarChar).Value = account;
                    sqlCommand.Parameters.Add("@library", SqlDbType.VarChar).Value = library;
                    sqlCommand.Parameters.Add("@library_address", SqlDbType.VarChar).Value = library_address;
                    sqlCommand.Parameters.Add("@contact", SqlDbType.VarChar).Value = contact;
                    sqlCommand.Parameters.Add("@phone", SqlDbType.VarChar).Value = phone;
                    sqlCommand.Parameters.Add("@binder", SqlDbType.VarChar).Value = binder;

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.Error("Exception in sp_AsaAccountTableUpdate: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        public static void ABLEAccountTableDelete(string account)
        {
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            logger.Info("ABLEAccountTableDelete Sql Connection String: " + sqlConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                logger.Info("sp_AsaAccountTableDelete parameters " + "Account " + account);
                try
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_AsaAccountTableDelete";
                    sqlCommand.Parameters.Add("@account", SqlDbType.VarChar).Value = account;

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.Error("Exception in sp_AsaAccountTableDelete: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="lot"></param>
        /// <param name="invoice"></param>
        /// <param name="total_cost"></param>
        public static void ABLEBillingTableInsert(string account, string lot, string invoice, string invoice_amount, string department)
        {
            int total_cost = 0;
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            logger.Info("ABLEBillingTableInsert Sql Connection String: " + sqlConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                logger.Info("sp_AsaBillingTableInsert parameters - " + "Account:" + account + " lot:" + lot + " invoice:" + invoice +
                                                                    " Invoice amount:" + invoice_amount + " Department:" + department);
                try
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_AsaBillingTableInsert";
                    sqlCommand.Parameters.Add("@account", SqlDbType.VarChar).Value = account;
                    sqlCommand.Parameters.Add("@lot", SqlDbType.VarChar).Value = lot;
                    sqlCommand.Parameters.Add("@invoice", SqlDbType.VarChar).Value = invoice;
                    sqlCommand.Parameters.Add("@department", SqlDbType.VarChar).Value = department;

                    if (Int32.TryParse(invoice_amount, out total_cost) == false)
                        total_cost = 0;
                        
                    sqlCommand.Parameters.Add("@total_cost", SqlDbType.Int).Value = total_cost;

                    sqlCommand.Parameters.Add("@manual_adjustment", SqlDbType.Int).Value = 0;

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.Error("Exception in sp_AsaBillingTableInsert: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        public static string ABLEGetDepartmentForAccount(string account)
        {
            string department = "";

            string sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            logger.Info("ABLEGetDepartmentForAccount Sql Connection String: " + sqlConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                logger.Info("sp_AsaAccountTableDelete parameters " + "Account " + account);
                try
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_GetAsaBillingDepartment";
                    sqlCommand.Parameters.Add("@account", SqlDbType.VarChar).Value = account;

                    sqlCommand.Parameters.Add("@department", SqlDbType.VarChar).Direction = ParameterDirection.ReturnValue;
                     
                    sqlCommand.ExecuteNonQuery();

                    department =  sqlCommand.Parameters["@department"].Value.ToString();
                }
                catch (Exception ex)
                {
                    logger.Error("Exception in sp_GetAsaBillingDepartment: " + ex.Message);
                }
            }

            return department;
        }
    }
}
