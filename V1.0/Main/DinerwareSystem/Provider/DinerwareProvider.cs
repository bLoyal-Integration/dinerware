using Dinerware;
using DinerwareSystem.DinerwareEngineService;
using DinerwareSystem.Helpers;
using System;
using System.ServiceModel;

namespace DinerwareSystem.Provider
{
    public class DinerwareProvider
    {

        #region Properties

        VirtualClientClient _virtualDinerwareClient;
        LoggerHelper _logger = LoggerHelper.Instance;

        #endregion

        #region Constructor

        public DinerwareProvider()
        {
            var endPointAddress = new EndpointAddress(ConfigurationHelper.Instance.URL_VIRTUALCLIENT);
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.Security.Mode = BasicHttpSecurityMode.None;
            _virtualDinerwareClient = new VirtualClientClient(binding, endPointAddress);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add bLoyal customer to dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customer">Customer Details</param>
        /// <returns>Customer ID</returns>
        public int AddCustomer(int userId, wsPerson customer)
        {
            return _virtualDinerwareClient.AddCustomer(userId, customer);
        }

        /// <summary>
        /// Get All Discount List
        /// </summary>
        /// <returns></returns>
        public Discount[] GetAllDiscountRule()
        {
            return _virtualDinerwareClient.GetAllDiscountList();
        }

        /// <summary>
        /// Add Transaction to Ticket
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public wsTicketChangeResult AddTransactionToTicket(int userid, int ticketID, wsTransaction transaction)
        {
            try
            {
                return _virtualDinerwareClient.addTransactionToTicket(userid, ticketID, transaction);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient AddTransactionToTicket");
            }
            return null;
        }

        /// <summary>
        /// Add bLoyal customer to dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customer">Customer Details</param>
        /// <returns>Customer ID</returns>
        public int UpdateCustomer(int userId, wsPerson customer)
        {
            return _virtualDinerwareClient.UpdateCustomer(userId, customer);
        }

        /// <summary>
        /// AssociateCustomerTickets
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketIDs"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool AssociateCustomerTickets(int userid, int[] ticketIDs, int customerId)
        {
            return _virtualDinerwareClient.AssociateCustomerTickets(userid, ticketIDs, customerId);
        }

        /// <summary>
        /// Find the exiting customer in dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customerId">CustomerId</param>
        /// <returns>Flag</returns>
        public wsPerson[] IsDinerwareCustomerAvailable(int userId, string searchTerm)
        {
            try
            {
                return _virtualDinerwareClient.FindCustomersByAny(userId, searchTerm);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient IsDinerwareCustomerAvailable");
            }
            return null;
        }

        /// <summary>
        /// Create Ticket of Dinerware customer in Dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="objwsTrialTicket">TrialTicket Details</param>
        public int CreateTickets(int userId, wsTrialTicket objwsTrialTicket, string cartExternalId = "")
        {
            try
            {
                var createTicketsResponse = _virtualDinerwareClient.TrialCommit(userId, objwsTrialTicket);

                int commitTicketId = _virtualDinerwareClient.CommitPendingTicket(createTicketsResponse.PendingID, new wsTransaction());

                if (commitTicketId > 0)
                {
                    int[] ticketArr = { commitTicketId };
                    if (objwsTrialTicket.CustomerID > 0)
                        _virtualDinerwareClient.AssociateCustomerTickets(userId, ticketArr, objwsTrialTicket.CustomerID);
                }

                return commitTicketId;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient CreateTickets");
            }
            return 0;
        }

        /// <summary>
        /// GetTenders
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        public TenderType[] GetTenders(int jobId)
        {
            return _virtualDinerwareClient.GetAllTenderTypes(jobId);
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public DinerwareEngineService.UserCollection GetAllUsers()
        {
            return _virtualDinerwareClient.GetAllUsers();
        }

        

        /// <summary>
        /// Get Trans For Ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="bIncludeVoided"></param>
        /// <returns></returns>
        public DinerwareEngineService.PaymentCollection GetTransForTicket(int ticketId, bool bIncludeVoided)
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
            try
            {
                return _virtualDinerwareClient.GetTicketMenuItems(userid, ticketId);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient GetTicketMenuItems");
            }
            return null;
        }

        /// <summary>
        /// Get Customer By CustomerId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>Customer</returns>
        public wsPerson GetCustomerById(int userId, int customerId)
        {
            try
            {
                return _virtualDinerwareClient.GetCustomer(userId, customerId);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient GetCustomerById");
            }
            return null;
        }

        /// <summary>
        /// Add Discount to Ticket
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="discountTypeId"></param>
        /// <param name="discountAmount"></param>
        /// <returns></returns>
        public wsDiscountAddResult AddDiscountToTicket(int userid, int ticketID, int discountTypeId, decimal discountAmount)
        {
            try
            {
                return _virtualDinerwareClient.addDiscountToTicket(userid, ticketID, discountTypeId, discountAmount);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient AddDiscountToTicket");
            }
            return null;
        }

        /// <summary>
        /// GetTicketContents
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="uncached"></param>
        /// <returns></returns>
        public TicketList GetTicketContents(int ticketID, bool uncached)
        {
            return _virtualDinerwareClient.getTicketContents(ticketID, uncached);
        }

        /// <summary>
        /// addDiscountToItem
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="ticketItemId"></param>
        /// <param name="discountTypeId"></param>
        /// <param name="discountAmount"></param>
        /// <returns></returns>
        public wsDiscountAddResult AddDiscountToItem(int userid, int ticketID, int ticketItemId, int discountTypeId, decimal discountAmount)
        {
            try
            {
                return _virtualDinerwareClient.addDiscountToItem(userid, ticketID, ticketItemId, discountTypeId, discountAmount);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient AddDiscountToItem");
                throw ex;
            }
        }

        /// <summary>
        /// RemoveDiscountFromTicket
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="discountInstanceID"></param>
        public void RemoveDiscountFromTicket(int userid, int ticketID, int discountInstanceID)
        {
            try
            {
                _virtualDinerwareClient.removeDiscountFromTicket(userid, ticketID, discountInstanceID);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient RemoveDiscountFromTicket");
            }
        }

        /// <summary>
        /// RemoveDiscountFromItem
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="ticketItemId"></param>
        /// <param name="discountInstanceID"></param>
        public void RemoveDiscountFromItem(int userid, int ticketID, int ticketItemId, int discountInstanceID)
        {
            try
            {
                _virtualDinerwareClient.removeDiscountFromItem(userid, ticketID, ticketItemId, discountInstanceID);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "VirtualDinerwareClient RemoveDiscountFromItem");
            }
        }


        #endregion
    }
}
