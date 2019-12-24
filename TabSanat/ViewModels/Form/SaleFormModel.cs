using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TabSanat.Overloads;
using TabSanat.ViewModels.Display;

namespace TabSanat.ViewModels.Form
{
    public class SaleFormModel
    {
        public SaleFormModel()
        {
            ExtrasThatAreSold = new List<ExtraViewModel>();
        }
        public Guid Id { get; set; }
        [Display(Name = "Satış Tarihi")]
        [Required(ErrorMessage = "Tarih zorunlu")]
        [DataType(DataType.Date)]
        public DateTime DateOfSale { get; set; }
        //
        [Display(Name = "Öğrenci Adı")]
        public Guid? StudentId { get; set; }
        //
        public string StudentName { get; set; }
        //
        public Guid AppUserId { get; set; }
        //
        public decimal TotalPrice { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        [Required(ErrorMessage = "Ödeme tipi seçin")]
        public Guid PaymentTypeId { get; set; }

        [ListMinimum(1, ErrorMessage = "En az bir ürün gerekli")]
        public List<ExtraViewModel> ExtrasThatAreSold { get; set; }
        public int ExtraAmountPicker { get; set; }

    }
}
