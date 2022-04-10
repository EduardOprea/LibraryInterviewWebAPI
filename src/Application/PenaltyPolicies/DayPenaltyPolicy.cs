using Application.Common.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PenaltyPolicies
{
    public class DayPenaltyPolicy : IPenaltyPolicy
    {
        private readonly RentSettings _settings;

        public DayPenaltyPolicy(IOptions<RentSettings> settings)
        {
            _settings = settings.Value;
        }

        public decimal Apply(DateTime mustReturnDate, DateTime actualReturnDate, decimal price)
        {
            if(price < 0)
            {
                return 0;
            }
            var dateDiff = actualReturnDate.Date.Subtract(mustReturnDate.Date);
            
            return dateDiff.Days > 0 ? 
                (_settings.PerDayPercentPenalty * price * dateDiff.Days) / 100 : 0;
        }
    }
}
