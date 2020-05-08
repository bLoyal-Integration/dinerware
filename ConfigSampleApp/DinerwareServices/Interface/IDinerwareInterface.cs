using DinerwareAPIServices.DinerwareServiceReference;

namespace DinerwareServices.Interface
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
        /// Find an exiting customer in the dinerware system
        /// </summary>
        /// <param name="userid">Userid</param>
        /// <param name="searchTerm">SearchTerm</param>
        /// <returns>Flag</returns>
        bool FindDinerwareCustomer(int userId, string searchTerm);

        /// <summary>
        /// Create Ticket of Dinerware customer in Dinerware system
        /// </summary>
        /// <param name="userid">Userid</param>
        /// <param name="objwsTrialTicket">Trial Ticket Details</param>
        void CreateTickets(int userId, wsTrialTicket objwsTrialTicket); 
    }
}
