using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq.Expressions;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Ogretmen + "," + UserRoles.Role_Admin)]
    // burada da yetkilendirme yaptık yani bu sayfaya kimin girebileceğini belirttik
    public class KiralamaController : Controller
    {  /* private readonly UygulamaDbContext _uygulamaDbContext;*/ //alt çizgi koyuyruz ki her yerde çağırabilelim sürekli new demeyelim !!tek bir tablomuz varsa bunu yazmak daha mantıklı

        private readonly IKiralamaRepository _kiralamapRepository;
        private IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
       
         //çoklu tablolarda bu şekilde repositoryler oluşturup burada çağırabiliriz
        private readonly IWebHostEnvironment _webHostEnvironment;
       
       


        //kitap turu repositoryden kitapların hepsini çektik
        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

           // List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();

            //veri tabanından kitap türlerini çekerek bu listeye getirecek
       return View(objKiralamaList);  //aşağıda yazdığımız redirectoaction metodu buraya geliyor yani işlemden sonra sayfada oluşan verileri gösteriyor 
        }
        public IActionResult EkleGuncelle(int? id)  //parametre yazıyoruz ki parametre alabilsin veya almadan da çalışsın
        {

            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
           .Select(k => new SelectListItem 
           {
               Text = k.KitapAdi,
               Value = k.Id.ToString()
           });

            ViewBag.KitapTuruList = KitapList;  //viewbag ile controllerdan viewe aktarma yapıyoruz

            if(id==null || id == 0)
            {   //ekle
                return View();
            }
            else
            { //güncelle
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre //parantez içinde filtreleme işlemi yapıyoruz
                if (kiralamaVt == null)
                {
                    return NotFound();

                }
                return View(kiralamaVt);
            }
          
        }

        [HttpPost] //bunu yazma sebebim ekle klasörüne yazdığımız post tan dolayı ben ekle butonuna bastığımda eklenin bağlı olduğu controllera yani buraya gelecek ve ekle isminde ama actionu httppost olan bölüme girecek 
        public IActionResult EkleGuncelle(Kiralama kiralama) // parantez kısmına yazmak istediğimiz nesneyi yani ekle bölümünde oluşturduğumuz formu yazmamız gerekir.
        {
            

            if (ModelState.IsValid)
            {

              
               

                if (kiralama.Id == 0)
                {
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Yeni kitap kiralama kaydı başarıyla oluşturuldu";
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "kitap kiralama güncellemesi başarılı";
                }

              //  _kitapRepository.Ekle(kitap); //vt ye yeni bir kayıt atacağını söylüyoruz
                _kiralamaRepository.Kaydet();
                //burda da onaylıyoruz bunu yapmamızın sebebi her yapmasını istediğimiz şeyi tekrar tekrar savechange etmemek için en sona bir tane KAYDET yazarak fazla kod yazmaktan kurtuluyoruz !bunu yazmazssak bilgiler vt ye eklenmez.
                TempData["basarili"] = "Kitap kiralama başarılı";
                return RedirectToAction("Index", "Kiralama"); //bu kodu kayıt ekledikten sonra tekrar yukarıya yani indexe gelmesi için yazdık}

            }
            return View();
        }

        
        //GET ACTİON
        public IActionResult Sil(int? id)  //bu kodlarda kitapları veritabanından ve sayfadan siliyor 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
           .Select(k => new SelectListItem
           {
               Text = k.KitapAdi,
               Value = k.Id.ToString()
           });

            ViewBag.KitapTuruList = KitapList;
            if (kiralamaVt == null)
            {
                return NotFound();

            }
            return View(kiralamaVt);
        }
        [HttpPost,ActionName("Sil")]
        public IActionResult SilPost( int? id)
        {
            Kiralama? kiralama= _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
            {
                return NotFound();
            }
            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["Sil"] = " Kayıt Silme İşlemi Başarılı! ";
            return RedirectToAction("index", "Kiralama");

          
          

        }
        //yukarıda ve diğer control kodlarında genel olarak şunu yapıyoruz önece idsinden vt den buluyoruz null olmasın diye soru işareti koyuyoruz if sorgusu ile eğer id si bulunmayan veya 0 olan varsa notfound metoduyla error veriyoruz varsa bulup devam ediyoruz daha sonra listden buluyoruz eğer listede yoksa yine error veriyoruz db ve hangi sayfayı istiyorsak adını yazarak ne olması gerektiğini yazıyoruz mesela yukarıda remove demiş sonra bunu savechange ile kaydediyoruz mutlaka sonrasında return redirectvs ile tekrar sayfaya geri dönderiyoruz.


    }

}
