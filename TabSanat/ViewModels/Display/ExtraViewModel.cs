using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Display
{
    public class ExtraViewModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }
        //
        [Display(Name = "Alış Fiyatı")]
        public decimal PriceToBuy { get; set; }
        //
        [Display(Name = "Satış Fiyatı")]
        public decimal PriceToSell { get; set; }
        //
        public int Amount { get; set; }
        //
        public List<SaleViewModel> SalesThatIncludeExtra { get; set; }

    }
}
