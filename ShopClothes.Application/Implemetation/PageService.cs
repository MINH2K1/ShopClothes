using AutoMapper;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.Blog;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure.Dto;
using ShopClothes.Infastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Implemetation
{
    public class PageService : IPageService
    {
        private readonly IRepository<Page, int> _pageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PageService(IRepository<Page, int> pageRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
         
        {
            this._pageRepository = pageRepository;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public void Add(PageViewModel pageVm)
        {
            var page = _mapper.Map<PageViewModel, Page>(pageVm);
            _pageRepository.Add(page);
        }

        public void Delete(int id)
        {
            _pageRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<PageViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task< List<PageViewModel>> GetAllAsync()
        {
            var result= await _pageRepository.FindAllAsync();
            return _mapper.Map<List<PageViewModel>>(result).ToList();
            
        }

        public PagedResult<PageViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _pageRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var model = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var data = _mapper.Map<List<PageViewModel>>(model).ToList();
            var paginationSet = new PagedResult<PageViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public PageViewModel GetByAlias(string alias)
        {
            return _mapper.Map<PageViewModel>(_pageRepository.FindSingle( x => x.Alias == alias));
        }

        public PageViewModel GetById(int id)
        {
            return _mapper.Map<Page, PageViewModel>(_pageRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(PageViewModel pageVm)
        {
            var page = _mapper.Map<PageViewModel, Page>(pageVm);
            _pageRepository.Update(page);
        }
    }
}
