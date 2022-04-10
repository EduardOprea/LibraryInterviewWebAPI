using Application.Common.Interfaces;
using Application.RentTransactions.Commands.RentBook;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.RentTransactions.Commands.RentBook
{
    public class RentBookCommandValidatorTests
    {
        private readonly RentBookCommandValidator _sut;
        private readonly Mock<IRepositoryWrapper> _repoWrapperMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public RentBookCommandValidatorTests()
        {
            _repoWrapperMock = new Mock<IRepositoryWrapper>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _sut = new RentBookCommandValidator(_repoWrapperMock.Object, _dateTimeProviderMock.Object);
        }

        [Fact]
        public async Task ShouldValidate_WhenBookIsNotRentedAndBadgeIsInValability()
        {
            //arrange
            var command = new RentBookCommand
            {
                BookId = 1,
                LibraryBadgeId = 1,
            };
            var badge = new LibraryBadge
            {
                Id = 1,
                Created = new DateTime(2022, 1, 1),
                Expired = new DateTime(2023, 1, 1),
                OwnerName = "Eduard Oprea"
            };

            var badgeRepo = new Mock<ILibraryBadgeRepository>();
            var rentTransactionsRepo = new Mock<IRentTransactionRepository>();

            badgeRepo.Setup(r => r.FindById(command.BookId))
                .Returns(badge);

            rentTransactionsRepo.Setup(r =>
                r.FindByCondition(It.IsAny<Expression<Func<RentTransaction, bool>>>()))
                .Returns(new List<RentTransaction>());

            _dateTimeProviderMock.Setup(d => d.DateTimeNow).Returns(new DateTime(2022, 5, 1));

            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(badgeRepo.Object);
            _repoWrapperMock.Setup(r => r.RentTransaction).Returns(rentTransactionsRepo.Object);

            //act
            var result = await _sut.ValidateAsync(command);
            //assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldValidate_WhenBookIsNotRentedAndBadgeIsInLastDayOfValability()
        {
            //arrange
            var command = new RentBookCommand
            {
                BookId = 1,
                LibraryBadgeId = 1,
            };
            var badge = new LibraryBadge
            {
                Id = 1,
                Created = new DateTime(2022, 1, 1),
                Expired = new DateTime(2023, 1, 1),
                OwnerName = "Eduard Oprea"
            };

            var badgeRepo = new Mock<ILibraryBadgeRepository>();
            var rentTransactionsRepo = new Mock<IRentTransactionRepository>();

            badgeRepo.Setup(r => r.FindById(command.BookId))
                .Returns(badge);

            rentTransactionsRepo.Setup(r =>
                r.FindByCondition(It.IsAny<Expression<Func<RentTransaction, bool>>>()))
                .Returns(new List<RentTransaction>());

            _dateTimeProviderMock.Setup(d => d.DateTimeNow).Returns(new DateTime(2023, 1, 1));

            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(badgeRepo.Object);
            _repoWrapperMock.Setup(r => r.RentTransaction).Returns(rentTransactionsRepo.Object);

            //act
            var result = await _sut.ValidateAsync(command);
            //assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldNotValidate_WhenLibraryBadgeIsExpired()
        {
            //arrange
            var command = new RentBookCommand
            {
                BookId = 1,
                LibraryBadgeId = 1,
            };
            var badge = new LibraryBadge
            {
                Id = 1,
                Created = new DateTime(2022, 1, 1),
                Expired = new DateTime(2023, 1, 1),
                OwnerName = "Eduard Oprea"
            };

            var badgeRepo = new Mock<ILibraryBadgeRepository>();
            var rentTransactionsRepo = new Mock<IRentTransactionRepository>();

            badgeRepo.Setup(r => r.FindById(command.BookId))
                .Returns(badge);

            rentTransactionsRepo.Setup(r =>
                    r.FindByCondition(It.IsAny<Expression<Func<RentTransaction, bool>>>()))
                .Returns(new List<RentTransaction>());

            _dateTimeProviderMock.Setup(d => d.DateTimeNow).Returns(new DateTime(2023, 1, 2));

            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(badgeRepo.Object);
            _repoWrapperMock.Setup(r => r.RentTransaction).Returns(rentTransactionsRepo.Object);

            //act
            var result = await _sut.ValidateAsync(command);
            //assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldNotValidate_WhenBookIsRented()
        {
            //arrange
            var command = new RentBookCommand
            {
                BookId = 1,
                LibraryBadgeId = 1,
            };
            var badge = new LibraryBadge
            {
                Id = 1,
                Created = new DateTime(2022, 1, 1),
                Expired = new DateTime(2023, 1, 1),
                OwnerName = "Eduard Oprea"
            };

            var rentTransactionOngoing = new RentTransaction
            {
                Id = 1
            };


            var badgeRepo = new Mock<ILibraryBadgeRepository>();
            var rentTransactionsRepo = new Mock<IRentTransactionRepository>();

            badgeRepo.Setup(r => r.FindById(command.BookId))
                .Returns(badge);

            rentTransactionsRepo.Setup(r =>
                r.FindByCondition(It.IsAny<Expression<Func<RentTransaction, bool>>>()))
                .Returns(new List<RentTransaction> { rentTransactionOngoing });

            _dateTimeProviderMock.Setup(d => d.DateTimeNow).Returns(new DateTime(2022, 5, 2));

            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(badgeRepo.Object);
            _repoWrapperMock.Setup(r => r.RentTransaction).Returns(rentTransactionsRepo.Object);

            //act
            var result = await _sut.ValidateAsync(command);
            //assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldNotValidate_WhenBookRentedAndBadgeExpired()
        {
            //arrange
            var command = new RentBookCommand
            {
                BookId = 1,
                LibraryBadgeId = 1,
            };
            var badge = new LibraryBadge
            {
                Id = 1,
                Created = new DateTime(2022, 1, 1),
                Expired = new DateTime(2023, 1, 1),
                OwnerName = "Eduard Oprea"
            };

            var rentTransactionOngoing = new RentTransaction
            {
                Id = 1
            };


            var badgeRepo = new Mock<ILibraryBadgeRepository>();
            var rentTransactionsRepo = new Mock<IRentTransactionRepository>();

            badgeRepo.Setup(r => r.FindById(command.BookId))
                .Returns(badge);

            rentTransactionsRepo.Setup(r =>
                r.FindByCondition(It.IsAny<Expression<Func<RentTransaction, bool>>>()))
                .Returns(new List<RentTransaction> { rentTransactionOngoing });

            _dateTimeProviderMock.Setup(d => d.DateTimeNow).Returns(new DateTime(2023, 5, 2));

            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(badgeRepo.Object);
            _repoWrapperMock.Setup(r => r.RentTransaction).Returns(rentTransactionsRepo.Object);

            //act
            var result = await _sut.ValidateAsync(command);
            //assert
            result.IsValid.Should().BeFalse();
        }
    }
}
