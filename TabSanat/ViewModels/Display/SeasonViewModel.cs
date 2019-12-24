using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TabSanat.ViewModels.Display
{
    public class SeasonViewModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Sezon Adı")]
        public string Name { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç")]
        public DateTime StartDate { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş")]
        public DateTime EndDate { get; set; }
        //
        public bool SeasonEnded { get; set; }
        //
        [Display(Name = "Toplam Ay")]
        public int AmountOfMonths { get; set; }
        //
        public List<CourseViewModel> CoursesInSeason { get; set; }
    }
}
