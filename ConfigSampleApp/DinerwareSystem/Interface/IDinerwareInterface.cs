using ConfigApp.DinerwareEngineService;
using Dinerware;



namespace ConfigApp.Interface
{
    /// <summary>
    /// This interface deal as contract to access Dinerware VirtualClient service methods.
    /// </summary>
    interface IDinerwareInterface
    {
        /// <summary>
        /// Add bLoyal customer to dinerware system
        /// </summary>
        /// <param name="userid">UserId</param>
        /// <param name="customer">Customer Details</param>
        /// <returns>Customer ID</returns>
        int AddCustomer(int userId, wsPerson customer);

        /// <summary>
        /// Update  customer 
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customer">Customer Details</param>
        /// <returns></returns>
        int UpdateCustomer(int userId, wsPerson customer);

        /// <summary>
        /// Find an exiting customer in dinerware system
        /// </summary>
        /// <param name="userid">Userid</param>
        /// <param name="searchTerm">SearchTerm</param>
        /// <returns>Flag</returns>
        wsPerson[] IsDinerwareCustomerAvailable(int userId, string searchTerm);

        /// <summary>
        /// Create Ticket of Dinerware customer in Dinerware system
        /// </summary>
        /// <param name="userid">Userid</param>
        /// <param name="objwsTrialTicket">Trial Ticket Details</param>
        int CreateTickets(int userId, wsTrialTicket objwsTrialTicket, string cartExternalId="");

        /// <summary>
        /// Get Ticket Menu Items
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="ticketID">ticketID</param>
        /// <returns></returns>
        wsMenuItem[] GetTicketMenuItems(int userid, int ticketID);

        /// <summary>
        /// Get Ticket Contents
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        TicketList GetTicketContents(int ticketId);

        /// <summary>
        /// Get Customer By CustomerId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>Customer</returns>
        wsPerson GetCustomerById(int userId, int customerId);

        /// <summary>
        /// Get Ticket By TicketId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        wsTicket GetTicketById(int Id);

        /// <summary>
        /// Add Screen Category(Like a menu item)
        /// </summary>
        /// <param name="screenCategoryName"></param>
        /// <returns></returns>
        int AddScreenCategory(string screenCategoryName);


        /// <summary>
        /// Add Menu Item to Dinerware
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        int AddMenuItem(MenuItem menuItem);

        /// <summary>
        /// Add Item To ScreenCategory
        /// </summary>
        /// <param name="iMenuItemID"></param>
        /// <param name="iCategoryID"></param>
        /// <param name="iOrdinal"></param>
        void AddItemToScreenCategory(int iMenuItemID, int iCategoryID, int iOrdinal);

         /// <summary>
        /// Edit Menu Item by menu name
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        int EditMenuItem(MenuItem menuItem);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        void getOpenTickets();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ticketId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        bool AssociateCustomerToTickets(int userId, int[] arrTicket, int customerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketName"></param>
        /// <returns></returns>
        wsTicket[] GetOpenTicketsByName(string ticketName);

        /// <summary>
        /// Add Discount To Ticket
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ticketID"></param>
        /// <param name="discountTypeId"></param>
        /// <param name="discountAmount"></param>
        /// <returns></returns>
        wsDiscountAddResult addDiscountToTicket(int userid, int ticketID, int discountTypeId, decimal discountAmount);

        /// <summary>
        /// Close Ticket
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="ticketId"></param>
        void CloseTicket(int user_id, int ticketId);

        /// <summary>
        /// Enroll Membership of loyalty Point 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="customerId"></param>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        wsEnrollMembershipResult EnrollMembership(string providerName, int customerId, string membershipId);


        /// <summary>
        /// Get All Dinerware Customers
        /// </summary>
        /// <returns></returns>
        wsCustomerResult[] GetAllCustomers();
          
    }
}
