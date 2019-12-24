using System;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Form
{
    public class ExpenseFormModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Harcama Adı")]
        public string Name { get; set; }
        //
        [Display(Name = "Satıştaki Ürün")]
        public Guid? ExtraId { get; set; }
        //
        public string UserName { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Tarih")]
        public DateTime Date { get; set; }
        //
        [Display(Name = "Adet")]
        public int Amount { get; set; }
        //
        [Display(Name = "Birim Fiyatı")]
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "Hatalı Fiyat")]
        public decimal PriceEach { get; set; }
        //
    }
}
