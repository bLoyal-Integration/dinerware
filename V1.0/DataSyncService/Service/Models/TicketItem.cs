namespace DataSyncService.Models
{
    public class TicketMenuItems
    {
        public string i_ticket_item_id { get; set; }
        public string i_ticket_id { get; set; }
        public string i_menu_item_id { get; set; }
        public string s_item { get; set; }
        public string c_price { get; set; }
        public string c_discount_amount { get; set; }
        public string c_tax_total { get; set; }
        public string c_ticketitem_net_price { get; set; }
        public string c_ticketitem_gross_price { get; set; }
        public string c_ticketitem_manual_discounts { get; set; }
        public string f_ticketitem_real_qty { get; set; }
        public string i_void_item_id { get; set; }
    }
}
