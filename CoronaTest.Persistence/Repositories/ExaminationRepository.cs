﻿using CoronaTest.Core.DTOs;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Persistence
{
    public class ExaminationRepository : IExaminationRepository
    {
        private ApplicationDbContext _dbContext;

        public ExaminationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddExaminationAsync(Examination examination) =>
                        await _dbContext.Examinations.AddAsync(examination);

        public async Task<IList<Examination>> GetAllExaminationsAsync() =>
                        await _dbContext.Examinations.ToListAsync();

        public async Task<Examination> GetExaminationByIdAsync(int id) =>
             await _dbContext.Examinations
            .FirstOrDefaultAsync(m => m.Id == id);

        public bool IsExistingExamination(int id) =>
                        _dbContext.Examinations.Any(e => e.Id == id);

        public void RemoveExamination(Examination examination) =>
            _dbContext.Examinations
            .Remove(examination);

        public void UpdateExamination(Examination examination) =>
            _dbContext.Attach(examination).State = EntityState.Modified;

        public async Task<int> GetCountAsync() =>
            await _dbContext.Examinations.CountAsync();

        public async Task<List<TestsDto>> GetAllExaminationsDtosAsync() =>
            await _dbContext.Examinations
                .Include(e => e.TestCenter)
                .Include(e => e.Participant)
                .Select(t => new TestsDto(t))
                .ToListAsync();

        public async Task<List<TestsDto>> GetFilteredTests
            (DateTime selectedDateFilterFrom, DateTime selectedDateFilterTo)
            {
                var a = await GetAllExaminationsDtosAsync();
                return a
                    .Where(p => p.ExaminationAt >= selectedDateFilterFrom 
                                && p.ExaminationAt <= selectedDateFilterTo)
                    .ToList();
            }

        public async Task<List<Examination>> GetExaminationsByDateTimeAsync(DateTime dt) =>
            await _dbContext.Examinations
            .Include(e => e.TestCenter)
            .Include(e => e.Participant)
                    .Where(t => t.ExaminationAt == dt)
                    .ToListAsync();

        public async Task<Examination> GetExaminationByIdentifierAsync(string examinationIdentifier)
            => await _dbContext.Examinations
               .Include(e => e.TestCenter)
               .Include(e => e.Participant)
               .SingleOrDefaultAsync(e => e.Identifier == examinationIdentifier)
               ;

    }
}
