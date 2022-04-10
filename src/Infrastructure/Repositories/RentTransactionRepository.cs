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
    public class RentTransactionRepository : RepositoryBase<RentTransaction>, IRentTransactionRepository
    {
        public RentTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
