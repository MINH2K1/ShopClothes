using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.ViewModel.Product
{
    public class ProductQuantityViewModel
    {
        public int ProductId { get; set; }

        public int SizeId { get; set; }


        public int ColorId { get; set; }

        public int Quantity { get; set; }

        public ProductViewModel Product { get; set; }

        public SizeViewModel Size { get; set; }

        public ColorViewModel Color { get; set; }
    }
}
