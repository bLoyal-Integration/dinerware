using DinerwareSystem.DinerwareEngineService;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;

namespace DinerwareSystem.Helpers
{
    public class DinerwareHelper
    {
        #region Properties      

        LoggerHelper _logger = LoggerHelper.Instance;
        DinerwareProvider _dinerwareProvider = new DinerwareProvider();

        #endregion

        #region Methods


        /// <summary>
        /// Add bLoyal customer to dinerware system with Create new ticket 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="customer">customer</param>
        /// <returns>Response</returns>
        public TicketResponse AddCustomerToDinerware(int userId, bLoyal.Connectors.LoyaltyEngine.Customer transactionCustomer, string cartExternalId = "")
        {
            try
            {
                wsPerson[] arryWsPerson = null;
                int customerId = 0;
                wsPerson dwWSPerson = new wsPerson { FNAME = transactionCustomer.FirstName, LNAME = transactionCustomer.LastName, PHONE = transactionCustomer.Phone1, ADDRESS1 = transactionCustomer.Address.Address1, ADDRESS2 = transactionCustomer.Address.Address2, CITY = transactionCustomer.Address.City, STATE = transactionCustomer.Address.StateCode, POSTAL = transactionCustomer.Address.PostalCode, ID = transactionCustomer.Id.ToString(), EMAIL = transactionCustomer.EmailAddress };

                if (transactionCustomer.BirthDate.HasValue)
                    dwWSPerson.DOB = transactionCustomer.BirthDate.Value;             

                if (!string.IsNullOrWhiteSpace(transactionCustomer.ExternalId))
                {
                    int.TryParse(transactionCustomer.ExternalId, out customerId);
                    if (customerId != 0)
                    {
                        var wsPerson = _dinerwareProvider.GetCustomerById(userId, customerId);
                        if (wsPerson == null)
                            customerId = 0;
                    }
                }

                if (customerId == 0 && !string.IsNullOrWhiteSpace(dwWSPerson.EMAIL))
                {
                    arryWsPerson = _dinerwareProvider.IsDinerwareCustomerAvailable(userId, dwWSPerson.EMAIL);
                    if (arryWsPerson != null && arryWsPerson.Length > 0)
                        int.TryParse(arryWsPerson[0].ID, out customerId);

                }

                if (customerId == 0)
                {
                    customerId = _dinerwareProvider.AddCustomer(userId, dwWSPerson);
                }
                else
                {
                    dwWSPerson.ID = customerId.ToString();
                    int customerID = _dinerwareProvider.UpdateCustomer(userId, dwWSPerson);
                }

                int ticketId = 0;

                if (customerId != 0)
                {
                    dwWSPerson.ID = customerId.ToString();
                    // Create ticket
                    ticketId = CreateTickets(userId, dwWSPerson, cartExternalId);
                }
                return new TicketResponse { CustomerId = customerId, TicketId = ticketId };
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "AddCustomerToDinerware");
            }
            return null;
        }

        /// <summary>
        /// Update Ticket of new customer
        /// </summary>
        /// <param name="transactionCustomer"></param>
        /// <param name="transactionTokenStr"></param>
        public int UpdateTicketWithCustomer(int userId, bLoyal.Connectors.LoyaltyEngine.Customer transactionCustomer, string cartExternalId, int openTicketId)
        {
            wsPerson[] arryWsPerson = null;
            wsPerson wsPerson = null;
            int customerId = 0;
            try
            {
                wsPerson dwWSPerson = new wsPerson { FNAME = transactionCustomer.FirstName, LNAME = transactionCustomer.LastName, PHONE = transactionCustomer.Phone1, ADDRESS1 = transactionCustomer.Address.Address1, ADDRESS2 = transactionCustomer.Address.Address2, CITY = transactionCustomer.Address.City, STATE = transactionCustomer.Address.StateCode, POSTAL = transactionCustomer.Address.PostalCode, ID = transactionCustomer.Id.ToString(), EMAIL = transactionCustomer.EmailAddress };

                if (transactionCustomer.BirthDate.HasValue)
                    dwWSPerson.DOB = transactionCustomer.BirthDate.Value;

                if (!string.IsNullOrWhiteSpace(transactionCustomer.ExternalId))
                {
                    int.TryParse(transactionCustomer.ExternalId, out customerId);
                    if (customerId != 0)
                    {
                        wsPerson = _dinerwareProvider.GetCustomerById(userId, customerId);
                        if (wsPerson == null)
                            customerId = 0;
                    }
                }

                if (customerId == 0 && !string.IsNullOrWhiteSpace(dwWSPerson.EMAIL))
                {
                    arryWsPerson = _dinerwareProvider.IsDinerwareCustomerAvailable(userId, dwWSPerson.EMAIL);
                    if (arryWsPerson != null && arryWsPerson.Length > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(arryWsPerson[0].ID))
                        {
                            int.TryParse(arryWsPerson[0].ID, out customerId);                         
                            wsPerson = arryWsPerson[0];
                        }
                    }
                }

                if (customerId != 0 && wsPerson != null) // update the exiting customer
                {
                    wsPerson.FNAME = dwWSPerson.FNAME;
                    wsPerson.LNAME = dwWSPerson.LNAME;
                    wsPerson.EMAIL = dwWSPerson.EMAIL;
                    wsPerson.PHONE = dwWSPerson.PHONE;
                    wsPerson.ADDRESS1 = dwWSPerson.ADDRESS1;
                    wsPerson.ADDRESS2 = dwWSPerson.ADDRESS2;
                    wsPerson.CITY = dwWSPerson.FNAME;
                    wsPerson.STATE = dwWSPerson.STATE;
                    wsPerson.POSTAL = dwWSPerson.POSTAL;
                    wsPerson.DOB = dwWSPerson.DOB;
                }

                string ticketName = $"{dwWSPerson.LNAME}, {dwWSPerson.FNAME}";

                customerId = customerId == 0 ? _dinerwareProvider.AddCustomer(userId, dwWSPerson) : _dinerwareProvider.UpdateCustomer(userId, wsPerson);

                int[] ticketArr = { openTicketId };

                if (customerId != 0)
                    _dinerwareProvider.AssociateCustomerTickets(userId, ticketArr, customerId);

            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "UpdateTicketWithCustomer");
            }
            return customerId;
        }

        /// <summary>
        /// Create Ticket of Dinerware customer in Dinerware system
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="customer">Customer</param>
        private int CreateTickets(int userId, wsPerson customer, string cartExternalId)
        {
            DinerwareProvider objDinerwareProvider = new DinerwareProvider();
            try
            {
                wsTrialTicket objwsTrialTicket = new wsTrialTicket();
                objwsTrialTicket.CreateTime = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(customer.ID))
                    objwsTrialTicket.CustomerID = int.Parse(customer.ID);

                objwsTrialTicket.TicketName = $"{customer.LNAME}, {customer.FNAME}";

                return objDinerwareProvider.CreateTickets(userId, objwsTrialTicket, cartExternalId);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CreateTickets");
                throw ex;
            }
        }

        public DinerwareEngineService.UserCollection GetAllDinerwareUsers()
        {
            try
            {
                DinerwareProvider objDinerwareProvider = new DinerwareProvider();
                return objDinerwareProvider.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
