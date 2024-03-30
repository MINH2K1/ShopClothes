using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Domain.Entity
{
    [Table("Sizes")]
    public class Size: DomainEntity<int>
    {

        [MaxLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
