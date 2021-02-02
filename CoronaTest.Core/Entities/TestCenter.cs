using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaTest.Core.Entities
{
    public class TestCenter : BaseEntity
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
