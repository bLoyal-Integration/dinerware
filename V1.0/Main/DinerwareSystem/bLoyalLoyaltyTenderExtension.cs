using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;
using System.Threading.Tasks;

namespace DinerwareSystem
{
    public class bLoyalLoyaltyTenderExtension : OrderEntryExtension
    {

        public override string displayName { get; } = "bLoyal Loyalty Tender";


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
                var configObj = ConfigurationHelper.Instance;
                if (configObj.ENABLE_bLOYAL)
                {
                    if (!string.IsNullOrWhiteSpace(configObj.LOYALTY_TENDER_CODE))
                    {
                        if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                        {
                            int ticketId = 0;
                            int.TryParse(currentOpenTicketId, out ticketId);

                            if (currentTicket != null && ticketId > 0 && currentTicket.AmountDue == 0)
                            {
                                frmTicketIsFullyPaid frmTicketIsFullyPaid = new frmTicketIsFullyPaid();
                                frmTicketIsFullyPaid.ShowDialog();
                            }
                            else if (ticketId > 0)
                            {                                                            
                                if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                                {
                                    bLoyal.Connectors.LoyaltyEngine.CalculatedCart calculatedCart = null;
                                    var services = new Provider.LoyaltyEngineServices();

                                    calculatedCart = AsyncHelper.RunSync(() => services.GetCartBySourceExternalId(currentOpenTicketId));

                                    if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Customer != null && calculatedCart.Cart.Customer.Uid != Guid.Empty)
                                    {
                                        bLoyalLoyaltyTender frmbLoyalLoyaltyTender = new bLoyalLoyaltyTender(calculatedCart.Cart.Customer, currentTicket);
                                        frmbLoyalLoyaltyTender.ShowDialog();
                                    }
                                    else
                                    {
                                        frmCustomerNotAssignToTicket customerNotAssign = new frmCustomerNotAssignToTicket();
                                        customerNotAssign.ShowDialog();
                                    }
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
                        frmLoyaltyTenderWarning loyaltyTenderWarning = new frmLoyaltyTenderWarning();
                        loyaltyTenderWarning.ShowDialog();
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
                LoggerHelper.Instance.WriteLogError(ex, "bLoyalLoyaltyTender OEButtonPressed");
            }
        }
        }
    }
