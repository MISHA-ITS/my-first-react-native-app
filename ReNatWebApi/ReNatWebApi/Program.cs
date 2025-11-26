using Core.Interfaces;
using Core.Models.Account;
using Core.Services;
using Domain;
using Domain.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyAPI;
using ReNatWebApi.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додаємо CORS політику
builder.Services.AddCors(options =>
{
    // Дозволяємо всі запити з будь-якого джерела
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Налаштування аутентифікації з використанням JWT
builder.Services.AddAuthentication(options =>
{
    // Вказуємо схему аутентифікації за замовчуванням
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Вказуємо схему виклику за замовчуванням
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Налаштування параметрів JWT Bearer
.AddJwtBearer(options =>
{
    // Вимикаємо вимогу HTTPS для метаданих (корисно для розробки)
    options.RequireHttpsMetadata = false;
    // Зберігаємо токен у контексті аутентифікації
    options.SaveToken = true;
    // Налаштування параметрів валідації токена
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Додаємо Swagger для документування API
var assemblyName = typeof(LoginModel).Assembly.GetName().Name;

// Налаштування Swagger з підтримкою JWT авторизації
builder.Services.AddSwaggerGen(opt =>
{
    // Додаємо коментарі XML у Swagger
    var fileDoc = $"{assemblyName}.xml";
    // Отримуємо повний шлях до XML файлу з коментарями
    var filePath = Path.Combine(AppContext.BaseDirectory, fileDoc);
    // Додаємо файл коментарів у налаштування Swagger
    opt.IncludeXmlComments(filePath);

    // Налаштування JWT авторизації у Swagger
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Вимагаємо авторизацію для доступу до захищених ендпоінтів
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<INoteCategoryService, NoteCategoryService>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMvc(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// Використовуємо CORS політику
app.UseCors("AllowAll");

var dir = builder.Configuration["ImagesDir"];
string path = Path.Combine(Directory.GetCurrentDirectory(), dir);
Directory.CreateDirectory(path);
// Налаштування для обслуговування статичних файлів (зображень)
app.UseStaticFiles(new StaticFileOptions
{
    //Вказуємо фізичний провайдер файлів і шлях запиту
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = $"/{dir}"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//await app.SeedData();

app.Run();
