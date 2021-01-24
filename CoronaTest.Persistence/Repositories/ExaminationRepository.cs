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
                        await _dbContext.Examinations.FirstOrDefaultAsync(m => m.Id == id);

        public bool IsExistingExamination(int id) =>
                        _dbContext.Examinations.Any(e => e.Id == id);

        public void RemoveExamination(Examination examination) =>
                                    _dbContext.Examinations.Remove(examination);

        public void UpdateExamination(Examination examination) =>
            _dbContext.Attach(examination).State = EntityState.Modified;

    }
}
