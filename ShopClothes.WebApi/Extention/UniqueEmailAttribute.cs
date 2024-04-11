using Microsoft.AspNetCore.Identity;
using ShopClothes.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace ShopClothes.WebApi.Extention
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userManager = (UserManager<AppUser>)validationContext.GetService(typeof(UserManager<AppUser>));

            // Kiểm tra xem giá trị đã được nhập hay chưa
            if (value != null)
            {
                var email = value.ToString();

                // Tìm kiếm người dùng có địa chỉ email tương tự trong cơ sở dữ liệu
                var existingUser = userManager.FindByEmailAsync(email).Result;
                if (existingUser != null)
                {
                    // Nếu đã tồn tại người dùng với địa chỉ email này, trả về thông báo lỗi
                    return new ValidationResult("Email already exists.");
                }
            }

            // Giá trị hợp lệ
            return ValidationResult.Success;
        }
    }
    }

