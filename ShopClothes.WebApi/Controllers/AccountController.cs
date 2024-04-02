using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopClothes.Domain.Entity;
using ShopClothes.WebApi.ViewModel;
using ShopClothes.WebApi.ViewModel.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopClothes.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        [HttpPost]
    
    public async Task< Response> Login([FromBody] LoginViewModel model)
        {

            var user = _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
            if (user.Result.Succeeded)
            {
                if (model.RememberMe)
                {
                    string token = await GenerateToken(model.Email);
                    string refreshToken = GenerateRefreshToken();
                    return new Response
                    {
                        Status = true,
                        message = "Login success",
                        JwtToken = token,
                        RefreshToken = refreshToken
                    };
                }
                else
                    return new Response
                    {
                        Status = true,
                        message = "Login success",
                    };
            }
            else
            {
                return new Response
                {
                    Status = false,
                    message = "Login fail",
                };
            }             
      }

    private async Task<string> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userRoles = _userManager.GetRolesAsync(user);
            IEnumerable<Claim> claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRoles.Result.FirstOrDefault())
               
            };
            SecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

             SigningCredentials signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256Signature);


             var SecurityToken = new JwtSecurityToken(
            claims: claim,
            expires: DateTime.Now.AddMinutes(120),
           issuer: _config.GetSection("Jwt:ValidIssuer").Value,
           audience: _config.GetSection("Jwt:ValidAudiences").Value,
           signingCredentials: signingCredentials
            );
         string tokenString = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return tokenString;
            }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }

    public async Task<LoginResponse> RefreshToken(RefreshTokenModel model)
    {
        ClaimsPrincipal principal = GetTokenPrincipal(model.JwtToken);
        LoginResponse response = new LoginResponse();
        if (principal?.Identity?.Name == null)
        {
            return response;
        }
        AppUser user = await _userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null || model.RefreshToken != user.RefreshToken || user.RefreshTokenExpiry < DateTime.Now)
        {
            return response;
        }
        response.IsLogedIn = true;
        response.JwtToken = await GenerateToken(user.Email);
        response.RefreshToken = GenerateRefreshToken();
        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiry = DateTime.Now.AddHours(12.0);
        await _userManager.UpdateAsync(user);
        return response;
    }

    private ClaimsPrincipal? GetTokenPrincipal(string token)
    {
    var securityKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
    var validation = new TokenValidationParameters
    {
        IssuerSigningKey = securityKey,
        ValidateLifetime = false,
        ValidateActor = false,
        ValidateIssuer = false,
        ValidateAudience = false,
    };
    return new JwtSecurityTokenHandler().ValidateToken(token,validation, out _);
    }
        public async Task<IActionResult> Logout(Guid id)
        {
            AppUser user= await _userManager.FindByIdAsync(id.ToString());
            user.RefreshToken = null;
            user.Token = null;
            await _userManager.UpdateAsync(user);
            _signInManager.SignOutAsync();
            return Ok();
        }
  }
}
