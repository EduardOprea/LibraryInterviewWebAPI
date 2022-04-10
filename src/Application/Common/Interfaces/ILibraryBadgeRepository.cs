using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ILibraryBadgeRepository : IRepositoryBase<LibraryBadge>
    {
        public LibraryBadge FindById(int id);
        public Task<LibraryBadge> FindByIdAsync(int id);
    }
}
