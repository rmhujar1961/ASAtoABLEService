using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASAtoABLEService
{
    class AsaCustomerRecord
    {
        public string CustomerNumber;
        //'A' = New Customer
        //'C' = Changed Customer
        //'D' = Deleted Customer
        public string StatusFlag;
        public string CustomerName;
        public string CustomerAddress1;
        public string CustomerAddress2;
        public string CustomerCity;
        public string CustomerState;
        public string CustomerZipCode;
        public string CustomerPhoneNumber;        //(9999999999)
        public string CustomerContactName;
        public string BinderID;
        public string UpchargeFlag ;               //  (Y/N)


        public string GetAddress()
        {
            StringBuilder customerAddress = new StringBuilder();

            //Build the Customer Address
            customerAddress.Append(CustomerAddress1);
            customerAddress.Append("\n");
            //Check for ancillary address
            if (CustomerAddress2.Length > 0)
            {
                customerAddress.Append(CustomerAddress2);
                customerAddress.Append("\n");
            }

            customerAddress.Append(CustomerCity);
            customerAddress.Append(" ");
            customerAddress.Append(CustomerState);
            customerAddress.Append(" ");
            customerAddress.Append(CustomerZipCode);

            return customerAddress.ToString();
        }

        public string GetPhoneNumber()
        {

            CustomerPhoneNumber = String.Format("{0:(###)###-####}", CustomerPhoneNumber);
            return CustomerPhoneNumber;
        }
    }
}
