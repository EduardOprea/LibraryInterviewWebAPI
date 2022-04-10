using Application.Books.Commands.CreateBook;
using Domain.Enums;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;


namespace Application.UnitTests.Books.Commands
{
    public class CreateBookCommandValidatorTests
    {
        private readonly CreateBookCommandValidator _sut = new CreateBookCommandValidator();
        private readonly string _validISBN10 = "0-9476-0298-4";
        private readonly string _validISBN13 = "978-6-3970-0206-3";

        [Fact]
        public async Task ShouldNotValidate_WhenTitleEmpty()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Gustave Flaubert",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "",
                ISBN = _validISBN13
            };
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeFalse();

        }
        [Fact]
        public async Task ShouldNotValidate_WhenTitleNull()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Gustave Flaubert",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = null,
                ISBN = _validISBN10
            };
            
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeFalse();

        }
        [Fact]
        public async Task ShouldNotValidate_WhenAuthorEmpty()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Some title",
                ISBN = _validISBN10
            };
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeFalse();

        }
        [Fact]
        public async Task ShouldNotValidate_WhenAuthorNull()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = null,
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Some title",
                ISBN = _validISBN10
            };
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeFalse();

        }

        [Fact]
        public async Task ShouldNotValidate_WhenISBNEmpty()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Some author",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Some title",
                ISBN = ""
            };
            
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeFalse();

        }

        [Fact]
        public async Task ShouldNotValidate_WhenISBNNull()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Some author",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Some title",
                ISBN = null
            };
            
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeFalse();

        }

        [Fact]
        public async Task ShouldNotValidate_WhenBaseRentPriceNegative()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Some author",
                BaseRentPrice = -2,
                Currency = Currency.RON,
                Title = "Some title",
                ISBN = _validISBN10
            };
            
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldValidate_WhenAllParamsAreValidAndISBNFormat10()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Some author",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Some title",
                ISBN = _validISBN10
            };
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldValidate_WhenAllParamsAreValidAndISBNFormat13()
        {
            //arrange
            var command = new CreateBookCommand()
            {
                Author = "Some author",
                BaseRentPrice = 5,
                Currency = Currency.RON,
                Title = "Some title",
                ISBN = _validISBN13
            };
            //act
            var validationResult = await _sut.ValidateAsync(command);
            //assert
            validationResult.IsValid.Should().BeTrue();
        }


    }
}
