using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASAtoABLEService
{
    class AsaInvoiceRecord
    {                 
        public string CustomerAccountNumber; 
        public string StatusFlag;           //(Not used)
        public string AbleLotID;
        public string OEInvoiceNo;              
        public string InvoiceAmount;        //99999999 (6.2)

        public string JobNumber;
        public string CustomerType;
        public string TotalOrderQty; 
        public string TaxAmount;
        public string FreightAmount;

    }
}
