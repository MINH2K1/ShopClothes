using AutoMapper;
using AutoMapper.QueryableExtensions;
using OfficeOpenXml;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.Common;
using ShopClothes.Application.ViewModel.Product;
using ShopClothes.Domain.Entity;
using ShopClothes.Domain.Enum;
using ShopClothes.Infastructure;
using ShopClothes.Infastructure.Dto;
using ShopClothes.Infastructure.Interface;
using ShopClothes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Implemetation
{
    public class ProductService : IProductService
    {

        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<ProductTag, int> _productTagRepository;
        private readonly IRepository<ProductQuantity, int> _productQuantityRepository;
        private readonly IRepository<ProductImage, int> _productImageRepository;
        private readonly IRepository<WholePrice, int> _wholePriceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(
            IRepository<Product, int> productRepository,
            IRepository<Tag, string> tagRepository,
            IRepository<ProductQuantity, int> productQuantityRepository,
            IRepository<ProductImage, int> productImageRepository,
            IRepository<WholePrice, int> wholePriceRepository,
            IRepository<ProductTag, int> productTagRepository,
            IUnitOfWork unitOfWork
,
            IMapper mapper
            )
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productQuantityRepository = productQuantityRepository;
            _productTagRepository = productTagRepository;
            _wholePriceRepository = wholePriceRepository;
            _productImageRepository = productImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
                var product = _mapper.Map<ProductViewModel, Product>(productVm);
                foreach (var productTag in productTags)
                {
                    product.ProductTags.Add(productTag);
                }
                _productRepository.Add(product);
            }
            return productVm;
        }

        public void AddImages(int productId, string[] images)
        {
            _productImageRepository.RemoveMultiple(_productImageRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var image in images)
            {
                _productImageRepository.Add(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty
                });
            }
        }

        public void AddQuantity(int productId, List<ProductQuantityViewModel> quantities)
        {
            _productQuantityRepository.RemoveMultiple(_productQuantityRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var quantity in quantities)
            {
                _productQuantityRepository.Add(new ProductQuantity()
                {
                    ProductId = productId,
                    ColorId = quantity.ColorId,
                    SizeId = quantity.SizeId,
                    Quantity = quantity.Quantity
                });
            }
        }

        public void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _wholePriceRepository.RemoveMultiple(_wholePriceRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var wholePrice in wholePrices)
            {
                _wholePriceRepository.Add(new WholePrice()
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });
            }
        }

        public bool CheckAvailability(int productId, int size, int color)
        {
            var quantity = _productQuantityRepository.FindSingle(x => x.ColorId == color && x.SizeId == size && x.ProductId == productId);
            if (quantity == null)
                return false;
            return quantity.Quantity > 0;
        }

        public void Delete(int id)
        {
            _productRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            var result = _productRepository.FindAll(x => x.ProductCategory).ToList();
            var x = _mapper.Map<List<ProductViewModel>>(result);
            return x;

        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query =  await _productRepository.FindAllAsync(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var data = _mapper.Map<List<ProductViewModel>>(query);
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ProductViewModel GetById(int id)
        {
            return _mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }

        public List<ProductViewModel> GetHotProduct(int top)
        {
            var products = _productRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
             .OrderByDescending(x => x.DateCreated)
            .Take(top);
            var result = _mapper.Map<List<ProductViewModel>>(products);
            return result;
        }

        public List<ProductImageViewModel> GetImages(int productId)
        {
            var products = _productImageRepository.FindAll(x => x.ProductId == productId);
            return _mapper.Map<List<ProductImageViewModel>>(products).ToList();
        }

        public List<ProductViewModel> GetLastest(int top)
        {
            var products = _productRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                 .Take(top);
            return _mapper.Map<List<ProductViewModel>>(products).ToList();
        }

        public List<TagViewModel> GetProductTags(int productId)
        {
            var tags = _tagRepository.FindAll();
            var productTags = _productTagRepository.FindAll();

            var query = from t in tags
                        join pt in productTags
                        on t.Id equals pt.TagId
                        where pt.ProductId == productId
                        select new TagViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name
                        };
            return query.ToList();
        }

        public List<ProductQuantityViewModel> GetQuantities(int productId)
        {
            var productQuantity = _productQuantityRepository.FindAll(x => x.ProductId == productId);

            return _mapper.Map<List<ProductQuantityViewModel>>(productQuantity).ToList();
        }

        public List<ProductViewModel> GetRelatedProducts(int id, int top)
        {
            var product = _productRepository.FindById(id);
            var query = _productRepository.FindAll(x => x.Status == Status.Active
                && x.Id != id && x.CategoryId == product.CategoryId)
            .OrderByDescending(x => x.DateCreated)
            .Take(top);
            return _mapper.Map<List<ProductViewModel>>(query).ToList();
        }

        public List<ProductViewModel> GetUpsellProducts(int top)
        {
            throw new NotImplementedException();
        }

        public List<WholePriceViewModel> GetWholePrices(int productId)
        {
            var result = _wholePriceRepository.FindAll(x => x.ProductId == productId);

            return _mapper.Map<List<WholePriceViewModel>>(result).ToList();
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Product product;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    product = new Product();
                    product.CategoryId = categoryId;

                    product.Name = workSheet.Cells[i, 1].Value.ToString();

                    product.Description = workSheet.Cells[i, 2].Value.ToString();

                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;

                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var promotionPrice);

                    product.PromotionPrice = promotionPrice;
                    product.Content = workSheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = workSheet.Cells[i, 7].Value.ToString();


                    product.HotFlag = hotFlag;
                    bool.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;

                    product.Status = Status.Active;

                    _productRepository.Add(product);
                }
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();

            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag();
                        tag.Id = tagId;
                        tag.Name = t;
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    _productTagRepository.RemoveMultiple(_productTagRepository.FindAll(x => x.Id == productVm.Id).ToList());
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }
        }

        PagedResult<ProductViewModel> IProductService.GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
