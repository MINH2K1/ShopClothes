using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Domain.Entity
{
    [Table("Slides")]
    public class Slide: DomainEntity<int>
    
    {
        [MaxLength(250)]
        [Required]
        public string Name { set; get; }

        [MaxLength(250)]
        public string Description { set; get; }

        [MaxLength(250)]
        [Required]
        public string Image { set; get; }

        [MaxLength(250)]
        public string Url { set; get; }

        public int? DisplayOrder { set; get; }

        public bool Status { set; get; }

        public string Content { set; get; }

        [MaxLength(25)]
        [Required]
        public string GroupAlias { get; set; }
    
    }
}
