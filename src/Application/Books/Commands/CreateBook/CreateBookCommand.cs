using Application.Common.Interfaces;
using Application.Common.Mapping;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public decimal BaseRentPrice { get; set; }
        public Currency Currency { get; set; }
    }

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
    {
        private readonly IRepositoryWrapper _repository;

        public CreateBookCommandHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();

            _repository.Book.Create(entity);

            await _repository.SaveAsync(cancellationToken);

            return entity.Id;

        }
    }
}
