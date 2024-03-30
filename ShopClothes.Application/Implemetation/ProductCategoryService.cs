using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.Product;
using ShopClothes.Domain.Entity;
using ShopClothes.Domain.Enum;
using ShopClothes.Infastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Implemetation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductCategoryService(
            IRepository<ProductCategory, int> productCategoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = _mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productCategoryVm;
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public async Task<List<ProductCategoryViewModel>> GetAllAsync()
        {
            var resultTask = _productCategoryRepository.FindAllAsync();
            var result = await resultTask;

            var orderedResult = result.OrderBy(x => x.ParentId);

            return _mapper.Map<List<ProductCategoryViewModel>>(orderedResult).ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var result = _productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
                var orderedResult = result.OrderBy(x => x.ParentId);
                return orderedResult.ProjectTo<ProductCategoryViewModel>(_mapper.ConfigurationProvider).ToList();
            }
            else
            {
                var result = _productCategoryRepository.FindAll();
                var orderedResult = result.OrderBy(x => x.ParentId);
                return orderedResult.ProjectTo<ProductCategoryViewModel>(_mapper.ConfigurationProvider).ToList();
            }
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.FindAll(x => x.Status == Status.Active
            && x.ParentId == parentId)
             .ProjectTo<ProductCategoryViewModel>(_mapper.ConfigurationProvider)
             .ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return _mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Products)
                  .OrderBy(x => x.HomeOrder)
                  .Take(top).ProjectTo<ProductCategoryViewModel>(_mapper.ConfigurationProvider);

            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Products = _productRepository
                //    .FindAll(x => x.HotFlag == true && x.CategoryId == category.Id)
                //    .OrderByDescending(x => x.DateCreated)
                //    .Take(5)
                //    .ProjectTo<ProductViewModel>().ToList();
            }
            return categories;
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = _mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Update(productCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _productCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
