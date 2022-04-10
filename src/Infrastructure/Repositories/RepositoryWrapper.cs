using Application.Common.Interfaces;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _context;
        private IBookRepository _book;
        private IRentTransactionRepository _rentTransaction;
        private ILibraryBadgeRepository _libraryBadge;

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBookRepository Book
        {
            get
            {
                if(_book == null)
                {
                    _book = new BookRepository(_context);
                }
                return _book;
            }
        }

        public IRentTransactionRepository RentTransaction
        {
            get
            {
                if (_rentTransaction == null)
                {
                    _rentTransaction = new RentTransactionRepository(_context);
                }
                return _rentTransaction;
            }
        }

        public ILibraryBadgeRepository LibraryBadge
        {
            get
            {
                if (_libraryBadge == null)
                {
                    _libraryBadge = new LibraryBadgeRepository(_context);
                }
                return _libraryBadge;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
