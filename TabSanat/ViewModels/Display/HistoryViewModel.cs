using System;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Display
{
    public class HistoryViewModel
    {
        public Guid Id { get; set; }
        //
        public string UserId { get; set; }
        [Display(Name = "Kullanıcı")]
        public string UserName { get; set; }
        //
        [Display(Name = "İşlem Açıklaması")]
        public string Description { get; set; }
        //
        [Display(Name = "İşlem Zamanı")]
        public DateTime DateTime { get; set; }

    }
}
