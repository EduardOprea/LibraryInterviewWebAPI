using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LibraryBadgeRepository : RepositoryBase<LibraryBadge>, ILibraryBadgeRepository
    {
        public LibraryBadgeRepository(ApplicationDbContext context) : base(context)
        {
        }
        public LibraryBadge FindById(int id)
        {
            return FindByCondition(b => b.Id == id).FirstOrDefault();
        }

        public async Task<LibraryBadge> FindByIdAsync(int id)
        {
            var badges = await FindByConditionAsync(b => b.Id == id);
            return badges.FirstOrDefault();
        }
    }
}
