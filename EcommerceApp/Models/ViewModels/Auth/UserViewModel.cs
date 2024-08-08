using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models.ViewModels.Auth
{
    public class UserViewModel : BaseViewModel
    {
        
        
        [Required(ErrorMessage = "Lütfen emaili boş geçmeyiniz...")]
        [EmailAddress(ErrorMessage = "Lütfen email formatında bir değer belirtiniz...")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen adınızı boş geçmeyiniz...")]
        [StringLength(50, ErrorMessage = "Gecerlı soyad gırınız.", MinimumLength = 1)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lütfen soyadınızı boş geçmeyiniz...")]
        [StringLength(50, ErrorMessage = "Gecerlı soyad gırınız.", MinimumLength = 1)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

    }
}
