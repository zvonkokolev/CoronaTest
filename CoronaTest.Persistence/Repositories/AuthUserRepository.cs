using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Persistence.Repositories
{
    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(AuthUser user)
            => await _dbContext
                    .AuthUsers
                    .AddAsync(user);

        public async Task<AuthUser> GetByEmailAsync(string eMail)
            => await _dbContext
                    .AuthUsers
                    .SingleOrDefaultAsync(u => u.Email == eMail);
    }
}
