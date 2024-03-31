namespace ShopClothes.WebApp.Service
{
     public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
