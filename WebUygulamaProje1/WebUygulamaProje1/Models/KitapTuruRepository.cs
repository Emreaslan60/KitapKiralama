using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Models
{
    public class KitapTuruRepository : Repository<KitapTuru>, IKitapTuruRepository
    {
        private  UygulamaDbContext _uygulamaDbContext;
        public KitapTuruRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }
        //yukarıdaki kodu repositorydeki yazdığımız kodu tekrar yazmamak için yazdık artık bu kod sayesinde her yerden çağırabiliriz.

        public void Guncelle(KitapTuru kitapTuru)
        {
            _uygulamaDbContext.Update(kitapTuru);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
