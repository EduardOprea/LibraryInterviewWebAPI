using Application.Books.Queries.GetAllBooks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mapping;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetById
{
    public class GetByIdQuery : IRequest<BookDto>
    {
        public int Id { get; set; }
    }

    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, BookDto>
    {
        private readonly IRepositoryWrapper _repository;

        public GetByIdQueryHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<BookDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _repository.Book.FindByIdAsync(request.Id);
            if (book == null)
            {
                throw new NotFoundException("Book", request.Id);
            }

            return book.ToDto();
        }
    }
}
