using Application.Common.Interfaces;
using Application.Common.Mapping;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RentTransactions.Queries.GetAll
{
    public class GetAllRentTransactionsQuery : IRequest<List<RentTransactionDto>>
    {
    }
    public class GetAllRentTransactionsQueryHandler
        : IRequestHandler<GetAllRentTransactionsQuery, List<RentTransactionDto>>
    {
        private readonly IRepositoryWrapper _repository;

        public GetAllRentTransactionsQueryHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<List<RentTransactionDto>> Handle(GetAllRentTransactionsQuery request, CancellationToken cancellationToken)
        {
            var rentTransactions = await _repository.RentTransaction.GetAllAsync();
            return rentTransactions.Select(b => b.ToDto()).ToList();
        }
    }
}
