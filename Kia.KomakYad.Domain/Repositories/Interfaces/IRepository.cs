using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> All();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
        Task<T> Get(int id);
    }
}
