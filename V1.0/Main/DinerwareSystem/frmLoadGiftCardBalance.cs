using bLoyal.Connectors.LoyaltyEngine;
using DinerwareSystem.Helpers;
using DinerwareSystem.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmLoadGiftCardBalance : Form
    {
        #region Private Member

        private int _currentOpenTicketId = 0;
        private string _cardNumber = string.Empty;
        private string _loadGiftItem = string.Empty;
        private string _giftCardNumber = string.Empty;

        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        Dinerware.Ticket _ticket = null;
        Dinerware.MenuItem _loadMenuItem = null;
        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;
        CalculatedCart _calculatedCart = null;
        LoggerHelper _logger = LoggerHelper.Instance;

        #endregion

        #region Public Member

        public string GiftCardNumber;   // Assign value by touch screen keyboard

        #endregion

        #region Public Method

        /// <summary>
        /// Load GiftCard
        /// </summary>
        public frmLoadGiftCardBalance()
        {
            InitializeComponent();
            lblEmptyCardNumberError.Visible = false;
        }

        /// <summary>
        /// Load GiftCard
        /// </summary>
        /// <param name="currentTicketid"></param>
        /// <param name="number"></param>
        /// <param name="ticket"></param>
        /// <param name="loadMenuItem"></param>
        /// <param name="calculatedCart"></param>
        public frmLoadGiftCardBalance(int currentTicketid, string number = "", Dinerware.Ticket ticket = null, Dinerware.MenuItem loadMenuItem = null, CalculatedCart calculatedCart = null, string loadGiftItem = "")
        {
            InitializeComponent();
            _currentOpenTicketId = currentTicketid;
            _cardNumber = number;
            lblEmptyCardNumberError.Visible = false;
            _ticket = ticket;
            _loadMenuItem = loadMenuItem;
            _calculatedCart = calculatedCart;
            _loadGiftItem = loadGiftItem;

            productNameLbl.Text = loadMenuItem.ItemName;
            productPriceLbl.Text = string.Format("${0}", Math.Round(loadMenuItem.Price, 2));
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Load GiftCard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLoadGiftCardBalance_Load(object sender, EventArgs e)
        {
            txtGiftCardNumber.Focus();
            this.ActiveControl = txtGiftCardNumber;
            txtGiftCardNumber.Text = _cardNumber;
        }

        public void CheckCardNumber()
        {
            if (!string.IsNullOrWhiteSpace(_giftCardNumber))
                return;

            frmNoGiftCardNumberMsg msg = new frmNoGiftCardNumberMsg();
            msg.ShowDialog();
        }

        /// <summary>
        /// Apply Load GiftCard Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyBtn_Click(object sender, EventArgs e)
        {
            try
            {
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
                        _logger.WriteLogError(ex, "FinancialCard for swap Load GiftCard");
                    }

                    lblEmptyCardNumberError.Visible = false;
                    if (!string.IsNullOrWhiteSpace(txtGiftCardNumber.Text))
                    {
                        CalculateTransactionHelper transaction = new CalculateTransactionHelper();

                        string checkLoadGiftCrad = transaction.CheckLoadGiftCard(txtGiftCardNumber.Text);

                        if (!string.IsNullOrWhiteSpace(checkLoadGiftCrad))
                        {
                            this.Hide();
                            frmLoadGiftCardWarning loadGiftCardWarning = new frmLoadGiftCardWarning(checkLoadGiftCrad);
                            loadGiftCardWarning.ShowDialog();
                            this.Show();
                        }
                        else
                        {
                            CalculateCartCommand request = null;
                            if (_calculatedCart != null && _calculatedCart.Cart != null && _calculatedCart.Cart.Lines != null && _calculatedCart.Cart.Lines.Any())
                            {
                                var line = _calculatedCart.Cart.Lines.ToList().Find(t => t.ProductName.Equals(_loadGiftItem, StringComparison.InvariantCultureIgnoreCase) && t.ProductExternalId.Equals(_loadMenuItem.TIID));
                                if (line != null)
                                {
                                    line.GiftCardNumber = txtGiftCardNumber.Text;
                                    _giftCardNumber = txtGiftCardNumber.Text;
                                }
                                else
                                {
                                    var newline = new CartLine { ProductName = _loadMenuItem.ItemName, ProductCode = _loadMenuItem.ID, GiftCardNumber = txtGiftCardNumber.Text, Price = _loadMenuItem.Price / _loadMenuItem.Quantity, Quantity = _loadMenuItem.Quantity, ExternalId = _loadMenuItem.ID, ProductExternalId = _loadMenuItem.TIID };
                                    _calculatedCart.Cart.Lines.Add(newline);
                                }
                                request = new CalculateCartCommand()
                                {
                                    Cart = _calculatedCart.Cart,
                                    Uid = _calculatedCart.Cart.Uid
                                };
                            }
                            else
                            {
                                var cart = new Cart { Lines = new List<CartLine>(), SourceExternalId = _currentOpenTicketId.ToString() };
                                var cartLineHelper = new CartLineHelper();

                                var lines = cartLineHelper.GetCartLines(_ticket);

                                if (lines != null && lines.Any())
                                {
                                    var line = lines.ToList().Find(t => t.ProductName.Equals(_loadGiftItem, StringComparison.InvariantCultureIgnoreCase) && t.ProductExternalId.Equals(_loadMenuItem.TIID));
                                    if (line != null)
                                    {
                                        line.GiftCardNumber = txtGiftCardNumber.Text;
                                        _giftCardNumber = txtGiftCardNumber.Text;
                                    }
                                    cart.Lines = lines;
                                    request = new CalculateCartCommand()
                                    {
                                        Cart = cart
                                    };
                                }
                            }

                            if (request != null)
                                _services.CalculateCart(request);

                            this.Close();
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal Load GiftCard applyBtn_Click");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoadGiftCard Cancel_Click");
            }
        }

        /// <summary>
        /// Keyboard Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            try
            {
                frmLoadGiftCardBalance frmLoadGC = new frmLoadGiftCardBalance();

                frmLoadGC = this;

                this.Hide();

                frmTouchscreenKeyboard frmTCBoard = new frmTouchscreenKeyboard(ref frmLoadGC, txtGiftCardNumber.Text);
                frmTCBoard.ShowDialog();

                txtGiftCardNumber.Text = GiftCardNumber;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "bLoyal Load GiftCard btnKeyboard_Click");
            }
        }

        #endregion

        private void lblEmptyCardNumberError_Click(object sender, EventArgs e)
        {

        }
    }
}
