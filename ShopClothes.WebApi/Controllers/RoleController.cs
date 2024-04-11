using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShopClothes.Application.Implemetation;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.System;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure.Interface;
using ShopClothes.WebApi.Hubs;


namespace ShopClothes.WebApi.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IHubContext<ShopClothesHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RoleController(IRoleService roleService,
            IAnnouncementService announcementSevice,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
            _roleService = roleService;
            _mapper = mapper;
        }
        public async Task<IActionResult> getAllAsync()
        {
            var data = await _roleService.GetAllAsync();
            return Ok(data);
        }

    }
}
