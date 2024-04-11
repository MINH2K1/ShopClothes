using Microsoft.AspNetCore.SignalR;
using ShopClothes.Application.ViewModel.System;

namespace ShopClothes.WebApi.Hubs
{
    public class ShopClothesHub:Hub
    {
        public async Task SendMessage(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
