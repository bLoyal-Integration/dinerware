using DinerwareSystem.ConfigurationCache;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;
using Dinerware;

namespace DinerwareSystem
{
    public class bLoyalCalculateTicketExtension : OrderEntryExtension
    {
        #region Properties


        public override string displayName { get; } = "bLoyal APPLY DISCOUNT";



        #endregion

        #region Public Methods

        public override void ButtonPressed(object parentForm, TicketCollection theTickets, Ticket currentTicket, User currentUser, Person currentCustomer)
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

                if (ConfigurationHelper.Instance.ENABLE_bLOYAL)
                {
                    if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                    {
                        int ticketId = 0;
                        int userId = 0;
                        int.TryParse(currentUser.ID, out userId);

                        if (int.TryParse(currentOpenTicketId, out ticketId) && currentTicket != null && currentTicket.Items != null)
                        {
                            if (currentTicket.Items.Count == 0 && currentTicket.AmountDue == 0 && currentTicket.PaymentTotal > 0)
                            {
                                frmTicketIsFullyPaid frmTicketIsFullyPaid = new frmTicketIsFullyPaid(true, Messages.ADD_IETM_WARNING);
                                frmTicketIsFullyPaid.ShowDialog();
                            }
                            else if (currentTicket.Items.Count > 0 && currentTicket.AmountDue == 0 && currentTicket.PaymentTotal > 0)
                            {
                                frmTicketIsFullyPaid frmTicketIsFullyPaid = new frmTicketIsFullyPaid(true, Messages.APPLY_PAYMENT_WARNING);
                                frmTicketIsFullyPaid.ShowDialog();
                            }
                            else if (currentTicket.Items.Count > 0 && currentTicket.AmountDue > 0)
                            {
                                if (DiscountSets.OrderLevelDiscountId > 0 && DiscountSets.ItemLevelDiscountId > 0 && DiscountSets.ItemLevelSalePriceId > 0)
                                {
                                    frmCalculateSalesTransaction frmCalculateSalesTransaction = new frmCalculateSalesTransaction(userId, ticketId, currentTicket);
                                    frmCalculateSalesTransaction.ShowDialog();
                                }
                                else
                                {
                                    frmbLoyalDiscountRuleWarning frmbLoyalDiscountRuleWarning = new frmbLoyalDiscountRuleWarning();
                                    frmbLoyalDiscountRuleWarning.ShowDialog();
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
                    DisableEnablebLoyalFunctionality disableEnablebLoyal = new DisableEnablebLoyalFunctionality();
                    disableEnablebLoyal.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "CalculateSalesTransaction OEButtonPressed");
            }
        }

        #endregion
    }
}
