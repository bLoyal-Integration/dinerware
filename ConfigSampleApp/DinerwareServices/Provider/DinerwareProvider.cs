using System.Linq;
using DinerwareAPIServices.DinerwareServiceReference;
using DinerwareServices.Interface;

namespace DinerwareServices.Provider
{
    public class DinerwareProvider : IDinerwareInterface
    {
        public VirtualClientClient virtualDinerwareClient;

        #region Constructor

        public DinerwareProvider()
        {
            virtualDinerwareClient = new VirtualClientClient();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add the bLoyal customer to dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customer">Customer Details</param>
        /// <returns>Customer ID</returns>
        public int AddCustomer(int userId, wsPerson customer)
        {
            return virtualDinerwareClient.AddCustomer(userId, customer);
        }

        /// <summary>
        /// Find the exiting customer in the dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customerId">CustomerId</param>
        /// <returns>Flag</returns>
        public bool FindDinerwareCustomer(int userId, string searchTerm)
        {
            var findCustomerResponse = virtualDinerwareClient.FindCustomersByAny(userId, searchTerm);
            if (findCustomerResponse.Count() != 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Create Ticket of Dinerware customer in the Dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="objwsTrialTicket">TrialTicket Details</param>
        public void CreateTickets(int userId, wsTrialTicket objwsTrialTicket)
        {
            var createTicketsResponse = virtualDinerwareClient.TrialCommit(userId, objwsTrialTicket);
            int pendingTicketId = createTicketsResponse.PendingID;
            var committedTicket = virtualDinerwareClient.CommitPendingTicketWithNoTransaction(pendingTicketId);

            //decimal TaxTotal = createTicketsResponse.TaxTotal;
            //decimal TicketTotal = createTicketsResponse.TicketTotal;
            //wsTransaction objwsTransaction = new wsTransaction();
            //objwsTransaction.ExchangeAmount = 0;
            //objwsTransaction.ExchangeRate = 0;
            //objwsTransaction.RefNumber = pendingTicketId.ToString();

            //virtualDinerwareClient.addTransactionToTicket(userId, committedTicket, objwsTransaction);

            //virtualDinerwareClient.CommitPendingTicket(pendingTicketId, objwsTransaction);

        }

        #endregion
    }
}
