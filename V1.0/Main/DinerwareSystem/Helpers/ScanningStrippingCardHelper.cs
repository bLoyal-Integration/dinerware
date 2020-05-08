using System;
/*
||----------------------------------------------------------------------------
|| Copyright (c) 2005-2006 EVT Solutions, Inc., All Rights Reserved.
||----------------------------------------------------------------------------
*/

namespace DinerwareSystem.Helpers
{
    public enum FinancialCardEntryMode
    {
        Manual,
        Swiped
    }

    /// <summary>
    /// Provides information about a financial card (credit card, gift card, etc).
    /// </summary>
    public class FinancialCard
    {
        private FinancialCardEntryMode _entryMode;
        private string _track1;
        private string _track2;
        private string _track3;
        private string _cardNumber;
        private string _cardName;
        private DateTime _cardExpiration;
        private string _cardCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialCard"/> class.
        /// </summary>
        public FinancialCard()
        {
            _track1 = String.Empty;
            _track2 = String.Empty;
            _track3 = String.Empty;
            _cardNumber = String.Empty;
            _cardName = String.Empty;
            _cardExpiration = DateTime.MinValue;
            _cardCode = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialCard"/> class using
        /// the supplied card data.
        /// </summary>
        /// <remarks>
        /// The <paramref name="cardData"/> parameter can contain either the card number
        /// or card swipe data according to the ISO 7811-1
        /// </remarks>
        /// <param name="cardData">A string representing the financial card</param>
        ///
        public FinancialCard(string cardData)
            : this()
        {
            ParseCardData(cardData);
        }

        public string Track1
        {
            get { return _track1; }
            set { _track1 = value; }
        }

        public string Track2
        {
            get { return _track2; }
            set { _track2 = value; }
        }

        public string Track3
        {
            get { return _track3; }
            set { _track3 = value; }
        }

        public string CardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }

        public string CardName
        {
            get { return _cardName; }
            set { _cardName = value; }
        }

        public DateTime CardExpiration
        {
            get { return _cardExpiration; }
            set { _cardExpiration = value; }
        }

        public string CardCode
        {
            get { return _cardCode; }
            set { _cardCode = value; }
        }

        public FinancialCardEntryMode EntryMode
        {
            get { return _entryMode; }
        }

        /// <summary>
        /// %A{CardNumber}^{Name}^{DiscretionaryData}?;{CardNumber}={OptionalData}?
        /// </summary>
        private void ParseCardData(string cardData)
        {
            // Default to manual entry
            _cardNumber = cardData;
            _entryMode = FinancialCardEntryMode.Manual;

            int trackEnd = 0;

            // If card data starts with a % character, then we have track1 data
            if (cardData.StartsWith("%"))
            {
                _entryMode = FinancialCardEntryMode.Swiped;

                // Look for end sentinal.  If not found, use entire length of data
                trackEnd = cardData.IndexOf('?');
                if (trackEnd == -1)
                    trackEnd = cardData.Length - 1;

                // Extract track1 Data
                _track1 = cardData.Substring(2, trackEnd - 2);
                if (_track1.Length > 0)
                {
                    // Extract the card number from track1
                    int firstCaret = _track1.IndexOf('^');
                    if (firstCaret == -1)
                    {
                        _cardNumber = _track1;
                    }
                    else
                    {
                        _cardNumber = _track1.Substring(0, firstCaret);

                        int secondCaret = _track1.IndexOf('^', firstCaret + 1);
                        if (secondCaret != -1)
                        {
                            _cardName = _track1.Substring(firstCaret + 1, secondCaret - firstCaret - 1);
                            string expiration = _track1.Substring(secondCaret + 1, 4);
                            try
                            {
                                _cardExpiration = new DateTime(2000 + Convert.ToInt32(expiration.Substring(0, 2)), Convert.ToInt32(expiration.Substring(2, 2)), 1);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }

            // If card data contains a semi-colon, we have track2 data
            int track2Start = cardData.IndexOf(';', trackEnd);
            if (track2Start >= 0)
            {
                _entryMode = FinancialCardEntryMode.Swiped;
                // Look for the end sentinal.  If not found, use entire length of track
                trackEnd = cardData.IndexOf('?', track2Start);
                if (trackEnd == -1)
                    trackEnd = cardData.Length - 1;

                // Extract track2 data
                _track2 = cardData.Substring(track2Start + 1, trackEnd - (track2Start + 1));

                // Equal sign separates the card number from the expiration
                int fieldSeparator = _track2.IndexOf('=');
                if (fieldSeparator == -1)
                    fieldSeparator = _track2.Length;
                else
                {
                    if (fieldSeparator + 4 < _track2.Length)
                    {
                        string expiration = _track2.Substring(fieldSeparator + 1, 4);
                        try
                        {
                            _cardExpiration = new DateTime(2000 + Convert.ToInt32(expiration.Substring(0, 2)), Convert.ToInt32(expiration.Substring(2, 2)), 1);
                        }
                        catch
                        {

                        }
                    }
                }

                _cardNumber = _track2.Substring(0, fieldSeparator);
            }

            // If card data contains a +, we have track3 data
            int track3Start = cardData.IndexOf('+', trackEnd);
            if (track3Start >= 0)
            {
                _entryMode = FinancialCardEntryMode.Swiped;
                // Look for the end sentinal.  If not found, use entire length of track
                trackEnd = cardData.IndexOf('?', track3Start);
                if (trackEnd == -1)
                    trackEnd = cardData.Length - 1;

                // Extract track3 data
                _track3 = cardData.Substring(track3Start + 1, trackEnd - (track3Start + 1));
            }

        }
    }
}
