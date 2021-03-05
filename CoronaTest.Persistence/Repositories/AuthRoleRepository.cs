using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Persistence.Repositories
{
    public class AuthRoleRepository : IAuthRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthRoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(AuthRole role)
            => await _dbContext.AuthRoles.AddAsync(role);
    }
}
