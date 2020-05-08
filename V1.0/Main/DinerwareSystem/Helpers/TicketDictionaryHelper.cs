using System;
using System.Collections.Generic;

namespace DinerwareSystem
{
    public class TicketDictionary
    {
        public static Dictionary<int, string> Dictionary { get; set; }
        public static Dictionary<int, Guid> CartDictionary { get; set; }

        public string GetTicketStatus(int ticketId)
        {
            try
            {
                if (Dictionary.ContainsKey(ticketId))
                    return Dictionary[ticketId];
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "TicketDictionary - GetTicketStatus");
            }
            return string.Empty;
        }

        public void UpdateTicketStatus(int ticketId, string status)
        {
            try
            {
                if (Dictionary.ContainsKey(ticketId))
                    Dictionary[ticketId] = status;
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "TicketDictionary - UpdateTicketStatus");
            }
        }       
    }
}
