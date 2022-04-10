using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Configuration
{
    public class RentSettings
    {
        public const string SectionName = "Rent";
        public int PerDayPercentPenalty { get; set; }
        public int DaysToReturn { get; set; }
    }
}
