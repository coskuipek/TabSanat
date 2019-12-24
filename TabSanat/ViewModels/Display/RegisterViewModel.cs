using System;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Display
{
    public class RegisterViewModel
    {
        public Guid Id { get; set; }
        /////////////////////////////
        [Display(Name = "Öğrenci")]
        public string StudentName { get; set; }
        /////////////////////////////
        [Display(Name = "Öğrenci")]
        public Guid StudentId { get; set; }
        /////////////////////////////
        [DataType(DataType.Date)]
        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; }
        /////////////////////////////
        [DataType(DataType.Date)]
        [Display(Name = "Başlama Tarihi")]
        public DateTime StartToCourseDate { get; set; }
        /////////////////////////////
        [Display(Name = "Kurs")]
        public Guid CourseId { get; set; }
        /////////////////////////////
        [Display(Name = "Kurs")]
        public string CourseName { get; set; }
        /////////////////////////////
        [Display(Name = "Ders Başı")]
        public decimal Price { get; set; }
        /////////////////////////////
        public string PriceDisplay { get; set; }
        /////////////////////////////
        [Display(Name = "Kurs Kalanı")]
        public decimal PaymentLeft { get; set; }
        /////////////////////////////
        public string SeasonName { get; set; }
        /////////////////////////////
        [Display(Name = "Ayrılma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? LeaveDate { get; set; }
        /////////////////////////////
        [Display(Name = "İndirim")]
        public string DiscountName { get; set; } = "Yok";
        /////////////////////////////
        [Display(Name = "Katılacağı Ders")]
        public int NrOfLessonStudentWillJoin { get; set; }
        /////////////////////////////
        public int NumberOfLatePayments { get; set; }
        /////////////////////////////
        public string DayOfWeek { get; set; }
        /////////////////////////////
        [Display(Name = "Grup")]
        public string GroupName { get; set; }
    }
}
