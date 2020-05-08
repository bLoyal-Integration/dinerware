using bLoyal.Connectors.LoyaltyEngine;
using bLoyal.Utilities;
using DinerwareSystem.DinerwareEngineService;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinerwareSystem.Helpers
{
    public class CalculateTransactionHelper
    {
        #region Properties

        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        DinerwareProvider _dinerwareProvider = new DinerwareProvider();
        PaymentEngineConnector _paymentEngine = null;
        LoggerHelper _logger = LoggerHelper.Instance;
        ConfigurationHelper _configurationHelper = ConfigurationHelper.Instance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Call CommitCart to bLoyal 
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public async Task CommitCartAsync(int ticketId, Dinerware.Ticket ticket = null)
        {
            try
            {
                if (ticketId > 0 && TicketDictionary.CartDictionary != null && TicketDictionary.CartDictionary.ContainsKey(ticketId) && TicketDictionary.CartDictionary[ticketId] != Guid.Empty)
                {
                    IList<CartPayment> payments = new List<CartPayment>();

                    GetCartPayments(ticketId, ticket, payments);

                    var commitCommand = new CommitCartCommand { CartUid = TicketDictionary.CartDictionary[ticketId], CartSourceExternalId = ticketId.ToString(), ReferenceNumber = ticketId.ToString(), Payments = payments };

                    var cartCommitment = await _services.CommitAsync(commitCommand);

                    TicketDictionary.Dictionary.Remove(ticketId);
                    TicketDictionary.CartDictionary.Remove(ticketId);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction CommitCartAsync");
            }
        }


        /// <summary>
        /// Get bLoyal Payments
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="ticket"></param>
        /// <param name="payments"></param>
        private void GetCartPayments(int ticketId, Dinerware.Ticket ticket, IList<CartPayment> payments)
        {
            try
            {
                var ticketTransactions = _dinerwareProvider.GetTransForTicket(ticketId, true);
                if (ticketTransactions != null)
                {
                    foreach (var payment in ticketTransactions)
                    {
                        if (!payment.IsCanceled)
                        {
                            string tenderCode = string.Empty;
                            if (payment.Type.Equals(_configurationHelper.DW_GIFTCARD_TENDER_NAME, StringComparison.CurrentCultureIgnoreCase))
                                tenderCode = _configurationHelper.GIFTCARD_TENDER_CODE;
                            else if (payment.Type.Equals(_configurationHelper.DW_LOYALTY_TENDER_NAME, StringComparison.CurrentCultureIgnoreCase))
                                tenderCode = _configurationHelper.LOYALTY_TENDER_CODE;
                            payments.Add(new CartPayment { Amount = payment.NetAmount, TransactionCode = payment.AuthNum, TenderCode = !string.IsNullOrWhiteSpace(tenderCode) ? tenderCode : payment.Type });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction GetCartPayments");
            }
        }

        /// <summary>
        /// Get Load GiftCard Balance
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="giftCardNumber"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public List<ApplyGiftCardSummary> ApplyLoadGiftCardBalance(int ticketId, int currentUserId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_configurationHelper.GIFTCARD_TENDER_CODE) && _configurationHelper.LOAD_GIFTCARD_ITEMS != null && _configurationHelper.LOAD_GIFTCARD_ITEMS.Any())
                {
                    bool isCalculateCartUpdate = false;
                    var applyGiftCardSummary = new List<ApplyGiftCardSummary>();
                    var calculatedCart = AsyncHelper.RunSync(() => _services.GetCartBySourceExternalIdAsync(ticketId.ToString()));

                    if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Lines != null)
                    {
                        foreach (var cartLine in calculatedCart.Cart.Lines)
                        {
                            decimal giftCardAmount = cartLine.GiftCardAmount.HasValue ? cartLine.GiftCardAmount.Value : 0;
                            if (!string.IsNullOrWhiteSpace(cartLine.GiftCardNumber) && giftCardAmount == 0)
                            {
                                decimal loadGiftCardAmount = (cartLine.Price * cartLine.Quantity) - (cartLine.Discount > 0 ? cartLine.Discount * cartLine.Quantity : 0);

                                _paymentEngine = new PaymentEngineConnector(_configurationHelper.LOGIN_DOMAIN, _configurationHelper.ACCESS_KEY, string.Empty, string.Empty, null);

                                var cardResponse = _paymentEngine.GetCardBalance(cartLine.GiftCardNumber, _configurationHelper.GIFTCARD_TENDER_CODE);
                                if (cardResponse != null)
                                {
                                    if (cardResponse.Status == PaymentEngine.CardRequestStatus.Approved && loadGiftCardAmount > 0)
                                    {
                                        string transCode = _paymentEngine.CardCredit(cartLine.GiftCardNumber, _configurationHelper.GIFTCARD_TENDER_CODE, ticketId.ToString(), loadGiftCardAmount);
                                        UpdateCart(loadGiftCardAmount, cartLine, calculatedCart, transCode);
                                        isCalculateCartUpdate = true;
                                        applyGiftCardSummary.Add(new ApplyGiftCardSummary { GiftCardNumber = cartLine.GiftCardNumber, IsNewCard = cardResponse.AvailableBalance > 0 ? false : true, NetAmount = loadGiftCardAmount, IsProvision = !string.IsNullOrWhiteSpace(cardResponse.Message) && cardResponse.Message.StartsWith("Gift card is not provisioned - number is auto provisioned enabled") });
                                    }
                                }

                            }
                        }

                        if (calculatedCart != null && isCalculateCartUpdate)
                            return UpdateCalculateCart(applyGiftCardSummary, calculatedCart);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction ApplyLoadGiftCardBalance");
            }
            return null;
        }

        /// <summary>
        /// Credit Load GiftCard Item
        /// </summary>
        /// <param name="calculatedCart"></param>
        /// <param name="amount"></param>
        /// <param name="giftCardNumber"></param>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public bool CreditLoadGiftCardItem(CalculatedCart calculatedCart, decimal amount, string giftCardNumber, string ticketId, CartLine line)
        {
            bool result = false;
            try
            {
                _paymentEngine = new PaymentEngineConnector(_configurationHelper.LOGIN_DOMAIN, _configurationHelper.ACCESS_KEY, string.Empty, string.Empty, null);

                string transCode = _paymentEngine.CardRedeem(giftCardNumber, _configurationHelper.GIFTCARD_TENDER_CODE, ticketId, amount);

                result = !string.IsNullOrWhiteSpace(transCode);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction CreditLoadGiftCardItem");
            }
            return result;
        }


        /// <summary>
        /// Update CalculateCart
        /// </summary>
        /// <param name="applyGiftCardSummary"></param>
        /// <param name="calculatedCart"></param>
        /// <returns></returns>
        private List<ApplyGiftCardSummary> UpdateCalculateCart(List<ApplyGiftCardSummary> applyGiftCardSummary, CalculatedCart calculatedCart)
        {
            try
            {
                var request = new CalculateCartCommand()
                {
                    Cart = calculatedCart.Cart,
                    Uid = calculatedCart.Cart.Uid
                };

                _services.CalculateCart(request);

                return applyGiftCardSummary;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction UpdateCalculateCart");
                return null;
            }
        }

        /// <summary>
        /// Refund bLoyal Tenders
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<bool> RefundbLoyalTendersAsync(int ticketId, decimal amount)
        {
            bool isCreditbLoyalTendersError = false;

            try
            {
                var ticketTransactions = _dinerwareProvider.GetTransForTicket(ticketId, true); // Updated tenders information not getting by cache - So we need to call Dinerware API

                bool refundPayment = true;

                if (ticketTransactions != null && ticketTransactions.Any())
                {
                    foreach (var payment in ticketTransactions)
                    {
                        if (refundPayment)
                        {
                            if ((payment.Type.Equals(_configurationHelper.DW_GIFTCARD_TENDER_NAME, StringComparison.CurrentCultureIgnoreCase)
                                || (payment.Type.Equals(_configurationHelper.DW_LOYALTY_TENDER_NAME, StringComparison.CurrentCultureIgnoreCase))
                                && payment.IsCanceled && amount == payment.NetAmount))
                            {
                                _paymentEngine = new PaymentEngineConnector(_configurationHelper.LOGIN_DOMAIN, _configurationHelper.ACCESS_KEY, string.Empty, string.Empty, null);

                                refundPayment = await PaymentRefundAsync(ticketId, amount, refundPayment, payment).ConfigureAwait(true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isCreditbLoyalTendersError = true;
                _logger.WriteLogError(ex, "Refund bLoyal Tenders - CreditbLoyalTenders");
            }
            return isCreditbLoyalTendersError;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Payment Refund
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="amount"></param>
        /// <param name="refundPayment"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        private async Task<bool> PaymentRefundAsync(int ticketId, decimal amount, bool refundPayment, Dinerware.Payment payment)
        {
            try
            {
                var cart = await _services.GetCartBySourceExternalIdAsync(ticketId.ToString()).ConfigureAwait(true);

                if (cart != null && cart.Cart != null && cart.Cart.Payments != null && cart.Cart.Payments.Any())
                {
                    var paymentRef = cart.Cart.Payments.ToList().Find(t => t.Amount.Equals(amount) && t.TransactionCode.Equals(payment.AuthNum));
                    if (paymentRef != null && !string.IsNullOrWhiteSpace(paymentRef.TransactionCode))
                    {
                        if (refundPayment)
                        {
                            if (!string.IsNullOrWhiteSpace(_paymentEngine.CardRefund(paymentRef.TransactionCode, ticketId.ToString(), amount)))
                            {
                                cart.Cart.Payments.Remove(paymentRef); //Suppose - if we have two payment method with same amount, we need to remove Refunded payment. - because it always find first one
                                var request = new CalculateCartCommand()
                                {
                                    Cart = cart.Cart
                                };

                                _services.CalculateCart(request);
                            }
                        }
                        refundPayment = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "Refund bLoyal Tenders - PaymentRefund");
            }
            return refundPayment;
        }

        /// <summary>
        /// Check Load Gift Card
        /// </summary>
        /// <param name="giftCardNumber"></param>
        /// <returns></returns>
        public string CheckLoadGiftCard(string giftCardNumber)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_configurationHelper.GIFTCARD_TENDER_CODE) && !string.IsNullOrWhiteSpace(_configurationHelper.GIFTCARD_SKU))
                {
                    _paymentEngine = new PaymentEngineConnector(_configurationHelper.LOGIN_DOMAIN, _configurationHelper.ACCESS_KEY, string.Empty, string.Empty, null);

                    var cardResponse = _paymentEngine.GetCardBalance(giftCardNumber, _configurationHelper.GIFTCARD_TENDER_CODE);
                    if (cardResponse != null)
                    {
                        if (cardResponse.AvailableBalance > 0)
                        {
                            var msg = new frmGiftCardExistsMsg(giftCardNumber);
                            msg.ShowDialog();
                        }

                        if (cardResponse.Status == PaymentEngine.CardRequestStatus.Approved && !string.IsNullOrWhiteSpace(cardResponse.Message) && (cardResponse.Message.StartsWith("Current balance is") || cardResponse.Message.StartsWith("Gift card is not provisioned - number is auto provisioned enabled")))
                            return string.Empty;
                        else if (cardResponse.Status == PaymentEngine.CardRequestStatus.Declined)
                            return cardResponse.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction CheckLoadGiftCard");
            }
            return string.Empty;
        }

        /// <summary>
        /// return CalculateCartCommand obj
        /// </summary>
        /// <param name="cartExternalId"></param>
        /// <param name="cart"></param>
        /// <param name="objwsMenuItem"></param>
        /// <returns></returns>
        private CalculateCartCommand CalculateCart(string cartExternalId, CalculatedCart cart, wsMenuItem[] objwsMenuItem)
        {
            CalculateCartCommand calculateCartCommand = null;
            try
            {
                var calculateCart = SalesTransactionLines(cartExternalId, objwsMenuItem, cart);

                if (calculateCart != null)
                {
                    calculateCartCommand = new CalculateCartCommand
                    {
                        Cart = calculateCart.Cart,
                        ReferenceNumber = calculateCart.Cart.Uid.ToString(),
                        Uid = Guid.NewGuid()
                    };
                }

                return calculateCartCommand;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction CalculateCart");
                return null;
            }
        }

        /// <summary>
        /// Create Sales Transaction Lines
        /// </summary>
        /// <param name="TransactionToken"></param>
        /// <param name="wsMenuItemList"></param>
        /// <returns></returns>
        private CalculatedCart SalesTransactionLines(string cartExternalId, wsMenuItem[] wsMenuItemList, CalculatedCart cart)
        {
            try
            {
                if (cart != null && wsMenuItemList != null && wsMenuItemList.Length > 0)
                {
                    var cartLines = new List<CartLine>();

                    foreach (var menuItem in wsMenuItemList)
                    {
                        decimal dwItemDiscount = GetDinerwareItemDiscount(menuItem);
                        var line = new CartLine
                        {
                            Price = menuItem.GROSS_PRICE,
                            ExternallyAppliedDiscount = dwItemDiscount > 0 ? true : false,
                            Discount = dwItemDiscount,
                            NetPrice = menuItem.NET_PRICE,
                            ProductCode = menuItem.DESCRIPTION,
                            ProductName = menuItem.NAME,
                            Quantity = menuItem.Quantity,
                            PriceSource = PriceSource.Base,
                            ProductExternalId = menuItem.ID,
                            ExternalId = menuItem.TICKETITEM_ID.ToString()
                        };
                        cartLines.Add(line);
                    }
                    cart.Cart.Lines = cartLines;

                    decimal dwTicketDiscount = GetDinerwareTicketDiscount(wsMenuItemList);
                    if (dwTicketDiscount != 0)
                    {
                        cart.Cart.ExternallyAppliedDiscount = true;
                        cart.Cart.Discount = dwTicketDiscount;
                    }
                }
                return cart;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction SalesTransactionLines");
                throw ex;
            }
        }

        /// <summary>
        /// Get Dinerware Item Discount
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        private decimal GetDinerwareItemDiscount(wsMenuItem menuItem)
        {
            decimal dwItemDiscount = 0;

            try
            {
                if (menuItem.DISCOUNTS != null && menuItem.DISCOUNTS.Length > 0)
                {
                    foreach (var itemDiscount in menuItem.DISCOUNTS)
                    {
                        if (itemDiscount.APPLIES_TO_ITEMS)
                            dwItemDiscount += itemDiscount.VALUE;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction GetDinerwareItemDiscount");
            }
            return dwItemDiscount;
        }

        /// <summary>
        /// Get Dinerware Ticket Discount
        /// </summary>
        /// <param name="wsMenuItemList"></param>
        /// <returns></returns>
        private decimal GetDinerwareTicketDiscount(wsMenuItem[] wsMenuItemList)
        {
            decimal dwTicketDiscount = 0;
            try
            {
                if (wsMenuItemList != null && wsMenuItemList.Length > 0)
                {
                    foreach (var menuItem in wsMenuItemList)
                    {
                        if (menuItem.TICKET_DISCOUNTS != null && menuItem.TICKET_DISCOUNTS.Length > 0)
                        {
                            foreach (var ticketDiscount in menuItem.TICKET_DISCOUNTS)
                            {
                                if (ticketDiscount.APPLIES_TO_TICKETS)
                                    dwTicketDiscount += ticketDiscount.VALUE;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction GetDinerwareTicketDiscount");
            }
            return dwTicketDiscount;
        }

        /// <summary>
        /// Update Cart
        /// </summary>
        /// <param name="loadGiftCardAmount"></param>
        /// <param name="paidAmount"></param>
        /// <param name="loadgiftCardLine"></param>
        /// <param name="calculatedCart"></param>
        /// <param name="transCode"></param>
        private void UpdateCart(decimal loadGiftCardAmount, CartLine loadgiftCardLine, CalculatedCart calculatedCart, string transCode)
        {
            try
            {
                loadgiftCardLine.GiftCardAmount = loadGiftCardAmount;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateTransaction UpdateCart");
            }
        }

        #endregion
    }
}
