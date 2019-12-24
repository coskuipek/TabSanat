using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TabSanat.ViewModels.Display
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Kursun Adı")]
        public string Name { get; set; }
        //
        [Display(Name = "Sezon")]
        public string SeasonName { get; set; }
        //
        [Display(Name = "Tek Ders ₺")]
        public decimal OneLessonPrice { get; set; }
        //
        [Display(Name = "Kurs Durumu")]
        public bool CourseEnded { get; set; }
        //
        [Display(Name = "Kayıtlı Öğrenci")]
        public int CountOfStudents { get; set; }
        //
        [Display(Name = "Kurs Günü")]
        public string DayOfWeekName { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }
        //
        [Display(Name = "Toplam Ders Adedi")]
        public int TotalNumberOfLessons { get; set; }
        //
        public List<DateTime> LessonDates { get; set; }
        //
        public List<RegisterViewModel> Registers { get; set; }
        [Display(Name = "Gruplar")]
        public string Groups { get; set; }
    }
}
