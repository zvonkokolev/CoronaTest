﻿using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CoronaTest.Persistence.Repositories
{
    public class VerificationTokenRepository : IVerificationTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VerificationTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VerificationToken> GetTokenByIdentifierAsync(Guid identifier)
            => await _dbContext
                .VerificationTokens
                .SingleAsync(verificationToken => verificationToken.Identifier == identifier);

        public async Task AddAsync(VerificationToken token)
            => await _dbContext
                .VerificationTokens
                .AddAsync(token);
    }
}
