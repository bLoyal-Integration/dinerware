using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;
using System.Threading.Tasks;

namespace DinerwareSystem
{
    public class bLoyalViewCustomerExtension : OrderEntryExtension
    {

        public override string displayName { get; } = "bLoyal VIEW CUSTOMER";

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

                        if (ticketId > 0)
                        {                           
                            var services = new Provider.LoyaltyEngineServices();
                            string key = string.Empty;

                            key = AsyncHelper.RunSync(() => services.GetSessionAsync());

                            if (currentTicket != null && currentTicket.AmountDue == 0 && currentTicket.PaymentTotal > 0)
                            {                                
                                frmTicketIsFullyPaid frmTicketIsFullyPaid = new frmTicketIsFullyPaid(true, Messages.APPLY_PAYMENT_WARNING);
                                frmTicketIsFullyPaid.Show();
                            }
                            else if (!string.IsNullOrWhiteSpace(key))
                            {
                                frmViewCustomer frmViewCustomer = new frmViewCustomer(key);
                                frmViewCustomer.Show();
                            }
                            else
                            {
                                if (ServiceURLHelper.IsbLoyalServiceUrlDown)
                                {
                                    frmbLoyalServiceUrlDownWarning frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                                    frmServiceUrlDown.Show();
                                }
                                else
                                {
                                    frmServerOfflineWarning offLineMsg = new frmServerOfflineWarning();
                                    offLineMsg.Show();
                                }
                            }
                        }
                        else
                        {
                            frmShowWarningMessage frmshowWarning = new frmShowWarningMessage();
                            frmshowWarning.Show();
                        }
                    }
                    else
                    {
                        frmbLoyalServiceUrlDownWarning frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                        frmServiceUrlDown.Show();
                    }
                }
                else
                {
                    DisableEnablebLoyalFunctionality disableEnablebLoyal = new DisableEnablebLoyalFunctionality();
                    disableEnablebLoyal.Show();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "VIEW CUSTOMER OEButtonPressed");
            }
        }

        }
    }
