using Application.Common.Interfaces;
using Application.Common.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<List<BookDto>>
    {

    }

    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDto>>
    {
        private readonly IRepositoryWrapper _repository;

        public GetAllBooksQueryHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<List<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _repository.Book.GetAllAsync();
            return books.Select(b => b.ToDto()).ToList();
        }
    }
}
