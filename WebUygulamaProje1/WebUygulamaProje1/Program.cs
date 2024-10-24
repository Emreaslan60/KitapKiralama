using Microsoft.EntityFrameworkCore;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UygulamaDbContext>(options => options.UseSqlServer(builder
    .Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser,IdentityRole >(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<UygulamaDbContext>().AddDefaultTokenProviders();  //identity  role ekledik  //tokenproved yazdık onu email hata mesajı almamak için yazdık

builder.Services.AddRazorPages(); //razor sayfalarını ekliyoruz
//D�KKAT Yeni bir Repository s�n�f� olu�turdu�unuzda mutlaka burada seviceslere eklemelisiniz;
//_kitapTuruRepository nesnesi =>Dependency �njection (" yani sayfay� enjekte ediyoruz buraya ")
builder.Services.AddScoped<IKitapTuruRepository, KitapTuruRepository>();
// sql ile ba�lant� kuruyoruz 


// K�TAP SAYFASINIDA EKLED�K
builder.Services.AddScoped<IKitapRepository, KitapRepository>();

//K�RALAMA REPOS�TORYLER�N� YAN� SAYFALARINI EKLED�K
builder.Services.AddScoped<IKiralamaRepository, KiralamaRepository>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

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

app.MapRazorPages(); //razorları burada da ekledik

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
