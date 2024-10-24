using System.Linq.Expressions;

namespace WebUygulamaProje1.Models
{
    public interface IRepository<T> where T:class
    {
        // T -> KitapTuru
        IEnumerable<T> GetAll(string? includeProps = null);  //burda da kitap türlerini çektik
        T Get(Expression<Func<T, bool>> filtre, string? includeProps = null);  //burda da kitap türlerini çektik

        void Ekle(T entity);
        void Sil(T entity);

        void SilAralik(IEnumerable<T> entities);
    }
}
