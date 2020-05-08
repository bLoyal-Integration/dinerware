using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bLoyal;
using bLoyal.Utilities;
using bLoyal.Connectors.LoyaltyEngine;
using System.Threading;
using Grid = bLoyal.Connectors.Grid;
using ConfigApp.PaymentEngine;
using System.ServiceModel;
using System.Net;


namespace ConfigApp.Provider
{
    public class PaymentEngineConnector
    {
        private string _accessKey;
        private string _webserviceUrl;
        private string _storeCode;
        private string _deviceCode;

        public PaymentEngineConnector(string loginDomain, string accessKey, string storeCode, string deviceCode, string customWebServiceUrl = null)
        {
            string baseUrl;

            if (!string.IsNullOrWhiteSpace(customWebServiceUrl))
                baseUrl = customWebServiceUrl;
            else if (loginDomain.StartsWith("localhost", StringComparison.InvariantCultureIgnoreCase))
                baseUrl = "http://" + loginDomain;
            else if (loginDomain.StartsWith("127.0.0.1", StringComparison.InvariantCultureIgnoreCase))
                baseUrl = "http://" + loginDomain;
            else
            {
                baseUrl = string.Format("https://{0}-ws.bloyal.com", loginDomain);
            }
            _webserviceUrl = String.Format("{0}/ws35/PaymentEngine.svc", baseUrl);
            _accessKey = accessKey;
            _storeCode = storeCode;
            _deviceCode = deviceCode;
        }

        //-----------------------------------------------------------------------------------------
        // GetPaymentEngineService - Gets the bLoyal gift and loyalty payment engine client proxy.
        //-----------------------------------------------------------------------------------------
        private PaymentEngineClient GetPaymentEngineService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            EndpointAddress address = new EndpointAddress(_webserviceUrl);

            BasicHttpBinding binding = new BasicHttpBinding();

            if (_webserviceUrl.Contains("https:"))
                binding.Security.Mode = BasicHttpSecurityMode.Transport;

            PaymentEngineClient svc = new PaymentEngineClient(binding, address);
            return svc;
          
        }



