using System.ComponentModel.DataAnnotations;

namespace ShopClothes.WebApp.Models.AccountViewModel
{
    public class ExternalLoginViewModel
    {

        [Required]
        public string FullName { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
