using ShopClothes.Domain.Enum;
using ShopClothes.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ShopClothes.Domain.Entity
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
       
        public string ?FullName { get; set; }

        public DateTime? BirthDay { set; get; }

        public decimal Balance { get; set; }

        public string ?Avatar { get; set; }

        public string ?RefreshToken { get; set; }
        public string ?Token { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateModified { get; set; }= DateTime.Now;
        public Status Status { get; set; }=Status.Active;
    }
}
