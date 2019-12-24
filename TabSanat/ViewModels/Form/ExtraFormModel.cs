using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TabSanat.ViewModels.Display;

namespace TabSanat.ViewModels.Form
{
    public class ExtraFormModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage = "İsim girmek zorunlu")]
        public string Name { get; set; }
        //
        [Display(Name = "Alış Fiyatı")]
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "Hatalı Fiyat")]
        [Required(ErrorMessage = "Alış Fiyatı zorunlu")]
        public decimal PriceToBuy { get; set; }
        //
        [Display(Name = "Satış Fiyatı")]
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "Hatalı Fiyat")]
        [Required(ErrorMessage = "Satış Fiyatı zorunlu")]
        public decimal PriceToSell { get; set; }
        //
        public int Amount { get; set; }
        //
        public List<SaleViewModel> SalesThatIncludeExtra { get; set; }

    }
}
