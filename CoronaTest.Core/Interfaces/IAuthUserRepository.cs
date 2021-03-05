using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Core.Interfaces
{
    public interface IAuthUserRepository
    {
        Task AddAsync(AuthUser user);
        Task<AuthUser> GetByEmailAsync(string eMail);
    }
}
