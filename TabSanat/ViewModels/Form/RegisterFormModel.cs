using System;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Form
{
    public class RegisterFormModel

    {
        [Display(Name = "Kayıt")]
        public Guid Id { get; set; }
        //
        [Display(Name = "Öğrenci")]
        [Required(ErrorMessage = "Öğrenci seçmek zorunludur")]
        public Guid StudentId { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Kayıt Tarihi")]
        [Required(ErrorMessage = "Kayıt oluşturma tarihi zorunludur")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        //
        [DataType(DataType.Date)]
        [Display(Name = "Kursa Başlama Tarihi")]
        [Required(ErrorMessage = "Kursa başlama tarihi zorunludur")]
        public DateTime StartToCourseDate { get; set; } = DateTime.Now;
        //
        [Display(Name = "Kurs")]
        public Guid CourseId { get; set; }
        //
        public decimal Price { get; set; }
        //
        public decimal PaymentLeft { get; set; }
        //
        [Display(Name = "İndirim")]
        public Guid? DiscountId { get; set; }
        //
        //[Display(Name = "Kayıt Yapılacak Kurslar")]
        //[ListMinimum(1, ErrorMessage = "En az bir kurs gerekli")]
        //public List<Guid> AssignToCourses { get; set; }
        ////
        [Display(Name = "Grup")]
        public Guid? GroupId { get; set; }
        public DateTime? LeaveDate { get; set; }
        //

    }
}
