using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        public int GetCount(Expression<Func<Book, bool>> expression);
        public Book FindById(int id);
        public Task<Book> FindByIdAsync(int id);
    }
}
