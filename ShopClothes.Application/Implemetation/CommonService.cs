using AutoMapper;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.Common;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure.Interface;
using ShopClothes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Implemetation
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<Footer, string> _footerRepository;
        private readonly IRepository<SystemConfig, string> _systemConfigRepository;
        readonly IRepository<Slide, int> _slideRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       
        public CommonService(IRepository<Footer, string> footerRepository,
            IRepository<SystemConfig, string> systemConfigRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IRepository<Slide, int> slideRepository)
        {
            _mapper = mapper;
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
        }

        public FooterViewModel GetFooter()
        {
            return _mapper.Map<Footer, FooterViewModel>(_footerRepository.FindSingle(x => x.Id ==
            CommonConstants.DefaultFooterId));
        }

        public List<SlideViewModel> GetSlides(string groupAlias)
        {
            var slides = _slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias);
            return  _mapper.Map<List<SlideViewModel>>(slides).ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            return _mapper.Map<SystemConfig, SystemConfigViewModel>(_systemConfigRepository.FindSingle(x => x.Id == code));
        }
    }
}
