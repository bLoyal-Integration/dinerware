using DataSyncService.DinerwareEngineService;
using DataSyncService.Helper;
using System;
using System.ServiceModel;

namespace DataSyncService.Provider
{
    public class DinerwareProvider
    {
        #region Properties

        VirtualClientClient _virtualDinerwareClient;

        #endregion

        #region Constructor

        public DinerwareProvider()
        {
            var conFigHelper = new ConfigurationHelper();
            var endPointAddress = new EndpointAddress(conFigHelper.URL_VIRTUALCLIENT);
            var binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.None;
            _virtualDinerwareClient = new VirtualClientClient(binding, endPointAddress);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Trans For Ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="bIncludeVoided"></param>
        /// <returns></returns>
        public PaymentCollection GetTransForTicket(int ticketId, bool bIncludeVoided)
        {
            return _virtualDinerwareClient.getTransForTicket(ticketId, bIncludeVoided);
        }

        /// <summary>
        /// Get Ticket Menu Items List
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="ticketId">ticketID</param>
        /// <returns>wsMenuItemList</returns>
        public wsMenuItem[] GetTicketMenuItems(int userid, int ticketId)
        {
            return _virtualDinerwareClient.GetTicketMenuItems(userid, ticketId);
        }

        /// <summary>
        /// Get Customer By CustomerId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>Customer</returns>
        public wsPerson GetCustomerById(int userId, int customerId)
        {
            return _virtualDinerwareClient.GetCustomer(userId, customerId);
        }

        #endregion
    }
}
