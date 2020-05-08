using bLoyal.Connectors;
using bLoyal.Connectors.LoyaltyEngine;
using ConfigApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ConfigApp.Provider
{
    public class LoyaltyEngineServices
    {

        #region Constatns

        private const string LOGIN_DOMAIN = "LoginDomain";
        private const string AACCESS_KEY = "AaccessKey";

        #endregion


        #region private Member

        ConfigurationHelper conFigHelper = new ConfigurationHelper();
        LoggerHelper logger = new LoggerHelper();
        bLoyal.Connectors.ServiceUrls _serviceUrls = null;

        //private readonly string _loginDomain = "ApiDemo";
        //private readonly string _accessKey = "a266ba78180ea6f0839a6317bd2f902fb494edaae6f53c566126eb7d27844f3fddcc67ce";

        //private readonly string _loginDomain = "EVTTest";
        //private readonly string _accessKey = "cb93cada5bc3331bf96caf5e961c58ec4813535abe6e4f49e082b7c68a860805d09a2590";

        //Aloha deviceCode and storeCode
        //private readonly string deviceCode = "000000000000994-07";
        //private readonly string storeCode = "000000000000994";

        private LoyaltyService service;

        #endregion

        #region API Exception Properties

        public APIExceptions apiException { get; set; }

        #endregion

        #region Constructor

        public LoyaltyEngineServices()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SetupResolver();
            //service = new LoyaltyService(conFigHelper.LOGIN_DOMAIN, conFigHelper.ACCESS_KEY);
            GetServiceURL(conFigHelper.LOGIN_DOMAIN);
            if (_serviceUrls != null)
                service = new LoyaltyService(_serviceUrls.LoyaltyEngineApiUrl, conFigHelper.ACCESS_KEY);
        }

        public bLoyal.Connectors.ServiceUrls GetServiceURL(string loginDomain)
        {
            try
            {
                if (_serviceUrls == null)
                {
                    var getServiceTask = Task.Run(async () => { _serviceUrls = await bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(conFigHelper.LOGIN_DOMAIN, conFigHelper.DOMAIN_URL); });
                    getServiceTask.Wait();
                }
                return _serviceUrls;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private static void SetupResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }


        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
                return typeof(JsonSerializer).Assembly;
            return null;
        }

        #endregion

        /// <summary>
        /// Get a DeviceUsageId so we can use it to call a POS Snippet
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetSessionAsync()
        {
            try
            {
                var session = await service.SystemResource.GetSessionAsync();
                return session.Key.ToString();
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetSessionAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetDeviceSessionKey Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// Create a CartUid
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateCartUid()
        {
            try
            {
                var cart = new Cart { Customer = new Customer(), Lines = new List<CartLine>() };
                var request = new CalculateCartCommand()
                {
                    Cart = cart,
                    CouponCodes = new List<string>()
                };
                var calculatedCart = await CalculateCart(request);
                if (calculatedCart != null && calculatedCart.Cart != null)
                    return calculatedCart.Cart.Uid.ToString();
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CreateCartUid Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// Create a CartUid
        /// </summary>
        /// <returns></returns>
        public string CreateCart()
        {
            try
            {
                var cart = new Cart { Customer = new Customer(), Lines = new List<CartLine>() };
                var request = new CalculateCartCommand()
                {
                    Cart = cart,
                    CouponCodes = new List<string>()
                };
                var calculatedCart = CalculateCartAsync(request);
                if (calculatedCart != null && calculatedCart.Cart != null)
                    return calculatedCart.Cart.Uid.ToString();
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CreateCartUid Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// CreateCartUid
        /// </summary>
        /// <param name="cartUid"></param>
        /// <returns></returns>
        public CalculatedCart CreateCartbySourceExternalId(string CartSourceExternalId)
        {
            try
            {
                var cart = new Cart { Customer = new Customer(), SourceExternalId = CartSourceExternalId, Lines = new List<CartLine>() };
                var request = new CalculateCartCommand()
                {
                    Cart = cart,
                    CouponCodes = new List<string>()
                };
                return CalculateCartAsync(request);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CreateCartbySourceExternalId Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
            return null;
        }

        /// <summary>
        /// Get Cart By SourceExternalId
        /// </summary>
        /// <param name="cartSourceExternalId"></param>
        /// <returns></returns>
        public CalculatedCart GetBySourceExternalIdAsync(string cartSourceExternalId)
        {
            try
            {
                return AsyncHelpers.RunSync<CalculatedCart>(() => service.CartResource.GetBySourceExternalIdAsync(cartSourceExternalId));
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetBySourceExternalIdAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetBySourceExternalIdAsync Exception Error= " + ex.Message);              
              
                return null;
            }
        }

        ///// <summary>
        ///// CreateCartUid
        ///// </summary>
        ///// <param name="cartUid"></param>
        ///// <returns></returns>
        //public async Task<CalculatedCart> CreateCartbySourceExternalId(string CartSourceExternalId)  
        //{
        //    try
        //    {
        //        var cart = new Cart { Customer = new Customer(), SourceExternalId = CartSourceExternalId, Lines = new List<CartLine>() };
        //        var request = new CalculateCartCommand()
        //        {
        //            Cart = cart,
        //            CouponCodes = new List<string>()
        //        };
        //        return await CalculateCart(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CreateCartbySourceExternalId Exception Error= " + ex.Message);
        //        if (ex.InnerException != null)
        //            logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
        //    }
        //    return null;
        //}

        public void DinerwareLogging(string msg)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("D:\\DinerwareErrorLogging.txt", true))
                {
                    writer.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Get Cart By SourceExternalId
        /// </summary>
        /// <param name="cartSourceExternalId"></param>
        /// <returns></returns>
        public async Task<CalculatedCart> GetCartBySourceExternalId(string cartSourceExternalId)
        {
            try
            {
                return await service.CartResource.GetBySourceExternalIdAsync(cartSourceExternalId);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetBySourceExternalIdAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetCartBySourceExternalId Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Customer By ExternalId
        /// </summary>
        /// <param name="cartExternalId"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerByExternalIdAsync(string cartExternalId)
        {
            try
            {
                return await service.CartResource.GetCustomerByExternalIdAsync(cartExternalId);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetCustomerByExternalIdAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetCustomerByExternalIdAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Customer By SourceExternalId
        /// </summary>
        /// <param name="cartSourceExternalId"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerBySourceExternalIdAsync(string cartSourceExternalId)
        {
            try
            {
                return await service.CartResource.GetCustomerBySourceExternalIdAsync(cartSourceExternalId);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetCustomerBySourceExternalIdAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetCustomerBySourceExternalId Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// Set Customer By ExternalId
        /// </summary>
        /// <param name="cartExternalId"></param>
        /// <param name="customer"></param>
        public async void SetCustomerByExternalId(string cartExternalId, Customer customer)
        {
            try
            {
                await service.CartResource.SetCustomerByExternalIdAsync(cartExternalId, customer);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: SetCustomerByExternalId Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Set Customer By SourceExternalId
        /// </summary>
        /// <param name="cartSourceExternalId"></param>
        /// <param name="customer"></param>
        public async void SetCustomerBySourceExternalId(string cartSourceExternalId, Customer customer)
        {
            try
            {
                await service.CartResource.SetCustomerBySourceExternalIdAsync(cartSourceExternalId, customer);
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: SetCustomerBySourceExternalId Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// CalculateCart
        /// </summary>
        /// <param name="calculateCartCommand"></param>
        /// <returns></returns>
        public async Task<CalculatedCart> CalculateCart(CalculateCartCommand calculateCartCommand)
        {
            try
            {
                return await service.CartResource.CalculateAsync(calculateCartCommand);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "CalculateAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CalculateCart Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }


        /// <summary>
        /// ApproveCartAsync
        /// </summary>
        /// <param name="approveCommand"></param>
        /// <returns></returns>
        public CartApproval ApproveCartAsync(ApproveCartCommand approveCommand)
        {
            try
            {
                return AsyncHelpers.RunSync<CartApproval>(() => service.CartResource.ApproveAsync(approveCommand));
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "ApproveAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: ApproveCartAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// CalculateAsync
        /// </summary>
        /// <param name="calculateCartCommand"></param>
        /// <returns></returns>
        public CalculatedCart CalculateCartAsync(CalculateCartCommand calculateCartCommand)
        {
            try
            {
                return AsyncHelpers.RunSync<CalculatedCart>(() => service.CartResource.CalculateAsync(calculateCartCommand));
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "CalculateAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CalculateAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        ///// <summary>
        ///// CalculateAsync
        ///// </summary>
        ///// <param name="calculateCartCommand"></param>
        ///// <returns></returns>
        //public CalculatedCart CalculateAsync(CalculateCartCommand calculateCartCommand)
        //{
        //    try
        //    {
        //        return AsyncHelpers.RunSync<CalculatedCart>(() => service.CartResource.CalculateAsync(calculateCartCommand));
        //    }
        //    catch (ApiException ex)
        //    {
        //        apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "CalculateAsync" };
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CalculateAsync Exception Error= " + ex.Message);
        //        if (ex.InnerException != null)
        //            logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// ApproveCartAsync
        ///// </summary>
        ///// <param name="approveCommand"></param>
        ///// <returns></returns>
        //public CartApproval ApproveCartAsync(ApproveCartCommand approveCommand)
        //{
        //    try
        //    {
        //        return AsyncHelpers.RunSync<CartApproval>(() => service.CartResource.ApproveAsync(approveCommand));
        //    }
        //    catch (ApiException ex)
        //    {
        //        apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "ApproveAsync" };
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.WriteLogError("**** LoyaltyEngineServices FAILURE: ApproveCartAsync Exception Error= " + ex.Message);
        //        if (ex.InnerException != null)
        //            logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
        //        return null;
        //    }
        //}

        /// <summary>
        /// GetCart
        /// </summary>
        /// <param name="cartUid"></param>
        /// <returns></returns>
        public async Task<CalculatedCart> GetCart(Guid cartUid)
        {
            try
            {
                return await service.CartResource.GetAsync(cartUid);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetCart Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// SetCartToCustomer
        /// </summary>
        /// <param name="cartUid"></param>
        /// <param name="customer"></param>
        public async void SetCartToCustomer(Guid cartUid, Customer customer)
        {
            try
            {
                await service.CartResource.SetCustomerAsync(cartUid, customer);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAsync" };
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: SetCartToCustomer Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// ApproveCart
        /// </summary>
        /// <param name="approveCommand"></param>
        /// <returns></returns>
        public async Task<CartApproval> ApproveCart(ApproveCartCommand approveCommand)
        {
            try
            {
                return await service.CartResource.ApproveAsync(approveCommand);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "ApproveAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: ApproveCart Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// CommitAsync
        /// </summary>
        /// <param name="commitCommand"></param>
        /// <returns></returns>
        public async Task<CartCommitment> CommitAsync(CommitCartCommand commitCommand)
        {
            try
            {
                return await service.CartResource.CommitAsync(commitCommand);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "ApproveAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: CommitAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// ApplyCouponAsync
        /// </summary>
        /// <param name="cartUid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Coupon> ApplyCouponAsync(Guid cartUid, string code)
        {
            try
            {
                return await service.CartResource.ApplyCouponAsync(cartUid, code);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAppliedCouponsAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: ApplyCouponAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// GetAppliedCouponsAsync
        /// </summary>
        /// <param name="cartUid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Coupon>> GetAppliedCouponsAsync(Guid cartUid)
        {
            try
            {
                return await service.CartResource.GetAppliedCouponsAsync(cartUid);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAppliedCouponsAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetAppliedCouponsAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Available Coupons
        /// </summary>
        /// <param name="cartUid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Coupon>> GetAvailableCouponsAsync(Guid cartUid)
        {
            try
            {
                return await service.CartResource.GetAvailableCouponsAsync(cartUid);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAvailableCouponsAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetAvailableCouponsAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// QuickSearch
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Customer>> QuickSearch(string searchCriteria)
        {
            try
            {
                return await service.CustomerResource.QuickSearchAsync(searchCriteria);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "QuickSearchAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: QuickSearch Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// AccrueLoyalty
        /// </summary>
        /// <param name="accrual"></param>
        /// <returns></returns>
        public async Task<LoyaltyPointsTransaction> AccrueLoyalty(AccrueLoyaltyPointsCommand accrual)
        {
            try
            {
                return await service.CustomerResource.AccrueLoyaltyAsync(accrual);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "AccrueLoyaltyAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: AccrueLoyalty Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }

        }

        /// <summary>
        /// LoyaltyPointAdjustment
        /// </summary>
        /// <param name="adjustment"></param>
        /// <returns></returns>
        public async Task<LoyaltyPointsTransaction> LoyaltyPointAdjustment(AdjustLoyaltyPointsCommand adjustment)
        {
            try
            {
                return await service.CustomerResource.AdjustLoyaltyAsync(adjustment);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "AdjustLoyaltyAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: LoyaltyPointAdjustment Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }

        }

        /// <summary>
        /// ClubActivation
        /// </summary>
        /// <param name="joinClub"></param>
        /// <returns></returns>
        public async Task<CommandResponse> ClubActivation(JoinClubCommand joinClub)
        {
            try
            {

                return await service.CustomerResource.JoinClubAsync(joinClub);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "JoinClubAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: ClubActivation Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }

        }

        /// <summary>
        /// DeactiveClub
        /// </summary>
        /// <param name="leaveClub"></param>
        /// <returns></returns>
        public async Task<CommandResponse> DeactiveClub(ExpireClubMembershipCommand leaveClub)
        {
            try
            {
                return await service.CustomerResource.ExpireClubMembershipAsync(leaveClub);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "ExpireClubMembershipAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: DeactiveClub Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }

        }

        /// <summary>
        /// SaveCustomer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<CommandResponse<Customer>> SaveCustomer(SaveCustomerCommand customer)
        {
            try
            {
                return await service.CustomerResource.SaveAsync(customer);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "SaveAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: SaveCustomer Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }

        }

        /// <summary>
        /// GetCustomerbyId
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerbyId(Guid customerUid)
        {
            try
            {
                return await service.CustomerResource.GetAsync(customerUid);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetCustomerbyId Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// GetCustomer
        /// </summary>
        /// <param name="cartUid"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomer(Guid cartUid)
        {
            try
            {
                return await service.CartResource.GetCustomerAsync(cartUid);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetCustomer Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// ResolveCustomer
        /// </summary>
        /// <param name="searchCustomer"></param>
        /// <returns></returns>
        public async Task<Customer> ResolveCustomer(Customer searchCustomer)
        {
            try
            {
                return await service.CustomerResource.ResolveAsync(searchCustomer);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "ResolveAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: ResolveCustomer Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// GetLoyaltyPointsAdjustment
        /// </summary>
        /// <param name="adjustment"></param>
        /// <returns></returns>
        public async Task<LoyaltyPointsTransaction> GetLoyaltyPointsAdjustment(AdjustLoyaltyPointsCommand adjustment)
        {
            try
            {
                return await service.CustomerResource.AdjustLoyaltyAsync(adjustment);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "AdjustLoyaltyAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetLoyaltyPointsAdjustment Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// LoyaltyPointRedemption
        /// </summary>
        /// <param name="loyaltyPointsRedemption"></param>
        /// <returns></returns>
        public async Task<LoyaltyPointsTransaction> LoyaltyPointRedemption(RedeemLoyaltyPointsCommand loyaltyPointsRedemption)
        {
            try
            {
                return await service.CustomerResource.RedeemLoyaltyAsync(loyaltyPointsRedemption);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "RedeemLoyaltyAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: LoyaltyPointRedemption Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// GetDeviceProfileAsync
        /// </summary>
        /// <returns></returns>
        public async Task<DeviceProfile> GetDeviceProfileAsync()
        {
            try
            {
                return await service.SystemResource.GetDeviceProfileAsync();
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetDeviceProfileAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: GetDeviceProfileAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

        /// <summary>
        /// ReverseCommandAsync
        /// </summary>
        /// <param name="reverseCommand"></param>
        /// <returns></returns>
        public async Task<CommandResponse> ReverseCommandAsync(ReverseCommand reverseCommand)
        {
            try
            {
                return await service.CommandResource.ReverseCommandAsync(reverseCommand);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "ReverseCommandAsync" };
                return null;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("**** LoyaltyEngineServices FAILURE: ReverseCommandAsync Exception Error= " + ex.Message);
                if (ex.InnerException != null)
                    logger.WriteLogError("**** InnerException  = " + ex.InnerException.Message);
                return null;
            }
        }

    }

    public class APIExceptions
    {
        public string ErrorCode = "";
        public string ErrorDescription = "";
        public string ErrorApi = "";
    }
}
