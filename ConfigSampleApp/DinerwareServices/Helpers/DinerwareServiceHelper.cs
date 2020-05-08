using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinerwareServices;
using DinerwareServices.Provider;
using DinerwareAPIServices.DinerwareServiceReference;

namespace DinerwareServices.Helpers
{
    public class DinerwareServiceHelper
    {
        #region Public Methods

        /// <summary>
        /// Add the bLoyal customer to dinerware system with Create ticket 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="customer">customer</param>
        /// <returns>Response</returns>
        public int AddCustomerToDinerware(int userId, wsPerson customer)
        {
            int addCustomerResponse = 0;
            try
            {
                DinerwareProvider objDinerwareProvider = new DinerwareProvider();

                string searchTerm = customer.LNAME + Constants.BLANK_SPACE + customer.FNAME;
                bool flag = FindDinerwareCustomer(userId, searchTerm);
                //Create customer in the dinerware
                if (flag)
                {
                    addCustomerResponse = objDinerwareProvider.AddCustomer(userId, customer);
                    // Create ticket
                    CreateTickets(userId, customer);
                    return addCustomerResponse;
                }
                else
                {
                    // Create ticket               
                    CreateTickets(userId, customer);
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }
            return addCustomerResponse;
        }

        /// <summary>
        /// Find the exiting customer in the dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTerm">searchTerm</param>
        /// <returns>Flag</returns>
        public bool FindDinerwareCustomer(int userId, string searchTerm)
        {
            DinerwareProvider objDinerwareProvider = new DinerwareProvider();
            return objDinerwareProvider.FindDinerwareCustomer(userId, searchTerm);
        }

        /// <summary>
        /// Create Ticket of Dinerware customer in the Dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customer">Customer</param>
        public void CreateTickets(int userId, wsPerson customer)
        {
            try
            {
                DinerwareProvider objDinerwareProvider = new DinerwareProvider();
                wsTrialTicket objwsTrialTicket = new wsTrialTicket();
                objwsTrialTicket.CreateTime = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(customer.ID))
                {
                    objwsTrialTicket.CustomerID = int.Parse(customer.ID);
                }
                objwsTrialTicket.TicketName = customer.LNAME + Constants.COMMA_BLANK_SPACE + customer.FNAME;
                objDinerwareProvider.CreateTickets(userId, objwsTrialTicket);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion
    }
}
