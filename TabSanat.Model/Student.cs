using System;
using System.Collections.Generic;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Student: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public DateTime? BirthDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public string PhotoPath { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TcKimlikNo { get; set; }
        public string FatherFullName { get; set; }
        public string FatherPhoneNo { get; set; }
        public string FatherJob { get; set; }
        public string MotherFullName { get; set; }
        public string MotherPhoneNo { get; set; }
        public string MotherJob { get; set; }
        public string Institution { get; set; }
        public decimal Balance { get; set; }
        public Guid? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public List<Registration> Registers { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Sale> Sales { get; set; }

        public Student()
        {
            Registers = new List<Registration>();
            Payments = new List<Payment>();
            Sales = new List<Sale>();
        }

    }
}
