using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunlu")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şifre zorunlu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
