using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using t4tea.data.Context;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.repository.Repositories;
using t4tea.service.advertise;
using t4tea.service.advertise.Dtos;
using t4tea.service.benifit;
using t4tea.service.benifit.Dtos;
using t4tea.service.category;
using t4tea.service.category.Dtos;
using t4tea.service.images;
using t4tea.service.images.Dtos;
using t4tea.service.product;
using t4tea.service.product.Dtos;
using t4tea.service.Authentication;
using t4tea.service.Email;
using t4tea.service.favourite;
using t4tea.service.favourite.Dtos;
using t4tea.service.Cart;
using t4tea.service.Order;
using t4tea.service.Cart.Dtos;
using t4tea.service.Review;
using t4tea.service.Review.Dtos;
using t4tea.service.shipAndDis.Dtos;
using t4tea.service.shipAndDis;
using t4tea.service.saveAndDeleteImage;
using t4tea.service.Flavour;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services
builder.Services.AddControllers();

builder.Services.AddDbContext<T4teaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<T4teaDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// 2. Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IBenifitServices, BenifitServices>();
builder.Services.AddScoped<IAdvertiseServices, AdvertiseServices>();
builder.Services.AddScoped<IImagesServices, ImagesServices>();
builder.Services.AddScoped<IFavouriteServices, FavouriteServices>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewsServices, ReviewsServices>();
builder.Services.AddScoped<IFlavourServices, FlavourServices>();
builder.Services.AddScoped<IShippingAndDiscountServices, ShippingAndDiscountServices>();
builder.Services.AddScoped<ISaveAndDeleteImageService, SaveAndDeleteImageService>();

// 3. AutoMapper
builder.Services.AddAutoMapper(typeof(CategoryProfile));
builder.Services.AddAutoMapper(typeof(productProfile));
builder.Services.AddAutoMapper(typeof(BenifitProfile));
builder.Services.AddAutoMapper(typeof(AdvertiseProfile));
builder.Services.AddAutoMapper(typeof(ImageProfile));
builder.Services.AddAutoMapper(typeof(FavouriteProfile));
builder.Services.AddAutoMapper(typeof(CartProfile));
builder.Services.AddAutoMapper(typeof(ReviewProfile));
builder.Services.AddAutoMapper(typeof(ShippingProfile));

// 4. Swagger (مع دعم JWT)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "T4Tea API", Version = "v1" });

    // دعم JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token like: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod());
});

var app = builder.Build();

// إعداد البورت (مفيد لـ Render أو أي سيرفر بيدي port في env variable)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// Middleware pipeline
app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

// ✅ استثناء swagger من التوثيق
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        context.User = new System.Security.Claims.ClaimsPrincipal();
    }

    await next();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
