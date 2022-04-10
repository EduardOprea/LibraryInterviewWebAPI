using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Book FindById(int id)
        {
            return FindByCondition(b => b.Id == id).FirstOrDefault();
        }

        public async Task<Book> FindByIdAsync(int id)
        {
            var book = await FindByConditionAsync(b => b.Id == id);
            return book.FirstOrDefault();
        }

        public int GetCount(Expression<Func<Book, bool>> expression)
        {
            return Context.Books
                .Where(expression)
                .Count();
        }
    }
}
