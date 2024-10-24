using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{

   
    public class KitapController : Controller
    {
        /* private readonly UygulamaDbContext _uygulamaDbContext;*/ //alt çizgi koyuyruz ki her yerde çağırabilelim sürekli new demeyelim !!tek bir tablomuz varsa bunu yazmak daha mantıklı
        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository; //çoklu tablolarda bu şekilde repositoryler oluşturup burada çağırabiliriz
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IWebHostEnvironment webHostEnvironment;


        //kitap turu repositoryden kitapların hepsini çektik
        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }



        [Authorize(Roles = "Admin,Ogrenci")] // burada da yetkilendirme yaptık yani bu sayfaya kimin girebileceğini belirttik
        public IActionResult Index()
        {

           // List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();
            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"KitapTuru").ToList();



            //veri tabanından kitap türlerini çekerek bu listeye getirecek
            return View(objKitapList);  //aşağıda yazdığımız redirectoaction metodu buraya geliyor yani işlemden sonra sayfada oluşan verileri gösteriyor 
        }

        [Authorize(Roles = UserRoles.Role_Admin)] // burada da yetkilendirme yaptık yani bu sayfaya kimin girebileceğini belirttik
        public IActionResult EkleGuncelle(int? id)  //parametre yazıyoruz ki parametre alabilsin veya almadan da çalışsın
        {

            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll()
           .Select(k => new SelectListItem 
           {
               Text = k.Ad,
               Value = k.Id.ToString()
           });

            ViewBag.KitapTuruList = KitapTuruList;  //viewbag ile controllerdan viewe aktarma yapıyoruz

            if(id==null || id == 0)
            {   //ekle
                return View();
            }
            else
            { //güncelle
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre //parantez içinde filtreleme işlemi yapıyoruz
                if (kitapVt == null)
                {
                    return NotFound();

                }
                return View(kitapVt);
            }
          
        }
        [Authorize(Roles = UserRoles.Role_Admin)] // burada da yetkilendirme yaptık yani bu sayfaya kimin girebileceğini belirttik

        [HttpPost] //bunu yazma sebebim ekle klasörüne yazdığımız post tan dolayı ben ekle butonuna bastığımda eklenin bağlı olduğu controllera yani buraya gelecek ve ekle isminde ama actionu httppost olan bölüme girecek 
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file) // parantez kısmına yazmak istediğimiz nesneyi yani ekle bölümünde oluşturduğumuz formu yazmamız gerekir.
        {
            

            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath; //dosya yolunu belirtiyoruz
                string KitapPath = Path.Combine(wwwRootPath, @"img");
                if(file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(KitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    kitap.ResimUrl = @"\img\" + file.FileName;
                }
               

                if (kitap.Id == 0)
                {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni kitap başarıyla oluşturuldu";
                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "kitap güncelleme başarılı";
                }

              //  _kitapRepository.Ekle(kitap); //vt ye yeni bir kayıt atacağını söylüyoruz
                _kitapRepository.Kaydet();
                //burda da onaylıyoruz bunu yapmamızın sebebi her yapmasını istediğimiz şeyi tekrar tekrar savechange etmemek için en sona bir tane KAYDET yazarak fazla kod yazmaktan kurtuluyoruz !bunu yazmazssak bilgiler vt ye eklenmez.
                TempData["basarili"] = " Yeni Kitap Türü Başarıyla Oluşturuldu! ";
                return RedirectToAction("Index", "Kitap"); //bu kodu kayıt ekledikten sonra tekrar yukarıya yani indexe gelmesi için yazdık}

            }
            return View();
        }

        //public IActionResult Guncelle(int? id)  //bu kodlarda kitapları veritabanında bulup yaptığımız güncellemeyi ekliyor //koyduğumuz soru işareti null gelme ihtimalini engelliyor.
        //{
        //    if(id==null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre //parantez içinde filtreleme işlemi yapıyoruz
        //    if (kitapVt == null)
        //    {
        //        return NotFound();

        //    }
        //    return View(kitapVt);
        //}

        //[HttpPost] 
        //public IActionResult Guncelle(Kitap kitap) 
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _kitapRepository.Guncelle(kitap);
        //        _kitapRepository.Kaydet();
        //        TempData["Guncelle"] = "  Kitap Türü Başarılıyla Güncellendi! ";
        //        return RedirectToAction("Index", "Kitap"); //bu kodu kayıt ekledikten sonra tekrar yukarıya yani indexe gelmesi için yazdık

        //    }

        //    return View();
        //}

        //temiz kod yazmak için aynı işleve sahip iki bölümü birleştridik ekleguncelle halini verdik.

        //GET ACTİON
        [Authorize(Roles = UserRoles.Role_Admin)] // burada da yetkilendirme yaptık yani bu sayfaya kimin girebileceğini belirttik
        public IActionResult Sil(int? id)  //bu kodlarda kitapları veritabanından ve sayfadan siliyor 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);
            if (kitapVt == null)
            {
                return NotFound();

            }
            return View(kitapVt);
        }
        [HttpPost,ActionName("Sil")]
        public IActionResult SilPost(int? id)
        {
            Kitap? kitap= _kitapRepository.Get(u => u.Id == id);
            if (kitap == null)
            {
                return NotFound();
            }
            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["Sil"] = " Kayıt Silme İşlemi Başarılı! ";
            return RedirectToAction("index", "Kitap");
        }
        //yukarıda ve diğer control kodlarında genel olarak şunu yapıyoruz önece idsinden vt den buluyoruz null olmasın diye soru işareti koyuyoruz if sorgusu ile eğer id si bulunmayan veya 0 olan varsa notfound metoduyla error veriyoruz varsa bulup devam ediyoruz daha sonra listden buluyoruz eğer listede yoksa yine error veriyoruz db ve hangi sayfayı istiyorsak adını yazarak ne olması gerektiğini yazıyoruz mesela yukarıda remove demiş sonra bunu savechange ile kaydediyoruz mutlaka sonrasında return redirectvs ile tekrar sayfaya geri dönderiyoruz.
    }
}
