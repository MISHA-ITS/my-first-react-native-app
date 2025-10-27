using Core.Interfaces;
using Core.Models.Account;
using Core.Services;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyAPI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ������ CORS �������
builder.Services.AddCors(options =>
{
    // ���������� �� ������ � ����-����� �������
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

// ������������ �������������� � ������������� JWT
builder.Services.AddAuthentication(options =>
{
    // ������� ����� �������������� �� �������������
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // ������� ����� ������� �� �������������
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// ������������ ��������� JWT Bearer
.AddJwtBearer(options =>
{
    // �������� ������ HTTPS ��� ��������� (������� ��� ��������)
    options.RequireHttpsMetadata = false;
    // �������� ����� � �������� ��������������
    options.SaveToken = true;
    // ������������ ��������� �������� ������
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

// ������ Swagger ��� �������������� API
var assemblyName = typeof(LoginModel).Assembly.GetName().Name;

// ������������ Swagger � ��������� JWT �����������
builder.Services.AddSwaggerGen(opt =>
{
    // ������ �������� XML � Swagger
    var fileDoc = $"{assemblyName}.xml";
    // �������� ������ ���� �� XML ����� � �����������
    var filePath = Path.Combine(AppContext.BaseDirectory, fileDoc);
    // ������ ���� ��������� � ������������ Swagger
    opt.IncludeXmlComments(filePath);

    // ������������ JWT ����������� � Swagger
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // �������� ����������� ��� ������� �� ��������� ��������
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

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// ������������� CORS �������
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var dir = builder.Configuration["ImagesDir"];
string path = Path.Combine(Directory.GetCurrentDirectory(), dir);
Directory.CreateDirectory(path);

// ������������ ��� �������������� ��������� ����� (���������)
app.UseStaticFiles(new StaticFileOptions
{
    //������� �������� ��������� ����� � ���� ������
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = $"/{dir}"
});

//await app.SeedData();

app.Run();
