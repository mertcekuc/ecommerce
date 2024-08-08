using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models.ViewModels.Auth
{
    public abstract class BaseViewModel
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adını boş geçmeyiniz...")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen şifreyi boş geçmeyiniz...")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
