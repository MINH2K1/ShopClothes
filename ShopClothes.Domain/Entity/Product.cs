using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopClothes.Domain.Interface;
using ShopClothes.Domain.Enum;

namespace ShopClothes.Domain.Entity
{
    [Table("Products")]
    public class Product : DomainEntity<int>, ISwitchable, IDateTracking
    {
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(255)]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        [Required]
        public decimal OriginalPrice { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public string Content { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public int? ViewCount { get; set; }

        [MaxLength(255)]
        public string Tags { get; set; }

        [MaxLength(255)]
        public string Unit { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { set; get; }

        public virtual ICollection<ProductTag> ProductTags { set; get; }

        public List <ProductImage> ProductImages{get; set;}
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }

        public Status Status { set; get; } = Status.Active;
    }
}
