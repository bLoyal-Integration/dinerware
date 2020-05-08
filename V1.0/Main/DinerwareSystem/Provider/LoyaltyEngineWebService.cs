using bLoyal.Connectors;
using bLoyal.Connectors.LoyaltyEngine;
using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DinerwareSystem.Provider
{
    public class LoyaltyEngineServices : SnippetConfiguration
    {  

        #region private Member

        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;
        LoggerHelper _logger = LoggerHelper.Instance;

        private LoyaltyService _service;

        #endregion

        #region API Exception Properties

        public APIExceptions apiException { get; set; }

        #endregion

        #region Constructor

        public LoyaltyEngineServices()
        {
            SetupResolver();

            ServiceURLHelper.GetServiceURL();

            if (ServiceURLHelper.Service_Urls != null)
            {
                _service = new LoyaltyService(ServiceURLHelper.Service_Urls.LoyaltyEngineApiUrl, _conFigHelper.ACCESS_KEY);
                //_service = new LoyaltyService("http://localhost:49653", _conFigHelper.ACCESS_KEY);

                SNIPPET_Config_URL = !string.IsNullOrWhiteSpace(_conFigHelper.SNIPPET_URL) ? _conFigHelper.SNIPPET_URL : ServiceURLHelper.Service_Urls.POSSnippetsUrl;
            }
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

        #region Public Methods

        /// <summary>
        /// Get a DeviceUsageId so we can use it to call a POS Snippet
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetSessionAsync()
        {
            try
            {
                var session = await _service.SystemResource.GetSessionAsync().ConfigureAwait(true);

                return session.Key;
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "GetSessionAsync" };

                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetSessionAsync");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetSessionAsync");
                return null;
            }
        }

        /// <summary>
        /// Create a CartUid
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateCartForNewTicket()
        {
            try
            {
                var cart = new Cart { Customer = new Customer(), Lines = new List<CartLine>() };
                var request = new CalculateCartCommand()
                {
                    Cart = cart,
                    CouponCodes = new List<string>()
                };

                var calculatedCart = await CalculateCartAsync(request);

                if (calculatedCart != null && calculatedCart.Cart != null)
                    return calculatedCart.Cart.Uid.ToString();
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "CalculateAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices CreateCartUid");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices CreateCartUid");
            }
            return string.Empty;
        }      
     
        /// <summary>
        /// Get Cart By SourceExternalId
        /// </summary>
        /// <param name="cartSourceExternalId"></param>
        /// <returns></returns>
        public async Task<CalculatedCart> GetCartBySourceExternalIdAsync(string cartSourceExternalId)
        {
            try
            {
                CalculatedCart cart = null;

                cart = await _service.CartResource.GetBySourceExternalIdAsync(cartSourceExternalId).ConfigureAwait(true);

                return cart;
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "GetBySourceExternalIdAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetCartBySourceExternalIdAsync");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetCartBySourceExternalIdAsync");
                return null;
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
                return await _service.CartResource.GetBySourceExternalIdAsync(cartSourceExternalId).ConfigureAwait(true);
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "GetBySourceExternalIdAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetCartBySourceExternalId");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetCartBySourceExternalId");
                return null;
            }
        }   

        /// <summary>
        /// CalculateCart
        /// </summary>
        /// <param name="calculateCartCommand"></param>
        /// <returns></returns>
        public async Task<CalculatedCart> CalculateCartAsync(CalculateCartCommand calculateCartCommand)
        {
            try
            {
                return await _service.CartResource.CalculateAsync(calculateCartCommand);
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "CalculateAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices CalculateCart");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices CalculateCart");
                return null;
            }
        }

        /// <summary>
        /// ApproveCartAsync
        /// </summary>
        /// <param name="approveCommand"></param>
        /// <returns></returns>
        public async Task<CartApproval> ApproveCartAsync(ApproveCartCommand approveCommand)
        {
            try
            {
                CartApproval cartApproval = null;
                cartApproval = await _service.CartResource.ApproveAsync(approveCommand).ConfigureAwait(true);
                return cartApproval;
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "ApproveAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices ApproveCartAsync");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices ApproveCartAsync");
                return null;
            }
        }

        /// <summary>
        /// CalculateAsync
        /// </summary>
        /// <param name="calculateCartCommand"></param>
        /// <returns></returns>
        public CalculatedCart CalculateCart(CalculateCartCommand calculateCartCommand)
        {
            try
            {
                CalculatedCart cart = null;

                cart = AsyncHelper.RunSync(() => _service.CartResource.CalculateAsync(calculateCartCommand));

                return cart;
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "CalculateAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices CalculateCartAsync");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices CalculateCartAsync");
                return null;
            }
        }

        /// <summary>
        /// Add GiftCarNumber in Cart
        /// </summary>
        /// <param name="transCode"></param>
        /// <param name="applyBalance"></param>
        /// <param name="giftCardNumber"></param>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public async Task<CalculatedCart> AddGiftCardPaymentTransactionAsync(string transCode = "", decimal applyBalance = 0, string giftCardNumber = "", string ticketId = "", IList<CartLine> lines = null)
        {
            CalculatedCart calculatedCart = null;
            CalculateCartCommand request = null;

            try
            {
                calculatedCart = await GetCartBySourceExternalIdAsync(ticketId.ToString()).ConfigureAwait(true);

                var payment = new CartPayment { Amount = applyBalance, TransactionCode = transCode };
                if (calculatedCart != null && calculatedCart.Cart != null)
                {
                    if (calculatedCart.Cart.Payments != null && calculatedCart.Cart.Payments.Any())
                        calculatedCart.Cart.Payments.Add(payment);
                    else
                    {
                        calculatedCart.Cart.Payments = new List<CartPayment>();
                        calculatedCart.Cart.Payments.Add(payment);
                    }
                    request = new CalculateCartCommand() { Cart = calculatedCart.Cart, Uid = calculatedCart.Cart.Uid };
                }
                else
                {
                    var cart = new Cart { Lines = new List<CartLine>(), SourceExternalId = ticketId, Payments = new List<CartPayment>() };
                    if (lines != null && lines.Any())
                        cart.Lines = lines;
                    cart.Payments.Add(payment);
                    request = new CalculateCartCommand() { Cart = cart };
                }

                if (request != null)
                    calculatedCart = CalculateCart(request);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices AddGiftCarNumberInCart");
            }
            return calculatedCart;
        }

        /// <summary>
        /// Calculate Cart
        /// </summary>
        /// <param name="transCode"></param>
        /// <param name="applyBalance"></param>
        /// <param name="giftCardNumber"></param>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        private CalculatedCart CalculateCart(string transCode, string applyBalance, string giftCardNumber, string ticketId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(transCode))
                {
                    var cart = new Cart { Customer = new Customer(), Lines = new List<CartLine>() };
                    var request = new CalculateCartCommand()
                    {
                        Cart = cart,
                        CouponCodes = new List<string>()
                    };

                   return CalculateCart(request);
                }
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "CalculateAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices CalculateCart");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices CalculateCart");
                return null;
            }
            return null;
        }

        /// <summary>
        /// GetCart
        /// </summary>
        /// <param name="cartUid"></param>
        /// <returns></returns>
        public async Task<CalculatedCart> GetCartAsync(Guid cartUid)
        {
            try
            {
                return await _service.CartResource.GetAsync(cartUid);
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "GetAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetCart");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices GetCart");
                return null;
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
                return await _service.CartResource.ApproveAsync(approveCommand);
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "ApproveAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices ApproveCart");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices ApproveCart");
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
                return await _service.CartResource.CommitAsync(commitCommand);
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Code) && ex.Code == "ServiceRedirect")
                {
                    ServiceURLHelper.IsbLoyalServiceUrlDown = true;
                    apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = "A bLoyal error occurred. Please restart your Workstation and try again.", ErrorApi = "CommitAsync" };
                }
                _logger.WriteLogError(ex, "LoyaltyEngineServices CommitAsync");
                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoyaltyEngineServices CommitAsync");
                return null;
            }
        }
      
        #endregion
    }
}
