using Kia.KomakYad.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected DataContext context;
        public GenericRepository(DataContext context)
        {
            this.context = context;
        }

        public virtual T Add(T entity)
        {
            return context.Add(entity).Entity;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await context.Set<T>().ToListAsync();
        }

        public virtual void Delete(T entity)
        {
            context.Remove(entity);
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public virtual void Update(T entity)
        {
            context.Update(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task<T> Get(int id)
        {
            return await context.FindAsync<T>(id);
        }
    }
}
