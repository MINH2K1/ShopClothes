using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Domain.Entity
{
    [Table("Colors")]
    public class Color : DomainEntity<int>
    {

        [MaxLength(250)]
        public string Name
        {
            get; set;
        }

        [MaxLength(250)]
        public string Code { get; set; }
    }
}
