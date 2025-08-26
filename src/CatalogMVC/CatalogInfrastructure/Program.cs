using CatalogDomain.Model;
using CatalogInfrastructure;
using CatalogInfrastructure.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MovieDataPortServiceFactory>();
builder.Services.AddScoped<IReportService, WordReportService>();
//builder.Services.AddScoped<IImportService<Movie>, MovieImportService>();
////builder.Services.AddScoped<IExportService<Movie>, MovieExportService>();
//builder.Services.AddScoped<IDataPortServiceFactory<Movie>, MovieDataPortServiceFactory>();

//Auth functionality
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbCatalogContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")
    ));

builder.Services
    .AddIdentity<AspNetCoreUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
