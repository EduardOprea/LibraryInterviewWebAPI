using Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RentTransactions.Commands.RentBook
{
    public class RentBookCommandValidator : AbstractValidator<RentBookCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        public RentBookCommandValidator(IRepositoryWrapper repository, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;

            RuleFor(x => x.BookId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1)
                .Must(id => !IsBookRented(id))
                .WithMessage("Book is already rented");


            RuleFor(x => x.LibraryBadgeId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1)
                .Must(id => ExistsLibraryBadge(id))
                .WithMessage("Library badge does not exists")
                .Must(id => IsLibraryBadgeValid(id))
                .WithMessage("Library badge is expired");

        }

        private bool ExistsLibraryBadge(int libraryBadgeId)
        {
            var badge = _repository.LibraryBadge.FindById(libraryBadgeId);

            return badge is null ? false : true;

        }
        private bool IsLibraryBadgeValid(int libraryBadgeId)
        {
            var badge = _repository.LibraryBadge.FindById(libraryBadgeId);

            return badge?.Expired >= _dateTimeProvider.DateTimeNow ? true : false;
        }
        private bool IsBookRented(int bookId)
        {
            return _repository.RentTransaction
                .FindByCondition(r => r.EndDate == null && r.BookId == bookId)
                .Count() > 0 ? true : false;
        }

    }
}
