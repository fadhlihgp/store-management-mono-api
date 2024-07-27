using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using store_management_mono_api.Context;
using store_management_mono_api.Middlewares;
using store_management_mono_api.Modules.AccountModule;
using store_management_mono_api.Modules.AccountModule._Interfaces;
using store_management_mono_api.Modules.AuthModule._Repositories;
using store_management_mono_api.Modules.AuthModule._Repositories._Interfaces;
using store_management_mono_api.Modules.CustomerModule._Repositories;
using store_management_mono_api.Modules.ImageModule._Repositories;
using store_management_mono_api.Modules.ImageModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._Repositories;
using store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;
using store_management_mono_api.Modules.OtherModule._Repositories;
using store_management_mono_api.Modules.OtherModule._Repositories._Interfaces;
using store_management_mono_api.Modules.ProductModule._Repositories;
using store_management_mono_api.Modules.ProductModule._Repositories._Interfaces;
using store_management_mono_api.Modules.PurchaseModule._Repositories;
using store_management_mono_api.Modules.PurchaseModule._Repositories._Interfaces;
using store_management_mono_api.Modules.StoreModule._Repositories;
using store_management_mono_api.Modules.StoreModule._Repositories._Interfaces;
using store_management_mono_api.Security;
using store_management_mono_api.Services;
using store_management_mono_api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("JWT Bearer", new OpenApiSecurityScheme
    {
        Description = "This is a JWT bearer authentication scheme",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = "JWT Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});
#endregion

// Add integration API with ApiUtility
builder.Services.AddHttpClient("ApiUtility", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["IntegrationApi:UtilityService"]);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IJwtUtil, JwtUtil>();
builder.Services.AddTransient<IForgetPasswordRepository, ForgetPasswordRepository>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<INoteOtherRepository, NoteOtherRepository>();
builder.Services.AddTransient<INoteIncomeExpenseRepository, NoteIncomeExpenseRepository>();
builder.Services.AddTransient<IDebtRepository, DebtRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<IApiUtilityService, ApiUtilityService>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IOtherRepository, OtherRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddTransient<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

#region LogSetting
var logPath = builder.Configuration["FilePath:LogFile"];
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
#endregion

#region JwtSetting
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});
#endregion
var app = builder.Build();
// set cors
app.UseCors(policyBuilder =>
    policyBuilder.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();