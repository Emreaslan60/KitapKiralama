using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Models
{
    public class KitapRepository : Repository<Kitap>, IKitapRepository
    {
        private  UygulamaDbContext _uygulamaDbContext;
        public KitapRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }
        //yukarıdaki kodu repositorydeki yazdığımız kodu tekrar yazmamak için yazdık artık bu kod sayesinde her yerden çağırabiliriz.

        public void Guncelle(Kitap kitap)
        {
            _uygulamaDbContext.Update(kitap);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
