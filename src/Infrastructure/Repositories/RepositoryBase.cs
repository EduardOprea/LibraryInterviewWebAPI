using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext Context { get; set; }

        protected RepositoryBase(ApplicationDbContext context)
        {
            Context = context;
        }

        public void Create(T entity) => Context.Set<T>().Add(entity);

        public void Delete(T entity) => Context.Set<T>().Remove(entity);
        public void Update(T entity) => Context.Set<T>().Update(entity);

        public IEnumerable<T> GetAll() => Context.Set<T>().ToList();
        public async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();
        
        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            Context.Set<T>().Where(expression);

        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression) =>
            await Context.Set<T>().Where(expression).ToListAsync();
    }
}
