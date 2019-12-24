using System;


namespace TabSanat.ViewModels.Display
{
    public class SaleItemViewModel
    {
        public Guid SaleId { get; set; }

        public Guid ExtraId { get; set; }
        public string ExtraName { get; set; }
        public int Amount { get; set; }
        public decimal PriceEach { get; set; }
    }
}
