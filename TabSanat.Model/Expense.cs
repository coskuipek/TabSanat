using System;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Expense: BaseEntity
    {
        public string Name { get; set; }
        public Guid? ExtraId { get; set; }
        public Extra Extra { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public decimal PriceEach { get; set; }
    }
}