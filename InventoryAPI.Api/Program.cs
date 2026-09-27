using InventoryAPI.Data;
using InventoryAPI.Helpers;
using InventoryAPI.Repositories;
using InventoryAPI.Services;
using InventoryAPI.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var seqUrl = builder.Configuration.GetValue<string>("Seq:Url");
var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console();

if (!string.IsNullOrWhiteSpace(seqUrl))
{
    loggerConfig = loggerConfig.WriteTo.Seq(seqUrl);
}

Log.Logger = loggerConfig.CreateBootstrapLogger();

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.WriteTo.Console();
    if (!string.IsNullOrWhiteSpace(seqUrl))
    {
        lc.WriteTo.Seq(seqUrl);
    }
});

// Add services to the container.

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddSingleton<JwtHelper>();

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddControllers();

var key = Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
