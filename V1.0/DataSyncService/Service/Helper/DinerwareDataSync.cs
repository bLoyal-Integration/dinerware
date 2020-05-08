using bLoyal;
using bLoyal.Connectors;
using bLoyal.Connectors.Grid;
using bLoyal.Connectors.Grid.Sales;
using DataSyncService.Model;
using DataSyncService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace DataSyncService.Helper
{
    public class DinerwareDataSync : ConfigurationHelper
    {
        #region Private Member

        private string _bLoyalConnectors_NugetPackageVersion = "1.0.4.7"; // Make sure whenever update bLoyal nuget package, then update version
        private string _bLoyal_Grid_NugetPackageVersion = "1.0.4.8"; // Make sure whenever update bLoyal Grid nuget package, then update version
        private string _lastSyncProductsDateTime = string.Empty;
        private string _lastSyncCustomersDateTime = string.Empty;
        private string _lastSyncProducts = string.Empty;
        private string _lastSyncCustomers = string.Empty;
        private int _totalDepartments = 0;
        private int _maxCount = 100;
        private bool _isImportCustomersSync = true;
        private bool _isImportSalesTransactionsSync = true;
        private bool _isImportProductsSync = true;
        private bool _isImportDepartmentsSync = true;

        DinerwareDBHelper _dinerwareDBHelper = new DinerwareDBHelper();
        List<ReportLine> _reportLines = new List<ReportLine>();
        Logger _logger = new Logger();
        LoggingHelper _log = new LoggingHelper();
        GridService _gridService = null;
        BatchResult _batchResult = BatchResult.PartialSuccess;

        #endregion

        #region Public Methods

        /// <summary>
        /// Start backend Integration 
        /// </summary>
        /// <returns></returns>
        public async Task DataSyncAsync()
        {
            Guid batchProfileUid = Guid.Empty;
            ReportLogEvent logReport = null;
            ServiceUrls serviceUrls = null;
            try
            {
                _log.ClearLogs();   // Clear local environment Logs()
                string dataSyncStartTime = $"--------------- *** Dinerware Backoffice Connector Data Sync Process Start DateTime - {DateTime.Now.ToString()} *** --------------";
                _reportLines.Add(new ReportLine { Message = dataSyncStartTime });
                _log.LogInfo(dataSyncStartTime);

                string reportName = string.Format(Constants.PROCESSING_HEADMSG, LOGIN_DOMAIN);
                _log.LogInfo(reportName);
                LogServiceVersion();
                _log.LogInfo("---------------------------------------------------------------------------------------------------------------");

                DataSyncServiceHelper.IsServiceRunning = true;
                logReport = new ReportLogEvent { EventType = LogEventType.Info, ReportName = reportName, ReportTitle = Constants.REPORT_TITLE };
                serviceUrls = await bLoyalService.GetServiceUrlsAsync(LOGIN_DOMAIN).ConfigureAwait(true);

                if (serviceUrls == null)
                {
                    string unabletoGetServiceUrl = string.Format(Constants.UNABLE_TO_GET_SERVICEURLS, LOGIN_DOMAIN, ACCESS_KEY);
                    _logger.LogError(unabletoGetServiceUrl);
                    _log.LogInfo(unabletoGetServiceUrl);
                    throw new Exception(unabletoGetServiceUrl);
                }

                if (serviceUrls != null)
                    _gridService = new GridService(serviceUrls.GridApiUrl, ACCESS_KEY);

                if (_gridService == null)
                {
                    string gridServerError = string.Format(Constants.GRID_SERVER_NOT_CONNECTING, LOGIN_DOMAIN, ACCESS_KEY);
                    _logger.LogError(gridServerError);
                    _log.LogInfo(gridServerError);
                    throw new Exception(gridServerError);
                }

                StartIntegrationBatchCommand command = new StartIntegrationBatchCommand
                {
                    BatchTitle = Constants.BATCHTITLE,
                    Uid = Guid.NewGuid(),
                    ConnecterVersion = GetVersion(),
                    ConnectorMachineName = Environment.MachineName,
                };

#if DEBUG
                command.BypassSyncNow = true;
#endif

                var batchProfile = await ProcessStartIntegrationBatchAsync(command).ConfigureAwait(true);

                batchProfileUid = batchProfile.BatchUid;

                var stateDictionary = batchProfile.CustomStateData as IDictionary<string, object>;
                if (stateDictionary != null)
                {
                    if (stateDictionary.ContainsKey(Constants.LASTSYNCPRODUCTS) && stateDictionary[Constants.LASTSYNCPRODUCTS] != null)
                        _lastSyncProductsDateTime = stateDictionary[Constants.LASTSYNCPRODUCTS].ToString();

                    if (stateDictionary.ContainsKey(Constants.LASTSYNCCUSTOMERS) && stateDictionary[Constants.LASTSYNCCUSTOMERS] != null)
                        _lastSyncCustomersDateTime = stateDictionary[Constants.LASTSYNCCUSTOMERS].ToString();

                    if (stateDictionary.ContainsKey(Constants.TOTALDEPARTMENTS) && stateDictionary[Constants.TOTALDEPARTMENTS] != null)
                        _totalDepartments = Convert.ToInt32(stateDictionary[Constants.TOTALDEPARTMENTS].ToString());
                }

                //// Data Import Dinerware to bLoyal               
                if (batchProfile.EntitySyncProfiles.Any(t => t.EntityName.Equals(typeof(Customer).Name) && (t.Direction == SyncDirection.Import || t.Direction == SyncDirection.Both)))
                {
                    await ImportCustomersAsync().ConfigureAwait(true);
                }
                if (batchProfile.EntitySyncProfiles.Any(t => t.EntityName.Equals(typeof(Department).Name) && (t.Direction == SyncDirection.Import || t.Direction == SyncDirection.Both)))
                {
                    await ImportDepartmentsAsync().ConfigureAwait(true);
                }
                /////*
                //// Dinerware departments and categories are not directly associated to one another. 
                //// Because bLoyal requires a category to be associated to a department, we've decided to only sync departments into bLoyal for now. 
                ////*/
                //////await ImportCategory(batchProfile.EntitySyncProfiles).ConfigureAwait(true);          

                if (batchProfile.EntitySyncProfiles.Any(t => t.EntityName.Equals(typeof(Product).Name) && (t.Direction == SyncDirection.Import || t.Direction == SyncDirection.Both)))
                {
                    await ImportProductsAsync().ConfigureAwait(true);
                }
                if (batchProfile.EntitySyncProfiles.Any(t => t.EntityName.Equals(typeof(SalesTransaction).Name) && (t.Direction == SyncDirection.Import || t.Direction == SyncDirection.Both)))
                {
                    await ImportSalesTransactionAsync().ConfigureAwait(true);
                }

                var closeCommand = CloseIntegrationBatch(batchProfile);

                await _gridService.CloseIntegrationBatchAsync(closeCommand).ConfigureAwait(true);

                var reportSection = new ReportSection();
                reportSection.Lines = _reportLines;
                var reportSections = new List<ReportSection>();
                reportSections.Add(reportSection);
                logReport.Sections = reportSections;
                _logger.LogReport(logReport, string.Format(Constants.DATASYNCCLOSETIME, DateTime.UtcNow.ToString()));
                await _logger.SubmitLogsAsync(serviceUrls.LoggingApiUrl, ACCESS_KEY).ConfigureAwait(true);

                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                _log.LogInfo($" Dinerware Data Sync Close Integration Batch, BatchProfileUid ={batchProfileUid}");
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                DataSyncServiceHelper.IsServiceRunning = false;
            }
            catch (Exception ex)
            {
                if (_gridService != null)
                {
                    _logger.LogErrorEx(ex, string.Format("**** FAILURE: Exception Error = {0}", ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : string.Empty));
                    _log.LogException(ex, string.Format("**** FAILURE: Exception Error = {0}", ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : string.Empty));

                    if (_isImportCustomersSync && _isImportDepartmentsSync && _isImportProductsSync && _isImportSalesTransactionsSync)
                        _batchResult = BatchResult.Success;
                    else if (!_isImportCustomersSync && !_isImportDepartmentsSync && !_isImportProductsSync && !_isImportSalesTransactionsSync)
                        _batchResult = BatchResult.Failed;

                    var closeCommand = new CloseIntegrationBatchCommand()
                    {
                        BatchUid = batchProfileUid,
                        BatchMessage = _batchResult == BatchResult.Failed ? "Batch failed to complete. See logs for more details." : _batchResult == BatchResult.PartialSuccess ? "Batch completed with some errors. See logs for more details." : "Batch completed successfully.",
                        Status = CommandStatus.Failed,
                        Result = _batchResult
                    };
                    var customStateData = new Dictionary<string, object>();
                    customStateData.Add(Constants.LASTSYNCPRODUCTS, _lastSyncProducts);
                    customStateData.Add(Constants.LASTSYNCCUSTOMERS, _lastSyncCustomers);
                    customStateData.Add(Constants.TOTALDEPARTMENTS, _totalDepartments);
                    closeCommand.CustomStateData = customStateData;
                    if (batchProfileUid != null && batchProfileUid != Guid.Empty)
                        await _gridService.CloseIntegrationBatchAsync(closeCommand).ConfigureAwait(true);
                }

                DataSyncServiceHelper.IsServiceRunning = false;
                if (serviceUrls != null)
                {
                    _logger.LogReport(logReport, string.Format(Constants.DATASYNCCLOSETIME, DateTime.UtcNow.ToString()));
                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                    _log.LogInfo(string.Format(Constants.DATASYNCCLOSETIME, DateTime.UtcNow.ToString()));
                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                    await _logger.SubmitLogsAsync(serviceUrls.LoggingApiUrl, ACCESS_KEY).ConfigureAwait(true);

                }
            }

            string dataSyncProcessEndTime = $"----------------- *** Dinerware Backoffice Connector Data Sync Process End DateTime - {DateTime.Now.ToString()} *** ------------------";
            _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
            _log.LogInfo(dataSyncProcessEndTime);
            _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check Sync Entity Profile AND Fill Close Integration Batch Command
        /// </summary>
        /// <param name="batchProfile"></param>
        /// <returns></returns>
        private CloseIntegrationBatchCommand CloseIntegrationBatch(IntegrationBatchProfile batchProfile)
        {
            foreach (var syncProfile in batchProfile.EntitySyncProfiles)
            {
                switch (syncProfile.EntityName)
                {
                    case Constants.DEPARTMENT:
                    case Constants.PRODUCT:
                    case Constants.SALESTRANSACTION:
                    case Constants.CUSTOMER:
                        break;
                    default:
                        _logger.LogWarning(string.Format(Constants.MIGRATIONDOESNOTSUPPORT, syncProfile.EntityName));

                        _log.LogInfo(string.Format(Constants.MIGRATIONDOESNOTSUPPORT, syncProfile.EntityName));
                        break;
                }
            }

            if (_isImportCustomersSync && _isImportDepartmentsSync && _isImportProductsSync && _isImportSalesTransactionsSync)
                _batchResult = BatchResult.Success;
            else if (!_isImportCustomersSync && !_isImportDepartmentsSync && !_isImportProductsSync && !_isImportSalesTransactionsSync)
                _batchResult = BatchResult.Failed;

            var closeCommand = new CloseIntegrationBatchCommand()
            {
                BatchUid = batchProfile.BatchUid,
                BatchMessage = _batchResult == BatchResult.Failed ? "Batch failed to complete. See logs for more details." : _batchResult == BatchResult.PartialSuccess ? "Batch completed with some errors. See logs for more details." : "Batch completed successfully.",
                Status = CommandStatus.Succeeded,
                Result = _batchResult
            };

            var customStateData = new Dictionary<string, object>();
            customStateData.Add(Constants.LASTSYNCPRODUCTS, _lastSyncProducts);
            customStateData.Add(Constants.LASTSYNCCUSTOMERS, _lastSyncCustomers);
            customStateData.Add(Constants.TOTALDEPARTMENTS, _totalDepartments);
            closeCommand.CustomStateData = customStateData;

            return closeCommand;
        }

        /// <summary>
        /// Process Start Integration Batch
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task<IntegrationBatchProfile> ProcessStartIntegrationBatchAsync(StartIntegrationBatchCommand command)
        {
            try
            {
                var batchProfile = await _gridService.StartIntegrationBatchAsync(command).ConfigureAwait(true);
                if (batchProfile == null)
                {
                    _logger.LogError(Constants.NO_ENTITY_DEFINED);
                    _log.LogInfo(Constants.NO_ENTITY_DEFINED);
                    throw new Exception(Constants.NO_ENTITY_DEFINED);
                }
                else if (!batchProfile.EntitySyncProfiles.Any())
                {
                    _logger.LogError(string.Format(Constants.NO_ENTITY_DEFINED));
                    _log.LogInfo(Constants.NO_ENTITY_DEFINED);
                    var closeCommand = new CloseIntegrationBatchCommand()
                    {
                        BatchUid = batchProfile.BatchUid,
                        BatchMessage = Constants.NO_ENTITY_DEFINED
                    };
                    await _gridService.CloseIntegrationBatchAsync(closeCommand).ConfigureAwait(true);
                }

                _log.LogInfo($"******** StartIntegrationBatch - EntitySyncProfiles Definition ");
                int entitySyncProfileCount = 1;
                foreach (var profiles in batchProfile.EntitySyncProfiles)
                {
                    _log.LogInfo($"******** {entitySyncProfileCount}. EntityName = {profiles.EntityName}");
                    entitySyncProfileCount++;
                }

                _log.LogInfo("---------------------------------------------------------------------------------------------------------------");

                return batchProfile;
            }
            catch (Exception ex)
            {
                LogError(ex, "ProcessStartIntegrationBatch");
                _log.LogException(ex, "ProcessStartIntegrationBatch");
                throw;
            }
        }

        /// <summary>
        /// Import Customers Dinerware to bLoyal
        /// </summary>
        /// <param name="entitySyncProfiles"></param>
        /// <returns></returns>
        private async Task ImportCustomersAsync()
        {
            _lastSyncCustomers = _lastSyncCustomersDateTime;
            List<Model.DWCustomer> wsCustomerList = null;

            _reportLines.Add(new ReportLine { Message = " *** Importing Customers from Dinerware *** " });
            _log.LogInfo("########################################### Importing Customers from Dinerware #########################################");

            try
            {
                int customerSyncCount = 0;
                wsCustomerList = _dinerwareDBHelper.GetAllDinerwareCustomer(_lastSyncCustomers);
                if (wsCustomerList != null && wsCustomerList.Count > 0)
                {
                    List<EntityChange<Customer>> customerChanges = new List<EntityChange<Customer>>();
                    DateTime entityCompareTime = !string.IsNullOrWhiteSpace(_lastSyncCustomers) ? Convert.ToDateTime(_lastSyncCustomers) : new DateTime();
                    foreach (var dwCus in wsCustomerList)
                    {
                        if (dwCus != null)
                        {
                            DateTime dt = dwCus.cust_edited;
                            if (dt != null && (string.IsNullOrEmpty(_lastSyncCustomers) || dt > entityCompareTime))
                            {
                                entityCompareTime = dt;
                                _lastSyncCustomers = dt.ToString();
                            }

                            var cust = new Customer
                            {
                                FirstName = dwCus.cust_fname,
                                LastName = dwCus.cust_lname,
                                EmailAddress = dwCus.cust_email,
                                Phone1 = dwCus.cust_phone,
                                ExternalId = dwCus.cust_id.ToString(),
                            };

                            try
                            {
                                Models.DWAddress address = _dinerwareDBHelper.GetDinerwareCustomerAddressById(dwCus.cust_id);
                                if (address != null)
                                {
                                    cust.Address1 = address.address_address1;
                                    cust.Address2 = address.address_address2;
                                    cust.StateName = address.address_st;
                                    cust.PostalCode = address.address_postal;
                                    cust.City = address.address_city;
                                }
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, "ImportCustomersAsync-GetDinerwareCustomerById");
                                _log.LogException(ex, "ImportCustomersAsync-GetDinerwareCustomerById");
                            }

                            DateTime cust_BirthDate;
                            if (!string.IsNullOrEmpty(dwCus.cust_dob) && DateTime.TryParse(dwCus.cust_dob, out cust_BirthDate))
                                cust.BirthDate = (bLoyal.Entities.Date)cust_BirthDate;

                            var entityChange = new EntityChange<Customer>()
                            {
                                Entity = cust,
                                ChangeType = EntityChangeType.Modified
                            };
                            customerChanges.Add(entityChange);
                            customerSyncCount++;
                            if (customerChanges.Count >= _maxCount)
                            {
                                await _gridService.SaveChangesAsync(customerChanges).ConfigureAwait(true);
                                customerChanges = new List<EntityChange<Customer>>();
                            }
                        }
                    }

                    if (customerChanges.Any())
                        await _gridService.SaveChangesAsync(customerChanges).ConfigureAwait(true);
                }

                string customerSyncResult = string.Format("******** Customers import Complete:  # Processed:{0} ", customerSyncCount);
                _reportLines.Add(new ReportLine { Message = customerSyncResult });
                _log.LogInfo(customerSyncResult);

                _isImportCustomersSync = true;
            }
            catch (Exception ex)
            {
                _isImportCustomersSync = false;
                LogError(ex, "ImportCustomersAsync");
                _log.LogException(ex, "ImportCustomersAsync");
            }
        }

        /// <summary>
        /// Import Departments Dinerware to bLoyal
        /// </summary>
        /// <param name="entitySyncProfiles"></param>
        /// <returns></returns>
        private async Task ImportDepartmentsAsync()
        {
            try
            {
                _reportLines.Add(new ReportLine { Message = "** Importing Department from Dinerware **" });
                _log.LogInfo("########################################### Importing Department from Dinerware ###########################################");

                var departmentChanges = new List<EntityChange<Department>>();
                var dataSet = _dinerwareDBHelper.GetAllRevenueClasses();
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    DataTable dtTable = dataSet.Tables[0];
                    if (dtTable != null && dtTable.Rows != null && dtTable.Rows.Count > 0 && dtTable.Rows.Count != _totalDepartments)
                    {
                        _totalDepartments = dtTable.Rows.Count;
                        foreach (DataRow row in dtTable.Rows)
                        {
                            string s_class_name = row["s_class_name"].ToString().Replace(" ", string.Empty);
                            Department entity = new Department
                            {
                                Code = s_class_name,
                                Name = row["s_class_name"].ToString(),
                                ExternalId = row["g_RevenueClass_id"].ToString()
                            };

                            var entityChange = new EntityChange<Department>()
                            {
                                Entity = entity,
                                ChangeType = EntityChangeType.Modified
                            };
                            departmentChanges.Add(entityChange);

                            if (departmentChanges.Count >= _maxCount)
                            {
                                await _gridService.SaveChangesAsync(departmentChanges).ConfigureAwait(true);
                                departmentChanges = new List<EntityChange<Department>>();
                            }
                        }
                    }
                }

                if (departmentChanges.Count > 0)
                    await _gridService.SaveChangesAsync(departmentChanges).ConfigureAwait(true);

                string syncProductCount = string.Format("******** Department import Complete:  # Processed:{0} ", departmentChanges.Count());
                _reportLines.Add(new ReportLine { Message = syncProductCount });
                _log.LogInfo(syncProductCount);

                _isImportDepartmentsSync = true;
            }
            catch (Exception ex)
            {
                _isImportDepartmentsSync = false;
                LogError(ex, "ImportDepartmentsAsync");
                _log.LogException(ex, "ImportDepartmentsAsync");
            }
        }

        /// <summary>
        /// Import Products Dinerware to bLoyal
        /// </summary>
        /// <param name="entitySyncProfiles"></param>
        /// <returns></returns>
        private async Task ImportProductsAsync()
        {
            _lastSyncProducts = _lastSyncProductsDateTime;
            try
            {

                _reportLines.Add(new ReportLine { Message = "** Importing Products from Dinerware **" });
                _log.LogInfo("########################################### Importing Products from Dinerware ###########################################");

                var productChanges = new List<EntityChange<Product>>();
                var dataSet = _dinerwareDBHelper.GetAllMenuItems(_lastSyncProducts);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    DataTable dtTable = dataSet.Tables[0];
                    if (dtTable != null && dtTable.Rows != null && dtTable.Rows.Count > 0)
                    {
                        DateTime entityCompareTime = new DateTime();
                        DateTime entityDateTime = new DateTime();
                        entityDateTime = DateTime.TryParse(_lastSyncProducts, out entityDateTime) ? entityDateTime : new DateTime();
                        foreach (DataRow row in dtTable.Rows)
                        {
                            DateTime dt;
                            dt = DateTime.TryParse(row["dt_edited"].ToString(), out dt) ? dt : new DateTime();
                            if (dt > entityDateTime)
                            {
                                if (dt != null && (string.IsNullOrEmpty(_lastSyncProducts) || (entityCompareTime != null && dt > entityCompareTime)))
                                {
                                    entityCompareTime = dt;
                                    _lastSyncProducts = dt.ToString();
                                }

                                string departmentCode = string.Empty;

                                try
                                {
                                    var revenueDataSet = _dinerwareDBHelper.GetRevenueClassById(row["i_revenue_class"].ToString());
                                    if (revenueDataSet != null &&
                                        revenueDataSet.Tables.Count > 0 &&
                                        revenueDataSet.Tables[0].Rows.Count > 0)
                                        departmentCode = revenueDataSet.Tables[0].Rows[0]["s_class_name"].ToString(); // need to take any one

                                }
                                catch (Exception ex)
                                {
                                    LogError(ex, "ImportProductsAsync-GetRevenueClassById");
                                    _log.LogException(ex, "ImportProductsAsync");
                                }
                                decimal base_Price = 0;
                                Product entity = new Product
                                {
                                    Name = row["s_long_name"].ToString(),
                                    Code = row["i_menu_item_id"].ToString(), // As per John request - if a product syncs to bLoyal with no SKU in its source system, we’ll set the external ID in both our external ID field and in our lookup code field.                                    
                                    BasePrice = !string.IsNullOrEmpty(row["c_menu_item_display_price"].ToString()) && decimal.TryParse(row["c_menu_item_display_price"].ToString(), out base_Price) ? base_Price : 0, // c_price and c_menu_item_display_price always same value(Item has a unique price)
                                    DepartmentCode = departmentCode,
                                    ExternalId = row["i_menu_item_id"].ToString()
                                };

                                var entityChange = new EntityChange<Product>()
                                {
                                    Entity = entity,
                                    ChangeType = EntityChangeType.Modified
                                };
                                productChanges.Add(entityChange);

                                if (productChanges.Count >= _maxCount)
                                {
                                    await _gridService.SaveChangesAsync(productChanges).ConfigureAwait(true);
                                    productChanges = new List<EntityChange<Product>>();
                                }
                            }
                        }
                    }
                }

                if (productChanges.Any())
                    await _gridService.SaveChangesAsync(productChanges).ConfigureAwait(true);

                string syncProductCount = string.Format("******** Product import Complete:  # Processed:{0} ", productChanges.Count());
                _reportLines.Add(new ReportLine { Message = syncProductCount });
                _log.LogInfo(syncProductCount);

                _isImportProductsSync = true;
            }
            catch (Exception ex)
            {
                _isImportProductsSync = false;
                LogError(ex, "ImportProductsAsync");
                _log.LogException(ex, "ImportProductsAsync");
            }
        }

        /// <summary>
        /// Sales Transaction Sync Dinerware To bLoyal
        /// </summary>
        /// <returns></returns>
        public async Task ImportSalesTransactionAsync()
        {
            try
            {
                _reportLines.Add(new ReportLine { Message = " *** Importing SalesTransaction from Dinerware *** " });
                _log.LogInfo("########################################### Importing SalesTransaction from Dinerware #########################################");

                DataSet ds = _dinerwareDBHelper.GetClosedTickets();
                if (ds != null && ds.Tables.Count > 0)
                    await SalesTransactionSyncAsync(ds).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _isImportSalesTransactionsSync = false;
                LogError(ex, "ImportSalesTransactionAsync");
                _log.LogException(ex, "ImportSalesTransactionAsync");
            }
        }

        /// <summary>
        /// Sales Transaction Sync
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="lastSyncSalesTransaction"></param>
        /// <returns></returns>
        private async Task SalesTransactionSyncAsync(DataSet ds)
        {
            try
            {
                DataTable dtTable = ds.Tables[0];
                var transactions = new List<EntityChange<SalesTransaction>>();
                int syncCount = 0;

                foreach (DataRow row in dtTable.Rows)
                {
                    transactions.AddRange(GetSalesTransactions(_gridService, row));
                    try
                    {
                        _dinerwareDBHelper.UpdateTicketStatusById(int.Parse(row["i_ticket_id"].ToString()));
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, "SalesTransactionSyncAsync-UpdateTicketStatusById");
                        _log.LogException(ex, "SalesTransactionSyncAsync-UpdateTicketStatusById");
                    }
                    _reportLines.Add(new ReportLine { Message = string.Format("   SalesTransaction import :  # TicketId = :{0} ", row["i_ticket_id"].ToString()) });
                    syncCount++;

                    if (transactions.Count() >= _maxCount)
                    {
                        await _gridService.SaveChangesAsync(transactions).ConfigureAwait(true);
                        transactions.Clear();
                    }
                }

                if (transactions.Count() > 0)
                    await _gridService.SaveChangesAsync(transactions).ConfigureAwait(true);

                string syncSalesTransCount = string.Format("******** SalesTransaction import Complete:  # Processed:{0} ", syncCount);
                _reportLines.Add(new ReportLine { Message = syncSalesTransCount });
                _log.LogInfo(syncSalesTransCount);

            }
            catch (Exception ex)
            {
                _isImportSalesTransactionsSync = false;
                LogError(ex, "SalesTransactionSyncAsync");
                _log.LogException(ex, "SalesTransactionSyncAsync");
            }
        }

        /// <summary>
        /// Get Sales Transaction
        /// </summary>
        /// <param name="_gridService"></param>
        /// <param name="transactions"></param>
        /// <param name="row"></param>
        private List<EntityChange<SalesTransaction>> GetSalesTransactions(GridService _gridService, DataRow row)
        {
            var transactions = new List<EntityChange<SalesTransaction>>();
            try
            {
                DWCustomer objwsPerson = null;
                int customerId = 0;
                if (int.TryParse(row["i_customer_id"].ToString(), out customerId))
                {
                    if (customerId > 0)
                    {
                        try
                        {
                            objwsPerson = _dinerwareDBHelper.GetDinerwareCustomerById(customerId);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, "GetSalesTransaction-GetCustomerById");
                            _log.LogException(ex, "GetSalesTransaction-GetCustomerById");
                        }
                    }
                }

                int ticketId = 0;
                int.TryParse(row["i_ticket_id"].ToString(), out ticketId);  // i_ticket_id always has string number

                List<TicketMenuItems> wsMenuItemList = null;
                try
                {
                    wsMenuItemList = _dinerwareDBHelper.GetTicketItemsByTicketId(ticketId);
                }
                catch (Exception ex)
                {
                    LogError(ex, "GetSalesTransaction-GetTicketMenuItems");
                    _log.LogException(ex, "GetSalesTransaction-GetTicketMenuItems");
                }

                Customer customer = null;
                if (objwsPerson != null)
                    customer = new Customer { FirstName = objwsPerson.cust_fname, LastName = objwsPerson.cust_lname, EmailAddress = objwsPerson.cust_email, ExternalId = objwsPerson.cust_id.ToString() };

                List<SalesTransLine> lines = null;

                if (wsMenuItemList != null && wsMenuItemList.Any())
                    lines = GetSalesTransLines(wsMenuItemList, ticketId);

                AppliedDiscount appliedDiscount = null;

                decimal orderDiscount = 0;
                try
                {
                    orderDiscount = _dinerwareDBHelper.GetTicketDiscountAmount(ticketId);
                    if (orderDiscount != 0)
                        appliedDiscount = new AppliedDiscount { Amount = orderDiscount };
                }
                catch (Exception ex)
                {
                    LogError(ex, "GetSalesTransaction-GetTicketDiscountAmount");
                    _log.LogException(ex, "GetSalesTransaction-GetTicketDiscountAmount");
                }

                try
                {
                    decimal orderExternalDiscount = _dinerwareDBHelper.GetTicketExternalDiscountAmount(ticketId);
                    if (orderExternalDiscount != 0)
                    {
                        if (orderDiscount != 0)
                            appliedDiscount = new AppliedDiscount { Amount = orderExternalDiscount + orderDiscount, External = true };
                        else
                            appliedDiscount = new AppliedDiscount { Amount = orderExternalDiscount, External = true };
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, "GetSalesTransaction-GetTicketExternalDiscountAmount");
                    _log.LogException(ex, "GetSalesTransaction-GetTicketExternalDiscountAmount");
                }

                var collectionList = new List<SalesTransPayment>();
                List<Transactions> paymentCollection = null;
                try
                {
                    paymentCollection = _dinerwareDBHelper.GetTransactionsByTicketId(ticketId);
                }
                catch (Exception ex)
                {
                    LogError(ex, "GetSalesTransaction-GetTransForTicket");
                    _log.LogException(ex, "GetSalesTransaction-GetTransForTicket");
                }
                decimal dinerwareTipAmount = 0;
                if (paymentCollection != null && paymentCollection.Count > 0)
                {
                    foreach (var payment in paymentCollection)
                    {
                        decimal netAmount = 0;
                        decimal.TryParse(payment.c_amount, out netAmount);
                        if (payment.s_credit_tran_type == "tip")
                        {
                            decimal tipAmount = Math.Abs(netAmount);
                            dinerwareTipAmount = dinerwareTipAmount + tipAmount;
                        }
                        else
                        {
                            if (netAmount > 0)
                            {
                                if (payment.s_credit_tran_type.Trim().Equals(DW_LOYALTYTENDERNAME, StringComparison.CurrentCultureIgnoreCase) || payment.s_credit_tran_type.Trim().Equals(DW_GIFTCARTTENDERNAME, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    collectionList.Add(new SalesTransPayment
                                    {
                                        Amount = netAmount,
                                        TenderCode = payment.s_credit_tran_type.Trim().Equals(DW_LOYALTYTENDERNAME, StringComparison.CurrentCultureIgnoreCase) ? TENDERCODE : GIFTCARDTENDERCODE
                                    });
                                }
                                else
                                {
                                    collectionList.Add(new SalesTransPayment
                                    {
                                        Amount = netAmount,
                                        External = true,
                                        TenderCode = payment.s_credit_tran_type.Trim()
                                    });
                                }
                            }
                        }
                    }
                }

                decimal gratuityAmount = 0;
                if (decimal.TryParse(row["c_auto_gratuity"].ToString(), out gratuityAmount) && gratuityAmount > 0)
                {
                    lines.Add(new SalesTransLine
                    {
                        ProductCode = "Gratuity",
                        Price = Math.Round(gratuityAmount, 2),
                        ProductName = "Gratuity",
                        Quantity = 1,
                        TaxDetails = new List<TaxDetail>()
                    });
                }

                var salesTrans = new SalesTransaction
                {
                    Lines = lines,
                    OrderDiscount = appliedDiscount,
                    Payments = collectionList,
                    Tip = dinerwareTipAmount,
                    SourceExternalId = row["i_ticket_id"].ToString(),
                    CartSourceExternalId = row["i_ticket_id"].ToString(),
                    ReferenceNumber = row["i_ticket_id"].ToString(),
                    Customer = customer,
                    CashierCode = row["i_user_id"].ToString(),
                    Created = Convert.ToDateTime(row["dt_close_time"].ToString()),
                };
                DateTime created_Local;
                if (DateTime.TryParse(row["dt_create_time"].ToString(), out created_Local))
                    salesTrans.CreatedLocal = created_Local;

                var changeRequest = new EntityChange<SalesTransaction>()
                {
                    Entity = salesTrans,
                    ChangeType = EntityChangeType.Modified
                };

                if (salesTrans.Lines != null && salesTrans.Lines.Any())
                    transactions.Add(changeRequest);

                return transactions;
            }
            catch (Exception ex)
            {
                _isImportSalesTransactionsSync = false;
                LogError(ex, "GetSalesTransaction");
                _log.LogException(ex, "GetSalesTransaction");
                return transactions;
            }
        }

        /// <summary>
        /// Get SalesTrans Lines
        /// </summary>
        /// <param name="wsMenuItemList"></param>
        /// <returns></returns>
        public List<SalesTransLine> GetSalesTransLines(List<TicketMenuItems> wsMenuItemList, int ticketId)
        {
            var lines = new List<SalesTransLine>();
            try
            {
                foreach (var menuItem in wsMenuItemList)
                {
                    try
                    {
                        List<TaxDetail> taxDetailList = null;
                        decimal itemPrice = 0;
                        decimal.TryParse(menuItem.c_price, out itemPrice);
                        decimal choiceItemPrice = 0;
                        decimal.TryParse(menuItem.c_ticketitem_choices_amount, out choiceItemPrice);
                        itemPrice += choiceItemPrice;
                        decimal itemTax = 0;

                        int ticketItemId = 0;
                        int.TryParse(menuItem.i_ticket_item_id, out ticketItemId);
                        try
                        {
                            itemTax = _dinerwareDBHelper.GetItemTaxAmount(ticketItemId);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, "GetSalesTransLines-GetItemTaxAmount");
                            _log.LogException(ex, "GetSalesTransLines-GetItemTaxAmount");
                        }

                        if (itemTax > 0)
                            taxDetailList = new List<TaxDetail> { new TaxDetail { Amount = itemTax } };

                        decimal lineDiscount = 0;
                        decimal lineSalePriceDiscount = 0;
                        decimal lineExternalDiscount = 0;
                        decimal orderDiscount = 0;

                        try
                        {
                            lineDiscount = _dinerwareDBHelper.GetItemDiscountAmount(ticketItemId);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, "GetSalesTransLines-GetItemDiscountAmount");
                            _log.LogException(ex, "GetSalesTransLines-GetItemDiscountAmount");
                        }
                        try
                        {
                            lineSalePriceDiscount = _dinerwareDBHelper.GetItemSalePriceDiscountAmount(ticketItemId);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, "GetSalesTransLines-GetItemSalePriceDiscountAmount");
                            _log.LogException(ex, "GetSalesTransLines-GetItemSalePriceDiscountAmount");
                        }
                        try
                        {
                            lineExternalDiscount = _dinerwareDBHelper.GetItemExternalDiscountAmount(ticketItemId);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, "GetSalesTransLines-GetItemExternalDiscountAmount");
                            _log.LogException(ex, "GetSalesTransLines-GetItemExternalDiscountAmount");
                        }
                        try
                        {
                            orderDiscount = _dinerwareDBHelper.GetOrderItemLevelDiscountAmount(ticketItemId);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, "GetSalesTransLines-GetOrderItemLevelDiscountAmount");
                            _log.LogException(ex, "GetSalesTransLines-GetOrderItemLevelDiscountAmount");
                        }

                        int quantity = 0;
                        int.TryParse(menuItem.f_ticketitem_real_qty, out quantity);

                        var line = new SalesTransLine
                        {
                            Discount = lineDiscount > 0 ? lineDiscount / quantity : 0,
                            OrderDiscount = orderDiscount / quantity,
                            ProductCode = menuItem.i_menu_item_id,
                            ProductExternalId = menuItem.i_menu_item_id,
                            Price = itemPrice,
                            ProductName = menuItem.s_item,
                            Quantity = quantity,
                            Comment = MapChoiceItemsToTicketItems(ticketItemId),
                            TaxDetails = taxDetailList != null ? taxDetailList : new List<TaxDetail>(),
                            Number = menuItem.i_split_group,
                        };

                        if (lineSalePriceDiscount > 0)
                        {
                            line.Price = itemPrice - lineSalePriceDiscount;
                            line.Source = PriceSource.SalePrice;
                        }

                        if (lineExternalDiscount > 0)
                        {
                            line.Discount = lineDiscount == 0 ? lineExternalDiscount / quantity : (lineDiscount / quantity) + lineExternalDiscount;
                            line.ExternallyAppliedDiscount = true;
                        }

                        lines.Add(line);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, "GetSalesTransLines-GetMenuItemById");
                        _log.LogException(ex, "GetSalesTransLines-GetMenuItemById");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "GetSalesTransLines");
                _log.LogException(ex, "GetSalesTransLines");
            }
            return lines;
        }

        private string MapChoiceItemsToTicketItems(int ticketItemId)
        {
            string comment = string.Empty;
            var choiceItems = _dinerwareDBHelper.GetChoiceItems(ticketItemId);
            foreach (var item in choiceItems)
                comment += AddToLineComment(item, string.IsNullOrWhiteSpace(comment));

            return comment;
        }

        private string AddToLineComment(ChoiceItem item, bool firstComment)
        {
            if (firstComment)
                return $"{item.s_choice_name} (Price:{item.m_choiceitem_price})";
            else
                return $", {item.s_choice_name} (Price:{item.m_choiceitem_price})";
        }

        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="ex"></param>
        private void LogError(Exception ex, string method)
        {
            try
            {
                _logger.LogErrorEx(ex, method);
            }
            catch
            {
                // We can not log in this area...
            }
        }

        /// <summary>
        /// GetVersion
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            try
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                if (version != null)
                    return $"{version.ToString()}";
            }
            catch (Exception ex)
            {
                LogError(ex, "GetVersion");
                _log.LogException(ex, "GetVersion");
            }
            return string.Empty;
        }

        /// <summary>
        /// Log Service Version
        /// </summary>
        private void LogServiceVersion()
        {
            try
            {
                _log.LogInfo("---------------------------------------------------------------------------------------------------------------");

                string version = GetVersion();
                _reportLines.Add(new ReportLine { Message = $"** 'bLoyal Dinerware backoffice Connector' Service Version = {version}" });
                _log.LogInfo($"** 'bLoyal Dinerware Backoffice Connector' Service Version = {version}");

                _reportLines.Add(new ReportLine { Message = $"** bLoyal Custom Domain URL = {DOMAIN_URL}" });
                _log.LogInfo($"** bLoyal Custom Domain URL = {DOMAIN_URL}");

                _reportLines.Add(new ReportLine { Message = $"** bLoyal Connectors Nuget Package Version = {_bLoyalConnectors_NugetPackageVersion}" });
                _log.LogInfo($"** 'bLoyal.Connectors' Nuget Package Version = {_bLoyalConnectors_NugetPackageVersion}");

                _reportLines.Add(new ReportLine { Message = $"** bLoyal Grid Service Nuget Package Version = {_bLoyal_Grid_NugetPackageVersion}" });
                _log.LogInfo($"** 'bLoyal.Connectors.Grid' Nuget Package Version = {_bLoyal_Grid_NugetPackageVersion}");

                _log.LogInfo("---------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                LogError(ex, "LogServiceVersion");
                _log.LogException(ex, "LogServiceVersion");
            }
        }

        #endregion
    }
}
