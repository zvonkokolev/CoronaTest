using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaTest.Api.Dtos
{
    public class StatisticsDto
    {
        public int CountOfExaminations { get; set; }
        public int CountOfUnknownResults { get; set; }
        public int CountOfPositiveResults { get; set; }
        public int CountOfNegativeResults { get; set; }
        public int Year { get; set; }
        public int WeekNumber { get; set; }
    }
}
