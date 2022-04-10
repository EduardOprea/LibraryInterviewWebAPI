using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetBooksCountByISBN
{
    public class GetBooksCountByISBNQuery : IRequest<int>
    {
        public string ISBN { get; }

        public GetBooksCountByISBNQuery(string isbn)
        {
            ISBN = isbn;
        }
    }
    public class GetBooksCountByISBNQueryHandler : IRequestHandler<GetBooksCountByISBNQuery, int>
    {
        private readonly IRepositoryWrapper _repository;

        public GetBooksCountByISBNQueryHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(GetBooksCountByISBNQuery request, CancellationToken cancellationToken)
        {
            var count = _repository.Book
                .GetCount(book => book.ISBN.Equals(request.ISBN));
            return await Task.FromResult(count);
        }
    }
}
