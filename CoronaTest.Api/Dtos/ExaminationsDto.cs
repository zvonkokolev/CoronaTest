using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoronaTest.Core.Enums.Enums;

namespace CoronaTest.Api.Dtos
{
    public class ExaminationsDto
    {
        public int Id { get; set; }
        public string ParticipantFullname { get; set; }
        public TestResult TestResult { get; set; }
        public DateTime ExaminationAt { get; set; }
        public string Identifier { get; set; }

        public ExaminationsDto(Examination examination)
        {
            Id = examination.Id;
            ParticipantFullname = $"{examination.Participant.Firstname} {examination.Participant.Lastname}";
            TestResult = examination.Result;
            ExaminationAt = examination.ExaminationAt;
            Identifier = examination.Identifier;
        }
    }
}
