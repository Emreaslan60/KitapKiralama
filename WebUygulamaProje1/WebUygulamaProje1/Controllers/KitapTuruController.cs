using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{


    [Authorize(Roles = UserRoles.Role_Admin)]  // burada da yetkilendirme yaptık yani bu sayfaya kimin girebileceğini belirttik
    public class KitapTuruController : Controller
    {
        /* private readonly UygulamaDbContext _uygulamaDbContext;*/ //alt çizgi koyuyruz ki her yerde çağırabilelim sürekli new demeyelim !!tek bir tablomuz varsa bunu yazmak daha mantıklı
        private readonly IKitapTuruRepository _kitapTuruRepository;  //çoklu tablolarda bu şekilde repositoryler oluşturup burada çağırabiliriz
        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapTuruRepository = context;
        }
        public IActionResult Index()
        {
            List<KitapTuru> objKitapTuruList = _kitapTuruRepository.GetAll().ToList();
            //veri tabanından kitap türlerini çekerek bu listeye getirecek
            return View(objKitapTuruList);  //aşağıda yazdığımız redirectoaction metodu buraya geliyor yani işlemden sonra sayfada oluşan verileri gösteriyor 
        }


        public IActionResult Ekle()  //iki tane aynı isimde tanımlama yapamayız ama bu şekilde olursa olur.
        {
            return View();
        }

        [HttpPost] //bunu yazma sebebim ekle klasörüne yazdığımız post tan dolayı ben ekle butonuna bastığımda eklenin bağlı olduğu controllera yani buraya gelecek ve ekle isminde ama actionu httppost olan bölüme girecek 
        public IActionResult Ekle(KitapTuru kitapTuru) // parantez kısmına yazmak istediğimiz nesneyi yani ekle bölümünde oluşturduğumuz formu yazmamız gerekir.
        {

            if (ModelState.IsValid)
            {
                _kitapTuruRepository.Ekle(kitapTuru); //vt ye yeni bir kayıt atacağını söylüyoruz
                _kitapTuruRepository.Kaydet();
                //burda da onaylıyoruz bunu yapmamızın sebebi her yapmasını istediğimiz şeyi tekrar tekrar savechange etmemek için en sona bir tane KAYDET yazarak fazla kod yazmaktan kurtuluyoruz !bunu yazmazssak bilgiler vt ye eklenmez.
                TempData["basarili"] = " Yeni Kitap Türü Başarılıyla Oluşturuldu! ";
                return RedirectToAction("Index", "KitapTuru"); //bu kodu kayıt ekledikten sonra tekrar yukarıya yani indexe gelmesi için yazdık}

            }
            return View();
        }

        public IActionResult Guncelle(int? id)  //bu kodlarda kitapları veritabanında bulup yaptığımız güncellemeyi ekliyor //koyduğumuz soru işareti null gelme ihtimalini engelliyor.
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get( u=>u.Id==id); //Expression<Func<T, bool>> filtre //parantez içinde filtreleme işlemi yapıyoruz
            if (kitapTuruVt == null)
            {
                return NotFound();

            }
            return View(kitapTuruVt);
        }

        [HttpPost] 
        public IActionResult Guncelle(KitapTuru kitapTuru) 
        {

            if (ModelState.IsValid)
            {
                _kitapTuruRepository.Guncelle(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["Guncelle"] = "  Kitap Türü Başarılıyla Güncellendi! ";
                return RedirectToAction("Index", "KitapTuru"); //bu kodu kayıt ekledikten sonra tekrar yukarıya yani indexe gelmesi için yazdık
              
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
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == id);
            if (kitapTuruVt == null)
            {
                return NotFound();

            }
            return View(kitapTuruVt);
        }
        [HttpPost,ActionName("Sil")]
        public IActionResult SilPost(int? id)
        {
            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == id);
            if (kitapTuru == null)
            {
                return NotFound();
            }
            _kitapTuruRepository.Sil(kitapTuru);
            _kitapTuruRepository.Kaydet();
            TempData["Sil"] = " Kayıt Silme İşlemi Başarılı! ";
            return RedirectToAction("index", "KitapTuru");
        }
        //yukarıda ve diğer control kodlarında genel olarak şunu yapıyoruz önece idsinden vt den buluyoruz null olmasın diye soru işareti koyuyoruz if sorgusu ile eğer id si bulunmayan veya 0 olan varsa notfound metoduyla error veriyoruz varsa bulup devam ediyoruz daha sonra listden buluyoruz eğer listede yoksa yine error veriyoruz db ve hangi sayfayı istiyorsak adını yazarak ne olması gerektiğini yazıyoruz mesela yukarıda remove demiş sonra bunu savechange ile kaydediyoruz mutlaka sonrasında return redirectvs ile tekrar sayfaya geri dönderiyoruz.
    }
}
