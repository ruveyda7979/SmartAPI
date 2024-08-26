using DBSmartAPIManager.DAL;
using DBSmartAPIManager.DAL.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login"; // Login sayfas�
        options.AccessDeniedPath = "/Home/AccessDenied"; // Eri�im engellendi�inde y�nlendirilecek sayfa
        options.LogoutPath = "/Home/Logout"; // Logout i�lemi i�in y�nlendirme
        options.SlidingExpiration = true; // Oturum s�resini yenile
        options.ExpireTimeSpan = TimeSpan.FromMinutes(50); // Oturum s�resi (30 dakika)
    });


// Call Bootstrap's RepositoryInitialize method
Bootstrap.RepositoryInitialize(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
