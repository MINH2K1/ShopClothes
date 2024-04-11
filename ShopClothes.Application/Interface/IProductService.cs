using Microsoft.AspNetCore.Http;
using ShopClothes.Application.ViewModel.Common;
using ShopClothes.Application.ViewModel.Product;
using ShopClothes.Infastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Interface
{
    public interface IProductService : IDisposable
    {
      
        void AddImages(int productId, List<string> images);
        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);

        void Update(ProductViewModel product);

        void Delete(int id);

        Task AddProduct(ProductViewModel productVm, List<IFormFile> file);

        void ImportExcel(string filePath, int categoryId);


        void Save();


        ProductViewModel GetById(int id);

        List<ProductQuantityViewModel> GetQuantities(int productId);

        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

        List<WholePriceViewModel> GetWholePrices(int productId);

        List<ProductViewModel> GetLastest(int top);

        List<ProductViewModel> GetHotProduct(int top);
        List<ProductViewModel> GetRelatedProducts(int id, int top);

        List<ProductViewModel> GetUpsellProducts(int top);

        List<TagViewModel> GetProductTags(int productId);
        bool CheckAvailability(int productId, int size, int color);
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        ProductViewModel Add(ProductViewModel product);
    }
    }
