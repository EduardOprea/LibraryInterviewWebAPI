using Application.Books.Commands.CreateBook;
using Application.Common.Interfaces;
using Application.UnitTests.Mocks;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Books.Commands
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapperMock;
        private readonly CreateBookCommandHandler _sut;
        public CreateBookCommandHandlerTests()
        {
            _repoWrapperMock = MockRepositories.GetRepoWrapper();
            _sut = new CreateBookCommandHandler(_repoWrapperMock.Object);
        }


        [Fact]
        public async Task CreateBook_ShouldCallBookRepoCreateOnce()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Gustave Flaubert",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Madame Bovary",
                ISBN = "0-3920-2258-3"
            };
            //act
            var result = await _sut.Handle(command, CancellationToken.None);
            //assert
            _repoWrapperMock.Verify(r => r.Book.Create(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task CreateOneBook_ShouldIncrementRepositoryBookCount()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Gustave Flaubert",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Madame Bovary",
                ISBN = "0-3920-2258-3"
            };
            //act
            var result = await _sut.Handle(command, CancellationToken.None);
            var books = await _repoWrapperMock.Object.Book.GetAllAsync();
            //assert
            books.Count().Should().Be(3);
        }
    }
}
