using DinerwareSystem;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;
using System.Threading.Tasks;
using Dinerware;
using bLoyal.Utilities;

namespace DinerwareFindCustomer
{
    public class FindCustomerExtension : OrderEntryExtension
    {
        #region Properties

        public override string displayName { get; } = "bLoyal FIND CUSTOMER";

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

                Guid? currentOpenTicketGuid;
                if (currentTicket != null)
                {
                    currentOpenTicket = currentTicket;
                    currentOpenTicketId = currentTicket.ID;
                    currentUserId = currentUser.ID;
                    currentOpenTicketGuid = currentTicket.TicketGuid;
                }
                else
                {
                    currentOpenTicket = null;
                    currentOpenTicketId = string.Empty;
                    currentUserId = string.Empty;
                    currentOpenTicketGuid = Guid.Empty;
                }

                if (ConfigurationHelper.Instance.ENABLE_bLOYAL)
                {
                    int ticketId = 0;
                    int.TryParse(currentOpenTicketId, out ticketId);

                    if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                    {
                        var services = new DinerwareSystem.Provider.LoyaltyEngineServices();
                        string key = string.Empty;

                        key = AsyncHelper.RunSync(() => services.GetSessionAsync());

                        frmTicketIsFullyPaid frmTicketIsFullyPaid = null;
                        if (currentTicket != null && ticketId != 0 && currentTicket.AmountDue == 0 && currentTicket.PaymentTotal > 0)
                        {
                            frmTicketIsFullyPaid = new frmTicketIsFullyPaid(true, Messages.APPLY_PAYMENT_WARNING);
                            frmTicketIsFullyPaid.ShowDialog();
                        }
                        else if (!string.IsNullOrWhiteSpace(key))
                        {
                            frmFindCustomer formFindCustomer = new frmFindCustomer(key);
                            formFindCustomer.ShowDialog();
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
                LoggerHelper.Instance.WriteLogError(ex, "Find Customer OEButtonPressed");
            }
        }

        #endregion

    }
}
