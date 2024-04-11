﻿using AutoMapper;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.Product;
using ShopClothes.Domain.Entity;
using ShopClothes.Domain.Enum;
using ShopClothes.Infastructure.Dto;
using ShopClothes.Infastructure.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Implemetation
{
    public class BillService : IBillService
    {
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IRepository<BillDetail, int> _orderDetailRepository;
        private readonly IRepository<Color, int> _colorRepository;
        private readonly IRepository<Size, int> _sizeRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public BillService(IRepository<Bill, int> orderRepository,
            IRepository<BillDetail, int> orderDetailRepository,
            IRepository<Color, int> colorRepository,
            IRepository<Product, int> productRepository,
            IRepository<Size, int> sizeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Create(BillViewModel billVm)
        {
            var order = _mapper.Map<BillViewModel, Bill>(billVm);
            var orderDetails = _mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            foreach (var detail in orderDetails)
            {
                var product =  _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
            }
            order.BillDetails = orderDetails;
            _orderRepository.Add(order);
           
        }

        public void Update(BillViewModel billVm)
        {
            //Mapping to order domain
            var order = _mapper.Map<BillViewModel, Bill>(billVm);

            //Get order Detail
            var newDetails = order.BillDetails;

            //new details added
            var addedDetails = newDetails.Where(x => x.Id == 0).ToList();

            //get updated details
            var updatedDetails = newDetails.Where(x => x.Id != 0).ToList();

            //Existed details
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == billVm.Id);

            //Clear db
            order.BillDetails.Clear();

            foreach (var detail in updatedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Update(detail);
            }

            foreach (var detail in addedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Add(detail);
            }

            _orderDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList());

            _orderRepository.Update(order);
         
        }

        public void UpdateStatus(int billId, BillStatus status)
        {
            var order = _orderRepository.FindById(billId);
            order.BillStatus = status;
            _orderRepository.Update(order);
         
        }

        public async Task<List<SizeViewModel>> GetSizes()
        {
            var size = await _sizeRepository.FindAllAsync();
              
            return _mapper.Map<List<SizeViewModel>>(size);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword
            , int pageIndex, int pageSize)
        {
            var query = _orderRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
            }
            var totalRow = query.Count();
            var oderBy = query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

           var data=  _mapper.Map<List<BillViewModel>>(oderBy).ToList(); 
            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public BillViewModel GetDetail(int billId)
        {
            var bill = _orderRepository.FindSingle(x => x.Id == billId);
            var billVm = _mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _orderDetailRepository.FindAll(x => x.BillId == billId);
                var data = _mapper.Map<List<BillDetailViewModel>>(billDetailVm).ToList();
            billVm.BillDetails = data;
            return billVm;
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
          var bill= _orderDetailRepository
                .FindAll(x => x.BillId == billId, c => c.Bill, c => c.Color, c => c.Size, c => c.Product);
            return _mapper.Map<List<BillDetailViewModel>>(bill).ToList();
        }

        public List<ColorViewModel> GetColors()
        {
            var colors= _colorRepository.FindAll();
            return _mapper.Map<List<ColorViewModel>>(colors).ToList();
        }

        public BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = _mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _orderDetailRepository.Add(billDetail);
            return billDetailVm;
        }

        public void DeleteDetail(int productId, int billId, int colorId, int sizeId)
        {
            var detail = _orderDetailRepository.FindSingle(x => x.ProductId == productId
           && x.BillId == billId && x.ColorId == colorId && x.SizeId == sizeId);
            _orderDetailRepository.Remove(detail);
        }

        public ColorViewModel GetColor(int id)
        {
            return _mapper.Map<Color, ColorViewModel>(_colorRepository.FindById(id));
        }

        public SizeViewModel GetSize(int id)
        {
            return _mapper.Map<Size, SizeViewModel>(_sizeRepository.FindById(id));
        }

     
    }
}
