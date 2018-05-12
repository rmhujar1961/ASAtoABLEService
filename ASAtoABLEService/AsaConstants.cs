using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASAtoABLEService
{
    public static class AsaConstants
    {
        public const string JOB_RECORD_TYPE = "JOB";
        public const string CUSTOMER_RECORD_TYPE = "CUS";
        public const string INVOICE_RECORD_TYPE = "INV";

        public static int RECORD_TYPE_LENGTH = 3;

        //JOB FIELD LENGTHS
        public const int ABLE_JOB_NUMBER_LENGTH = 6;             
        public const int WORK_ORDER_NUMBER_LENGTH = 6;
        public const int CUSTOMER_NUMBER_LENGTH = 5;
 	    public const int ABLE_DATE_EXPECTED = 6;
 	    public const int ABLE_TRUCK_ROUTE_LENGTH = 5;
        public const int ABLE_SHIPPING_DAY_LENGTH = 2;
        public const int ABLE_LOT_ID_LENGTH = 8;
        public const int JOB_RECORD_FILLER = 109;

        //CUSTOMER FIELD LENGTHS 
        public const int STATUS_FLAG_LENGTH = 1;
        public const int CUSTOMER_NAME_LENGTH = 25;
        public const int CUSTOMER_ADDRESS1_LENGTH = 25;
        public const int CUSTOMER_ADDRESS2_LENGTH = 25;
        public const int CUSTOMER_CITY_LENGTH = 15;
        public const int CUSTOMER_STATE_LENGTH = 2;
        public const int CUSTOMER_ZIP_CODE_LENGTH = 9;
        public const int CUSTOMER_PHONE_NUMBER_LENGTH = 10;
        public const int CUSTOMER_CONTACT_NAME_LENGTH = 15;
        public const int BINDER_ID_LENGTH = 6;
        public const int UPCHARGE_FLAG_LENGTH = 1; 
        public const int CUSTOMER_RECORD_FILLER = 8;

        //INVOICE FIELD LENGTHS
        public const int CUSTOMER_ACCOUNT_NUMBER_LENGTH = 5;                   
        public const int INVOICE_NUMBER_LENGTH = 6;              
        public const int INVOICE_AMOUNT_LENGTH = 8;         
        public const int JOB_NUMBER_LENGTH = 6;
        public const int CUSTOMER_TYPE_LENGTH = 1;
        public const int TOTAL_ORDER_QTY_LENGTH = 7;
        public const int TAX_AMOUNT_LENGTH = 7;
        public const int FREIGHT_AMOUNT_LENGTH = 7;
        public const int INVOICE_Filler1_LENGTH = 91;
        public const int INVOICE_Filler2_LENGTH = 105;
        public const int INVOICE_Filler3_LENGTH = 113;

    }
}
