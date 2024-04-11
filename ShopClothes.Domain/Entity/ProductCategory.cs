using ShopClothes.Domain.Enum;
using ShopClothes.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Domain.Entity
{
    public class ProductCategory : DomainEntity<int>,
        IHasSeoMetaData, ISwitchable, ISortable, IDateTracking
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public int? HomeOrder { get; set; }

        public string Image { get; set; }

        public bool? HomeFlag { get; set; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }

        public virtual ICollection<Product> Products { set; get; }
      
    }
}
