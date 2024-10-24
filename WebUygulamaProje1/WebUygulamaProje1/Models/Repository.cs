﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly UygulamaDbContext _uygulamaDbContext;
        internal DbSet<T> dbSet; //dbSet= _uygulamaDbContext.KitapTurleri yani add remove vs yaparken bu uygulamadbcontext vs uzun uzun değil sadece dbSet.remove,Add vs şekilde yazacaz


        public Repository(UygulamaDbContext uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
            this.dbSet = _uygulamaDbContext.Set<T>();
            _uygulamaDbContext.Kitaplar.Include(k => k.KitapTuru).Include(k => k.KitapTuruId); //bu satırı kitap türlerini sayfada getirmek için yazdık
        }

        public void Ekle(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filtre, string? includeProps = null) //burda da kitap türlerini getirmek için yazdık
        {
            IQueryable<T> sorgu = dbSet;
            sorgu = sorgu.Where(filtre);

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var includeProp in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) ////burda da kitap türlerini getirmek için yazdık
                {
                    sorgu = sorgu.Include(includeProp);
                }
            }
            return sorgu.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProps=null)  //burda da kitap türlerini getirmek için yazdık
        {
            IQueryable<T> sorgu = dbSet;

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach(var includeProp in includeProps.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries)) ////burda da kitap türlerini getirmek için yazdık
                {
                    sorgu=sorgu.Include(includeProp);
                } 
            }
            return sorgu.ToList();
        }

        public void Sil(T entity)
        {
            dbSet.Remove(entity);
        }

        public void SilAralik(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
