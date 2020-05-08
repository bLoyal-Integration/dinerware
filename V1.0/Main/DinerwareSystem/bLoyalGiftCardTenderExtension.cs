using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;

namespace DinerwareSystem
{
    public class bLoyalGiftCardTenderExtension : OrderEntryExtension
    {

        public override string displayName { get; } = "bLoyal Gift Card Tender";

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
                    if (!string.IsNullOrWhiteSpace(configObj.GIFTCARD_TENDER_CODE))
                    {
                        if (!ServiceURLHelper.IsbLoyalServiceUrlDown)
                        {
                            int ticketId = 0;
                            int.TryParse(currentOpenTicketId, out ticketId);
                            // Load Gift Card Tender Window                               
                            var gcTender = ticketId > 0 ? new frmbLoyalGiftCardTender(true, currentTicket) : new frmbLoyalGiftCardTender(false, currentTicket);
                            gcTender.ShowDialog();
                        }
                        else
                        {
                            frmbLoyalServiceUrlDownWarning frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                            frmServiceUrlDown.ShowDialog();
                        }
                    }
                    else
                    {
                        frmCheckGiftCardWarning tenderWarning = new frmCheckGiftCardWarning();
                        tenderWarning.ShowDialog();
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
                LoggerHelper.Instance.WriteLogError(ex, "LoyalGiftCardTender OEButtonPressed");
            }
        }

    }
}
