using System;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Payment : BaseEntity
    {
        public DateTime PaymentDate { get; set; }
        public DateTime TimeToShow { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid RegistrationId { get; set; }
        public Registration Registration { get; set; }
        public Guid PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Price { get; set; }
        public bool IsGiveBack { get; set; }
        public string Taksit { get; set; }
    }
}
