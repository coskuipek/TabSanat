using System;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Display
{
    public class ExpenseViewModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Harcama Adı")]
        public string Name { get; set; }
        //
        public Guid? ExtraId { get; set; }
        //
        [Display(Name = "Kullanıcı")]
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
        public decimal PriceEach { get; set; }
        //
    }
}
