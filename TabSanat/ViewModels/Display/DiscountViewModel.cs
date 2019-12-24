using System;
using System.ComponentModel.DataAnnotations;


namespace TabSanat.ViewModels.Display
{
    public class DiscountViewModel
    {
        [Display(Name = "İndirim Tipi")]
        public Guid? Id { get; set; }
        [Display(Name = "İndirim Tipi")]
        public string Name { get; set; }
        [Display(Name = "İndirim Miktarı")]
        public int AmountOfDiscount { get; set; }
        public bool IsFixedAmount { get; set; }
        public bool OnlyOnce { get; set; }

    }
}
