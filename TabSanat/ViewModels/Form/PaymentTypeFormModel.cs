using System;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Display
{
    public class PaymentTypeFormModel
    {

        public Guid Id { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public string Name { get; set; }


    }
}
