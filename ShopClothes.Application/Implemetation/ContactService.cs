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
    public class ContactService : IContactService
    {
        private readonly IRepository<Contact, string> _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactService(IRepository<Contact, string> contactRepository,IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._contactRepository = contactRepository;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public void Add(ContactViewModel pageVm)
        {
            var page = _mapper.Map<ContactViewModel, Contact>(pageVm);
            _contactRepository.Add(page);
        }

        public void Delete(string id)
        {
            _contactRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ContactViewModel> GetAll()
        {
            var contact = _contactRepository.FindAll();
            return _mapper.Map<List<ContactViewModel>>(contact).ToList();
        }

        public PagedResult<ContactViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _contactRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var model = query.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var data = _mapper.Map<List<ContactViewModel>>(model).ToList();
            var paginationSet = new PagedResult<ContactViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ContactViewModel GetById(string id)
        {
            return _mapper.Map<Contact, ContactViewModel>(_contactRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(ContactViewModel pageVm)
        {
            var page = _mapper.Map<ContactViewModel, Contact>(pageVm);
            _contactRepository.Update(page);
        }
    }
}
