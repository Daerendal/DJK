using DJK.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DJK.Models;
using AspNetCore.ReCaptcha;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DJKConnectionString");

builder.Services.AddDbContext<DJKDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<DJKDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

string Base64UrlEncode(string v)
{
    throw new NotImplementedException();
}

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
})
   .AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
});

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.MaxModelValidationErrors = 50;
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        _=> "Pole jest wymagane");
});

builder.Services.AddReCaptcha(builder.Configuration.GetSection("GoogleReCaptcha"));


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithRedirects("/ErrorPage/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();
