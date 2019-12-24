using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TabSanat.ViewModels.Form
{
    public class CourseFormModel
    {
        public CourseFormModel()
        {
            Groups = new List<string>();
        }

        public Guid Id { get; set; }
        //
        [Display(Name = "Kursun Adı")]
        [Required(ErrorMessage = "Kurs adı zorunlu.")]
        public string Name { get; set; }
        //
        [Display(Name = "Tek Ders Fiyatı")]
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "Hatalı Fiyat")]
        public decimal OneLessonPrice { get; set; }
        //
        [Display(Name = "Kayıtlı Öğrenci")]
        public int CountOfStudents { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }
        //
        public Guid SeasonId { get; set; }
        //
        [Display(Name = "Kurs Günü")]
        [Required(ErrorMessage = "Kurs günü seçmek zorunlu.")]
        public DayOfWeek? DayOfWeek { get; set; }
        [Display(Name = "Kurs eklemeye devam et")]
        public bool ContinueAdding { get; set; }
        public List<string> Groups { get; set; }
    }
}
