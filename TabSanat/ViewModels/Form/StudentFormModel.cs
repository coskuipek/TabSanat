using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TabSanat.ViewModels.Display;

namespace TabSanat.ViewModels.Form
{
    public class StudentFormModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Adı")]
        [Required(ErrorMessage = "İsim zorunlu")]
        public string FirstName { get; set; }
        [Display(Name = "Soyadı")]
        [Required(ErrorMessage = "Soyadı zorunlu")]
        public string LastName { get; set; }
        [Display(Name = "İsim")]
        public string FullName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Eklenme Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        [Display(Name = "Resim")]
        public IFormFile Photo { get; set; }
        [DataType(DataType.ImageUrl)]
        public string PhotoPath { get; set; }
        [Display(Name = "Telefon")]
        public string PhoneNo { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name = "TC No")]
        public string TcKimlikNo { get; set; }
        [Display(Name = "Baba Tam Adı")]
        public string FatherFullName { get; set; }
        [Display(Name = "Baba Telefon")]
        public string FatherPhoneNo { get; set; }
        [Display(Name = "Baba İş")]
        public string FatherJob { get; set; }
        [Display(Name = "Anne Tam Adı")]
        public string MotherFullName { get; set; }
        [Display(Name = "Anne Telefon")]
        public string MotherPhoneNo { get; set; }
        [Display(Name = "Anne İş")]
        public string MotherJob { get; set; }
        [Display(Name = "Varsa; Kurum")]
        public string Institution { get; set; }
        public List<Guid> AssignToCourses { get; set; }
        public List<Guid> AddExtras { get; set; }
        [Display(Name = "Toplam Alacak")]
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "Hatalı Fiyat")]
        public decimal Balance { get; set; }
        public DiscountViewModel DiscountOfStudent { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Kursa Kayıt Tarihi")]
        public DateTime StartToCourseDate { get; set; } = DateTime.Now;


    }
}
