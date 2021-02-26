using CoronaTest.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoronaTest.Core.DTOs
{
    public class ZentrumDto
    {
        public string Name { get; set; }
        [DisplayName("Stadt")]
        public string City { get; set; }
        [DisplayName("Postleitzahl")]
        public string Postalcode { get; set; }
        [DisplayName("Strasse")]
        public string Street { get; set; }
        [DisplayName("Kapazität")]
        public int SlotCapacity { get; set; }

        public ICollection<Campaign> AvailableInCampaigns { get; set; }
    }
}
