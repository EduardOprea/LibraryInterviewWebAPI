using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IRepositoryWrapper
    {
        IBookRepository Book { get; }
        IRentTransactionRepository RentTransaction { get; }
        ILibraryBadgeRepository LibraryBadge { get; }
        int Save();
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
