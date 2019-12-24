using System;
using System.Collections.Generic;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Sale : BaseEntity
    {
        public DateTime Date { get; set; }
        public Guid? StudentId { get; set; }
        public Student Student { get; set; }
        public Guid PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public List<SaleItem> SaleItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}