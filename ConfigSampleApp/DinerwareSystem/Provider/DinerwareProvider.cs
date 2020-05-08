
using Dinerware;
using ConfigApp;
using ConfigApp.DinerwareEngineService;
using ConfigApp.Helpers;
using ConfigApp.Interface;
using System;
using System.IO;
using System.ServiceModel;

namespace ConfigApp.Provider
{
    public class DinerwareProvider
    {
        public VirtualClientClient virtualDinerwareClient;
        LoggerHelper logger = new LoggerHelper();

        #region Constructor

        public DinerwareProvider()
        {
            ConfigurationHelper conFigHelper = new ConfigurationHelper();
            var endPointAddress = new System.ServiceModel.EndpointAddress(conFigHelper.URL_VIRTUALCLIENT);
            var binding = new System.ServiceModel.BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.None;
            virtualDinerwareClient = new VirtualClientClient(binding, endPointAddress);
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
            try
            {
                return virtualDinerwareClient.AddCustomer(userId, customer);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AddCustomer Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add Transaction to Ticket
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public wsTicketChangeResult addTransactionToTicket(int userid, int ticketID, wsTransaction transaction)
        {
            try
            {
                return virtualDinerwareClient.addTransactionToTicket(userid, ticketID, transaction);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: addTransactionToTicket Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add Tender in Dinerware
        /// </summary>
        /// <param name="oTenderType"></param>
        /// <param name="sActionType"></param>
        /// <returns></returns>
        public int AddTenderType(TenderType oTenderType, string sActionType)
        {
            try
            {
                return virtualDinerwareClient.AddTenderType(oTenderType, sActionType);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AddTenderType Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Get All Active Categories
        /// </summary>
        /// <returns></returns>
        public ConfigApp.DinerwareEngineService.CategoryCollection GetAllActiveCategories()
        {
            try
            {
                return virtualDinerwareClient.GetAllActiveCategories();
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetAllActiveCategories Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add bLoyal customer to dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customer">Customer Details</param>
        /// <returns>Customer ID</returns>
        public int UpdateCustomer(int userId, wsPerson customer)
        {
            try
            {
                return virtualDinerwareClient.UpdateCustomer(userId, customer);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: UpdateCustomer Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        public bool AssociateCustomerTickets(int userid, int[] ticketIDs, int customerId)
        {
            try
            {
                return virtualDinerwareClient.AssociateCustomerTickets(userid, ticketIDs, customerId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AssociateCustomerTickets Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
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
                return virtualDinerwareClient.FindCustomersByAny(userId, searchTerm);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: IsDinerwareCustomerAvailable Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }

        }

        public int GetUpdatedSchema()
        {
            try
            {
                return virtualDinerwareClient.GetSchema();
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetUpdatedSchema Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
            return 0;
        }

        public wsCustomerResult[] GetAllCustomers()
        {
            try
            {
                return virtualDinerwareClient.GetAllCustomers(10000, 10000, "");
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetAllCustomers Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }

        }
    

        /// <summary>
        /// Get Ticket Contents
        /// </summary>
        /// <param name="ticketId">ticketId</param>
        /// <returns>TicketList</returns>
        public TicketList GetTicketContents(int ticketId)
        {
            try
            {
                return virtualDinerwareClient.getTicketContents(ticketId, true);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetTicketContents Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Get Ticket By TicketId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public wsTicket GetTicketById(int Id)
        {
            try
            {
                return virtualDinerwareClient.getTicket(Id);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetTicketById Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Get Trans For Ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="bIncludeVoided"></param>
        /// <returns></returns>
        public DinerwareEngineService.PaymentCollection GetTransForTicket(int ticketId, bool bIncludeVoided)
        {
            try
            {
                return virtualDinerwareClient.getTransForTicket(ticketId, bIncludeVoided);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetTransForTicket Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
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
                return virtualDinerwareClient.GetTicketMenuItems(userid, ticketId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetTicketMenuItems Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
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
                return virtualDinerwareClient.GetCustomer(userId, customerId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetCustomerById Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add Screen Category(Like a menu item)
        /// </summary>
        /// <param name="screenCategoryName"></param>
        /// <returns></returns>
        public int AddScreenCategory(string screenCategoryName)
        {
            try
            {
                return virtualDinerwareClient.AddScreenCategory(screenCategoryName);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AddScreenCategory Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add Revenue Class
        /// </summary>
        /// <param name="oRevenueClass"></param>
        /// <returns></returns>
        public int AddRevenueClass(Dinerware.RevenueClass oRevenueClass)
        {
            try
            {
                return virtualDinerwareClient.AddRevenueClass(oRevenueClass);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AddRevenueClass Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add Menu Item to Dinerware
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public int AddMenuItem(MenuItem menuItem)
        {
            try
            {
                return virtualDinerwareClient.AddMenuItem(menuItem);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AddMenuItem Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Edit Menu Item by menu name
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public int EditMenuItem(MenuItem menuItem)
        {
            try
            {
                return virtualDinerwareClient.EditMenuItem(menuItem);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: EditMenuItem Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add Item To ScreenCategory
        /// </summary>
        /// <param name="iMenuItemID"></param>
        /// <param name="iCategoryID"></param>
        /// <param name="iOrdinal"></param>
        public void AddItemToScreenCategory(int iMenuItemID, int iCategoryID, int iOrdinal)
        {
            try
            {
                virtualDinerwareClient.AddItemToScreenCategory(iMenuItemID, iCategoryID, iOrdinal);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AddItemToScreenCategory Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void getOpenTickets()
        {
            try
            {
                var wsTicketList = virtualDinerwareClient.getOpenTickets();
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: getOpenTickets Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="arrTicket"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool AssociateCustomerToTickets(int userId, int[] arrTicket, int customerId)
        {
            try
            {
                return virtualDinerwareClient.AssociateCustomerTickets(userId, arrTicket, customerId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AssociateCustomerToTickets Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        public wsTicketItemVoid[] GetVoidedTicketItems(int ticketId)
        {
            try
            {
                return virtualDinerwareClient.GetVoidedTicketItems(ticketId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetVoidedTicketItems Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketName"></param>
        /// <returns></returns>
        public wsTicket[] GetOpenTicketsByName(string ticketName)
        {
            try
            {
                return virtualDinerwareClient.getOpenTicketsByName(ticketName);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetOpenTicketsByName Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Add Discount to Ticket
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="discountTypeId"></param>
        /// <param name="discountAmount"></param>
        /// <returns></returns>
        public wsDiscountAddResult addDiscountToTicket(int userid, int ticketID, int discountTypeId, decimal discountAmount)
        {
            try
            {
                return virtualDinerwareClient.addDiscountToTicket(userid, ticketID, discountTypeId, discountAmount);
            }
            catch (Exception ex)
            {               
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        public int EditTicketStatus(int i_StatusID, string s_Status, bool b_Active)
        {
            try
            {
                return virtualDinerwareClient.EditTicketStatus(i_StatusID, s_Status, b_Active);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: addItemToTicket Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }



        public void ReopenClosedTicket(int user_id, int ticketId)
        {
            try
            {
                virtualDinerwareClient.ReopenClosedTicket(user_id, ticketId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: ReopenClosedTicket Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        public Dinerware.TicketList GetTicketContents(int ticketID, bool uncached)
        {
            try
            {
                return virtualDinerwareClient.getTicketContents(ticketID, uncached);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: GetTicketContents Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                //throw ex;
            }
            return null;
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
        public wsDiscountAddResult addDiscountToItem(int userid, int ticketID, int ticketItemId, int discountTypeId, decimal discountAmount)
        {
            try
            {
                return virtualDinerwareClient.addDiscountToItem(userid, ticketID, ticketItemId, discountTypeId, discountAmount);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: addDiscountToItem Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
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
                var removeDiscount = virtualDinerwareClient.removeDiscountFromTicket(userid, ticketID, discountInstanceID);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: RemoveDiscountFromTicket Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
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
                var removeDiscount = virtualDinerwareClient.removeDiscountFromItem(userid, ticketID, ticketItemId, discountInstanceID);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: RemoveDiscountFromItem Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Enroll Membership of loyalty Point 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="customerId"></param>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        public wsEnrollMembershipResult EnrollMembership(string providerName, int customerId, string membershipId)
        {
            try
            {
                return virtualDinerwareClient.EnrollMembership(providerName, customerId, membershipId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: EnrollMembership Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Close Ticket
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="ticketId"></param>
        public void CloseTicket(int user_id, int ticketId)
        {
            try
            {
                virtualDinerwareClient.CloseTicket(user_id, ticketId);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: CloseTicket Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
        }


        /// <summary>
        /// Add Item to Ticket
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ticketId"></param>
        /// <param name="coverIndex"></param>
        /// <param name="trm"></param>
        /// <returns></returns>
        public wsTicketChangeResult AddItemToTicket(int userId, int ticketId, int coverIndex, ConfigApp.DinerwareEngineService.wsTrialMenuItem trm)
        {
            try
            {
                return virtualDinerwareClient.addItemToTicket(userId, ticketId, coverIndex, trm);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** Dinerware VirtualClient FAILURE: AddItemToTicket Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
            return null;
        }

        public void DinerwareLogging(string msg)
        {
            try
            {
                logger.WriteLogError("*** Add DinerwareLogging = " + msg);
                //using (StreamWriter writer = new StreamWriter("D:\\DinerwareErrorLogging.txt", true))
                //{
                //    writer.WriteLine(msg);
                //}
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
