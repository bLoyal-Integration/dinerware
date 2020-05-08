using bLoyal.Utilities;
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
    public partial class frmbLoyalGiftCardTender : Form 
    {
        #region Private Member

        private int _currentUserId = 1;
        private int _currentOpenTicketId = 0;
        private string _cartExternalId = string.Empty;
        private decimal _availableBalance = 0;
        private decimal _ticketAmountDue = 0;
        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;
        DinerwareProvider _dinerwareProvider = new DinerwareProvider();
        LoggerHelper _logger = LoggerHelper.Instance;
        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        PaymentEngineConnector _paymentEngine = null;
        private bool _isTicketOpen = true;
        private Dinerware.Ticket _currentTicket = null;
        private string _transactionCode = string.Empty;
        private decimal _applyAmount = 0;
        private string _giftCardNumber = string.Empty;

        #endregion

        #region Public Member

        // Assign value by touch screen keyboard
        public string GiftCardNumber;
        public string GiftCardAmount;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gift Card Tender
        /// </summary>
        /// <param name="isTicketOpen"></param>
        /// <param name="currentTicket"></param>
        public frmbLoyalGiftCardTender(bool isTicketOpen = true, Dinerware.Ticket currentTicket = null)
        {
            _isTicketOpen = isTicketOpen;
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
        /// Load bLoyalGiftCardTender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmbLoyalGiftCardTender_Load(object sender, EventArgs e)
        {
            try
            {
                txtGiftCardNumber.Focus();
                this.ActiveControl = txtGiftCardNumber;
                GetTicketAmountDue();
                textApplyBalance.Text = Math.Round(_ticketAmountDue, 2).ToString();
                lblAvailableBalance.Visible = false;
                lblEmptyCardNumberError.Visible = false;
                lblAvailableBalanceAmt.Text = string.Empty;
                lblApplyBalanceError.Text = string.Empty;
                if (!_isTicketOpen)
                {
                    applyBtn.Enabled = false;
                    textApplyBalance.Text = string.Empty;
                    textApplyBalance.ReadOnly = true;
                    applyBtn.BackColor = Color.Gray;
                }
                this.FormClosing += frmbLoyalGiftCardTender_FormClosing;
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseForm);

                int.TryParse(bLoyalGiftCardTenderExtension.currentUserId, out _currentUserId);
                int.TryParse(bLoyalGiftCardTenderExtension.currentOpenTicketId, out _currentOpenTicketId);
                _cartExternalId = bLoyalGiftCardTenderExtension.currentOpenTicketId;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmbLoyalGiftCardTender_Load");
            }
        }

        /// <summary>
        /// Close Form Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmbLoyalGiftCardTender_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Hide();

                if (!string.IsNullOrWhiteSpace(_transactionCode) && _applyAmount > 0 && !string.IsNullOrWhiteSpace(_giftCardNumber))
                {
                    var cartLineHelper = new CartLineHelper();

                    var lines = cartLineHelper.GetCartLines(_currentTicket);

                    AsyncHelper.RunSync(() => _services.AddGiftCardPaymentTransactionAsync(_transactionCode, _applyAmount, _giftCardNumber, _currentOpenTicketId.ToString(), lines));
                }

                _transactionCode = string.Empty;
                _applyAmount = 0;
                _giftCardNumber = string.Empty;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmbLoyalGiftCardTender_FormClosing");
            }
        }

        /// <summary>
        /// Check balance button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBalanceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                lblAvailableBalanceAmt.Text = string.Empty;
                lblApplyBalanceError.Text = string.Empty;
                lblEmptyCardNumberError.Visible = false;

                if (string.IsNullOrWhiteSpace(txtGiftCardNumber.Text))
                {
                    lblEmptyCardNumberError.Visible = true;
                    lblEmptyCardNumberError.Text = "Please enter gift card number.";
                }
                else
                {
                    try
                    {
                        FinancialCard financialCardHelper = new FinancialCard(txtGiftCardNumber.Text);
                        txtGiftCardNumber.Text = financialCardHelper.CardNumber;
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteLogError(ex, "FinancialCard for swap GiftCard Tender");
                    }
                    GetGiftCardBalance();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal Gift Card checkBalanceBtn_Click");
            }

        }

        /// <summary>
        /// Get Ticket AmountDue
        /// </summary>
        private void GetTicketAmountDue()
        {
            try
            {
                _ticketAmountDue = _currentTicket.AmountDue;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "GetTicketAmountDue in GiftCard Tender");
            }
        }

        /// <summary>
        /// Get Gift Card Balance
        /// </summary>
        private PaymentEngine.CardResponse GetGiftCardBalance(bool isCheckBalance = true)
        {
            var cardResponse = new PaymentEngine.CardResponse();
            try
            {
                GetTicketAmountDue();

                _paymentEngine = new PaymentEngineConnector(_conFigHelper.LOGIN_DOMAIN, _conFigHelper.ACCESS_KEY, string.Empty, string.Empty, null);

                cardResponse = _paymentEngine.GetCardBalance(txtGiftCardNumber.Text, _conFigHelper.GIFTCARD_TENDER_CODE);
                if (cardResponse != null)
                {
                    if (cardResponse.AvailableBalance != 0)
                    {
                        lblAvailableBalance.Visible = true;
                        _availableBalance = Math.Round(cardResponse.AvailableBalance, 2);
                        if (isCheckBalance)
                        {
                            if (_availableBalance <= _ticketAmountDue)
                            {
                                _ticketAmountDue = _availableBalance;
                                textApplyBalance.Text = Math.Round(_ticketAmountDue, 2).ToString();
                            }
                            else
                                textApplyBalance.Text = Math.Round(_ticketAmountDue, 2).ToString();
                        }
                        _availableBalance = Math.Round(_availableBalance, 2);
                        lblAvailableBalanceAmt.Visible = true;
                        lblAvailableBalanceAmt.Text = _availableBalance.ToString();
                    }
                    else if (cardResponse.AvailableBalance == 0 
                        && cardResponse.Status == PaymentEngine.CardRequestStatus.Approved 
                        && !string.IsNullOrWhiteSpace(cardResponse.Message) 
                        && (cardResponse.Message.StartsWith("Current balance is") 
                            || cardResponse.Message.StartsWith("Gift card is not provisioned - number is auto provisioned enabled")))
                    {
                        lblAvailableBalance.Visible = true;
                        lblAvailableBalanceAmt.Text = "0";
                        textApplyBalance.Text = "0";
                        lblApplyBalanceError.Text = cardResponse.Message;
                    }
                    else if (cardResponse.Status == PaymentEngine.CardRequestStatus.Declined)
                    {
                        textApplyBalance.Text = "0";
                        lblAvailableBalance.Visible = false;
                        lblAvailableBalanceAmt.Visible = false;
                        lblApplyBalanceError.Text = cardResponse.Message;
                    }
                    else if (cardResponse.AvailableBalance == 0 && cardResponse.Status == PaymentEngine.CardRequestStatus.Approved)
                    {
                        textApplyBalance.Text = "0";
                        lblAvailableBalance.Visible = true;
                        lblAvailableBalanceAmt.Visible = true;
                        lblAvailableBalanceAmt.Text = "0";
                    }

                }
                else
                {
                    lblApplyBalanceError.Text = "Service unavailable. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal GiftCardTender GetApplyCardBalance");
                frmbLoyalServiceUrlDownWarning frmServiceUrlDown = new frmbLoyalServiceUrlDownWarning();
                frmServiceUrlDown.ShowDialog();
            }
            return cardResponse;
        }

        /// <summary>
        /// Cancel button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            bLoyalGiftCardTenderExtension.currentOpenTicketId = string.Empty;
            _cartExternalId = string.Empty;
            this.Close();
        }

        /// <summary>
        /// Apply button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtGiftCardNumber.Text))
                {
                    int tenderId = 0;
                    var tenders = TendersCache.Tenders;

                    if (tenders != null && tenders.Any())
                    {
                        var getGiftCardTender = tenders.ToList().Find(t => t.TenderTypeName.Equals(_conFigHelper.DW_GIFTCARD_TENDER_NAME, StringComparison.CurrentCultureIgnoreCase));
                        if (getGiftCardTender != null && !string.IsNullOrWhiteSpace(getGiftCardTender.ID))
                            int.TryParse(getGiftCardTender.ID, out tenderId);
                    }
                    if (tenderId != 0)
                    {
                        decimal applyBalanceAmt = 0;
                        decimal.TryParse(textApplyBalance.Text, out applyBalanceAmt);

                        try
                        {
                            FinancialCard financialCardHelper = new FinancialCard(txtGiftCardNumber.Text);
                            txtGiftCardNumber.Text = financialCardHelper.CardNumber;
                        }
                        catch (Exception ex)
                        {
                            _logger.WriteLogError(ex, "applyBtn_Click error for Swap GiftCard");
                        }

                        var cardResponse = GetGiftCardBalance(false);
                        lblEmptyCardNumberError.Visible = false;
                        if (cardResponse.AvailableBalance == 0 && cardResponse.Status == PaymentEngine.CardRequestStatus.Approved && !string.IsNullOrWhiteSpace(cardResponse.Message) && (cardResponse.Message.StartsWith("Current balance is") || cardResponse.Message.StartsWith("Gift card is not provisioned - number is auto provisioned enabled")))
                        {
                            lblAvailableBalance.Visible = true;
                            lblAvailableBalanceAmt.Text = "0";
                            lblApplyBalanceError.Text = cardResponse.Message;
                        }
                        else if (cardResponse.Status == PaymentEngine.CardRequestStatus.Declined)
                        {
                            lblAvailableBalance.Visible = false;
                            lblAvailableBalanceAmt.Text = "0";
                            lblApplyBalanceError.Text = cardResponse.Message;
                        }
                        else if (applyBalanceAmt <= 0)
                        {
                            lblApplyBalanceError.Text = "Please enter an amount greater than 0.";
                        }
                        else if (_ticketAmountDue == 0)
                        {
                            var ticketMenuItems = _dinerwareProvider.GetTicketMenuItems(_currentUserId, _currentOpenTicketId);
                            lblApplyBalanceError.Text = ticketMenuItems != null && ticketMenuItems.Count() > 0 ? "Amount due on this ticket is $0." : "Please add at least 1 item to apply Gift Card tender.";
                        }
                        else if (applyBalanceAmt > _ticketAmountDue)
                        {
                            lblApplyBalanceError.Text = "Entered amount exceeds ticket amount due of $" + Math.Round(_ticketAmountDue, 2);
                        }
                        else if (((_availableBalance >= applyBalanceAmt) || _availableBalance <= _ticketAmountDue))
                        {
                            if (AddTransactionToTicket(tenderId, _availableBalance >= applyBalanceAmt ? applyBalanceAmt : _availableBalance))
                            {
                                bLoyalGiftCardTenderExtension.currentOpenTicketId = string.Empty;
                                this.Close();
                            }
                            else
                                lblApplyBalanceError.Text = Messages.SERVICE_UNAVAILBLE_WARNING;
                        }                       
                    }
                    else
                    {
                        //bLoyal GiftCard Tenders not find in Dinerware
                        this.Hide();
                        frmTenderNotFoundWarning show = new frmTenderNotFoundWarning(Messages.GC_TENDER_NOT_CONFIGURED);
                        show.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    lblEmptyCardNumberError.Visible = true;
                    lblEmptyCardNumberError.Text = "Please enter gift card number.";
                }

            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal Gift Card applyBtn_Click");
            }
        }

        /// <summary>
        /// Add Payment Transaction to Ticket
        /// </summary>
        /// <param name="tenderId"></param>
        /// <param name="applyBalanceAmt"></param>
        private bool AddTransactionToTicket(int tenderId, decimal applyBalanceAmt)
        {
            bool result = false;
            try
            {
                _transactionCode = _paymentEngine.CardRedeem(txtGiftCardNumber.Text, _conFigHelper.GIFTCARD_TENDER_CODE, _cartExternalId, applyBalanceAmt);

                if (!string.IsNullOrWhiteSpace(_transactionCode))
                {
                    var changeResult = _dinerwareProvider.AddTransactionToTicket(_currentUserId, _currentOpenTicketId, new wsTransaction
                    {
                        PaymentAmount = applyBalanceAmt,
                        TenderType = Constants.BLOYALGIFTCARDTENDER,
                        TenderTypeID = tenderId,
                        RefNumber = _transactionCode
                    });

                    if (changeResult.Result == TicketChangeResult.Success)
                    {
                        _giftCardNumber = txtGiftCardNumber.Text;
                        decimal.TryParse(textApplyBalance.Text, out _applyAmount);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal Gift Card ApplyAllAvailableBalance");
            }
            return result;
        }


        /// <summary>
        /// Open KeyBoard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnkeyboard_Click(object sender, EventArgs e)
        {
            try
            {
                var frmGiftCard = new frmbLoyalGiftCardTender();
                frmGiftCard = this;

                this.Hide();

                frmLoadGiftCardKeyBoard frmTCBoard = new frmLoadGiftCardKeyBoard(ref frmGiftCard, _isTicketOpen, textApplyBalance.Text, txtGiftCardNumber.Text);
                frmTCBoard.ShowDialog();

                txtGiftCardNumber.Text = GiftCardNumber;
                textApplyBalance.Text = GiftCardAmount;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal GiftCardTender btnkeyboard_Click");
            }
        }

        #endregion
    }
}
