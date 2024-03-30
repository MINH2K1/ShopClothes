using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.System;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Implemetation
{
    public class UserService:IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;   
        }
        public async Task<bool> AddAsync(AppUserViewModel userVm)
        {
            var user = new AppUser()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                PhoneNumber = userVm.PhoneNumber,
                DateCreated = DateTime.Now,
                FullName = userVm.FullName,
            };
            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (result.Succeeded && userVm.Roles.Count>0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                    await _userManager.AddToRolesAsync(appUser, userVm.Roles);
                return true;
            }
            return false;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result= await  _userManager.DeleteAsync(user);
            
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var viewModels = _mapper.Map<List<AppUserViewModel>>(users);
            return viewModels;
        }

        public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.UserName.Contains(keyword)
                || x.Email.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                BirthDay = x.BirthDay.ToString(),
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated

            }).ToList();
            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppUserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var role = await _userManager.GetRolesAsync(user);

            var userVm = _mapper.Map<AppUserViewModel>(user);
            userVm.Roles = role.ToList();
            return userVm;
               
        }

        public async Task UpdateAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());


            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user,
                userVm.Roles.Except(roles).ToArray());

            if (result.Succeeded)
            {
                string[] needRemoveRoles = roles.Except(userVm.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                //Update user detail
                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                await _userManager.UpdateAsync(user);
            }

        }
    }
}
