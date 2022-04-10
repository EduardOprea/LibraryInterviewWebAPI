using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Mocks
{
    public static class MockRepositories
    {
        public static Mock<IRepositoryWrapper> GetRepoWrapper()
        {
            var repoWrapperMock = new Mock<IRepositoryWrapper>();
            var bookRepoMock = GetBookRepo();
            repoWrapperMock.Setup(r => r.Book).Returns(bookRepoMock.Object);
            return repoWrapperMock;

        }
        public static Mock<IBookRepository> GetBookRepo()
        {
            var bookRepoMock = new Mock<IBookRepository>();
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Cei trei muschetari",
                    Author = "Alexandre Dumas",
                    ISBN = "1224424121",
                    BaseRentPrice = 2,
                    Currency = Currency.RON
                },
                new Book
                {
                    Id = 2,
                    Title = "Dupa 20 de ani",
                    Author = "Alexandre Dumas",
                    ISBN = "1224424123",
                    BaseRentPrice = 3,
                    Currency = Currency.RON
                }
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(books);
            bookRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);
            bookRepoMock.Setup(r => r.Create(It.IsAny<Book>()))
                .Callback((Book book) => books.Add(book));
            return bookRepoMock;
        }
    }
}
