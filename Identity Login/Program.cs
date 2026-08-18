using Identity_Login.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Identity_Login.Models;
using Identity_Login.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Identity_Login.Services;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Net;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<QrCodeService>();

// ✅ OPTIMIZED: Use targeted SSL bypass only for HttpClient (not global)
builder.Services.AddSingleton<BlobStorageService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var httpClientHandler = new HttpClientHandler();

    // Only bypass SSL for development
    if (!builder.Environment.IsProduction())
    {
        httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => true;
    }

    return new BlobStorageService(config, httpClientHandler);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConverter, SynchronizedConverter>(provider =>
    new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<RazorViewToStringRenderer>();

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;

    options.SignIn.RequireConfirmedAccount = true;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = $"/Identity/Account/Login";
    opt.LogoutPath = $"/Identity/Account/Logout";
    opt.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Search}/{id?}");

app.Run();