        //-----------------------------------------------------------------------------------------
        // GetCardBalance - Gets the current balance on a gift card, egift card, loyalty card, or certificate. 
        //-----------------------------------------------------------------------------------------
        public CardResponse GetCardBalance(string cardNumber, string tenderCode, Guid? customerUid = null)
        {
            var svc = GetPaymentEngineService();

            try
            {
                CardBalanceRequest request = new CardBalanceRequest();
                request.CustomerUid = customerUid ?? Guid.Empty;
                request.TenderCode = tenderCode;
                request.CardNumber = cardNumber;
                request.CardPin = "";   // Some clients require a PIN.
                request.Swiped = false; // Set to true when a card is swiped and false if manually keyed. 
                CardResponse response = svc.GetCardBalance(_accessKey, _storeCode, _deviceCode, request);     // StoreCode and DeviceCode are not required if you are using a device AccessKey.
                List<string> msg = new List<string>();
                if (response.Status == CardRequestStatus.Approved)
                {
                    msg.Add(string.Format("CardBalance() succeeded.  Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                else
                {
                    msg.Add(string.Format("CardBalance() FAILED.  Status={0} ", response.Status));
                    msg.Add(string.Format("    Message:{0} ", response.Message));
                }
                //tbResults.Lines = msg.ToArray();

                svc.Close(); // Closes the transport channel.
                svc = null;

                return response;
            }
            catch (System.Exception ex)
            {
                //tbResults.Text = string.Format("GetCardBalance() failed.  Exception: {0}", ex.ToString());
            }
            finally
            {
                if (svc != null)
                    svc.Abort();
            }
            return null;
        }

        //-----------------------------------------------------------------------------------------
        // CardAuthorize - Authorizes that the card has an adequate balance and puts a hold on the card. 
        //  Returns the tranactionCode for the request.
        //-----------------------------------------------------------------------------------------
        public string CardAuthorize(string cardNumber, string tenderCode, string transExternalId, decimal amount, System.Windows.Forms.TextBox tbResults, Guid? customerUid = null)
        {
            var transCode = "";
            var svc = GetPaymentEngineService();

            try
            {
                CardAuthorizeRequest request = new CardAuthorizeRequest();

                // Build up the card authorization request based on what the user/cashier entered.
                request.CardNumber = cardNumber;
                request.CustomerUid = customerUid ?? Guid.Empty;        // Optionally provide the bLoyal customer uid if you have it.
                request.CardPin = "";   // Some clients require a PIN.
                request.Swiped = false; // Set to true when a card is swiped and false if manually keyed. 
                request.Amount = amount;
                request.TenderCode = tenderCode;        // Used bLoyal to determine wehtehr it's a gift card, egift, coupon, on account, or loyalty tender.
                request.TransactionExternalId = transExternalId;    // Provide either your transaction number or the bLoyal TransactionToken.

                CardResponse response = svc.CardAuthorize(_accessKey, _storeCode, _deviceCode, request);    // StoreCode and DeviceCode are not required if you are using a device AccessKey.
                List<string> msg = new List<string>();
                if (response.Status == CardRequestStatus.Approved)
                {
                    transCode = response.TransactionCode;
                    msg.Add(string.Format("CardAuthorize() succeeded.  Authorization Code:{0} ", response.TransactionCode));
                    msg.Add(string.Format("    Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                else
                {
                    msg.Add(string.Format("CardAuthorize() FAILED.  Status={0} ", response.Status));
                    msg.Add(string.Format("    Message:{0} ", response.Message));
                    msg.Add(string.Format("    Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                tbResults.Lines = msg.ToArray();
            }
            catch (System.Exception ex)
            {
                tbResults.Text = string.Format("CardAuthorize() failed.  Exception: {0}", ex.ToString());
            }
            finally
            {
                if (svc != null)
                    svc.Abort();
            }
            return transCode;
        }

        //-----------------------------------------------------------------------------------------
        // CardRefund - Refunds an amount to a previously captured card transaction.
        //  Returns the tranactionCode for the request.
        //-----------------------------------------------------------------------------------------
        public string CardRefund(string captureTransCode, string transExternalId, decimal amount, System.Windows.Forms.TextBox tbResults)
        {
            var transCode = "";
            var svc = GetPaymentEngineService();

            try
            {
                CardRefundRequest request = new CardRefundRequest();

                request.Amount = amount;
                request.TransactionCode = captureTransCode;         // Transction code of the previously captured card amount.
                request.TransactionExternalId = transExternalId;    // Provide either your transaction number or the bLoyal TransactionToken.

                CardResponse response = svc.CardRefund(_accessKey, _storeCode, _deviceCode, request);   // StoreCode and DeviceCode are not required if you are using a device AccessKey.
                List<string> msg = new List<string>();
                if (response.Status == CardRequestStatus.Approved)
                {
                    transCode = response.TransactionCode;

                    msg.Add(string.Format("CardRefund() succeeded.  Transaction Code:{0}", response.TransactionCode));
                    msg.Add(string.Format("    Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                else
                {
                    msg.Add(string.Format("CardRefund() FAILED.  Status={0} ", response.Status));
                    msg.Add(string.Format("    Message:{0} ", response.Message));
                }
                tbResults.Lines = msg.ToArray();
            }
            catch (System.Exception ex)
            {
                tbResults.Text = string.Format("CardRefund() failed.  Exception: {0}", ex.ToString());
            }
            finally
            {
                if (svc != null)
                    svc.Abort();
            }

            return transCode;
        }


        //-----------------------------------------------------------------------------------------
        // CardReleaseAuth - Releases a previously held authorization.
        //  Returns the tranactionCode for the request.
        //-----------------------------------------------------------------------------------------
        public string CardReleaseAuth(string authTransCode, System.Windows.Forms.TextBox tbResults)
        {
            var transCode = "";
            var svc = GetPaymentEngineService();

            try
            {
                CardReleaseRequest request = new CardReleaseRequest();

                request.TransactionCode = authTransCode;

                CardResponse response = svc.CardRelease(_accessKey, _storeCode, _deviceCode, request);  // StoreCode and DeviceCode are not required if you are using a device AccessKey.
                List<string> msg = new List<string>();
                if (response.Status == CardRequestStatus.Approved)
                {
                    transCode = response.TransactionCode;

                    msg.Add(string.Format("CardRelease() succeeded.  Transaction Code:{0}", response.TransactionCode));
                    msg.Add(string.Format("    Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                else
                {
                    msg.Add(string.Format("CardRelease() FAILED.  Status={0} ", response.Status));
                    msg.Add(string.Format("    Message:{0} ", response.Message));
                }
                tbResults.Lines = msg.ToArray();

                svc.Close(); // Closes the transport channel.
                svc = null;
            }
            catch (System.Exception ex)
            {
                tbResults.Text = string.Format("CardRelease() failed.  Exception: {0}", ex.ToString());
            }
            finally
            {
                if (svc != null)
                    svc.Abort();
            }
            return transCode;
        }


        //-----------------------------------------------------------------------------------------
        // CardCapture - Captures a previously authorized card amount.
        //  Returns the tranactionCode for the request.
        //-----------------------------------------------------------------------------------------
        public string CardCapture(string authTransCode, string transExternalId, decimal amount, System.Windows.Forms.TextBox tbResults)
        {
            var transCode = "";
            var svc = GetPaymentEngineService();

            try
            {
                CardCaptureRequest request = new CardCaptureRequest();

                request.TransactionCode = authTransCode;    // Transaction code for the previously authorized transaction.
                request.Amount = amount;
                request.TransactionExternalId = transExternalId;
                CardResponse response = svc.CardCapture(_accessKey, _storeCode, _deviceCode, request);  // StoreCode and DeviceCode are not required if you are using a device AccessKey.
                List<string> msg = new List<string>();
                if (response.Status == CardRequestStatus.Approved)
                {
                    transCode = response.TransactionCode;

                    msg.Add(string.Format("CardCapture() succeeded.  Transaction Code:{0}", response.TransactionCode));
                    msg.Add(string.Format("    Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                else
                {
                    msg.Add(string.Format("CardCapture() FAILED.  Status={0} ", response.Status));
                    msg.Add(string.Format("    Message:{0} ", response.Message));
                }
                tbResults.Lines = msg.ToArray();
            }
            catch (System.Exception ex)
            {
                tbResults.Text = string.Format("CardCapture() failed.  Exception: {0}", ex.ToString());
            }
            finally
            {
                if (svc != null)
                    svc.Abort();
            }
            return transCode;
        }

        //-----------------------------------------------------------------------------------------
        // CardRedeem - Redeems an amount to a card in a single call (i.e. does an Auth/Capture in a single call. 
        //  Returns the tranactionCode for the request.
        //-----------------------------------------------------------------------------------------
        public string CardRedeem(string cardNumber, string tenderCode, string transExternalId, decimal amount, Guid? customerUid = null)
        {
            var transCode = "";
            var svc = GetPaymentEngineService();

            try
            {
                CardRedeemRequest request = new CardRedeemRequest();

                // Build up the card redeem request based on what the user/cashier entered.
                request.CardNumber = cardNumber;
                request.CustomerUid = customerUid ?? Guid.Empty;    // Optionally provide the bLoyal customer uid if you have it.
                request.CardPin = "";   // Some clients require a PIN.
                request.Swiped = false; // Set to true when a card is swiped and false if manually keyed. 
                request.Amount = amount;
                request.TenderCode = tenderCode;
                request.TransactionExternalId = transExternalId;    // Provide either your transaction number or the bLoyal TransactionToken.

                CardResponse response = svc.CardRedeem(_accessKey, _storeCode, _deviceCode, request);   // StoreCode and DeviceCode are not required if you are using a device AccessKey.
                List<string> msg = new List<string>();
                if (response.Status == CardRequestStatus.Approved)
                {
                    transCode = response.TransactionCode;

                    msg.Add(string.Format("CardRedeem() succeeded.  Transaction Code:{0}", response.TransactionCode));
                    msg.Add(string.Format("    Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                else
                {
                    msg.Add(string.Format("CardRedeem() FAILED.  Status={0} ", response.Status));
                    msg.Add(string.Format("    Message:{0} ", response.Message));
                }
                //tbResults.Lines = msg.ToArray();
            }
            catch (System.Exception ex)
            {
                //tbResults.Text = string.Format("CardRedeem() failed.  Exception: {0}", ex.ToString());
            }
            finally
            {
                if (svc != null)
                    svc.Abort();
            }
            return transCode;
        }

        //-----------------------------------------------------------------------------------------
        // CardCredit - Credits a card with an amount.  This is used to to activate or add to a card.
        //  Returns the tranactionCode for the request.
        //-----------------------------------------------------------------------------------------
        public string CardCredit(string cardNumber, string tenderCode, string transExternalId, decimal amount, System.Windows.Forms.TextBox tbResults, Guid? customerUid = null)
        {
            var transCode = "";
            var svc = GetPaymentEngineService();

            try
            {
                CardCreditRequest request = new CardCreditRequest();

                // Build up the card credit request based on what the user/cashier entered.
                request.CardNumber = cardNumber;
                request.CustomerUid = customerUid ?? Guid.Empty;        // Optionally provide the bLoyal customer uid if you have it.
                //                request.CardPin = "";   // Some clients require a PIN.
                request.Swiped = false; // Set to true when a card is swiped and false if manually keyed. 
                request.Amount = amount;
                request.TenderCode = tenderCode;    // This is optional.  Fill in if you have it to validate the card type with the tender.
                request.TransactionExternalId = transExternalId;    // Provide either your transaction number or the bLoyal TransactionToken.

                CardResponse response = svc.CardCredit(_accessKey, _storeCode, _deviceCode, request);   // StoreCode and DeviceCode are not required if you are using a device AccessKey.
                List<string> msg = new List<string>();
                if (response.Status == CardRequestStatus.Approved)
                {
                    transCode = response.TransactionCode;

                    msg.Add(string.Format("CardCredit() succeeded.  Transaction Code:{0}", response.TransactionCode));
                    msg.Add(string.Format("    Current Balance={0}, Available Balance={1} ", response.CurrentBalance, response.AvailableBalance));
                }
                else
                {
                    msg.Add(string.Format("CardCredit() FAILED.  Status={0} ", response.Status));
                    msg.Add(string.Format("    Message:{0} ", response.Message));
                }
                tbResults.Lines = msg.ToArray();

                svc.Close(); // Closes the transport channel.
                svc = null;
            }
            catch (System.Exception ex)
            {
                tbResults.Text = string.Format("CardCredit() failed.  Exception: {0}", ex.ToString());
            }
            finally
            {
                if (svc != null)
                    svc.Abort();
            }
            return transCode;
        }
    }
}
