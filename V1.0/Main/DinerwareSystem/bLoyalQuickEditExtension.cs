using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;
using System.Threading.Tasks;

namespace DinerwareSystem
{
    public class bLoyalQuickEditExtension : OrderEntryExtension
    {
        public override string displayName { get; } = "bLoyal QUICK EDIT";

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

                if (ConfigurationHelper.Instance.ENABLE_bLOYAL)
                {
                    if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                    {
                        int ticketId = 0;
                        int.TryParse(currentOpenTicketId, out ticketId);

                        if (ticketId != 0)
                        {
                            var services = new Provider.LoyaltyEngineServices();
                            string key = string.Empty;

                            key = AsyncHelper.RunSync(() => services.GetSessionAsync());

                            if (currentTicket != null && ticketId > 0 && currentTicket.AmountDue == 0 && currentTicket.PaymentTotal > 0)
                            {                               
                                frmTicketIsFullyPaid frmTicketIsFullyPaid = new frmTicketIsFullyPaid(true, Messages.APPLY_PAYMENT_WARNING);
                                frmTicketIsFullyPaid.ShowDialog();
                            }
                            else if (!string.IsNullOrWhiteSpace(key))
                            {
                                frmQuickEdit frmQuickEdit = new frmQuickEdit(key);
                                frmQuickEdit.ShowDialog();
                            }
                            else
                            {
                                if (ServiceURLHelper.IsbLoyalServiceUrlDown)
                                {
                                    frmbLoyalServiceUrlDownWarning frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                                    frmServiceUrlDown.ShowDialog();
                                }
                                else
                                {
                                    frmServerOfflineWarning offLineMsg = new frmServerOfflineWarning();
                                    offLineMsg.ShowDialog();
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
                LoggerHelper.Instance.WriteLogError(ex, "QuickEdit OEButtonPressed");
            }
        }
        }

    }
