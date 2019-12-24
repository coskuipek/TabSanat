using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Display
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Ödeme Tarihi")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }
        //
        [Display(Name = "Öğrenci Adı")]
        public Guid? StudentId { get; set; }
        //
        [Display(Name = "Öğrenci Adı")]
        public string StudentName { get; set; }
        //
        [Display(Name = "Kurs Adı")]
        public string CourseName { get; set; }
        //
        public Guid CourseId { get; set; }
        //
        [Display(Name = "Ödenen")]
        public decimal Price { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public string PaymentTypeName { get; set; }
        //
        public ExtraViewModel Extra { get; set; }
        //
        public string ExtraName { get; set; }
        //
        public RegisterViewModel Register { get; set; }
        //
        [Display(Name = "Ödemeyi Alan")]
        public string UserName { get; set; }
        //
        public StudentViewModel Student { get; set; }
        //
        public List<ExtraViewModel> Extras { get; set; }
        //
        public bool IsGiveBack { get; set; }
        public int AmountOfLessons { get; set; }
        [Display(Name = "Taksit")]
        public string Taksit { get; set; }
    }
}
