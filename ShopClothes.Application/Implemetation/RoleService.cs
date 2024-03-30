using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopClothes.Application.Interface;
using ShopClothes.Application.ViewModel.System;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure;
using ShopClothes.Infastructure.Dto;
using ShopClothes.Infastructure.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Implemetation
{
    public class RoleService:IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepository<Function, string> _functionRepository;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IRepository<Announcement, Guid> _announRepository;
        private readonly IRepository<AnnouncementUser, int> _announUserRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(RoleManager<AppRole> roleManager,
            IRepository<Function, string> functionRepository,
            IRepository<Permission, int> permissionRepository,
            IRepository<Announcement, Guid> announRepository,
            IRepository<AnnouncementUser, int> announUserRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _roleManager = roleManager;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _announRepository = announRepository;
            _announUserRepository = announUserRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(AnnouncementViewModel announcement, 
            List<AnnouncementUserViewModel> announcementUsers, 
            AppRoleViewModel userVm)
        {
           var role =await _roleManager.FindByNameAsync(userVm.Name.ToString());
            if (role == null)
            {
                var roleCreate= await _roleManager.CreateAsync(new AppRole()
                {
                    Name = userVm.Name,
                    Description = userVm.Description,
                });
                if (roleCreate.Succeeded)
                {
                    var announcementEntity = _mapper.Map<Announcement>(announcement);
                    announcementEntity.Id = Guid.NewGuid();
                    announcementEntity.DateCreated = DateTime.Now;
                    _announRepository.Add(announcementEntity);
                   
                    foreach (var item in announcementUsers)
                    {
                        var announcementUserEntity = _mapper.Map<AnnouncementUser>(item);
                        announcementUserEntity.AnnouncementId = announcementEntity.Id;
                        _announUserRepository.Add(announcementUserEntity);
                    }
                    return true;
                }
              else  return false;

            }
            return false;

        }

        public Task<bool> CheckPermission(int functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }

    

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            var roles = await  _roleManager.Roles.ToListAsync();
            var viewModels =  _mapper.Map<List<AppRoleViewModel>>(roles).ToList();
            return  viewModels;
        }

        public PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = _mapper.Map<List<AppRoleViewModel>>(query).ToList();
            var paginationSet = new PagedResult<AppRoleViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;

        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
         
            var result= _mapper.Map<AppRole, AppRoleViewModel>(role);

            return result;
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        select new PermissionViewModel()
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanCreate = p != null ? p.CanCreate : false,
                            CanDelete = p != null ? p.CanDelete : false,
                            CanRead = p != null ? p.CanRead : false,
                            CanUpdate = p != null ? p.CanUpdate : false
                        };
            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionVms, Guid roleId)
        {
            var permissions = _mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionVms);
            var oldPermission = _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToList();
            if (oldPermission.Count > 0)
            {
                _permissionRepository.RemoveMultiple(oldPermission);
            }
            foreach (var permission in permissions)
            {
                _permissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public async Task UpdateAsync(AppRoleViewModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            await _roleManager.UpdateAsync(role);
        }

      
    }
}
