using Application.Books.Queries.GetAllBooks;
using Application.Common.Interfaces;
using Application.UnitTests.Mocks;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Books.Queries
{
    public class GetAllBooksQueryHandlerTests
    {
        private readonly GetAllBooksQueryHandler _sut;
        private readonly Mock<IRepositoryWrapper> _repoWrapperMock;

        public GetAllBooksQueryHandlerTests()
        {
            _repoWrapperMock = MockRepositories.GetRepoWrapper();
            _sut = new GetAllBooksQueryHandler(_repoWrapperMock.Object);
        }

        [Fact]
        public async Task GetAllBooksQueryHandler_ShouldReturnBookDtoList()
        {
            var query = new GetAllBooksQuery();

            var result = await _sut.Handle(query, CancellationToken.None);

            result.Should().BeOfType<List<BookDto>>();
        }

        [Fact]
        public async Task GetAllBooksQueryHandler_ShouldReturnAppropiateBookCount()
        {
            var query = new GetAllBooksQuery();

            var result = await _sut.Handle(query, CancellationToken.None);

            result.Count().Should().Be(2);
        }
    }
}
