using ShopClothes.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.ViewModel.System
{
    public class AppRoleViewModel
    {
        public Guid? Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
    }
}
