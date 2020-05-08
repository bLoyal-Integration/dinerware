using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Linq;

namespace DinerwareSystem
{
    public class bLoyalLoadGiftCardExtension : OrderEntryExtension
    {

        public override string displayName { get; } = "bLoyal Load Gift Card";

        public override void ButtonPressed(object parentForm, Dinerware.TicketCollection theTickets, Dinerware.Ticket currentTicket, Dinerware.User currentUser, Dinerware.Person currentCustomer)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                {
                    frmConfigurationSettingsWarning frmConfigurationSettingsWarning = new frmConfigurationSettingsWarning();
                    frmConfigurationSettingsWarning.ShowDialog();
                    return;
                }
                if (currentTicket != null)
                {
                    currentOpenTicket = currentTicket;
                    currentOpenTicketId = currentTicket.ID;
                    currentUserId = currentUser.ID;
                }
                else
                {
                    currentOpenTicket = null;
                    currentOpenTicketId = string.Empty;
                    currentUserId = string.Empty;
                }

                var configurationHelper = ConfigurationHelper.Instance;
                if (configurationHelper.ENABLE_bLOYAL)
                {
                    if (!string.IsNullOrWhiteSpace(configurationHelper.GIFTCARD_TENDER_CODE))
                    {
                        if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                        {
                            int ticketId = 0;
                            int.TryParse(currentOpenTicketId, out ticketId);

                            if (ticketId != 0)
                            {
                                if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                                {
                                    // Load Gift Card Tender Window
                                    frmLoadGiftCardWarning loadGiftCardWarning = null;

                                    var loadGiftCardItems = configurationHelper.LOAD_GIFTCARD_ITEMS.Where(p => currentTicket.Items.Any(p2 => p2.ItemName.Equals(p, StringComparison.InvariantCultureIgnoreCase)));
                                    if (loadGiftCardItems != null && loadGiftCardItems.Any())
                                    {
                                        foreach (var loadGiftItem in loadGiftCardItems)
                                        {                                                                               
                                            var loadGiftCardMenuItems = currentTicket.Items.ToList().FindAll(t => t.ItemName.Equals(loadGiftItem, StringComparison.InvariantCultureIgnoreCase));
                                            if (loadGiftCardMenuItems != null)
                                            {
                                                foreach (var loadItem in loadGiftCardMenuItems)
                                                {
                                                    var service = new LoyaltyEngineServices();
                                                    var calculatedCart = AsyncHelper.RunSync(() => service.GetCartBySourceExternalIdAsync(currentTicket.ID));

                                                    bLoyal.Connectors.LoyaltyEngine.CartLine giftCartLine = null;
                                                    if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Lines != null && calculatedCart.Cart.Lines.Any())
                                                        giftCartLine = calculatedCart.Cart.Lines.FirstOrDefault(t => t.ProductName.Equals(loadGiftItem, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(t.GiftCardNumber) && !string.IsNullOrWhiteSpace(t.ProductExternalId) && t.ProductExternalId.Equals(loadItem.TIID));

                                                    if (giftCartLine == null || string.IsNullOrWhiteSpace(giftCartLine.GiftCardNumber))
                                                    {
                                                        frmLoadGiftCardBalance loadGiftCardBalance = new frmLoadGiftCardBalance(ticketId, string.Empty, currentTicket, loadItem, calculatedCart, loadGiftItem);
                                                        loadGiftCardBalance.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        loadGiftCardWarning = new frmLoadGiftCardWarning(Messages.ADD_LOAD_GIFTCARD_WARNING);
                                        loadGiftCardWarning.ShowDialog();
                                    }
                                }
                                else
                                {
                                    if (ServiceURLHelper.IsbLoyalServiceUrlDown)
                                    {
                                        frmbLoyalServiceUrlDownWarning frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                                        frmServiceUrlDown.ShowDialog();
                                    }
                                }
                            }
                            else
                            {
                                frmShowWarningMessage frmshowWarning = new frmShowWarningMessage();
                                frmshowWarning.ShowDialog();
                            }
                        }
                        else
                        {
                            frmbLoyalServiceUrlDownWarning frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                            frmServiceUrlDown.ShowDialog();
                        }
                    }
                    else
                    {
                        frmCheckGiftCardWarning giftCardWarning = new frmCheckGiftCardWarning();
                        giftCardWarning.ShowDialog();
                    }
                }
                else
                {
                    DisableEnablebLoyalFunctionality disableEnablebLoyal = new DisableEnablebLoyalFunctionality();
                    disableEnablebLoyal.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "bLoyalLoadGiftCard OEButtonPressed");
            }
        }
    }
}
