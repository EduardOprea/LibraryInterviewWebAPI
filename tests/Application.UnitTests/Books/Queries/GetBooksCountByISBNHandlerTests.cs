using Application.Books.Queries.GetBooksCountByISBN;
using Application.Common.Interfaces;
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
    public class GetBooksCountByISBNHandlerTests
    {
        private readonly GetBooksCountByISBNQueryHandler _sut;
        private readonly Mock<IRepositoryWrapper> _repoWrapperMock;

        public GetBooksCountByISBNHandlerTests()
        {
            _repoWrapperMock = new Mock<IRepositoryWrapper>();
            _sut = new GetBooksCountByISBNQueryHandler(_repoWrapperMock.Object);
        }

        [Fact]
        public async Task GetBooksCountByISBN_ShouldReturnRightCount()
        {
            //arrange
            var isbn = "0-1027-9206-2";
            var isbnCount = 5;
            var query = new GetBooksCountByISBNQuery(isbn);
            var _bookRepoMock = new Mock<IBookRepository>();
            _bookRepoMock.Setup(r => r.GetCount(b => b.ISBN.Equals(query.ISBN)))
                .Returns(isbnCount);
            _repoWrapperMock.Setup(r => r.Book)
                .Returns(_bookRepoMock.Object);
            //act
            var ret = await _sut.Handle(query, CancellationToken.None);
            //assert
            ret.Should().Be(isbnCount);
        }
    }
}
