using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUygulamaProje1.Models;




// vt entity fw tablo oluşturmak için ilgili model sınıflarını buraya eklemelisiniz..
namespace WebUygulamaProje1.Utility
{
    public class UygulamaDbContext: IdentityDbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }

        public DbSet<KitapTuru> KitapTürleri { get; set; }

        public DbSet<Kitap>Kitaplar { get; set; }  //Kitap sınıfından Kitaplar adlı tablo oluşturuyoruz. 

        public DbSet<Kiralama> Kiralamalar { get; set; }
 
       public DbSet<ApplicationUser> ApplicationUsers { get; set; }

       



    }





}
