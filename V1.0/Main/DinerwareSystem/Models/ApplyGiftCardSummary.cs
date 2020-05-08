namespace DinerwareSystem.Models
{
    public class ApplyGiftCardSummary
    {
        public decimal NetAmount { get; set; }
        public string GiftCardNumber { get; set; }
        public bool IsProvision { get; set; }
        public bool IsNewCard { get; set; }
    }
}
