using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaTest.Api.Dtos
{
    public class TestCenterDto
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
    }
}
