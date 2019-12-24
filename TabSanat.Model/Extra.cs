using System;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Extra: BaseEntity
    {
        public string Name { get; set; }
        public decimal PriceToBuy { get; set; }
        public decimal PriceToSell { get; set; }
    }
}
