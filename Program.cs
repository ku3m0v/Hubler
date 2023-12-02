using System.Text;
using Hubler.BAL.Implementations;
using Hubler.BAL.Interfaces;
using Hubler.DAL.Implementations;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "Hubler",
        Description = "Supermarkets information system"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddSingleton<IEmployeeDAL, EmployeeDAL>();
builder.Services.AddSingleton<ISupermarketDAL, SupermarketDAL>();
builder.Services.AddSingleton<IAddressDAL, AddressDAL>();
builder.Services.AddSingleton<ICashRegisterDAL, CashRegisterDAL>();
builder.Services.AddSingleton<ILkRoleDAL, LkRoleDAL>();
builder.Services.AddSingleton<IBinaryContentDAL, BinaryContentDAL>();
builder.Services.AddSingleton<ICashRegisterDAL, CashRegisterDAL>();
builder.Services.AddSingleton<IInventoryDAL, InventoryDAL>();
builder.Services.AddSingleton<ILkStatusDAL, LkStatusDAL>();
builder.Services.AddSingleton<INonPerishableDAL, NonPerishableDAL>();
builder.Services.AddSingleton<IPerishableDAL, PerishableDAL>();
builder.Services.AddSingleton<IProductDAL, ProductDAL>();
builder.Services.AddSingleton<IProductOrderDAL, ProductOrderDAL>();
builder.Services.AddSingleton<ISaleDAL, SaleDAL>();
builder.Services.AddSingleton<ISaleDetailDAL, SaleDetailDAL>();
builder.Services.AddSingleton<IWarehouseDAL, WarehouseDAL>();
builder.Services.AddSingleton<ILogDAL, LogDAL>();
builder.Services.AddSingleton<IViewDAL<ExpiredInventory>, ExpiredInventoryDAL>();
builder.Services.AddSingleton<IViewDAL<ExpiredWarehouse>, ExpiredWarehouseDAL>();
builder.Services.AddSingleton<IViewDAL<Top5ProductsBySupermarket>, Top5ProductsBySupermarketDAL>();
builder.Services.AddSingleton<IViewDAL<SupermarketSalesSummary>, SupermarketSalesSummaryDAL>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings["securityKey"]))  
    };
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("*") // The origin of the frontend application
                .AllowAnyHeader()    
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("MyAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();