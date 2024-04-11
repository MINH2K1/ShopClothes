using ShopClothes.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ShopClothes.Domain.Test
{
    public class DomainTest
    {
        [Fact]
        public void AppUser_NotNull_Noparagram()
        {
            var appUser = new AppUser();
            Assert.NotNull(appUser);
        }
        [Fact]
        public void Announcement_NotNull_Noparagram()
        {
            var announcement = new Announcement();
            Assert.NotNull(announcement);
        }
        [Fact]
        public void AnnouncementUser_NotNull_Noparagram()
        {
            var announcementUser = new AnnouncementUser();
            Assert.NotNull(announcementUser);
        }
        [Fact]
        public void AppRole_NotNull_Noparagram()
        {
            var appRole = new AppRole();
            Assert.NotNull(appRole);
        }
        [Fact]
        public void Color_NotNull_Noparagram()
        {
            var color = new Color();
            Assert.NotNull(color);
        }
        [Fact]
        public void Footer_NotNull_Noparagram()
        {
            var footer = new Footer();
            Assert.NotNull(footer);
        }
      
      
        [Fact]
        public void Bill_NotNull_Noparagram()
        {
            var menu = new Bill();
            Assert.NotNull(menu);
        }
        [Fact]

        public void BillDetail_NotNull_Noparagram()
        {
            var menu = new BillDetail();
            Assert.NotNull(menu);
        }
        [Fact]

        public void Blog_NotNull_Noparagram()
        {
            var menu = new Blog();
            Assert.NotNull(menu);
        }
        [Fact]

        public void BlogTag_NotNull_Noparagram()
        {
            var menu = new BlogTag();
            Assert.NotNull(menu);
        }
        [Fact]

        public void Size_NotNull_Noparagram()
        {
            var menu = new Size();
            Assert.NotNull(menu);
        }
        [Fact]

        public void Slide_NotNull_Noparagram()
        {
            var menu = new Slide();
            Assert.NotNull(menu);
        }
        [Fact]

        public void Tag_NotNull_Noparagram()
        {
            var menu = new Tag();
            Assert.NotNull(menu);
        }
       
    
        [Fact]

        public void Product_NotNull_Noparagram()
        {
            var menu = new Product();
            Assert.NotNull(menu);
        }
        [Fact]

        public void ProductCategory_NotNull_Noparagram()
        {
            var menu = new ProductCategory();
            Assert.NotNull(menu);
        }
        [Fact]
        public void ProductQuantity_NotNull_Noparagram()
        {
            var menu = new ProductQuantity();
            Assert.NotNull(menu);
        }
        [Fact]
        public void ProductTag_NotNull_Noparagram()
        {
            var menu = new ProductTag();
            Assert.NotNull(menu);
        }
        [Fact]
        public void SystemConfig_NotNull_Noparagram()
        {
            var menu = new SystemConfig();
            Assert.NotNull(menu);
        }
        [Fact]
        public void Page_NotNull_Noparagram()
        {
            var menu = new Page();
            Assert.NotNull(menu);
        }
        [Fact]
        public void Language_NotNull_Noparagram()
        {
            var menu = new Language();
            Assert.NotNull(menu);
        }
        [Fact]
        public void ContactDetail_NotNull_Noparagram()
        {
            var menu = new Contact();
            Assert.NotNull(menu);
        }
        [Fact]
        public void WholePrice_NotNull_Noparagram()
        {
            var menu = new WholePrice();
            Assert.NotNull(menu);
        }
    }
}
