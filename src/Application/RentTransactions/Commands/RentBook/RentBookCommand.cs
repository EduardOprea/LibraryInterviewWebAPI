using Application.Common.Configuration;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RentTransactions.Commands.RentBook
{
    public class RentBookCommand : IRequest<RentBookCommandResult>
    {
        public int BookId { get; set; }
        public int LibraryBadgeId { get; set; }

    }
    public class RentBookCommandHandler : IRequestHandler<RentBookCommand, RentBookCommandResult>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly RentSettings _rentSettings;

        public RentBookCommandHandler(IRepositoryWrapper repository, IDateTimeProvider dateTimeProvider, IOptions<RentSettings> options)
        {
            _rentSettings = options.Value;
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<RentBookCommandResult> Handle(RentBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.Book.FindByIdAsync(request.BookId);
            var badge = await _repository.LibraryBadge.FindByIdAsync(request.LibraryBadgeId);
            if (book == null)
            {
                throw new NotFoundException("Book", request.BookId);
            }
            if (badge == null)
            {
                throw new NotFoundException("LibraryBadge", request.LibraryBadgeId);
            }
            var now = _dateTimeProvider.DateTimeNow;
            var mustReturnDate = ComputeMustReturnDate(now, TimeSpan.FromDays(_rentSettings.DaysToReturn));
            var rentTransaction = new RentTransaction()
            {
                BookId = book.Id,
                LibraryBadgeId = badge.Id,
                StartDate = now,
                MustReturnDate = mustReturnDate,
                EndDate = null,
                RentPrice = book.BaseRentPrice,
                Currency = book.Currency
            };
            _repository.RentTransaction.Create(rentTransaction);
            await _repository.SaveAsync(cancellationToken);
            return new RentBookCommandResult()
            {
                RentTransactionId = rentTransaction.Id,
                MustReturnDate = mustReturnDate
            };
        }
        private DateTime ComputeMustReturnDate(DateTime startDate, TimeSpan rentInterval)
        {
            return startDate.Add(rentInterval).Date;
        }

    }

}
