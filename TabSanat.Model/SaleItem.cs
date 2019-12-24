using System;

namespace TabSanat.Model
{
    public class SaleItem
    {
        public Guid SaleId { get; set; }
        public Sale Sale { get; set; }
        public Guid ExtraId { get; set; }
        public Extra Extra { get; set; }
        public int Amount { get; set; }
        public decimal PriceEach { get; set; }
    }
}