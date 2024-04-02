using ShopClothes.WebApi.ViewModel.Auth;

namespace ShopClothes.WebApi.ViewModel
{
    public class Response
    {
        public string ?message { get; set; }
        public bool Status { get; set; }
        public string ?RefreshToken{ get; set; }
        public string ?JwtToken { get; set; }
    }
}
