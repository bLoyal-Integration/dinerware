using bLoyal.Connectors.LoyaltyEngine;
using bLoyal.Utilities;
using Dinerware;
using DinerwareSystem.ConfigurationCache;
using DinerwareSystem.DinerwareEngineService;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class bLoyalLoyaltyTender : Form
    {
        #region Private Member

        private int _currentUserId = 1;
        private int _currentOpenTicketId = 0;
        private string _cartExternalId;
        private decimal _availableBalance = 0;
        private decimal _ticketAmountDue = 0;
        private string _transactionCode;
        private decimal _applyAmount;
        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;
        DinerwareProvider _dwProvider = new DinerwareProvider();
        LoggerHelper _logger = LoggerHelper.Instance;
        PaymentEngineConnector _paymentEngine = null;
        Customer _customer = null;
        Ticket _currentTicket = null;

        #endregion

        #region Public Member

        // Assign value by touch screen keyboard    
        public string LoyaltyTenderAmount;

        #endregion

        #region Public Methods

        public bLoyalLoyaltyTender()
        {
            InitializeComponent();
        }

        public bLoyalLoyaltyTender(Customer customer, Ticket currentTicket = null)
        {
            _customer = customer;
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
        /// bLoyal Loyalty Tender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bLoyalLoyaltyTender_Load(object sender, EventArgs e)
        {
            textApplyBalance.Focus();
            this.ActiveControl = textApplyBalance;

            // Close snippets automatically
            Timer frmCloseTimer = new Timer();
            frmCloseTimer.Interval = 300000;
            frmCloseTimer.Start();
            frmCloseTimer.Tick += new EventHandler(CloseForm);

            int.TryParse(bLoyalLoyaltyTenderExtension.currentUserId, out _currentUserId);
            int.TryParse(bLoyalLoyaltyTenderExtension.currentOpenTicketId, out _currentOpenTicketId);
            _cartExternalId = bLoyalLoyaltyTenderExtension.currentOpenTicketId;

            lblAvailableBalance.Enabled = true;
            lblAvailableBalance.Visible = false;
            labAvailableBalanceVal.Enabled = true;
            labAvailableBalanceVal.Visible = false;
            lblCustomerNotAssigntoTicket.Enabled = true;
            lblCustomerNotAssigntoTicket.Visible = false;
            lblApplyBalanceError.Text = "";
            this.FormClosing += frmbLoyalLoyaltyTender_FormClosing;

            GetLoyaltyBalance();
        }

        /// <summary>
        /// Cancel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            bLoyalLoyaltyTenderExtension.currentOpenTicketId = string.Empty;
            this.Close();
        }

        /// <summary>
        /// check Balance 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBalanceBtn_Click(object sender, EventArgs e)
        {
            lblApplyBalanceError.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(_cartExternalId))
            {
                GetLoyaltyBalance();
                lblAvailableBalance.Enabled = false;
                lblAvailableBalance.Visible = true;
                labAvailableBalanceVal.Enabled = false;
                labAvailableBalanceVal.Visible = true;
                lblAvailableBalance.ForeColor = Color.Black;
                labAvailableBalanceVal.ForeColor = Color.Black;
                labAvailableBalanceVal.Text = _availableBalance.ToString();
            }
        }

        /// <summary>
        /// Get Loyalty Balance
        /// </summary>
        /// <returns></returns>
        private void GetLoyaltyBalance()
        {
            try
            {
                _ticketAmountDue = _currentTicket.AmountDue;
                if (_customer != null && _customer.Uid != null && _customer.Uid != Guid.Empty)
                {
                    _paymentEngine = new PaymentEngineConnector(_conFigHelper.LOGIN_DOMAIN, _conFigHelper.ACCESS_KEY, string.Empty, string.Empty, null);

                    var cardResponse = _paymentEngine.GetCardBalance(string.Empty, _conFigHelper.LOYALTY_TENDER_CODE, _customer.Uid);

                    if (cardResponse != null)
                    {
                        if (cardResponse.AvailableBalance != 0)
                            _availableBalance = Math.Round(cardResponse.AvailableBalance, 2);

                        var balance = _availableBalance >= _ticketAmountDue ? Math.Round(_ticketAmountDue, 2) : Math.Round(_availableBalance, 2);
                        textApplyBalance.Text = _ticketAmountDue == 0 ? "0" : balance.ToString();

                    }
                    else
                    {
                        lblApplyBalanceError.Text = Messages.SERVICE_UNAVAILBLE_WARNING;
                    }
                }
                else if (_ticketAmountDue != 0)
                {
                    lblCustomerNotAssigntoTicket.Enabled = false;
                    lblCustomerNotAssigntoTicket.Visible = true;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "GetApplyCardBalance");
            }
        }

        /// <summary>
        /// apply tender button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int tenderId = 0;
                var tenders = TendersCache.Tenders;

                if (tenders != null && tenders.Any())
                {
                    var getGiftCardTender = tenders.ToList().Find(t => t.TenderTypeName.Equals(_conFigHelper.DW_LOYALTY_TENDER_NAME, StringComparison.CurrentCultureIgnoreCase));
                    if (getGiftCardTender != null && !string.IsNullOrWhiteSpace(getGiftCardTender.ID))
                        int.TryParse(getGiftCardTender.ID, out tenderId);
                }

                decimal applyBalance = 0;
                decimal.TryParse(textApplyBalance.Text, out applyBalance);

                if (tenderId != 0)
                {
                    if (applyBalance <= 0)
                    {
                        lblApplyBalanceError.Text = "Please enter an amount greater than 0.";
                    }
                    else if (_ticketAmountDue == 0)
                    {
                        var ticketMenuItems = _dwProvider.GetTicketMenuItems(_currentUserId, _currentOpenTicketId);
                        lblApplyBalanceError.Text = ticketMenuItems != null && ticketMenuItems.Count() > 0 ? "Amount due on this ticket is $0." : "Please add at least 1 item to apply loyalty tender.";
                    }
                    else if (applyBalance > _ticketAmountDue)
                    {
                        lblApplyBalanceError.Text = "Entered amount exceeds ticket amount due of $" + Math.Round(_ticketAmountDue, 2);
                    }
                    else if (_availableBalance > 0 && applyBalance > 0)
                    {
                        ApplyLoyaltyTender(tenderId, applyBalance);
                        bLoyalLoyaltyTenderExtension.currentOpenTicketId = string.Empty;
                    }
                    else if (applyBalance > _availableBalance)
                    {
                        lblApplyBalanceError.Text = "Entered amount exceeds available balance of $" + Math.Round(_availableBalance, 2);
                    }
                }
                else
                {
                    //bLoyal Loyalty Tenders not find in Dinerware
                    this.Hide();
                    frmTenderNotFoundWarning show = new frmTenderNotFoundWarning(Messages.LOYALTY_TENDER_NOT_CONFIGURED);
                    show.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "applyBtn_Click");
            }
        }

        /// <summary>
        /// Apply Loyalty Tender
        /// </summary>
        /// <param name="tenderId"></param>
        /// <param name="applyBalance"></param>
        private void ApplyLoyaltyTender(int tenderId, decimal applyBalance)
        {
            try
            {
                _paymentEngine = new PaymentEngineConnector(_conFigHelper.LOGIN_DOMAIN, _conFigHelper.ACCESS_KEY, string.Empty, string.Empty, null);

                _transactionCode = _paymentEngine.CardRedeem(string.Empty, _conFigHelper.LOYALTY_TENDER_CODE, _cartExternalId, applyBalance, _customer.Uid);

                if (!string.IsNullOrWhiteSpace(_transactionCode))
                {
                    _applyAmount = applyBalance;

                    var response = _dwProvider.AddTransactionToTicket(_currentUserId, _currentOpenTicketId, new wsTransaction
                    {
                        PaymentAmount = applyBalance,
                        TenderType = Constants.BLOYALLOYALTYTENDER,
                        TenderTypeID = tenderId,
                        RefNumber = _transactionCode
                    });

                    if (response.Result == TicketChangeResult.Success)
                        this.Close();
                }
                lblApplyBalanceError.Text = Messages.SERVICE_UNAVAILBLE_WARNING;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "ApplyLoyaltyTender");
            }
        }

        /// <summary>
        /// Close Form Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmbLoyalLoyaltyTender_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Hide();

                if (!string.IsNullOrWhiteSpace(_transactionCode) && _applyAmount > 0)
                {
                    var cartLineHelper = new CartLineHelper();

                    var lines = cartLineHelper.GetCartLines(_currentTicket);

                    AsyncHelper.RunSync(() => _services.AddGiftCardPaymentTransactionAsync(_transactionCode, _applyAmount, string.Empty, _currentOpenTicketId.ToString(), lines));
                }
                _transactionCode = string.Empty;
                _applyAmount = 0;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmbLoyalLoyaltyTender_FormClosing");
            }
        }

        /// <summary>
        ///  Open KeyBoard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnkeyboard_Click(object sender, EventArgs e)
        {
            try
            {
                bLoyalLoyaltyTender frmLoyaltytender = this;

                this.Hide();

                frmLoyaltyTenderTouchScreen frmTCBoard = new frmLoyaltyTenderTouchScreen(ref frmLoyaltytender, textApplyBalance.Text);
                frmTCBoard.ShowDialog();

                textApplyBalance.Text = LoyaltyTenderAmount;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal GiftCardTender btnkeyboard_Click");
            }
        }

        #endregion
    }
}
