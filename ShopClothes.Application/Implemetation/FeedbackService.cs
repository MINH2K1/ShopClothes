using AutoMapper;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.Common;
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
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<Feedback, int> _feedbackRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(
            IRepository<Feedback, int> feedbackRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Add(FeedbackViewModel feedbackVm)
        {
            var page = _mapper.Map<FeedbackViewModel, Feedback>(feedbackVm);
            _feedbackRepository.Add(page);
        }

        public void Delete(int id)
        {
            _feedbackRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<FeedbackViewModel> GetAll()
        {
            var result = _feedbackRepository.FindAll();
           return _mapper.Map<List<FeedbackViewModel>>(result).ToList();
            
        }

        public PagedResult<FeedbackViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _feedbackRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var model= _mapper.Map<List<FeedbackViewModel>>(data).ToList();
            var paginationSet = new PagedResult<FeedbackViewModel>()
            {
                Results = model,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public FeedbackViewModel GetById(int id)
        {
            return _mapper.Map<Feedback, FeedbackViewModel>(_feedbackRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(FeedbackViewModel feedbackVm)
        {
            var page = _mapper.Map<FeedbackViewModel, Feedback>(feedbackVm);
            _feedbackRepository.Update(page);
        }
    }
}
