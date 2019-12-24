using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabSanat.ViewModels.Display
{
    public class DateListViewModel
    {
        public DateTime? StartOfRegister { get; set; }
        public int? AmountOfLessonsPaid { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<DateTime> PaidDates { get; set; }
        public List<DateTime> IncomingPaidDates { get; set; }
    }
}
