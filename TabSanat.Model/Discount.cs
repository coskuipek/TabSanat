using System;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Discount: BaseEntity
    {
        public string Name { get; set; }
        public int AmountOfDiscount { get; set; }
        public bool IsFixedAmount { get; set; }
        public bool OnlyOnce { get; set; }
    }
}
