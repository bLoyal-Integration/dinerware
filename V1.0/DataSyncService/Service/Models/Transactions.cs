namespace DataSyncService.Models
{
    public class Transactions
    {
        public string i_transaction_id { get; set; }
        public string c_amount { get; set; }
        public string i_ticket_id { get; set; }
        public string s_credit_auth { get; set; }
        public string s_ref_num { get; set; }
        public string s_credit_tran_type { get; set; }
        public string b_cancel { get; set; }
    }
}
