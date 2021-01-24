using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Core.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant[]> GetAllParticipantsAsync();
        Task<Participant> GetParticipantByIdAsync(int id);
        Task<Participant> GetParticipantByPhoneAsync(string phone);
        Task AddParticipantAsync(Participant newParticipant);
        Task<bool> CheckIfParticipantSvnExistsAsync(string svn);
        void UpdateParticipantsData(Participant participant);
        Task<Participant> RemoveParticipantAsync(int participantId);
        Task<int> GetCountAsync();
    }
}
