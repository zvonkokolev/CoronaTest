using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoronaTest.Core.DTOs
{
    public class KampagneDto
    {
        public string Name { get; set; }
        [DisplayName("Von")]
        public DateTime From { get; set; }
        [DisplayName("Bis")]
        public DateTime To { get; set; }

        public ICollection<TestCenter> AvailableTestCenters { get; set; }
    }
}
