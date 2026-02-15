using CatalogServiceAPI.Data;
using CatalogServiceAPI.Interfaces;
using CatalogServiceAPI.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
//Cloudinary setup
var cloudinaryUrl = builder.Configuration["CloudinarySettings:Url"];
builder.Services.AddSingleton(new Cloudinary(cloudinaryUrl));

// Services
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
