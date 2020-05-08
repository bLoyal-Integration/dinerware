using bLoyal.Connectors.Grid.Sales;
using DataSyncService.DinerwareEngineService;
using DataSyncService.Model;
using DataSyncService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataSyncService.Helper
{
    public class DinerwareDBHelper : ConfigurationHelper
    {
        #region Public Methods


        /// <summary>
        /// Get Dinerware Customer By Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DWAddress GetDinerwareCustomerAddressById(int customerId)
        {
            Models.DWAddress address = null;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM [{0}].[dbo].[CustomerAddresses] where address_cust_id =@address_cust_id", DATABASENAME), conn))
                {
                    cmd.Parameters.AddWithValue("@address_cust_id", customerId);
                    conn.Open();
                    using (SqlDataReader oReader = cmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            address = new DWAddress
                            {
                                address_cust_id = Convert.ToInt32(oReader["address_cust_id"].ToString()),
                                address_address1 = oReader["address_address1"].ToString(),
                                address_address2 = oReader["address_address2"].ToString(),
                                address_city = oReader["address_city"].ToString(),
                                address_st = oReader["address_st"].ToString(),
                                address_postal = oReader["address_postal"].ToString(),
                                address_type = oReader["address_type"].ToString(),
                                address_notes = oReader["address_notes"].ToString(),
                                address_phone = oReader["address_phone"].ToString()
                            };
                        }
                    }
                }
            }
            return address;
        }

        /// <summary>
        /// Update Ticket of Add CustomerID by TicketID
        /// </summary>
        /// <param name="ticketId">ticketId</param>
        /// <param name="customerID">customerID</param>
        public void RevenueClassById(int revenueClassId, Guid g_RevenueClass_Id)
        {
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand("update RevenueClass set g_RevenueClass_id =@g_RevenueClass_id where i_class_id = @i_class_id", conn))
                {
                    cmd.Parameters.AddWithValue("@i_class_id", revenueClassId);
                    cmd.Parameters.AddWithValue("@g_RevenueClass_id", g_RevenueClass_Id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get Revenue Class By ID
        /// </summary>
        /// <param name="g_RevenueClass_Id"></param>
        /// <returns></returns>
        public int GetRevenueClassById(Guid g_RevenueClass_Id)
        {
            int i_class_Id = 0;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT i_class_id FROM [{0}].[dbo].[RevenueClass] where g_RevenueClass_id =@g_RevenueClass_id", DATABASENAME), conn))
                {
                    cmd.Parameters.AddWithValue("@g_RevenueClass_id", g_RevenueClass_Id);
                    conn.Open();
                    int.TryParse(cmd.ExecuteScalar().ToString(), out i_class_Id);
                }
            }
            return i_class_Id;
        }

        /// <summary>
        /// Get Discount Rule discountUid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetDiscountRuleByName(string discountName)
        {
            int discountId = 0;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT i_discount_type_id FROM [{0}].[dbo].[DiscountType] where  s_name = @s_name", DATABASENAME), conn))
                {
                    cmd.Parameters.AddWithValue("@s_name", discountName);
                    conn.Open();
                    int.TryParse(cmd.ExecuteScalar().ToString(), out discountId);
                    return discountId;
                }
            }
        }

        /// <summary>
        /// Get All Close and FullyPaid Ticket
        /// </summary>
        /// <returns></returns>
        public DataSet GetClosedTickets()
        {
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM [{0}].[dbo].[Ticket] WHERE b_use_custom_status ='false' AND b_closed ='true' AND i_void_ticket_id is null", DATABASENAME), conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// Get All Revenue Classes
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllRevenueClasses()
        {
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM RevenueClass", conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// Get RevenueClasses By Id
        /// </summary>
        /// <returns></returns>
        public DataSet GetRevenueClassById(string id)
        {
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM RevenueClass where i_class_id=@i_class_id", conn))
                {
                    cmd.Parameters.AddWithValue("@i_class_id", int.Parse(id));
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// Get Item SalePrice DiscountAmount
        /// </summary>
        /// <param name="ticketItemId"></param>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public decimal GetItemSalePriceDiscountAmount(int ticketItemId)
        {
            decimal discountAmount = 0;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SUM(c_discount_value) FROM [{0}].[dbo].[DiscountItem] where i_ticket_item_id = @i_ticket_item_id AND s_discount_name ='{1}'", DATABASENAME, DW_SALEPRICE_LEVEL_DISCOUNT_TYPE_NAME), conn))
                {
                    cmd.Parameters.AddWithValue("@i_ticket_item_id", ticketItemId);
                    conn.Open();
                    decimal.TryParse(cmd.ExecuteScalar().ToString(), out discountAmount);
                    return discountAmount;
                }
            }
        }

        /// <summary>
        /// GetItem Discount Amount
        /// </summary>
        /// <param name="ticketItemId"></param>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public decimal GetItemDiscountAmount(int ticketItemId)
        {
            decimal discountAmount = 0;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SUM(c_discount_value) FROM [{0}].[dbo].[DiscountItem] where i_ticket_item_id = @i_ticket_item_id AND s_discount_name ='{1}'", DATABASENAME, DW_ITEM_LEVEL_DISCOUNT_TYPE_NAME), conn))
                {
                    cmd.Parameters.AddWithValue("@i_ticket_item_id", ticketItemId);
                    conn.Open();
                    decimal.TryParse(cmd.ExecuteScalar().ToString(), out discountAmount);
                    return discountAmount;
                }
            }
        }

        /// <summary>
        /// GetItem Discount Amount
        /// </summary>
        /// <param name="ticketItemId"></param>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public decimal GetItemExternalDiscountAmount(int ticketItemId)
        {
            decimal discountAmount = 0;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SUM(c_discount_value) FROM [{0}].[dbo].[DiscountItem] where i_ticket_item_id = @i_ticket_item_id AND b_prorated_from_ticket='0' AND s_discount_name !='{1}'  AND s_discount_name !='{2}'", DATABASENAME, DW_ITEM_LEVEL_DISCOUNT_TYPE_NAME, DW_SALEPRICE_LEVEL_DISCOUNT_TYPE_NAME), conn))
                {
                    cmd.Parameters.AddWithValue("@i_ticket_item_id", ticketItemId);
                    conn.Open();
                    decimal.TryParse(cmd.ExecuteScalar().ToString(), out discountAmount);
                    return discountAmount;
                }
            }
        }

        /// <summary>
        /// Get Order Item Level Discount Amount
        /// </summary>
        /// <param name="ticketItemId"></param>
        /// <returns></returns>
        public decimal GetOrderItemLevelDiscountAmount(int ticketItemId)
        {
            decimal discountAmount = 0;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SUM(c_discount_value) FROM [{0}].[dbo].[DiscountItem] where i_ticket_item_id = @i_ticket_item_id AND b_prorated_from_ticket='1'", DATABASENAME), conn))
                {
                    cmd.Parameters.AddWithValue("@i_ticket_item_id", ticketItemId);
                    conn.Open();
                    decimal.TryParse(cmd.ExecuteScalar().ToString(), out discountAmount);
                    return discountAmount;
                }
            }
        }

        /// <summary>
        /// GetTicketDiscountAmount
        /// </summary>
        /// <param name="ticketItemId"></param>
        /// <returns></returns>
        public decimal GetTicketDiscountAmount(int ticketId)
        {
            decimal discountAmount = 0;
            int bLoyalOrderDiscountId = GetDiscountRuleByName(DW_ORDER_LEVEL_DISCOUNT_TYPE_NAME);
            if (bLoyalOrderDiscountId != 0)
            {
                using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
                {
                    using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SUM(c_discount_value) FROM [{0}].[dbo].[DiscountTicket] where i_ticket_id = @i_ticket_id AND i_discount_type_id =@i_discount_type_id ", DATABASENAME), conn))
                    {
                        cmd.Parameters.AddWithValue("@i_ticket_id", ticketId);
                        cmd.Parameters.AddWithValue("@i_discount_type_id", bLoyalOrderDiscountId);
                        conn.Open();
                        decimal.TryParse(cmd.ExecuteScalar().ToString(), out discountAmount);
                    }
                }
            }
            return discountAmount;
        }


        /// <summary>
        /// GetTicketDiscountAmount
        /// </summary>
        /// <param name="ticketItemId"></param>
        /// <returns></returns>
        public decimal GetTicketExternalDiscountAmount(int ticketId)
        {
            decimal discountAmount = 0;
            int bLoyalOrderDiscountId = GetDiscountRuleByName(DW_ORDER_LEVEL_DISCOUNT_TYPE_NAME);
            if (bLoyalOrderDiscountId != 0)
            {
                using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
                {
                    using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SUM(c_discount_value) FROM [{0}].[dbo].[DiscountTicket] where i_ticket_id = @i_ticket_id AND i_discount_type_id !=@i_discount_type_id", DATABASENAME), conn))
                    {
                        cmd.Parameters.AddWithValue("@i_ticket_id", ticketId);
                        cmd.Parameters.AddWithValue("@i_discount_type_id", bLoyalOrderDiscountId);
                        conn.Open();
                        decimal.TryParse(cmd.ExecuteScalar().ToString(), out discountAmount);
                    }
                }
            }
            return discountAmount;
        }

        /// <summary>
        /// Get Item Tax Amount
        /// </summary>
        /// <param name="ticketItemId"></param>
        /// <returns></returns>
        public decimal GetItemTaxAmount(int ticketItemId)
        {
            decimal amount = 0;

            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SUM(c_amount) FROM [{0}].[dbo].[TaxInstanceItem] where i_ticket_item_id = @i_ticket_item_id", DATABASENAME), conn))
                {
                    cmd.Parameters.AddWithValue("@i_ticket_item_id", ticketItemId);
                    conn.Open();
                    decimal.TryParse(cmd.ExecuteScalar().ToString(), out amount);
                }
            }

            return amount;
        }

        /// <summary>
        /// Get All MenuItems
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllMenuItems(string lastSyncProducts)
        {
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                DateTime lastSyncTime = new DateTime();
                SqlCommand cmd = null;
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(lastSyncProducts))
                {
                    lastSyncTime = Convert.ToDateTime(lastSyncProducts);
                    cmd = new SqlCommand(string.Format("Select *FROM [{0}].[dbo].[MenuItem] where dt_edited > @dt_edited AND b_active='true'", DATABASENAME));
                    cmd.Parameters.AddWithValue("@dt_edited", lastSyncTime);
                }
                else
                {
                    cmd = new SqlCommand(string.Format("Select *FROM [{0}].[dbo].[MenuItem]", DATABASENAME));
                }
                cmd.Connection = conn;
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                return ds;
            }
        }

        /// <summary>
        /// Update Ticket of Add CustomerID by TicketID
        /// </summary>
        /// <param name="ticketId">ticketId</param>
        /// <param name="customerID">customerID</param>
        public void UpdateTicketStatusById(int ticketId)
        {
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                using (SqlCommand cmd = new SqlCommand("update Ticket set b_use_custom_status ='1' where i_ticket_id = @ticket_id", conn))
                {
                    cmd.Parameters.AddWithValue("@ticket_id", ticketId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get all Dinerware Customer
        /// </summary>
        /// <returns></returns>
        public List<DWCustomer> GetAllDinerwareCustomer(string lastSyncCustomers)
        {
            var dwCus = new List<DWCustomer>();
            DateTime lastSyncTime = new DateTime();
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                SqlCommand cmd = new SqlCommand();
                string cmdStr = string.Empty;
                if (!string.IsNullOrEmpty(lastSyncCustomers))
                {
                    DateTime.TryParse(lastSyncCustomers, out lastSyncTime);
                    cmdStr = string.Format("SELECT [cust_id],[cust_fname],[cust_edited],[cust_lname],[cust_phone],[cust_email],[cust_dob],[cust_active],[cust_fullname],[cust_phone_prefix],[cust_phone_last_four],[cust_phone_last_seven],[cust_membership_id],[cust_membership_cardinfo],[cust_callerID],[g_customers_id] FROM [{0}].[dbo].[Customers] where cust_edited > @cust_edited", DATABASENAME);
                    cmd.CommandText = cmdStr;
                    cmd.Parameters.AddWithValue("@cust_edited", lastSyncTime);
                }
                else
                {
                    cmdStr = string.Format("SELECT [cust_id],[cust_fname],[cust_edited],[cust_lname],[cust_phone],[cust_email],[cust_dob],[cust_active],[cust_fullname],[cust_phone_prefix],[cust_phone_last_four],[cust_phone_last_seven],[cust_membership_id],[cust_membership_cardinfo],[cust_callerID],[g_customers_id] FROM [{0}].[dbo].[Customers]", DATABASENAME);
                    cmd.CommandText = cmdStr;
                }
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var customer = new DWCustomer
                        {
                            cust_id = Convert.ToInt32(oReader["cust_id"].ToString()),
                            cust_fname = oReader["cust_fname"].ToString(),
                            cust_lname = oReader["cust_lname"].ToString(),
                            cust_phone = oReader["cust_phone"].ToString(),
                            cust_email = oReader["cust_email"].ToString(),
                            cust_dob = oReader["cust_dob"].ToString(),
                            cust_active = oReader["cust_active"].ToString(),
                            cust_fullname = oReader["cust_fullname"].ToString(),
                            cust_phone_prefix = oReader["cust_phone_prefix"].ToString(),
                            cust_phone_last_four = oReader["cust_phone_last_four"].ToString(),
                            cust_phone_last_seven = oReader["cust_phone_last_seven"].ToString(),
                            cust_membership_id = oReader["cust_membership_id"].ToString(),
                            cust_membership_cardinfo = oReader["cust_membership_cardinfo"].ToString(),
                            cust_callerID = oReader["cust_callerID"].ToString(),
                            g_customers_id = oReader["g_customers_id"].ToString(),
                            cust_edited = Convert.ToDateTime(oReader["cust_edited"].ToString())
                        };
                        dwCus.Add(customer);
                    }
                }
            };
            return dwCus;
        }

        /// <summary>
        /// Get Dinerware Customer By Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DWCustomer GetDinerwareCustomerById(Int32 customerId)
        {
            DWCustomer customer = null;
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                SqlCommand cmd = new SqlCommand();

                string cmdStr = string.Format("SELECT [cust_id],[cust_fname],[cust_edited],[cust_lname],[cust_phone],[cust_email],[cust_dob],[cust_active],[cust_fullname],[cust_phone_prefix],[cust_phone_last_four],[cust_phone_last_seven],[cust_membership_id],[cust_membership_cardinfo],[cust_callerID],[g_customers_id] FROM [{0}].[dbo].[Customers] where cust_id = @cust_id", DATABASENAME);
                cmd.CommandText = cmdStr;
                cmd.Parameters.AddWithValue("@cust_id", customerId);

                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        customer = new DWCustomer
                        {
                            cust_id = Convert.ToInt32(oReader["cust_id"].ToString()),
                            cust_fname = oReader["cust_fname"].ToString(),
                            cust_lname = oReader["cust_lname"].ToString(),
                            cust_phone = oReader["cust_phone"].ToString(),
                            cust_email = oReader["cust_email"].ToString(),
                            cust_dob = oReader["cust_dob"].ToString(),
                            cust_active = oReader["cust_active"].ToString(),
                            cust_fullname = oReader["cust_fullname"].ToString(),
                            cust_phone_prefix = oReader["cust_phone_prefix"].ToString(),
                            cust_phone_last_four = oReader["cust_phone_last_four"].ToString(),
                            cust_phone_last_seven = oReader["cust_phone_last_seven"].ToString(),
                            cust_membership_id = oReader["cust_membership_id"].ToString(),
                            cust_membership_cardinfo = oReader["cust_membership_cardinfo"].ToString(),
                            cust_callerID = oReader["cust_callerID"].ToString(),
                            g_customers_id = oReader["g_customers_id"].ToString(),
                            cust_edited = Convert.ToDateTime(oReader["cust_edited"].ToString())
                        };
                    }
                }
            };
            return customer;
        }

        /// <summary>
        /// Get Menu ItemById
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public DataSet GetMenuItemById(int itemId)
        {
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM [{0}].[dbo].[MenuItem] where i_menu_item_id=@i_menu_item_id", DATABASENAME), conn))
                {
                    cmd.Parameters.AddWithValue("@i_menu_item_id", itemId);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// Get TicketItems By Ticket Id
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public List<TicketMenuItems> GetTicketItemsByTicketId(Int32 ticketId)
        {
            List<TicketMenuItems> ticketItems = new List<TicketMenuItems>();
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                SqlCommand cmd = new SqlCommand();

                string cmdStr = string.Format("SELECT *FROM [{0}].[dbo].[TicketItem] where i_ticket_id = @i_ticket_id AND i_void_item_id is null", DATABASENAME);
                cmd.CommandText = cmdStr;
                cmd.Parameters.AddWithValue("@i_ticket_id", ticketId);

                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        ticketItems.Add(new TicketMenuItems
                        {
                            i_ticket_item_id = oReader["i_ticket_item_id"].ToString(),
                            i_ticket_id = oReader["i_ticket_id"].ToString(),
                            i_menu_item_id = oReader["i_menu_item_id"].ToString(),
                            s_item = oReader["s_item"].ToString(),
                            c_price = oReader["c_price"].ToString(),
                            c_discount_amount = oReader["c_discount_amount"].ToString(),
                            c_tax_total = oReader["c_tax_total"].ToString(),
                            c_ticketitem_net_price = oReader["c_ticketitem_net_price"].ToString(),
                            c_ticketitem_gross_price = oReader["c_ticketitem_gross_price"].ToString(),
                            c_ticketitem_manual_discounts = oReader["c_ticketitem_manual_discounts"].ToString(),
                            c_ticketitem_choices_amount = oReader["c_ticketitem_choices_amount"].ToString(),
                            f_ticketitem_real_qty = oReader["f_ticketitem_real_qty"].ToString(),
                            i_void_item_id = oReader["i_void_item_id"].ToString(),
                            i_split_group = oReader["i_split_group"].ToString()

                        });
                    }
                }
            };
            return ticketItems;
        }

        public List<ChoiceItem> GetChoiceItems(int ticketItemId)
        {
            List<ChoiceItem> choiceItems = new List<ChoiceItem>();
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                SqlCommand cmd = new SqlCommand();

                string cmdStr = $"SELECT *FROM [{DATABASENAME}].[dbo].[ChoiceItem] where {ticketItemId} = i_ticket_item_id";
                cmd.CommandText = cmdStr;

                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        choiceItems.Add(new ChoiceItem
                        {
                            i_choice_item_id = oReader["i_choice_item_id"].ToString(),
                            i_choice_id = oReader["i_choice_id"].ToString(),
                            i_ticket_item_id = oReader["i_ticket_item_id"].ToString(),
                            s_choice_name = oReader["s_choice_name"].ToString(),
                            m_choice_price_mod = Convert.ToDecimal(oReader["m_choice_price_mod"].ToString()),
                            m_choiceitem_price = Convert.ToDecimal(oReader["m_choiceitem_price"].ToString()),
                            m_ci_quantity = Convert.ToDecimal(oReader["m_ci_quantity"].ToString()),
                            m_ci_original_quantity = Convert.ToDecimal(oReader["m_ci_original_quantity"].ToString()),
                        });
                    }
                }
            };
            return choiceItems;
        }

        /// <summary>
        /// Get Transactions By Ticket Id
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public List<Transactions> GetTransactionsByTicketId(int ticketId)
        {
            List<Transactions> transactions = new List<Transactions>();
            using (SqlConnection conn = new SqlConnection(DATABASE_CONNECTION_STR))
            {
                SqlCommand cmd = new SqlCommand();

                string cmdStr = string.Format("SELECT *FROM [{0}].[dbo].[Transactions] where i_ticket_id = @i_ticket_id AND b_cancel ='0'", DATABASENAME);
                cmd.CommandText = cmdStr;
                cmd.Parameters.AddWithValue("@i_ticket_id", ticketId);

                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        transactions.Add(new Transactions
                        {
                            i_transaction_id = oReader["i_transaction_id"].ToString(),
                            c_amount = oReader["c_amount"].ToString(),
                            i_ticket_id = oReader["i_ticket_id"].ToString(),
                            s_credit_auth = oReader["s_credit_auth"].ToString(),
                            s_ref_num = oReader["s_ref_num"].ToString(),
                            s_credit_tran_type = oReader["s_credit_tran_type"].ToString()
                        });
                    }
                }
            };
            return transactions;
        }

        /// <summary>
        /// Get SalesTrans Lines
        /// </summary>
        /// <param name="wsMenuItemList"></param>
        /// <returns></returns>
        public List<SalesTransLine> GetSalesTransLines(wsMenuItem[] wsMenuItemList)
        {
            var lines = new List<SalesTransLine>();
            foreach (var menuItem in wsMenuItemList)
            {
                DataSet ds = GetMenuItemById(int.Parse(menuItem.ID));
                DataTable dtTable = ds.Tables[0];
                if (dtTable != null && dtTable.Rows != null && dtTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dtTable.Rows)
                    {
                        TaxDetail taxDetail = null;
                        List<TaxDetail> taxDetailList = null;
                        if (menuItem.TAX_TOTAL != 0)
                        {
                            taxDetail = new TaxDetail { Amount = menuItem.TAX_TOTAL };
                            taxDetailList = new List<TaxDetail>();
                            taxDetailList.Add(taxDetail);
                        }

                        decimal lineDiscount = GetItemDiscountAmount(menuItem.TICKETITEM_ID);
                        var line = new SalesTransLine
                        {
                            Discount = lineDiscount,
                            ProductCode = row["s_description"].ToString(),
                            Price = menuItem.GROSS_PRICE,
                            ProductName = menuItem.NAME,
                            Quantity = menuItem.Quantity,
                            TaxDetails = taxDetailList != null ? taxDetailList : new List<TaxDetail>()
                        };
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }




        #endregion
    }
}
