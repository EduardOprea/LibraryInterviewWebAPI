using Application.Common.Configuration;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.RentTransactions.Commands.RentBook;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.RentTransactions.Commands.RentBook
{
    public class RentBookCommandHandlerTests
    {

        private readonly RentBookCommandHandler _sut;
        private readonly Mock<IRepositoryWrapper> _repoWrapperMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public RentBookCommandHandlerTests()
        {
            _repoWrapperMock = new Mock<IRepositoryWrapper>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var optionsMock = Options.Create(new RentSettings
            {
                DaysToReturn = 14,
                PerDayPercentPenalty = 1
            });
            _sut = new RentBookCommandHandler(_repoWrapperMock.Object,
                _dateTimeProviderMock.Object, optionsMock);
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenBookDoesNotExists()
        {
            //arrange
            var command = new RentBookCommand
            {
                BookId = 5,
                LibraryBadgeId = 5
            };
            var _bookRepoMock = new Mock<IBookRepository>();
            _bookRepoMock.Setup(x => x.FindByIdAsync(command.BookId))
                .ReturnsAsync((Book)null);

            var _libraryBadgeRepoMock = new Mock<ILibraryBadgeRepository>();
            _repoWrapperMock.Setup(r => r.Book).Returns(_bookRepoMock.Object);
            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(_libraryBadgeRepoMock.Object);
            //act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);
            //assert
            await act.Should().ThrowAsync<NotFoundException>()
                .Where(e => e.Message.ToLower().Contains("book"));
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenLibraryBadgeDoesNotExists()
        {
            //arrange
            var command = new RentBookCommand
            {
                BookId = 5,
                LibraryBadgeId = 5
            };

            var book = new Book
            {
                Id = 5
            };
            var _bookRepoMock = new Mock<IBookRepository>();
            _bookRepoMock.Setup(x => x.FindByIdAsync(command.BookId))
                .ReturnsAsync(book);
            var _libraryBadgeRepoMock = new Mock<ILibraryBadgeRepository>();
            _libraryBadgeRepoMock.Setup(x => x.FindByIdAsync(command.LibraryBadgeId))
                .ReturnsAsync((LibraryBadge)null);
            _repoWrapperMock.Setup(r => r.Book).Returns(_bookRepoMock.Object);
            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(_libraryBadgeRepoMock.Object);
            //act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);
            //assert
            await act.Should().ThrowAsync<NotFoundException>()
                .Where(e => e.Message.ToLower().Contains("librarybadge"));
        }

        [Fact]
        public async Task ShouldCallRentTransactionRepositoryCreateOnce()
        {
            var command = new RentBookCommand
            {
                BookId = 5,
                LibraryBadgeId = 5
            };

            var book = new Book
            {
                Id = 5,
            };
            var badge = new LibraryBadge
            {
                Id = 5,
            };


            var _libraryBadgeRepoMock = new Mock<ILibraryBadgeRepository>();
            _libraryBadgeRepoMock.Setup(x => x.FindByIdAsync(command.LibraryBadgeId))
                .ReturnsAsync(badge);

            var _bookRepoMock = new Mock<IBookRepository>();
            _bookRepoMock.Setup(x => x.FindByIdAsync(command.BookId))
                .ReturnsAsync(book);

            var _rentTransactionMock = new Mock<IRentTransactionRepository>();


            _repoWrapperMock.Setup(r => r.Book).Returns(_bookRepoMock.Object);
            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(_libraryBadgeRepoMock.Object);
            _repoWrapperMock.Setup(r => r.RentTransaction).Returns(_rentTransactionMock.Object);
            //act
            var result = await _sut.Handle(command, CancellationToken.None);
            //assert
            _rentTransactionMock.Verify(m => m.Create(It.IsAny<RentTransaction>()), Times.Once);
            
            
        }
        [Fact]
        public async Task ShouldReturnCorrectMustReturnDateWithMidnightTime()
        {
            var command = new RentBookCommand
            {
                BookId = 5,
                LibraryBadgeId = 5
            };

            var book = new Book
            {
                Id = 5,
            };
            var badge = new LibraryBadge
            {
                Id = 5,
            };


            var _libraryBadgeRepoMock = new Mock<ILibraryBadgeRepository>();
            _libraryBadgeRepoMock.Setup(x => x.FindByIdAsync(command.LibraryBadgeId))
                .ReturnsAsync(badge);

            var _bookRepoMock = new Mock<IBookRepository>();
            _bookRepoMock.Setup(x => x.FindByIdAsync(command.BookId))
                .ReturnsAsync(book);

            var _rentTransactionMock = new Mock<IRentTransactionRepository>();


            _repoWrapperMock.Setup(r => r.Book).Returns(_bookRepoMock.Object);
            _repoWrapperMock.Setup(r => r.LibraryBadge).Returns(_libraryBadgeRepoMock.Object);
            _repoWrapperMock.Setup(r => r.RentTransaction).Returns(_rentTransactionMock.Object);

            _dateTimeProviderMock.Setup(d => d.DateTimeNow).Returns(new DateTime(2022, 1, 1, 15, 15, 33));
            //act
            var result = await _sut.Handle(command, CancellationToken.None);
            //assert
            result.MustReturnDate.Should().Be(new DateTime(2022, 1, 15,0,0,0));

        }


    }
}
