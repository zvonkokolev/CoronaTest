using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaTest.Persistence
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ParticipantRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddParticipantAsync(Participant newParticipant)
             => await _dbContext.Participants.AddAsync(newParticipant);

        public void UpdateParticipantsData(Participant participant)
        {
            _dbContext.Participants.Update(participant);
        }

        public async Task<Participant> RemoveParticipantAsync(int participantId)
        {
            Participant customer = await _dbContext.Participants
                 .Where(p => p.Id == participantId)
                 .FirstOrDefaultAsync();

            _dbContext.Participants.Remove(customer);
            return customer;
        }

        public async Task<Participant> GetPhoneBySvnAsync(string svn) =>
            await _dbContext.Participants.
            Where(s => s.SocialSecurityNumber.Equals(svn)).
            FirstOrDefaultAsync();

        public async Task<bool> CheckIfParticipantSvnExistsAsync(string svn)
        {
            var Participant = await _dbContext.Participants
                .Where(u => u.SocialSecurityNumber == svn)
                .FirstOrDefaultAsync();
            return Participant != null;

        }

        public async Task<Participant[]> GetAllParticipantsAsync()
             => await _dbContext.Participants.ToArrayAsync();

        public async Task<Participant> GetParticipantByPhoneAsync(string phone)
             => await _dbContext.Participants
            .Where(u => u.Mobilephone == phone)
            .FirstOrDefaultAsync();

        public async Task<Participant> GetParticipantByIdAsync(int id)
             => await _dbContext.Participants
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        public async Task<int> GetCountAsync() =>
            await _dbContext.Participants.CountAsync();
    }
}
