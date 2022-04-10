using Application.Books.Commands.CreateBook;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests.Books.Commands
{
    [Collection(nameof(TestingFixture))]
    public class CreateBookCommandTests
    {
        private readonly TestingFixture _testingFixture;

        public CreateBookCommandTests(TestingFixture testing)
        {
            _testingFixture = testing;
        }
        [Fact]
        public async Task ShouldThrowException_WhenCommandEmpty()
        {
            var command = new CreateBookCommand();

            await FluentActions.Invoking(() =>
                _testingFixture.SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }
        [Fact]
        public async Task ShouldCreateBook()
        {
            string validISBN = "978-4-8828-6462-2";
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Lev Tolstoi",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Razboi si pace",
                ISBN = validISBN
            };
            //act
            var bookId = await _testingFixture.SendAsync(command);
            var item = await _testingFixture.FindAsync<Book>(bookId);
            //assert
            item.Should().NotBeNull();
            item.Author.Should().Be(command.Author);
            item.Title.Should().Be(command.Title);
            item.ISBN.Should().Be(command.ISBN);
            item.BaseRentPrice.Should().Be(command.BaseRentPrice);
            item.Currency.Should().Be(command.Currency);

        }
    }
}
