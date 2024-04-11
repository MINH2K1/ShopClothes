using ShopClothes.WebApi.Extention;
using System.ComponentModel.DataAnnotations;

namespace ShopClothes.WebApi.ViewModel.Auth
{
    public class RegisterViewModel
    {
        [Required]

        public string UserName { get; set;}
        [EmailAddress]
        [Required]
        [UniqueEmail]
        public string Email { get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set;}
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
        public string Role { get; set; }
    }
}
