using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaTest.Api.Dtos
{
    public class CampaignDto
    {
        public string Name { get; set; }
        [DisplayName("Von")]
        public DateTime From { get; set; }
        [DisplayName("Bis")]
        public DateTime To { get; set; }
    }
}
