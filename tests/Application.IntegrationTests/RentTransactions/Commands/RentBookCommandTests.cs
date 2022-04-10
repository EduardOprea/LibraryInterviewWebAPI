using Application.Common.Exceptions;
using Application.RentTransactions.Commands.RentBook;
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
    public class RentBookCommandTests
    {
        private readonly TestingFixture _testingFixture;

        public RentBookCommandTests(TestingFixture testing)
        {
            _testingFixture = testing;
        }

        [Fact]
        public async Task ShouldCreateRentTransaction_WhenEverythingIsValid()
        {
            var bookEntity = new Book
            {
                Author = "Marin Preda",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Morometii",
                ISBN = "some random chars"
            };


            var badgeEntity = new LibraryBadge
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddDays(365),
                OwnerName = "Yoda"
            };
            await _testingFixture.AddAsync(bookEntity);
            await _testingFixture.AddAsync(badgeEntity);


            var command = new RentBookCommand
            {
                BookId = bookEntity.Id,
                LibraryBadgeId = badgeEntity.Id
            };
            var result = await _testingFixture.SendAsync(command);
            result.RentTransactionId.Should().BeGreaterThan(0);
            result.MustReturnDate.Should().BeAfter(DateTime.Now);
        }

        [Fact]
        public async Task ShouldThrowValidationException_WhenLibraryBadgeInvalid()
        {
            var bookEntity = new Book
            {
                Author = "Marin Preda",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Morometii",
                ISBN = "some random chars"
            };


            var badgeEntity = new LibraryBadge
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.Subtract(TimeSpan.FromMinutes(1)),
                OwnerName = "Yoda"
            };
            await _testingFixture.AddAsync(bookEntity);
            await _testingFixture.AddAsync(badgeEntity);


            var command = new RentBookCommand
            {
                BookId = bookEntity.Id,
                LibraryBadgeId = badgeEntity.Id
            };

            await FluentActions.Invoking(() =>
                _testingFixture.SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenBookDoesNotExists()
        {

            var badgeEntity = new LibraryBadge
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddDays(200),
                OwnerName = "Yoda"
            };

            await _testingFixture.AddAsync(badgeEntity);


            var command = new RentBookCommand
            {
                BookId = 2534,
                LibraryBadgeId = badgeEntity.Id
            };

            await FluentActions.Invoking(() =>
                _testingFixture.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }
        [Fact]
        public async Task ShouldThrowValidationException_WhenLibraryBadgeDoesNotExists()
        {

            var bookEntity = new Book
            {
                Author = "Marin Preda",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Morometii",
                ISBN = "some random chars"
            };

            await _testingFixture.AddAsync(bookEntity);


            var command = new RentBookCommand
            {
                BookId = bookEntity.Id,
                LibraryBadgeId = 4000
            };

            await FluentActions.Invoking(() =>
                _testingFixture.SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ShouldThrowValidationException_WhenBookIsRented()
        {

            var bookEntity = new Book
            {
                Author = "Marin Preda",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Morometii",
                ISBN = "some random chars"
            };

            var badgeEntity = new LibraryBadge
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddDays(200),
                OwnerName = "Yoda"
            };

            await _testingFixture.AddAsync(bookEntity);
            await _testingFixture.AddAsync(badgeEntity);


            var rentTransaction = new RentTransaction
            {
                BookId = bookEntity.Id,
                LibraryBadgeId = badgeEntity.Id,
                RentPrice = 5,
                Currency = Currency.RON,
                StartDate = DateTime.Now,
                EndDate = null,
                MustReturnDate = DateTime.Now.AddDays(15), 
            };
            await _testingFixture.AddAsync(rentTransaction);

            var command = new RentBookCommand
            {
                BookId = bookEntity.Id,
                LibraryBadgeId = badgeEntity.Id
            };

            await FluentActions.Invoking(() =>
                _testingFixture.SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

    }
}
