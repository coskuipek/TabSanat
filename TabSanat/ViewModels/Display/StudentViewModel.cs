using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TabSanat.ViewModels.Display
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Adı")]
        public string FirstName { get; set; }
        //
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }
        //
        [Display(Name = "İsim")]
        public string FullName { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Eklenme Tarihi")]
        public DateTime RegisterDate { get; set; }
        //
        [DataType(DataType.ImageUrl)]
        public string PhotoPath { get; set; }
        //
        [Display(Name = "Telefon")]
        public string PhoneNo { get; set; }
        //
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //
        [Display(Name = "Adres")]
        public string Address { get; set; }
        //
        [Display(Name = "TC No")]
        public string TcKimlikNo { get; set; }
        //
        [Display(Name = "Baba Tam Adı")]
        public string FatherFullName { get; set; }
        //
        [Display(Name = "Baba Telefon")]
        public string FatherPhoneNo { get; set; }
        [Display(Name = "Baba İş")]
        public string FatherJob { get; set; }
        //
        [Display(Name = "Anne Tam Adı")]
        public string MotherFullName { get; set; }
        //
        [Display(Name = "Anne Telefon")]
        public string MotherPhoneNo { get; set; }
        [Display(Name = "Anne İş")]
        public string MotherJob { get; set; }
        //
        [Display(Name = "Varsa; Kurum")]
        public string Institution { get; set; }
        //
        [Display(Name = "Toplam Alacak")]
        public decimal Balance { get; set; }
        //
        public decimal StudentSignToCoursePrice { get; set; }
        //
        public decimal StudentPricePerLesson { get; set; }
        //
        public int NumberOfLatePayments { get; set; }
        //
        public int LessonsAfterStudentSignedUp { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Kursa Kayıt Tarihi")]
        public DateTime StudentsRegisterDateToCourse { get; set; }
        //
        public DateTime? StudentLeaveDateToCourse { get; set; }
        //
        public List<RegisterViewModel> Registers { get; set; }
        //
        public List<SaleViewModel> Sales { get; set; }
        //
        public List<PaymentViewModel> Payments { get; set; }
        //
        [Display(Name = "İndirim Tipi")]
        public string DiscountName { get; set; }
        //
        [Display(Name = "İndirim Miktarı")]
        public string DiscountAmount { get; set; }

    }
}
