using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TabSanat.Model;

namespace TabSanat.ViewModels.Form
{
    public class PaymentFormModel
    {
        public Guid Id { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Ödeme Tarihi")]
        public DateTime PaymentDate { get; set; }
        //
        [Display(Name = "Öğrenci Adı")]
        [Required(ErrorMessage = "Öğrenci adı gerekli")]
        public Guid StudentId { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public Guid PaymentTypeId { get; set; }
        //
        [Display(Name = "Kurs")]
        [Required(ErrorMessage = "Kurs seçilmesi gerekli")]
        public Guid RegisterId { get; set; }
        //
        [Display(Name = "Ödenen")]
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "Hatalı Fiyat")]
        public decimal Price { get; set; }
        //
        public bool IsGiveBack { get; set; }
        public int Taksit { get; set; }

        public List<PaymentType> PaymentTypes { get; set; }
        public PaymentFormModel()
        {
            PaymentTypes = new List<PaymentType>();
        }
    }
}
