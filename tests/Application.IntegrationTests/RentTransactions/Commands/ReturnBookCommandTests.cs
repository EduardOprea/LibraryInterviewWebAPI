using Application.Common.Exceptions;
using Application.RentTransactions.Commands.ReturnBook;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests.RentTransactions.Commands
{
    [Collection(nameof(TestingFixture))]
    public class ReturnBookCommandTests
    {
        private readonly TestingFixture _testingFixture;

        public ReturnBookCommandTests(TestingFixture testingFixture)
        {
            _testingFixture = testingFixture;
        }
        [Fact]
        public async Task ShouldReturnBookSuccesfullyWithNoPenalty()
        {
            var book = new Book
            {
                Author = "Mircea Cartarescu",
                Title = "Levantul",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                ISBN = "973-23-0236-4"
            };
            var badge = new LibraryBadge
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddDays(55),
                OwnerName = "Jean Valjean"
            };
            await _testingFixture.AddAsync(book);
            await _testingFixture.AddAsync(badge);
            var rentTransaction = new RentTransaction
            {
               BookId = book.Id,
               LibraryBadgeId = badge.Id,
               RentPrice = 5,
               Currency = Currency.RON,
               StartDate = DateTime.Now,
               MustReturnDate = DateTime.Now.AddDays(14),
               EndDate = null
            };
            await _testingFixture.AddAsync(rentTransaction);

            var command = new ReturnBookCommand
            {
                BookId = book.Id,
                LibraryBadgeId = badge.Id
            };

            var result = await _testingFixture.SendAsync(command);
            var finishedRentTransaction = await _testingFixture
                .FindAsync<RentTransaction>(rentTransaction.Id);


            finishedRentTransaction.EndDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            result.Penalty.Should().Be(0);
            result.Currency.Should().Be(rentTransaction.Currency);


        }

        [Fact]
        public async Task ShouldReturnBookSuccesfullyWithPenalty()
        {
            var book = new Book
            {
                Author = "Mircea Cartarescu",
                Title = "Levantul",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                ISBN = "973-23-0236-4"
            };
            var badge = new LibraryBadge
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddDays(55),
                OwnerName = "Jean Valjean"
            };
            await _testingFixture.AddAsync(book);
            await _testingFixture.AddAsync(badge);
            var rentTransaction = new RentTransaction
            {
                BookId = book.Id,
                LibraryBadgeId = badge.Id,
                RentPrice = 5,
                Currency = Currency.RON,
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                MustReturnDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                EndDate = null
            };
            await _testingFixture.AddAsync(rentTransaction);

            var command = new ReturnBookCommand
            {
                BookId = book.Id,
                LibraryBadgeId = badge.Id
            };

            var result = await _testingFixture.SendAsync(command);
            var finishedRentTransaction = await _testingFixture
                .FindAsync<RentTransaction>(rentTransaction.Id);


            finishedRentTransaction.EndDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            result.Penalty.Should().BeGreaterThan(0);
            result.Currency.Should().Be(rentTransaction.Currency);


        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenBookWasRentedAndReturned()
        {
            var book = new Book
            {
                Author = "Mircea Cartarescu",
                Title = "Levantul",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                ISBN = "973-23-0236-4"
            };
            var badge = new LibraryBadge
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddDays(55),
                OwnerName = "Jean Valjean"
            };
            await _testingFixture.AddAsync(book);
            await _testingFixture.AddAsync(badge);
            var rentTransaction = new RentTransaction
            {
                BookId = book.Id,
                LibraryBadgeId = badge.Id,
                RentPrice = 5,
                Currency = Currency.RON,
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                MustReturnDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                EndDate = DateTime.Now,
            };
            await _testingFixture.AddAsync(rentTransaction);

            var command = new ReturnBookCommand
            {
                BookId = book.Id,
                LibraryBadgeId = badge.Id
            };

            await FluentActions.Invoking(() =>
                _testingFixture.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }
    }
}
