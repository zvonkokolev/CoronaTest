using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaTest.Core.Entities
{
    public class Campaign : BaseEntity
    {
        public string Name { get; set; }
        [DisplayName("Von")]
        public DateTime From { get; set; }
        [DisplayName("Bis")]
        public DateTime To { get; set; }

        public ICollection<TestCenter> AvailableTestCenters { get; set; }
    }
}
