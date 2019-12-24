using System;
using System.Collections.Generic;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Registration: BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime StartToCourseDate { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Group Group { get; set; }
        public Guid? GroupId { get; set; }
        public decimal Price { get; set; }
        public decimal PaymentLeft { get; set; }
        public int NrOfLessonStudentWillJoin { get; set; }
        public int ExtraPaidLessonCount { get; set; }
        public Guid? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public DateTime? LeaveDate { get; set; }
        public List<Payment> Payments { get; set; }
        public Registration()
        {
            Payments = new List<Payment>();
        }
    }
}
