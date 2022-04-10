using Application.Books.Commands.CreateBook;
using Application.Books.Queries.GetAllBooks;
using Application.RentTransactions.Queries.GetAll;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mapping
{
    public static class ManualMappingExtensions
    {
        public static BookDto ToDto(this Book book)
        {
            return new BookDto()
            {
               Id = book.Id,
               Title = book.Title,
               Author = book.Author,
               ISBN = book.ISBN,
               BaseRentPrice = book.BaseRentPrice,
               Currency = book.Currency,
            };
        }

        public static RentTransactionDto ToDto(this RentTransaction rentTransaction)
        {
            return new RentTransactionDto()
            {
                BookId = rentTransaction.BookId,
                LibraryBadgeId = rentTransaction.LibraryBadgeId,
                StartDate = rentTransaction.StartDate,
                MustReturnDate = rentTransaction.MustReturnDate,
                EndDate = rentTransaction.EndDate,
                RentPrice = rentTransaction.RentPrice,
                Currency = rentTransaction.Currency,
                Id = rentTransaction.Id
            };
        }

        public static Book ToEntity(this CreateBookCommand command)
        {
            return new Book()
            {
                Title = command.Title,
                Author = command.Author,
                ISBN = command.ISBN,
                BaseRentPrice = command.BaseRentPrice,
                Currency = command.Currency,
            };
        }

    }
}
