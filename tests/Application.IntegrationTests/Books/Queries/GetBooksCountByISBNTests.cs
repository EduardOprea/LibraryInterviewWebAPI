using Application.Books.Queries.GetBooksCountByISBN;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests.Books.Queries
{
    [Collection(nameof(TestingFixture))]
    public class GetBooksCountByISBNTests
    {
        private readonly TestingFixture _testingFixture;

        public GetBooksCountByISBNTests(TestingFixture testingFixture)
        {
            _testingFixture = testingFixture;
        }

        [Fact]
        public async Task ShouldReturnCorrectCount()
        {
            string isbn = "978-0-9608-5432-5";
            await _testingFixture.AddAsync(new Book
            {
                Author = "Lev Tolstoi",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Razboi si pace",
                ISBN = isbn
            });

            await _testingFixture.AddAsync(new Book
            {
                Author = "Marin Preda",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Morometii",
                ISBN = isbn
            });

            var query = new GetBooksCountByISBNQuery(isbn);

            var result = await _testingFixture.SendAsync(query);

            result.Should().Be(2);
        }

        [Fact]
        public async Task ShouldReturnZero_WhenISBNDoesNotExists()
        {
            string isbn = "something really random";
            

            var query = new GetBooksCountByISBNQuery(isbn);

            var result = await _testingFixture.SendAsync(query);

            result.Should().Be(0);
        }
    }
}
