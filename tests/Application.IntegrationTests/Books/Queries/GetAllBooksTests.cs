using Application.Books.Queries.GetAllBooks;
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
    public class GetAllBooksTests
    {
        private readonly TestingFixture _testingFixture;

        public GetAllBooksTests(TestingFixture testing)
        {
            _testingFixture = testing;
        }

        [Fact]
        public async Task ShouldReturnBooks_WhenThereAre()
        {
            var firstBook = new Book
            {
                Author = "Lev Tolstoi",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Razboi si pace",
                ISBN = "some random chars"
            };

            var secondBook = new Book
            {
                Author = "Marin Preda",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Morometii",
                ISBN = "some random chars"
            };
            await _testingFixture.AddAsync(firstBook);
            await _testingFixture.AddAsync(secondBook);

            var query = new GetAllBooksQuery();
            var result = await _testingFixture.SendAsync(query);
            result.Should().NotBeEmpty()
                .And.HaveCountGreaterThan(2)
                .And.Contain(x => x.Id == firstBook.Id)
                .And.Contain(x => x.Id == secondBook.Id);
        }

    }
}
