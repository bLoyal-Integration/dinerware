using bLoyal.Connectors.LoyaltyEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DinerwareSystem.Helpers
{
    public class CartLineHelper
    {
        LoggerHelper _logger = LoggerHelper.Instance;

        /// <summary>
        /// Get Ticket Cart Lines
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public IList<CartLine> GetCartLines(Dinerware.Ticket ticket)
        {
            try
            {
                if (ticket != null && ticket.Items != null && ticket.Items.Any())
                {
                    var cartLines = ticket.Items.Select(menuItem => new CartLine
                    {
                        Price = menuItem.Price / menuItem.Quantity,
                        ProductCode = menuItem.ID,
                        ProductExternalId = menuItem.TIID,
                        ProductName = menuItem.ItemName,
                        Quantity = menuItem.Quantity,
                        PriceSource = PriceSource.Base,                       
                        ExternalId = menuItem.ID
                    }).ToList();
                    return cartLines;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CartLineHelper - GetCartLines");                
            }
            return null;
        }
    }
}
