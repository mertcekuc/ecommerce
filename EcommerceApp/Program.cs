using Microsoft.AspNetCore.Identity;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models.Authentication;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EcommerceContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, AppRole>(_ =>
{
    _.Password.RequiredLength = 5; //En az ka� karakterli olmas� gerekti�ini belirtiyoruz.
    _.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunlulu�unu kald�r�yoruz.
    _.Password.RequireLowercase = false; //K���k harf zorunlulu�unu kald�r�yoruz.
    _.Password.RequireUppercase = false; //B�y�k harf zorunlulu�unu kald�r�yoruz.
    _.Password.RequireDigit = false; //0-9 aras� say�sal karakter zorunlulu�unu kald�r�yoruz.

    _.User.RequireUniqueEmail = true; //Email adreslerini tekille�tiriyoruz.
    _.User.AllowedUserNameCharacters = "abc�defghi�jklmno�pqrs�tu�vwxyzABC�DEFGHI�JKLMNO�PQRS�TU�VWXYZ0123456789-._@+"; //Kullan�c� ad�nda ge�erli olan karakterleri belirtiyoruz.
}).AddEntityFrameworkStores<EcommerceDbContext>();


//builder.Services.AddRazorPages();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseAuthentication();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
