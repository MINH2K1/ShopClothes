using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopClothes.Application.AutoMapper;
using ShopClothes.Application.Implemetation;
using ShopClothes.Application.Interface;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure;
using ShopClothes.Infastructure.DbContext;
using ShopClothes.Infastructure.Interface;
using ShopClothes.WebApi.Hubs;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShopClothesDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
               o => o.MigrationsAssembly("ShopClothes.Infastructure")));
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<ShopClothesDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));

//Services
builder.Services.AddTransient<IProductCategoryService, ProductCategoryService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IBillService, BillService>();
builder.Services.AddTransient<IBlogService, BlogService>();
builder.Services.AddTransient<ICommonService, CommonService>();
builder.Services.AddTransient<IFeedbackService, FeedbackService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<IPageService, PageService>();
builder.Services.AddTransient<IAnnouncementService, AnnouncementService>();


builder.Services.AddSignalR();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audiences"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapHub<ShopClothesHub>("/shopclotheshub");
app.MapControllers();

app.Run();
