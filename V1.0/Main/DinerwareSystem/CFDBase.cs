using bLoyal.Connectors.LoyaltyEngine;
using bLoyal.Utilities;
using Dinerware;
using Dinerware.WorkstationInterfaces;
using DinerwareSystem.ConfigurationCache;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DinerwareSystem
{
    public class CFDBase : IAddIn, IDisplayActions, IExtendedDataConsumer
    {
        #region Private Members       

        private int _ticketId { get; set; }

        Ticket _targetTicket = null;

        CalculateTransactionHelper _calculateTransaction = new CalculateTransactionHelper();

        ConfigurationHelper _configurationHelper = ConfigurationHelper.Instance;

        LoggerHelper _loggerHelper = LoggerHelper.Instance;

        #endregion

        #region public Members       

        public Guid AddinKey => new Guid(Constants.ADDINGUID);
        public string Author => Constants.AUTHORPROPERTY;
        public string Copyright => string.Empty;
        public Guid LicenseFeature => Guid.Empty;
        public string Name => string.Empty;
        public bool OnBrain => false;
        public bool OnWorkstation => true;
        public IExtendedDataStorage ExtendedDataContext { get; set; }

        #endregion

        #region

        /// <summary>
        /// CFDBase constructor
        /// </summary>
        public CFDBase()
        {
            try
            {
                var configurationHelper = ConfigurationHelper.NewInstance;  // Update the Configuration Instance

                if (!configurationHelper.IS_Test_Virtual_Client_Connection && !configurationHelper.IS_Test_BLoyal_Connection)
                    return;

                if (configurationHelper != null && configurationHelper.ENABLE_bLOYAL)
                {
                    _configurationHelper = configurationHelper;

                    // Update bLoyal Service URL
                    ServiceURLHelper.UpdateServiceURL();

                    var dinerwareProvider = new DinerwareProvider();

                    GetTenders(dinerwareProvider);
                    GetDiscountSets(dinerwareProvider, configurationHelper);
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "CFDBase - constructor");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Update Ticket
        /// </summary>
        /// <param name="theTicket"></param>
        /// <param name="theInfo"></param>
        private void UpdateTicket(Ticket theTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(theTicket);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// AddChoice
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        /// <param name="targetChoice"></param>
        public void addChoice(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem, Choice targetChoice)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// Add new Item event
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        public void addItem(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicketAndUpdateDictionary(targetTicket);
        }

        /// <summary>
        /// Refresh Ticket And Update Ticket Dictionary
        /// </summary>
        /// <param name="targetTicket"></param>
        private void RefreshTicketAndUpdateDictionary(Ticket targetTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL)
            {
                RefreshTicket(targetTicket);
                UpdateTicketDictionary(_ticketId);
            }
        }

        /// <summary>
        /// CancelTicket - cancel button event
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        public void cancelTicket(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// Track Payment and Close Ticket Window
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="tenderAmount"></param>
        /// <param name="changeAmount"></param>
        public async void changeDue(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, decimal tenderAmount, decimal changeAmount)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL)
                {
                    RefreshTicket(targetTicket);

                    if (targetTicket != null && targetTicket.AmountDue == 0)
                    {
                        var response = _calculateTransaction.ApplyLoadGiftCardBalance(_ticketId, theInfo.userID);

                        if (response != null && response.Any())
                        {
                            foreach (var loadItem in response)
                            {
                                frmApplyGiftCardSummary gcSummary = new frmApplyGiftCardSummary(loadItem.NetAmount, loadItem.GiftCardNumber, loadItem.IsProvision, loadItem.IsNewCard);
                                gcSummary.ShowDialog();
                            }
                        }

                        if (_ticketId > 0 && TicketDictionary.Dictionary != null && TicketDictionary.Dictionary.ContainsKey(_ticketId)
                            && TicketDictionary.Dictionary[_ticketId] == Constants.CALCULATEDCURRENT)
                        {
                            var approveCartCommand = new ApproveCartCommand
                            {
                                CartUid = TicketDictionary.CartDictionary[_ticketId],
                                CartSourceExternalId = _ticketId.ToString(),
                                ReferenceNumber = _ticketId.ToString()
                            };
                            var service = new LoyaltyEngineServices();
                            var cartApproval = service.ApproveCartAsync(approveCartCommand);

                            await _calculateTransaction.CommitCartAsync(_ticketId, targetTicket);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "changeDue");
            }
        }

        /// <summary>
        /// ChangeTicket
        /// </summary>
        /// <param name="theinfo"></param>
        /// <param name="targetTicket"></param>
        public void changeTicket(IDisplayActions.displayActionInfo theinfo, Ticket targetTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// DoneWithMultipleItemAction
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        public void doneWithMultipleItemAction(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// ExitDinerware
        /// </summary>
        /// <param name="theInfo"></param>
        public void exitDinerware(IDisplayActions.displayActionInfo theInfo)
        {

        }

        /// <summary>
        /// idle
        /// </summary>
        /// <param name="theInfo"></param>
        public void idle(IDisplayActions.displayActionInfo theInfo)
        {

        }

        /// <summary>
        /// itemQuantityPrice
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        public void itemQuantityPrice(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem)
        {

        }

        /// <summary>
        /// New Transaction Track for Ticket
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetTender"></param>
        /// <param name="tenderedAmount"></param>
        /// <param name="amountDue"></param>
        /// <param name="changeDue"></param>
        public void newTransaction(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, TenderType targetTender, decimal tenderedAmount, decimal amountDue, decimal changeDue)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// OpenTicket
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        public void openTicket(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL)
                {
                    RefreshTicket(targetTicket);

                    if (_ticketId > 0)
                    {
                        if (TicketDictionary.Dictionary == null)
                            TicketDictionary.Dictionary = new Dictionary<int, string>();

                        if (!TicketDictionary.Dictionary.ContainsKey(_ticketId))
                            TicketDictionary.Dictionary.Add(_ticketId, Constants.CALCULATEDNOTCURRENT);
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "openTicket");
            }
        }

        /// <summary>
        /// RemoveChoice
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        /// <param name="targetChoice"></param>
        public void removeChoice(IDisplayActions.displayActionInfo theInfo, Dinerware.Ticket targetTicket, Dinerware.MenuItem targetItem, Dinerware.Choice targetChoice)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                RefreshTicket(targetTicket);
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "removeChoice");
            }
        }

        /// <summary>
        /// RemoveItem
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        public void removeItem(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL && targetItem != null && _configurationHelper.LOAD_GIFTCARD_ITEMS != null && _configurationHelper.LOAD_GIFTCARD_ITEMS.Any())
                {
                    RefreshTicket(targetTicket);
                    UpdateTicketDictionary(_ticketId);

                    var loadGiftCardItem = _configurationHelper.LOAD_GIFTCARD_ITEMS.Find(t => t.Equals(targetItem.ItemName, StringComparison.InvariantCultureIgnoreCase));
                    if (!string.IsNullOrWhiteSpace(loadGiftCardItem))
                    {
                        var service = new LoyaltyEngineServices();

                        var calculatedCart = AsyncHelper.RunSync(() => service.GetCartBySourceExternalIdAsync(_targetTicket.ID));

                        if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Lines != null && calculatedCart.Cart.Lines.Any())
                        {
                            var lines = calculatedCart.Cart.Lines;
                            var giftCartLine = calculatedCart.Cart.Lines.ToList().Find(t => t.ProductName.Equals(loadGiftCardItem, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(t.GiftCardNumber) && t.ProductExternalId.Equals(targetItem.TIID));
                            if (giftCartLine != null && giftCartLine.GiftCardAmount.HasValue && giftCartLine.GiftCardAmount.Value > 0)
                            {
                                if (!_calculateTransaction.CreditLoadGiftCardItem(calculatedCart, targetItem.Price, giftCartLine.GiftCardNumber, targetTicket.ID, giftCartLine))
                                {
                                    frmServerOfflineWarning offLineMsg = new frmServerOfflineWarning();
                                    offLineMsg.ShowDialog();
                                }
                                else
                                    lines.Remove(giftCartLine);
                            }

                            calculatedCart.Cart.Lines = lines;
                            var request = new CalculateCartCommand()
                            {
                                Cart = calculatedCart.Cart,
                                Uid = calculatedCart.Cart.Uid
                            };

                            service.CalculateCart(request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "removeItem");
            }
        }

        /// <summary>
        /// RemoveTransaction
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="displayField"></param>
        /// <param name="paymentAmount"></param>
        /// <param name="amountDue"></param>
        public void removeTransaction(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, string displayField, decimal paymentAmount, decimal amountDue)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL)
                {
                    RefreshTicket(targetTicket);

                    if (_ticketId != 0)
                    {
                        bool isCreditbLoyalTendersError = AsyncHelper.RunSync(() => _calculateTransaction.RefundbLoyalTendersAsync(_ticketId, paymentAmount));

                        if (isCreditbLoyalTendersError)
                        {
                            System.Windows.Forms.Form frmServiceUrlDown = null;
                            if (ServiceURLHelper.IsbLoyalServiceUrlDown)
                                frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                            else
                                frmServiceUrlDown = new frmServerOfflineWarning();

                            frmServiceUrlDown.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "removeTransaction");
            }
        }

        /// <summary>
        /// EepeatItem
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        public void repeatItem(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicketAndUpdateDictionary(targetTicket);
        }

        /// <summary>
        /// Update the ticket status in dictionary
        /// </summary>
        /// <param name="ticketId"></param>
        private void UpdateTicketDictionary(int ticketId)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (TicketDictionary.Dictionary == null)
                    TicketDictionary.Dictionary = new Dictionary<int, string>();
                string ticketStatus = string.Empty;

                if (!TicketDictionary.Dictionary.ContainsKey(ticketId))
                    TicketDictionary.Dictionary.Add(ticketId, Constants.CALCULATEDNOTCURRENT);
                else if (TicketDictionary.Dictionary.ContainsKey(ticketId))
                {
                    ticketStatus = TicketDictionary.Dictionary[ticketId];
                    TicketDictionary.Dictionary[ticketId] = ticketStatus == Constants.CALCULATEDCURRENT ? Constants.CALCULATEDNOTCURRENT : Constants.NOTCALCULATED;
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "UpdateTicketDictionary");
            }
        }

        /// <summary>
        /// SpecialRequest
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        public void specialRequest(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// RefreshTicket
        /// </summary>
        /// <param name="targetTicket"></param>
        private void RefreshTicket(Ticket targetTicket)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL)
                {
                    _targetTicket = targetTicket;
                    int ticketId = 0;
                    int.TryParse(targetTicket.ID, out ticketId);
                    _ticketId = ticketId;
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "RefreshTicket");
            }
        }

        /// <summary>
        /// AddItemDiscount
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        /// <param name="theDiscount"></param>
        public void addItemDiscount(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem, Dinerware.Discount theDiscount)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicketAndUpdateDictionary(targetTicket);
        }

        /// <summary>
        /// AddTicketDiscount
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="theDiscount"></param>
        public void addTicketDiscount(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, Dinerware.Discount theDiscount)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicketAndUpdateDictionary(targetTicket);
        }

        /// <summary>
        /// CommitTicket - Ok button event
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        public void commitTicket(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// RemoveItemDiscount
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        /// <param name="theDiscount"></param>
        public void removeItemDiscount(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, MenuItem targetItem, Dinerware.Discount theDiscount)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicketAndUpdateDictionary(targetTicket);
        }

        /// <summary>
        /// RemoveTicketDiscount
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="theDiscount"></param>
        public void removeTicketDiscount(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket, Dinerware.Discount theDiscount)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicketAndUpdateDictionary(targetTicket);
        }

        /// <summary>
        /// StartMultipleItemAction
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        public void startMultipleItemAction(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        /// <summary>
        /// Void Item
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        /// <param name="targetItem"></param>
        public void voidItem(IDisplayActions.displayActionInfo theInfo, Dinerware.Ticket targetTicket, Dinerware.MenuItem targetItem)
        {

        }

        /// <summary>
        /// Void Ticket
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        public void voidTicket(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL && targetTicket != null && targetTicket.Items != null && targetTicket.Items.Any() && _configurationHelper.LOAD_GIFTCARD_ITEMS != null && _configurationHelper.LOAD_GIFTCARD_ITEMS.Any())
                {
                    RefreshTicket(targetTicket);

                    var loadGiftCardItems = _configurationHelper.LOAD_GIFTCARD_ITEMS.Where(p => targetTicket.Items.Any(p2 => p2.ItemName.Equals(p, StringComparison.InvariantCultureIgnoreCase)));
                    if (loadGiftCardItems != null && loadGiftCardItems.Any())
                    {
                        var service = new LoyaltyEngineServices();
                        bool isCreditLoadGiftCard = false;

                        var calculatedCart = AsyncHelper.RunSync(() => service.GetCartBySourceExternalIdAsync(_targetTicket.ID));


                        if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Lines != null)
                        {
                            var lines = new List<CartLine>(calculatedCart.Cart.Lines.ToList());

                            foreach (var giftCartLine in calculatedCart.Cart.Lines)
                            {
                                if (giftCartLine.GiftCardAmount.HasValue && giftCartLine.GiftCardAmount.Value > 0)
                                {
                                    if (!_calculateTransaction.CreditLoadGiftCardItem(calculatedCart, giftCartLine.GiftCardAmount.Value, giftCartLine.GiftCardNumber, targetTicket.ID, giftCartLine))
                                    {
                                        frmServerOfflineWarning offLineMsg = new frmServerOfflineWarning();
                                        offLineMsg.ShowDialog();
                                    }
                                    else
                                    {
                                        isCreditLoadGiftCard = true;
                                        lines.Remove(giftCartLine);
                                    }
                                }
                            }

                            if (isCreditLoadGiftCard)
                            {
                                calculatedCart.Cart.Lines = lines;
                                var request = new CalculateCartCommand()
                                {
                                    Cart = calculatedCart.Cart,
                                    Uid = calculatedCart.Cart.Uid
                                };

                                service.CalculateCart(request);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "voidTicket");
            }
        }

        /// <summary>
        /// EnterPaymentWindow for PaymentWindow - event
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="theTicket"></param>
        public void enterPaymentWindow(IDisplayActions.displayActionInfo theInfo, Ticket theTicket)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL && theTicket != null && theTicket.Items != null && theTicket.Items.Any())
                {
                    RefreshTicket(theTicket);

                    if (theTicket != null)
                    {
                        var services = new LoyaltyEngineServices();

                        if (_configurationHelper.LOAD_GIFTCARD_ITEMS != null && _configurationHelper.LOAD_GIFTCARD_ITEMS.Any())
                        {
                            var loadGiftCardItems = _configurationHelper.LOAD_GIFTCARD_ITEMS.Where(p => theTicket.Items.Any(p2 => p2.ItemName.Equals(p, StringComparison.InvariantCultureIgnoreCase)));
                            if (loadGiftCardItems != null && loadGiftCardItems.Any())
                            {
                                foreach (var loadGiftItem in loadGiftCardItems)
                                {
                                    var loadGiftCardMenuItems = theTicket.Items.ToList().FindAll(t => t.ItemName.Equals(loadGiftItem, StringComparison.InvariantCultureIgnoreCase));
                                    if (loadGiftCardMenuItems != null)
                                    {
                                        foreach (var loadItem in loadGiftCardMenuItems)
                                        {
                                            var calculatedCart = AsyncHelper.RunSync(() => services.GetCartBySourceExternalIdAsync(theTicket.ID));

                                            CartLine giftCartLine = null;
                                            if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Lines != null && calculatedCart.Cart.Lines.Any())
                                                giftCartLine = calculatedCart.Cart.Lines.FirstOrDefault(t => t.ProductName.Equals(loadGiftItem, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(t.GiftCardNumber) && !string.IsNullOrWhiteSpace(t.ProductExternalId) && t.ProductExternalId.Equals(loadItem.TIID));

                                            if (giftCartLine == null || string.IsNullOrWhiteSpace(giftCartLine.GiftCardNumber))
                                            {
                                                frmLoadGiftCardBalance loadGiftCardBalance = new frmLoadGiftCardBalance(_ticketId, string.Empty, _targetTicket, loadItem, calculatedCart, loadGiftItem);
                                                loadGiftCardBalance.ShowDialog();
                                                loadGiftCardBalance.CheckCardNumber();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (TicketDictionary.Dictionary != null && TicketDictionary.Dictionary.ContainsKey(_ticketId))
                        {
                            if (TicketDictionary.Dictionary[_ticketId] == Constants.CALCULATEDCURRENT
                                && TicketDictionary.CartDictionary != null && TicketDictionary.CartDictionary.ContainsKey(_ticketId)
                                && TicketDictionary.CartDictionary[_ticketId] != null)
                            {
                                var approveCartCommand = new ApproveCartCommand
                                {
                                    CartUid = TicketDictionary.CartDictionary[_ticketId],
                                    CartSourceExternalId = _ticketId.ToString(),
                                    ReferenceNumber = _ticketId.ToString()
                                };

                                CartApproval cartApproval = AsyncHelper.RunSync(() => services.ApproveCartAsync(approveCartCommand));

                                if (cartApproval != null && cartApproval.Alerts != null && cartApproval.Alerts.Count > 0)
                                {
                                    foreach (var alert in cartApproval.Alerts)
                                    {
                                        if (alert.Uid != null && alert.Uid != Guid.Empty && !string.IsNullOrWhiteSpace(alert.SnippetUrl))
                                        {
                                            frmAlerts frmAlert = new frmAlerts(TicketDictionary.CartDictionary[_ticketId].ToString(), alert.Uid.ToString(), alert.SnippetUrl);
                                            frmAlert.ShowDialog();
                                        }
                                    }
                                }
                            }
                            else if (_configurationHelper.IS_CALCULATE_DISCOUNT_WARNING)
                            {
                                frmTicketMsgForm discountWarning = new frmTicketMsgForm(theInfo.userID, _ticketId);
                                discountWarning.ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "enterPaymentWindow");
            }
        }

        /// <summary>
        /// Exit Payment Window and close ticket event 
        /// </summary>
        /// <param name="theInfo"></param>
        public async void exitPaymentWindow(IDisplayActions.displayActionInfo theInfo)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                if (_configurationHelper != null && _configurationHelper.ENABLE_bLOYAL)
                {
                    if (_ticketId != 0)
                    {
                        if (_targetTicket != null && _targetTicket.AmountDue == 0)
                        {
                            var response = _calculateTransaction.ApplyLoadGiftCardBalance(_ticketId, theInfo.userID);

                            if (response != null)
                            {
                                foreach (var loadItem in response)
                                {
                                    frmApplyGiftCardSummary gcSummary = new frmApplyGiftCardSummary(loadItem.NetAmount, loadItem.GiftCardNumber, loadItem.IsProvision, loadItem.IsNewCard);
                                    gcSummary.ShowDialog();
                                }
                            }

                            if (_ticketId > 0
                                && TicketDictionary.Dictionary != null
                                && TicketDictionary.Dictionary.ContainsKey(_ticketId)
                                && TicketDictionary.Dictionary[_ticketId] == Constants.CALCULATEDCURRENT)
                            {
                                var approveCartCommand = new ApproveCartCommand
                                {
                                    CartUid = TicketDictionary.CartDictionary[_ticketId],
                                    CartSourceExternalId = _ticketId.ToString(),
                                    ReferenceNumber = _ticketId.ToString()
                                };
                                var service = new LoyaltyEngineServices();
                                var cartApproval = service.ApproveCartAsync(approveCartCommand);

                                await _calculateTransaction.CommitCartAsync(_ticketId);
                            }

                            _ticketId = 0;  // after process complete set ticket id to 0
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "exitPaymentWindow");
            }
        }

        /// <summary>
        /// login event
        /// </summary>
        /// <param name="theInfo"></param>
        public void login(IDisplayActions.displayActionInfo theInfo)
        {

        }

        /// <summary>
        /// Get Tenders
        /// </summary>
        /// <param name="dinerwareProvider"></param>
        private void GetTenders(DinerwareProvider dinerwareProvider)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;
                TendersCache.Tenders = dinerwareProvider.GetTenders(0);
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "GetTenders");
            }
        }

        /// <summary>
        /// Get Discount Sets
        /// </summary>
        /// <param name="dinerwareProvider"></param>
        /// <param name="configurationHelper"></param>
        private void GetDiscountSets(DinerwareProvider dinerwareProvider, ConfigurationHelper configurationHelper)
        {
            try
            {
                if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                    return;

                var discountRules = dinerwareProvider.GetAllDiscountRule();
                if (discountRules != null && discountRules.Any())
                {
                    var bLoyalOrderDiscountType = discountRules.FirstOrDefault(t => t.Name.Equals(configurationHelper.DW_ORDER_LEVEL_DISCOUNT_TYPE_NAME, StringComparison.CurrentCultureIgnoreCase));
                    if (bLoyalOrderDiscountType != null && bLoyalOrderDiscountType.Active
                        && bLoyalOrderDiscountType.AmountDelta.Equals(0)
                        && !bLoyalOrderDiscountType.IsAutoApplied && bLoyalOrderDiscountType.AppliesToTickets
                        && !bLoyalOrderDiscountType.AppliesToItems && bLoyalOrderDiscountType.AppliesOnAllDays)
                    {
                        int orderDiscountId = 0;
                        int.TryParse(bLoyalOrderDiscountType.TypeID, out orderDiscountId);
                        DiscountSets.OrderLevelDiscountId = orderDiscountId;
                    }
                    var bLoyalItemDiscountType = discountRules.FirstOrDefault(t => t.Name.Equals(configurationHelper.DW_ITEM_LEVEL_DISCOUNT_TYPE_NAME, StringComparison.CurrentCultureIgnoreCase));
                    if (bLoyalItemDiscountType != null && bLoyalItemDiscountType.Active && bLoyalItemDiscountType.AmountDelta.Equals(0)
                        && !bLoyalItemDiscountType.IsAutoApplied && bLoyalItemDiscountType.AppliesToItems
                        && !bLoyalItemDiscountType.AppliesToTickets && bLoyalItemDiscountType.AppliesOnAllDays)
                    {
                        int itemDiscountId = 0;
                        int.TryParse(bLoyalItemDiscountType.TypeID, out itemDiscountId);
                        DiscountSets.ItemLevelDiscountId = itemDiscountId;
                    }
                    var bLoyalSalePriceDiscountType = discountRules.FirstOrDefault(t => t.Name.Equals(configurationHelper.DW_SALEPRICE_LEVEL_DISCOUNT_TYPE_NAME, StringComparison.CurrentCultureIgnoreCase));
                    if (bLoyalSalePriceDiscountType != null && bLoyalSalePriceDiscountType.Active && bLoyalSalePriceDiscountType.AmountDelta.Equals(0)
                      && !bLoyalSalePriceDiscountType.IsAutoApplied && bLoyalSalePriceDiscountType.AppliesToItems
                      && !bLoyalSalePriceDiscountType.AppliesToTickets && bLoyalSalePriceDiscountType.AppliesOnAllDays)
                    {
                        int salePriceDiscountId = 0;
                        int.TryParse(bLoyalSalePriceDiscountType.TypeID, out salePriceDiscountId);
                        DiscountSets.ItemLevelSalePriceId = salePriceDiscountId;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.WriteLogError(ex, "GetDiscountSets");
            }
        }

        /// <summary>
        /// logout event
        /// </summary>
        /// <param name="theInfo"></param>
        public void logout(IDisplayActions.displayActionInfo theInfo)
        {

        }

        /// <summary>
        /// newTicket event
        /// </summary>
        /// <param name="theInfo"></param>
        /// <param name="targetTicket"></param>
        public void newTicket(IDisplayActions.displayActionInfo theInfo, Ticket targetTicket)
        {
            if (!ConfigurationHelper.Instance.IS_Test_Virtual_Client_Connection || !ConfigurationHelper.Instance.IS_Test_BLoyal_Connection)
                return;
            RefreshTicket(targetTicket);
        }

        #endregion
    }
}
