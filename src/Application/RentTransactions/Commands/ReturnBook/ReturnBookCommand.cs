using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.PenaltyPolicies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RentTransactions.Commands.ReturnBook
{
    public class ReturnBookCommand : IRequest<ReturnBookCommandResult>
    {
        public int BookId { get; set; }
        public int LibraryBadgeId { get; set; }
    }

    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, ReturnBookCommandResult>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPenaltyPolicy _penaltyPolicy;

        public ReturnBookCommandHandler(IRepositoryWrapper repository, IDateTimeProvider dateTimeProvider, IPenaltyPolicy penaltyPolicy)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
            _penaltyPolicy = penaltyPolicy;
        }

        public async Task<ReturnBookCommandResult> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.DateTimeNow;
            var rentTransactions = await _repository.RentTransaction
                .FindByConditionAsync(r => r.BookId == request.BookId
                                            && r.LibraryBadgeId == request.LibraryBadgeId
                                            && r.EndDate == null);
            var rentTransaction = rentTransactions.FirstOrDefault();

            if (rentTransaction == null)
            {
                throw new NotFoundException("The book does not apear to be rented");
            }

            rentTransaction.EndDate = now;

            _repository.RentTransaction.Update(rentTransaction);
            await _repository.SaveAsync(cancellationToken);

            var penalty = _penaltyPolicy.Apply(rentTransaction.MustReturnDate, now, rentTransaction.RentPrice);
            return new ReturnBookCommandResult()
            {
                Penalty = penalty,
                Currency = rentTransaction.Currency
            };
        }
    }

}
