using System;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Form
{
    public class KurstanCikarFormModel
    {
        [Display(Name = "Kayıt")]
        public Guid Id { get; set; }
        //
        [Display(Name = "Öğrenci")]
        [Required(ErrorMessage = "Öğrenci seçmek zorunludur")]
        public Guid StudentId { get; set; }
        //
        public DateTime? LeaveDate { get; set; }
    }
}
