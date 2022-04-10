using Application.Common.Configuration;
using Application.PenaltyPolicies;
using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.RentTransactions.Commands.ReturnBook
{
    public class DayPenaltyPolicyTests
    {
        private readonly DayPenaltyPolicy _sut;

        public DayPenaltyPolicyTests()
        {
            var optionsMock = Options.Create(new RentSettings
            {
                DaysToReturn = 14,
                PerDayPercentPenalty = 1
            });
            _sut = new DayPenaltyPolicy(optionsMock);
        }
        [Theory]
        [MemberData(nameof(DayPenaltyTestData))]
        public void ShouldReturnCorrectPenalty(DateTime mustReturnDate, DateTime actualReturnDate,
            decimal price, decimal expectedPenalty)
        {
            var penalty = _sut.Apply(mustReturnDate, actualReturnDate, price);
            penalty.Should().Be(expectedPenalty);
        }

        public static IEnumerable<object[]> DayPenaltyTestData()
        {
            yield return new object[] { new DateTime(2022, 5, 15), new DateTime(2022, 5, 14), 55, 0 };
            yield return new object[] { new DateTime(2022, 5, 14), new DateTime(2022, 5, 14,22,15,0), 120, 0 };
            yield return new object[] { new DateTime(2022, 5, 15), new DateTime(2022, 5, 16), 55, new decimal(0.55) };
            yield return new object[] { new DateTime(2022, 5, 15), new DateTime(2022, 5, 16, 23,59,0), 55, new decimal(0.55) };
            yield return new object[] { new DateTime(2022, 5, 15), new DateTime(2022, 5, 17, 23, 59, 0), 55, new decimal(1.1) };
            yield return new object[] { new DateTime(2022, 5, 15), new DateTime(2022, 5, 18, 12, 59, 0), 60, new decimal(1.8) };
            yield return new object[] { new DateTime(2022, 5, 15), new DateTime(2022, 5, 17, 23, 59, 0), 0, 0 };
            yield return new object[] { new DateTime(2022, 5, 15), new DateTime(2022, 5, 17, 23, 59, 0), -20, 0 };
        }



    }
}
