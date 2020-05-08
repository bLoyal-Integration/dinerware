using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace DinerwareSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SetupResolver();

                /*-------------------------------------------------------------------------------------------*/
                //Test all bLoyal POS Snippets - This is for testing pupose only
                /*-------------------------------------------------------------------------------------------*/


                //Application.Run(new frmLoadGiftCardBalance());
                //Application.Run(new frmLoyaltyTenderTouchScreen());
                //Application.Run(new frmApplyGiftCardSummary(100, "4783658734658475"));
                //Application.Run(new frmLoadGiftCardWarning(""));
                //Application.Run(new frmLoadGiftCardKeyBoard());
                //Application.Run(new frmLoadGiftCardKeyBoard());
                //Application.Run(new frmLoadGiftCardBalance());
                //Application.Run(new frmTouchscreenKeyboard());
                //Application.Run(new frmUpdatebLoyalServiceUrlMsg(true));
                //Application.Run(new frmFindCustomer());
                //Application.Run(new frmQuickSignUp());
                //Application.Run(new frmOrderSyncing());
                //Application.Run(new frmViewCustomer());
                //Application.Run(new frmApplyCoupon());
                //Application.Run(new frmCreateOrder());
                //Application.Run(new frmDataSync());  
                //Application.Run(new frmConfiguration()); 
                //Application.Run(new DisableEnablebLoyalFunctionality()); 
                //Application.Run(new frmCalculateSalesTransaction());
                //Application.Run(new frmAlerts());
                //Application.Run(new frmQuickEdit());
                //Application.Run(new frmCustomersSync());
                //Application.Run(new frmSyncDiscountRule());
                //Application.Run(new ShowWarningMessage());
                //Application.Run(new bLoyalLoyaltyTender());  
                //Application.Run(new frmIsTicketOpen());  
                //Application.Run(new frmTicketMsgForm());     

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Setup Resolver
        /// </summary>
        private static void SetupResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        /// <summary>
        /// Current Domain On Assembly Resolve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
                return typeof(JsonSerializer).Assembly;
            return null;
        }

    }
}
