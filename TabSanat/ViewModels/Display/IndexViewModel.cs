using System.Collections.Generic;


namespace TabSanat.ViewModels.Display
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            BirthdatesToday = new List<StudentViewModel>();
            BirthdatesThisWeek = new List<StudentViewModel>();
            BirthdatesThisMonth = new List<StudentViewModel>();
        }
        public ICollection<StudentViewModel> BirthdatesToday { get; set; }
        public ICollection<StudentViewModel> BirthdatesThisWeek { get; set; }
        public ICollection<StudentViewModel> BirthdatesThisMonth { get; set; }
        public List<CourseViewModel> CoursesList { get; set; }

    }
}
