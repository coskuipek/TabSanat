using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TabSanat.ViewModels.Display
{
    public class SaleViewModel
    {
        public SaleViewModel()
        {
            SaleItems = new List<SaleItemViewModel>();
        }
        public Guid Id { get; set; }
        //
        [Display(Name = "Satış Tarihi")]
        [DataType(DataType.Date)]
        public DateTime DateOfSale { get; set; }
        //
        [Display(Name = "Öğrenci")]
        public Guid? StudentId { get; set; }
        //
        [Display(Name = "Öğrenci")]
        public string StudentName { get; set; }
        //
        [Display(Name = "Ödenen")]
        public decimal TotalPrice { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public Guid PaymentTypeId { get; set; }
        //
        [Display(Name = "Ödeme Tipi")]
        public string PaymentTypeName { get; set; }
        //
        public string ExtraName { get; set; }
        //
        [Display(Name = "Satışı Yapan")]
        public string AppUserName { get; set; }
        //
        public List<SaleItemViewModel> SaleItems { get; set; }
    }
}
