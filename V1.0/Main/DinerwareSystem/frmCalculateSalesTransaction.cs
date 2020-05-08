using bLoyal.Connectors.LoyaltyEngine;
using Dinerware;
using DinerwareSystem.ConfigurationCache;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmCalculateSalesTransaction : Form
    {
        #region Properties

        public bool CloseWindowConfirm { get; set; }

        private int _currentUserId = 1;
        private int _currentOpenTicketId = 0;
        private string _cartExternalId = string.Empty;
        private Button _okBtn;
        private Label _lblApplyDiscount;

        private GroupBox _appliedCouponsgroupBox;
        private ListBox _appliedCouponslistBox;
        private GroupBox _appliedDiscountsGB;
        private ListBox _appliedDiscountslistBox;
        private GroupBox _loyaltyDetialsGB;
        private Label _lblLoyaltyCurrencyAccrued;
        private Label _lblLoyaltyPointsAccrued;
        private Label _label1;
        private Label _lblLoyalCAVal;
        private Label _lblLoyaltyPointsUsedVal;
        private Label _lblLoyaltyPAVal;
        private GroupBox _groupBox1;
        private ListBox _appliedPricesBox;

        LoggerHelper _logger = LoggerHelper.Instance;
        CalculatedCart _calculatedCartResponse = null;
        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        DinerwareProvider _dinerwareProvider = new DinerwareProvider();
        DinerwareHelper _dinerwareHelper = new DinerwareHelper();
        ConfigurationHelper _configHelper = ConfigurationHelper.Instance;
        Ticket _currentTicket;

        #endregion

        #region Public Methods

        public frmCalculateSalesTransaction()
        {
            InitializeComponent();
        }

        public frmCalculateSalesTransaction(int userId, int ticketId, Ticket currentTicket)
        {
            _currentUserId = userId;
            _currentOpenTicketId = ticketId;
            _cartExternalId = bLoyalCalculateTicketExtension.currentOpenTicketId;
            _currentTicket = currentTicket;
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Close automatic window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Calculate Sales Transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCalculate_SalesTransaction_Load(object sender, EventArgs e)
        {
            try
            {
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseForm);
             
                CalculateTicketDiscount();

                this.FormClosing += frmApplyDiscount_FormClosing;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmCalculateSalesTransaction_Load_1");
            }
        }

        /// <summary>
        /// Apply Discount Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmApplyDiscount_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                CloseWindowConfirm = true;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmApplyDiscount_FormClosing");
            }
        }

        /// <summary>
        /// Calculate Ticket Discount
        /// </summary>
        /// <returns></returns>
        private void CalculateTicketDiscount()
        {
            try
            {                
                bLoyalCalculateTicketExtension.currentOpenTicketId = string.Empty;
                TicketList ticketContents = null;

                RemovebLoyalDiscount(); // Remove old apply bLoyal Discounts      
                       
                ticketContents = _dinerwareProvider.GetTicketContents(_currentOpenTicketId, true); // After removed discount - we need to get updated ticket information
                var calculateCartCommand = GetCalculateCart(_cartExternalId, ticketContents);

                var calculateCart = calculateCartCommand != null && calculateCartCommand.Cart != null
                    && calculateCartCommand.Cart.Lines != null && calculateCartCommand.Cart.Lines.Any() ?
                    _services.CalculateCart(calculateCartCommand) : null;

                _calculatedCartResponse = calculateCart;

                if (calculateCart != null && calculateCart.LoyaltySummary != null)
                {
                    if (calculateCart.LoyaltySummary.AppliedCoupons != null)
                    {
                        foreach (var coupon in calculateCart.LoyaltySummary.AppliedCoupons)
                        {
                            _appliedCouponslistBox.Items.Add(coupon.Name + "(" + coupon.Code + ")");
                        }
                    }
                    if (calculateCart.LoyaltySummary.AppliedDiscounts != null)
                    {
                        foreach (var discount in calculateCart.LoyaltySummary.AppliedDiscounts)
                        {
                            _appliedDiscountslistBox.Items.Add(discount.Name);
                        }
                    }
                    if (calculateCart.LoyaltySummary.AppliedPrices != null)
                    {
                        foreach (var prices in calculateCart.LoyaltySummary.AppliedPrices)
                        {
                            _appliedPricesBox.Items.Add(prices.Name);
                        }
                    }

                    ApplybLoyalDiscount(ticketContents, calculateCart);

                    _lblLoyalCAVal.Text = Math.Round(calculateCart.LoyaltySummary.LoyaltyCurrencyAccrued, 2).ToString();
                    _lblLoyaltyPAVal.Text = calculateCart.LoyaltySummary.LoyaltyPointsAccrued.ToString();
                    _lblLoyaltyPointsUsedVal.Text = calculateCart.LoyaltySummary.LoyaltyPointsUsed.ToString();

                    UpdateTicketDictionary(calculateCart);
                    _lblApplyDiscount.Text = Messages.DISCOUNT_APPLIED_MSG;
                }
                else
                    MessageBox.Show("Unable to apply bLoyal Discounts");

                _okBtn.Visible = true;
                _cartExternalId = string.Empty;

                if (_configHelper != null && !_configHelper.IS_DISCOUNT_SUMMARY)
                {
                    this.Hide();
                    ShowAlerts();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateSalesTransaction");
            }
        }

        /// <summary>
        /// Show Alerts
        /// </summary>
        private void ShowAlerts()
        {
            try
            {
                if (_calculatedCartResponse != null && _calculatedCartResponse.LoyaltySummary != null && _calculatedCartResponse.LoyaltySummary.Alerts != null && _calculatedCartResponse.LoyaltySummary.Alerts.Any())
                {
                    foreach (var alert in _calculatedCartResponse.LoyaltySummary.Alerts)
                    {
                        if (alert.Uid != Guid.Empty && !string.IsNullOrWhiteSpace(alert.SnippetUrl))
                        {
                            frmAlerts frmAlert = new frmAlerts(_calculatedCartResponse.Cart.Uid.ToString(), alert.Uid.ToString(), alert.SnippetUrl);
                            frmAlert.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "ShowAlerts");
            }
        }

        /// <summary>
        /// Update Ticket Dictionary
        /// </summary>
        /// <param name="calculateCart"></param>
        private void UpdateTicketDictionary(CalculatedCart calculateCart)
        {
            try
            {
                if (TicketDictionary.Dictionary == null)
                    TicketDictionary.Dictionary = new Dictionary<int, string>();

                if (TicketDictionary.Dictionary.ContainsKey(_currentOpenTicketId))
                    TicketDictionary.Dictionary[_currentOpenTicketId] = "CalculatedCurrent";
                else
                    TicketDictionary.Dictionary.Add(_currentOpenTicketId, "CalculatedCurrent");


                if (calculateCart != null && calculateCart.Cart != null && calculateCart.Cart.Uid != Guid.Empty)
                {
                    if (TicketDictionary.CartDictionary == null)
                        TicketDictionary.CartDictionary = new Dictionary<int, Guid>();

                    if (TicketDictionary.CartDictionary.ContainsKey(_currentOpenTicketId))
                        TicketDictionary.CartDictionary[_currentOpenTicketId] = calculateCart.Cart.Uid;
                    else
                        TicketDictionary.CartDictionary.Add(_currentOpenTicketId, calculateCart.Cart.Uid);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "UpdateTicketDictionary");
            }
        }

        /// <summary>
        /// Apply Discount to Ticket
        /// </summary>
        /// <param name="ticketContents"></param>
        /// <param name="calculateTransaction"></param>
        private void ApplybLoyalDiscount(TicketList ticketContents, CalculatedCart calculateTransaction)
        {
            try
            {
                //Apply Discount to Ticket                    
                if (calculateTransaction.Cart != null && calculateTransaction.Cart.Lines != null && calculateTransaction.Cart.Lines.Any()
                    && ticketContents != null && ticketContents.Tickets != null && ticketContents.Tickets[0].Items != null && ticketContents.Tickets[0].Items.Any())
                {
                    var itemList = ticketContents.Tickets[0].Items.ToList();
                    foreach (var line in calculateTransaction.Cart.Lines)
                    {
                        var ticketItem = itemList.Find(t => t.ID.Equals(line.ProductCode, StringComparison.CurrentCultureIgnoreCase) && t.TIID.Equals(line.ExternalId));
                        if (ticketItem != null)
                        {
                            int ticketItemId = 0;
                            int.TryParse(ticketItem.TIID, out ticketItemId);
                            if (line.Discount > 0 && !line.ExternallyAppliedDiscount)
                            {
                                if (ticketItem.Discounts != null)
                                {
                                    foreach (var exDis in ticketItem.Discounts)
                                    {
                                        int instanceId = 0;
                                        int.TryParse(exDis.InstanceID, out instanceId);
                                        _dinerwareProvider.RemoveDiscountFromItem(_currentUserId, _currentOpenTicketId, ticketItemId, instanceId);
                                    }
                                }
                                _dinerwareProvider.AddDiscountToItem(_currentUserId, _currentOpenTicketId, ticketItemId, DiscountSets.ItemLevelDiscountId, line.Discount * line.Quantity);
                            }
                            if (!string.IsNullOrWhiteSpace(line.SalePriceReasonCode) && (line.Price - line.SalePrice) > 0)
                            {
                                int discountId = 24;
                                _dinerwareProvider.AddDiscountToItem(_currentUserId, _currentOpenTicketId, ticketItemId, discountId, line.Price - line.SalePrice);
                            }
                            itemList.Remove(ticketItem);
                        }
                        else
                        {
                            var fbItem = ticketContents.Tickets[0].Items.ToList().Find(t => t.ID.Equals(line.ProductCode, StringComparison.CurrentCultureIgnoreCase) && t.TIID.Equals(line.ParentExternalId));
                            if (fbItem != null && line.Discount > 0 && !line.ExternallyAppliedDiscount)
                            {
                                int ticketItemId = 0;
                                int.TryParse(fbItem.TIID, out ticketItemId);
                                _dinerwareProvider.AddDiscountToItem(_currentUserId, _currentOpenTicketId, ticketItemId, DiscountSets.ItemLevelDiscountId, line.Discount * line.Quantity);
                            }
                        }
                    }

                    if (calculateTransaction.Cart.Discount > 0 && !calculateTransaction.Cart.ExternallyAppliedDiscount)
                    {
                        var ticket = ticketContents.Tickets[0];
                        if (ticket.Discounts != null && ticket.Discounts.Any())
                        {
                            foreach (var ticketDiscount in ticket.Discounts)
                            {
                                int instanceId = 0;
                                int.TryParse(ticketDiscount.InstanceID, out instanceId);
                                _dinerwareProvider.RemoveDiscountFromTicket(_currentUserId, _currentOpenTicketId, instanceId);
                            }
                        }
                        _dinerwareProvider.AddDiscountToTicket(_currentUserId, _currentOpenTicketId, DiscountSets.OrderLevelDiscountId, calculateTransaction.Cart.Discount);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "ApplybLoyalDiscount");
            }
        }

        /// <summary>
        /// Remove bLoyal Discount
        /// </summary>
        /// <param name="ticketList"></param>
        private void RemovebLoyalDiscount()
        {
            try
            {
                if (_currentTicket != null && _currentTicket.Items != null && _currentTicket.Items.Any())
                {
                    if (_currentTicket.Discounts != null && _currentTicket.Discounts.Any())
                    {
                        // Remove order level discount
                        var orderDiscounts = _currentTicket.Discounts.ToList().Where(t => t.TypeID.Equals(DiscountSets.OrderLevelDiscountId));
                        if (orderDiscounts != null)
                        {
                            foreach (var orderDiscount in orderDiscounts)
                            {
                                int instanceId = 0;
                                int.TryParse(orderDiscount.InstanceID, out instanceId);
                                _dinerwareProvider.RemoveDiscountFromTicket(_currentUserId, _currentOpenTicketId, instanceId);
                            }
                        }
                    }

                    foreach (var ticketItem in _currentTicket.Items)
                        RemoveDiscounts(DiscountSets.ItemLevelDiscountId, DiscountSets.ItemLevelSalePriceId, ticketItem);

                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "RemovebLoyalDiscount");
            }
        }

        /// <summary>
        /// Remove Discounts
        /// </summary>
        /// <param name="itemLevelDiscountId"></param>
        /// <param name="itemLevelSalePriceId"></param>
        /// <param name="ticketItem"></param>
        private void RemoveDiscounts(int itemLevelDiscountId, int itemLevelSalePriceId, Dinerware.MenuItem ticketItem)
        {
            try
            {
                if (ticketItem.Discounts != null && ticketItem.Discounts.Any())
                {
                    var itemDiscounts = ticketItem.Discounts.ToList().Where(t => t.TypeID.Equals(itemLevelDiscountId.ToString()));
                    var salesItems = ticketItem.Discounts.ToList().Where(t => t.TypeID.Equals(itemLevelSalePriceId.ToString()));
                    if (itemDiscounts != null)
                    {
                        foreach (var item in itemDiscounts)
                            RemoveItemDiscount(ticketItem, item);
                    }
                    if (salesItems != null)
                    {
                        foreach (var salesItem in salesItems)
                            RemoveItemDiscount(ticketItem, salesItem);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "RemoveDiscounts");
            }
        }

        /// <summary>
        /// Remove Item Discount
        /// </summary>
        /// <param name="ticketItem"></param>
        /// <param name="item"></param>
        private void RemoveItemDiscount(Dinerware.MenuItem ticketItem, Dinerware.Discount item)
        {
            try
            {
                int ticketItemId = 0;
                int itemInstanceId = 0;
                int.TryParse(ticketItem.TIID, out ticketItemId);
                int.TryParse(item.InstanceID, out itemInstanceId);
                _dinerwareProvider.RemoveDiscountFromItem(_currentUserId, _currentOpenTicketId, ticketItemId, itemInstanceId);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "RemoveItemDiscount");
            }
        }

        /// <summary>
        /// return CalculateCartCommand obj
        /// </summary>
        /// <param name="cartExternalId"></param>
        /// <param name="cart"></param>
        /// <param name="objwsMenuItem"></param>
        /// <returns></returns>
        private CalculateCartCommand GetCalculateCart(string cartExternalId, TicketList ticketContents)
        {
            try
            {
                var calculateCart = SalesTransactionLines(ticketContents);
                if (calculateCart != null)
                {
                    calculateCart.Cart.SourceExternalId = cartExternalId;
                    return new CalculateCartCommand
                    {
                        Cart = calculateCart.Cart,
                        ReferenceNumber = cartExternalId
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CalculateCart");
            }
            return null;
        }

        /// <summary>
        /// Create Sales Transaction Lines
        /// </summary>
        /// <param name="TransactionToken"></param>
        /// <param name="wsMenuItemList"></param>
        /// <returns></returns>
        private CalculatedCart SalesTransactionLines(TicketList ticketContent)
        {
            try
            {
                var cart = new CalculatedCart { Cart = new Cart { Lines = new List<CartLine>() } };
                if (ticketContent != null && ticketContent.Tickets != null && ticketContent.Tickets.Any() && ticketContent.Tickets[0].Items != null && ticketContent.Tickets[0].Items.Any())
                {
                    var wsMenuItemList = ticketContent.Tickets[0].Items;
                    decimal orderTotelDiscount = 0;
                    foreach (var menuItem in wsMenuItemList)
                    {
                        decimal lineDiscount = menuItem.ItemDiscountTotalManual / menuItem.Quantity;
                        decimal ticketorderDiscount = menuItem.ManualDiscountTotal > menuItem.ItemDiscountTotalManual ? (menuItem.ManualDiscountTotal / menuItem.Quantity) - lineDiscount : 0;
                        orderTotelDiscount += ticketorderDiscount;
                        var line = new CartLine
                        {
                            Price = menuItem.GrossPrice / menuItem.Quantity,
                            Discount = lineDiscount,
                            OrderDiscount = ticketorderDiscount,
                            NetPrice = menuItem.Price != 0 ? menuItem.Price / menuItem.Quantity : 0,
                            ProductCode = menuItem.ID,
                            ProductName = menuItem.ItemName,
                            Quantity = menuItem.Quantity,
                            PriceSource = PriceSource.Base,
                            ProductExternalId = menuItem.ID,
                            ExternalId = menuItem.TIID
                        };
                        cart.Cart.Lines.Add(line);
                        if (lineDiscount > 0)
                            line.ExternallyAppliedDiscount = true;
                    }
                    if (orderTotelDiscount > 0)
                    {
                        cart.Cart.ExternallyAppliedDiscount = true;
                        cart.Cart.Discount = orderTotelDiscount;
                    }

                    if (ticketContent.Tickets[0].Customer != null)
                    {
                        cart.Cart.Customer = new Customer
                        {
                            ExternalId = ticketContent.Tickets[0].Customer.ID,
                            FirstName = ticketContent.Tickets[0].Customer.FName,
                            LastName = ticketContent.Tickets[0].Customer.LName,
                            EmailAddress = ticketContent.Tickets[0].Customer.Email
                        };
                    }
                }
                return cart;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "SalesTransactionLines");
                return null;
            }
        }

        /// <summary>
        /// Close discount summary window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                ShowAlerts();
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "okBtn_Click");
            }
            _calculatedCartResponse = null;
            this.Close();
        }

        private void appliedCouponslistBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmCalculateSalesTransaction_Shown(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// InitializeComponent
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCalculateSalesTransaction));
            this._okBtn = new System.Windows.Forms.Button();
            this._lblApplyDiscount = new System.Windows.Forms.Label();
            this._appliedCouponsgroupBox = new System.Windows.Forms.GroupBox();
            this._appliedCouponslistBox = new System.Windows.Forms.ListBox();
            this._appliedDiscountsGB = new System.Windows.Forms.GroupBox();
            this._appliedDiscountslistBox = new System.Windows.Forms.ListBox();
            this._loyaltyDetialsGB = new System.Windows.Forms.GroupBox();
            this._lblLoyaltyCurrencyAccrued = new System.Windows.Forms.Label();
            this._lblLoyaltyPointsAccrued = new System.Windows.Forms.Label();
            this._label1 = new System.Windows.Forms.Label();
            this._lblLoyalCAVal = new System.Windows.Forms.Label();
            this._lblLoyaltyPointsUsedVal = new System.Windows.Forms.Label();
            this._lblLoyaltyPAVal = new System.Windows.Forms.Label();
            this._groupBox1 = new System.Windows.Forms.GroupBox();
            this._appliedPricesBox = new System.Windows.Forms.ListBox();
            this._appliedCouponsgroupBox.SuspendLayout();
            this._appliedDiscountsGB.SuspendLayout();
            this._loyaltyDetialsGB.SuspendLayout();
            this._groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this._okBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this._okBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this._okBtn.ForeColor = System.Drawing.Color.Snow;
            this._okBtn.Location = new System.Drawing.Point(540, 379);
            this._okBtn.Name = "okBtn";
            this._okBtn.Size = new System.Drawing.Size(153, 47);
            this._okBtn.TabIndex = 17;
            this._okBtn.Text = "OK";
            this._okBtn.UseVisualStyleBackColor = false;
            this._okBtn.Visible = false;
            this._okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // lblApplyDiscount
            // 
            this._lblApplyDiscount.AutoSize = true;
            this._lblApplyDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblApplyDiscount.Location = new System.Drawing.Point(155, 9);
            this._lblApplyDiscount.Name = "lblApplyDiscount";
            this._lblApplyDiscount.Size = new System.Drawing.Size(314, 20);
            this._lblApplyDiscount.TabIndex = 18;
            this._lblApplyDiscount.Text = "Apply bLoyal Discount is in-progress...";
            this._lblApplyDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // appliedCouponsgroupBox
            // 
            this._appliedCouponsgroupBox.Controls.Add(this._appliedCouponslistBox);
            this._appliedCouponsgroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._appliedCouponsgroupBox.Location = new System.Drawing.Point(12, 47);
            this._appliedCouponsgroupBox.Name = "appliedCouponsgroupBox";
            this._appliedCouponsgroupBox.Size = new System.Drawing.Size(315, 142);
            this._appliedCouponsgroupBox.TabIndex = 19;
            this._appliedCouponsgroupBox.TabStop = false;
            this._appliedCouponsgroupBox.Text = "Applied Coupons";
            // 
            // appliedCouponslistBox
            // 
            this._appliedCouponslistBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._appliedCouponslistBox.FormattingEnabled = true;
            this._appliedCouponslistBox.ItemHeight = 17;
            this._appliedCouponslistBox.Location = new System.Drawing.Point(6, 22);
            this._appliedCouponslistBox.Name = "appliedCouponslistBox";
            this._appliedCouponslistBox.ScrollAlwaysVisible = true;
            this._appliedCouponslistBox.Size = new System.Drawing.Size(296, 106);
            this._appliedCouponslistBox.TabIndex = 1;
            this._appliedCouponslistBox.SelectedIndexChanged += new System.EventHandler(this.appliedCouponslistBox_SelectedIndexChanged);
            // 
            // appliedDiscountsGB
            // 
            this._appliedDiscountsGB.Controls.Add(this._appliedDiscountslistBox);
            this._appliedDiscountsGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._appliedDiscountsGB.Location = new System.Drawing.Point(354, 48);
            this._appliedDiscountsGB.Name = "appliedDiscountsGB";
            this._appliedDiscountsGB.Size = new System.Drawing.Size(315, 141);
            this._appliedDiscountsGB.TabIndex = 20;
            this._appliedDiscountsGB.TabStop = false;
            this._appliedDiscountsGB.Text = "Applied Discounts";
            // 
            // appliedDiscountslistBox
            // 
            this._appliedDiscountslistBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._appliedDiscountslistBox.FormattingEnabled = true;
            this._appliedDiscountslistBox.ItemHeight = 17;
            this._appliedDiscountslistBox.Location = new System.Drawing.Point(9, 22);
            this._appliedDiscountslistBox.Name = "appliedDiscountslistBox";
            this._appliedDiscountslistBox.ScrollAlwaysVisible = true;
            this._appliedDiscountslistBox.Size = new System.Drawing.Size(293, 106);
            this._appliedDiscountslistBox.TabIndex = 4;
            // 
            // loyaltyDetialsGB
            // 
            this._loyaltyDetialsGB.Controls.Add(this._lblLoyaltyCurrencyAccrued);
            this._loyaltyDetialsGB.Controls.Add(this._lblLoyaltyPointsAccrued);
            this._loyaltyDetialsGB.Controls.Add(this._label1);
            this._loyaltyDetialsGB.Controls.Add(this._lblLoyalCAVal);
            this._loyaltyDetialsGB.Controls.Add(this._lblLoyaltyPointsUsedVal);
            this._loyaltyDetialsGB.Controls.Add(this._lblLoyaltyPAVal);
            this._loyaltyDetialsGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._loyaltyDetialsGB.Location = new System.Drawing.Point(354, 212);
            this._loyaltyDetialsGB.Name = "loyaltyDetialsGB";
            this._loyaltyDetialsGB.Size = new System.Drawing.Size(312, 141);
            this._loyaltyDetialsGB.TabIndex = 21;
            this._loyaltyDetialsGB.TabStop = false;
            this._loyaltyDetialsGB.Text = "Loyalty";
            // 
            // lblLoyaltyCurrencyAccrued
            // 
            this._lblLoyaltyCurrencyAccrued.AutoSize = true;
            this._lblLoyaltyCurrencyAccrued.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblLoyaltyCurrencyAccrued.Location = new System.Drawing.Point(6, 22);
            this._lblLoyaltyCurrencyAccrued.Name = "lblLoyaltyCurrencyAccrued";
            this._lblLoyaltyCurrencyAccrued.Size = new System.Drawing.Size(199, 17);
            this._lblLoyaltyCurrencyAccrued.TabIndex = 5;
            this._lblLoyaltyCurrencyAccrued.Text = "Loyalty Currency Accrued:";
            // 
            // lblLoyaltyPointsAccrued
            // 
            this._lblLoyaltyPointsAccrued.AutoSize = true;
            this._lblLoyaltyPointsAccrued.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblLoyaltyPointsAccrued.Location = new System.Drawing.Point(6, 59);
            this._lblLoyaltyPointsAccrued.Name = "lblLoyaltyPointsAccrued";
            this._lblLoyaltyPointsAccrued.Size = new System.Drawing.Size(179, 17);
            this._lblLoyaltyPointsAccrued.TabIndex = 7;
            this._lblLoyaltyPointsAccrued.Text = "Loyalty Points Accrued:";
            // 
            // label1
            // 
            this._label1.AutoSize = true;
            this._label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label1.Location = new System.Drawing.Point(6, 100);
            this._label1.Name = "label1";
            this._label1.Size = new System.Drawing.Size(157, 17);
            this._label1.TabIndex = 11;
            this._label1.Text = "Loyalty Points Used:";
            // 
            // lblLoyalCAVal
            // 
            this._lblLoyalCAVal.AutoSize = true;
            this._lblLoyalCAVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblLoyalCAVal.Location = new System.Drawing.Point(211, 22);
            this._lblLoyalCAVal.Name = "lblLoyalCAVal";
            this._lblLoyalCAVal.Size = new System.Drawing.Size(16, 17);
            this._lblLoyalCAVal.TabIndex = 6;
            this._lblLoyalCAVal.Text = "0";
            // 
            // lblLoyaltyPointsUsedVal
            // 
            this._lblLoyaltyPointsUsedVal.AutoSize = true;
            this._lblLoyaltyPointsUsedVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblLoyaltyPointsUsedVal.Location = new System.Drawing.Point(169, 100);
            this._lblLoyaltyPointsUsedVal.Name = "lblLoyaltyPointsUsedVal";
            this._lblLoyaltyPointsUsedVal.Size = new System.Drawing.Size(16, 17);
            this._lblLoyaltyPointsUsedVal.TabIndex = 12;
            this._lblLoyaltyPointsUsedVal.Text = "0";
            // 
            // lblLoyaltyPAVal
            // 
            this._lblLoyaltyPAVal.AutoSize = true;
            this._lblLoyaltyPAVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblLoyaltyPAVal.Location = new System.Drawing.Point(191, 59);
            this._lblLoyaltyPAVal.Name = "lblLoyaltyPAVal";
            this._lblLoyaltyPAVal.Size = new System.Drawing.Size(16, 17);
            this._lblLoyaltyPAVal.TabIndex = 8;
            this._lblLoyaltyPAVal.Text = "0";
            // 
            // groupBox1
            // 
            this._groupBox1.Controls.Add(this._appliedPricesBox);
            this._groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._groupBox1.Location = new System.Drawing.Point(18, 211);
            this._groupBox1.Name = "groupBox1";
            this._groupBox1.Size = new System.Drawing.Size(315, 142);
            this._groupBox1.TabIndex = 22;
            this._groupBox1.TabStop = false;
            this._groupBox1.Text = "Applied Prices";
            // 
            // appliedPricesBox
            // 
            this._appliedPricesBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._appliedPricesBox.FormattingEnabled = true;
            this._appliedPricesBox.ItemHeight = 17;
            this._appliedPricesBox.Location = new System.Drawing.Point(6, 22);
            this._appliedPricesBox.Name = "appliedPricesBox";
            this._appliedPricesBox.ScrollAlwaysVisible = true;
            this._appliedPricesBox.Size = new System.Drawing.Size(296, 106);
            this._appliedPricesBox.TabIndex = 1;
            // 
            // frmCalculateSalesTransaction
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(759, 438);
            this.Controls.Add(this._groupBox1);
            this.Controls.Add(this._loyaltyDetialsGB);
            this.Controls.Add(this._appliedDiscountsGB);
            this.Controls.Add(this._appliedCouponsgroupBox);
            this.Controls.Add(this._lblApplyDiscount);
            this.Controls.Add(this._okBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCalculateSalesTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bLoyal Loyalty";
            this.Load += new System.EventHandler(this.frmCalculate_SalesTransaction_Load);
            this.Shown += new System.EventHandler(this.frmCalculateSalesTransaction_Shown);
            this._appliedCouponsgroupBox.ResumeLayout(false);
            this._appliedDiscountsGB.ResumeLayout(false);
            this._loyaltyDetialsGB.ResumeLayout(false);
            this._loyaltyDetialsGB.PerformLayout();
            this._groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
