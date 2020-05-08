using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;
using System.Collections.Generic;

namespace DinerwareSystem
{
    class bLoyalUpdateServiceUrlExtension : ManagerExtension
    {
        public override string displayName { get; } = "Get bLoyal Service URLs";

        public override void ButtonPressed(object parentForm, Dinerware.User currentUser)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                {
                    frmConfigurationSettingsWarning frmConfigurationSettingsWarning = new frmConfigurationSettingsWarning();
                    frmConfigurationSettingsWarning.ShowDialog();
                    return;
                }

                if (ConfigurationHelper.Instance.ENABLE_bLOYAL)
                {
                    // Update bLoyal Service URL
                    var bLoyalService = ServiceURLHelper.UpdateServiceURL();
                    DinerwareEngineService.VirtualClientClient virtualDinerwareClient;
                    var endPointAddress = new System.ServiceModel.EndpointAddress(ConfigurationHelper.Instance.URL_VIRTUALCLIENT);
                    var binding = new System.ServiceModel.BasicHttpBinding();
                    binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                    virtualDinerwareClient = new DinerwareEngineService.VirtualClientClient(binding, endPointAddress);
                    var allTenders = virtualDinerwareClient.GetAllTenderTypes(0);

                    var tendersString = new List<string>();
                    foreach (var tender in allTenders)
                        tendersString.Add($"{tender.TenderTypeName}");

                    if (bLoyalService != null)
                    {
                        var response = Newtonsoft.Json.JsonConvert.SerializeObject(bLoyalService);
                        List<string> msg = new List<string>();
                        msg.Add("GetServiceUrls Succeeded.  ");
                        msg.Add("  ");
                        msg.Add($"   LoginDomain: {ConfigurationHelper.Instance.LOGIN_DOMAIN}");
                        msg.Add($"   Dinerware Tender Names: {string.Join(", ", tendersString)}");
                        msg.Add($"   Director: {bLoyalService.DirectorUrl}");
                        msg.Add("  ");
                        msg.Add($"   LoyaltyEngineApi: {bLoyalService.LoyaltyEngineApiUrl}");
                        msg.Add($"   OrderEngineApi: {bLoyalService.OrderEngineApiUrl}");
                        msg.Add($"   GridApi: {bLoyalService.GridApiUrl}");
                        msg.Add($"   PaymentApi: {bLoyalService.PaymentApiUrl}");
                        msg.Add($"   WebSnippetsApi: {bLoyalService.WebSnippetsApiUrl}");
                        msg.Add($"   EngagementEngineApi: {bLoyalService.EngagementEngineApiUrl}");
                        msg.Add("  ");
                        msg.Add($"   POSSnippetsUrl: {bLoyalService.POSSnippetsUrl}");
                        msg.Add($"   MyMobileLoyaltyUrl: {bLoyalService.MyMobileLoyaltyUrl}");
                        msg.Add($"   WebSnippetsUrl: {bLoyalService.WebSnippetsUrl}");
                        msg.Add($"   LoggingApiUrl: {bLoyalService.LoggingApiUrl}");
                        frmGetbLoyalServiceURLs show = new frmGetbLoyalServiceURLs(msg);
                        show.ShowDialog();
                    }
                    else
                    {
                        frmUpdatebLoyalServiceUrlMsg frmServiceMsg =  new frmUpdatebLoyalServiceUrlMsg(true);
                        frmServiceMsg.ShowDialog();
                    }
                }
                else
                {
                    DisableEnablebLoyalFunctionality disableShow = new DisableEnablebLoyalFunctionality();
                    disableShow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "Update bLoyal Service Url OEButtonPressed");
            }
        }


    }
}

