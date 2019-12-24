using System;
using System.ComponentModel.DataAnnotations;


namespace TabSanat.ViewModels.Display
{
    public class IncomingViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Tip")]
        public string Type { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Tarih")]
        public DateTime Date { get; set; }
        //
        [Display(Name = "Öğrenci")]
        public Guid? StudentId { get; set; }
        //
        [Display(Name = "Öğrenci")]
        public string StudentName { get; set; }
        //
        [Display(Name = "Ödenen")]
        public decimal TotalPrice { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public Guid PaymentTypeId { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public string PaymentTypeName { get; set; }
        //
        [Display(Name = "Satışı Yapan")]
        public string AppUserName { get; set; }
        [Display(Name = "Taksit")]
        public string Taksit { get; set; }
    }
}
