using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Display
{
    public class PaymentTypeViewModel
    {
        public PaymentTypeViewModel()
        {
            Payments = new List<PaymentViewModel>();
        }

        public Guid Id { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public string Name { get; set; }
        //

        public List<PaymentViewModel> Payments { get; set; }
    }
}
