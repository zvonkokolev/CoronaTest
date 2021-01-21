using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Core.Interfaces
{
    public interface IVerificationTokenRepository
    {
        Task<VerificationToken> GetTokenByIdentifierAsync(Guid identifier);
        Task AddAsync(VerificationToken token);
    }
}
