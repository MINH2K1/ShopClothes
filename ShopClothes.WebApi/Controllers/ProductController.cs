using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.Product;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure.Interface;
using ShopClothes.WebApi.ViewModel.Auth;
using System.Security.Claims;
//using Microsoft.AspNetCore.Hosting;
namespace ShopClothes.WebApi.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
       
        private readonly IProductCategoryService _productCategoryService;
      //  private readonly IHostingEnvironment _hostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IProductService productService
                                
            )
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
          
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductViewModel productVm, List<IFormFile> files)
        {         
            await  _productService.AddProduct(productVm,files);     
                _productService.Save();
                return new OkObjectResult(productVm);
        }





















        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productService.GetAll();
            return new OkObjectResult(model);
        }
        [HttpPost]
        public IActionResult GetId(int id)
        {
            var model = _productService.GetById(id);
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _productService.Delete(id);
                _productService.Save();

                return new OkObjectResult(id);
            }
        }
        [HttpPost]
        public IActionResult SaveQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            _productService.AddQuantity(productId, quantities);
            _productService.Save();
            return new OkObjectResult(quantities);
        }
        [HttpGet]
        public IActionResult GetQuantities(int productId)
        {
            var quantities = _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }
        [HttpPost]
        public IActionResult SaveImages(int productId, List<string> images)
        {
          //  _productService.AddImages(productId,List<string> images);
            _productService.Save();
            return new OkObjectResult(images);
        }
        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            var images = _productService.GetImages(productId);
            return new OkObjectResult(images);
        }
        [HttpPost]
        public IActionResult SaveWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _productService.AddWholePrice(productId, wholePrices);
            _productService.Save();
            return new OkObjectResult(wholePrices);
        }
        [HttpGet]
        public IActionResult GetWholePrices(int productId)
        {
            var wholePrices = _productService.GetWholePrices(productId);
            return new OkObjectResult(wholePrices);
        }

    }

}
