using System;

namespace DataSyncService.Model
{
    public class DWCustomer
    {
        public Int32 cust_id { get; set; }
        public string cust_fname { get; set; }
        public string cust_lname { get; set; }
        public string cust_phone { get; set; }
        public string cust_email { get; set; }
        public string cust_dob { get; set; }
        public string cust_active { get; set; }
        public string cust_fullname { get; set; }
        public string cust_phone_prefix { get; set; }
        public string cust_phone_last_four { get; set; }
        public string cust_phone_last_seven { get; set; }
        public string cust_membership_id { get; set; }
        public string cust_membership_cardinfo { get; set; }
        public string cust_callerID { get; set; }
        public string g_customers_id { get; set; }
        public DateTime cust_edited { get; set; }       
    }  
}
