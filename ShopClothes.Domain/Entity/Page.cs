using ShopClothes.Domain.Enum;
using ShopClothes.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Domain.Entity
{
     public class Page : DomainEntity<int>, ISwitchable, IDateTracking
    {

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [MaxLength(256)]
        [Required]
        public string Alias { set; get; }

        public string Content { set; get; }
        public Status Status { set; get; }
        public DateTime DateCreated { get ; set ; }
        public DateTime DateModified { get; set ; }
    }
}
