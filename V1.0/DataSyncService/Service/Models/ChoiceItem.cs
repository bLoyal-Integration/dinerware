using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSyncService.Models
{
    public class ChoiceItem
    {
        public string i_choice_item_id { get; set; }
        public string i_choice_id { get; set; }
        public string i_ticket_item_id { get; set; }
        public string s_choice_name { get; set; }
        public decimal m_choice_price_mod { get; set; }
        public decimal m_choiceitem_price { get; set; }
        public decimal m_ci_quantity { get; set; }
        public decimal m_ci_original_quantity { get; set; }
    }
}
