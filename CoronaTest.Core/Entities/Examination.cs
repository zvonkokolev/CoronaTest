using System;
using System.Collections.Generic;
using System.Text;
using static CoronaTest.Core.Enums.Enums;

namespace CoronaTest.Core.Entities
{
    public class Examination : BaseEntity
    {
        public Campaign Campaign { get; set; }
        public Participant Participant { get; set; }
        public TestCenter TestCenter { get; set; }
        public TestResult Result { get; set; }
        public ExaminationStates State { get; set; }
        public DateTime ExaminationAt { get; set; }
        public string Identifier { get; set; }

        public static Examination CreateNew()
        {
            return new Examination();
        }

        public string GetReservation() =>
            $"Reservation für {Identifier} " +
            $"am {ExaminationAt} " +
            $"im {TestCenter?.Name}";


        public string CancelReservation() =>
            $"Storno für {Identifier} " +
            $"am {ExaminationAt} " +
            $"im {TestCenter?.Name}";
    }
}
