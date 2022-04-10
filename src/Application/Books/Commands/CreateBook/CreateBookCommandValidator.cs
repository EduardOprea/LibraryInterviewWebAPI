using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Title)
                .MinimumLength(1)
                .NotNull();

            RuleFor(x => x.Author)
                .MinimumLength(1)
                .NotNull();

            RuleFor(x => x.ISBN)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(1)
                .NotNull()
                .Must(isbn => IsISBNValid(isbn))
                .WithMessage("ISBN is invalid");

            RuleFor(x => x.BaseRentPrice)
                .GreaterThanOrEqualTo(0);
        }

        private bool IsISBNValid(string isbn)
        {
            var normalisedIsbn = isbn.Replace("-", "").Replace(" ", "");
            if (normalisedIsbn.Length == 13)
                return CheckISBN13(normalisedIsbn);
            if (normalisedIsbn.Length == 10)
                return CheckISBN10(normalisedIsbn);
            return false;
        }
        private bool CheckISBN13(string isbn)
        {
            int sum = 0;
            foreach (var (index, digit) in isbn.Select((digit, index) => (index, digit)))
            {
                if (char.IsDigit(digit)) sum += (digit - '0') * (index % 2 == 0 ? 1 : 3);
                else return false;
            }
            return sum % 10 == 0;
        }
        private bool CheckISBN10(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = isbn[i] - '0';

                if (0 > digit || 9 < digit)
                    return false;

                sum += (digit * (10 - i));
            }

            // Checking last digit.
            char last = isbn[9];
            if (last != 'X' && (last < '0'
                             || last > '9'))
                return false;

            // If last digit is 'X', add 10
            // to sum, else add its value.
            sum += ((last == 'X') ? 10 :
                              (last - '0'));

            // Return true if weighted sum
            // of digits is divisible by 11.
            return (sum % 11 == 0);
        }
    }
}
