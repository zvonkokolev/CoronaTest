﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoronaTest.Core.Entities
{
    public class TestCenter : BaseEntity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Postalcode { get; set; }
        public string Street { get; set; }
        public int SlotCapacity { get; set; }
        
        public List<Campaign> AvailableInCampaigns { get; set; }
    }
}
