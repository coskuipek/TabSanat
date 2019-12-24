using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Identity
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Adı")]
        public string FirstName { get; set; }
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }
        [Display(Name = "Erişim Yetkisi")]
        public string RoleName { get; set; }
    }

    public class AccountDetailsViewModel : AccountViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Erişim Yetkisi")]
        public string RoleId { get; set; }
    }

    public class AccountEditViewModel : AccountViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPasswordFirst { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPasswordFirst", ErrorMessage = "Şifreler aynı değil.")]
        [Display(Name = "Yeni Şifre Tekrar")]
        public string NewPasswordSecond { get; set; }
        [Required]
        [Display(Name = "Erişim Yetkisi")]
        public string RoleId { get; set; }
    }
}
