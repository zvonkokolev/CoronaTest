using CoronaTest.Core.DTOs;
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
				  .Include(_ => _.Participant)
				  .Include(_ => _.TestCenter)
				  .Include(_ => _.Campaign)
				  .Select(e => new TestsDto(e))
				  .ToListAsync();

		public async Task<List<TestsDto>> GetFilteredTests
			 (DateTime selectedDateFilterFrom, DateTime selectedDateFilterTo)
		{

			return await _dbContext.Examinations
				.Include(_ => _.Participant)
				.Include(_ => _.TestCenter)
				.Include(_ => _.Campaign)
				.Where(p => p.ExaminationAt >= selectedDateFilterFrom
								&& p.ExaminationAt <= selectedDateFilterTo)
				.Select(p => new TestsDto(p))
				.ToListAsync();
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

		public async Task<TestsDto> GetExaminationDtoByIdAsync(int id) =>
			  await _dbContext.Examinations
			  .Include(e => e.TestCenter)
			 .Include(e => e.Participant)
			 .Where(e => e.Id == id)
			 .Select(t => new TestsDto(t))
			 .SingleOrDefaultAsync();

		public async Task<IEnumerable<Examination>> GetExaminationsWithFilterAsync(string postalCode = null, DateTime? from = null, DateTime? to = null)
		{
			var query = _dbContext
				.Examinations
				.Include(_ => _.Participant)
				.Include(_ => _.TestCenter)
				.Include(_ => _.Campaign)
				.AsQueryable();

			if (postalCode != null)
			{
				query = query.Where(_ => _.TestCenter.Postalcode == postalCode);
			}
			if (from != null)
			{
				query = query.Where(_ => _.ExaminationAt.Date >= from.Value.Date);
			}
			if (to != null)
			{
				query = query.Where(_ => _.ExaminationAt.Date <= to.Value.Date);
			}

			return await query
				 .OrderBy(_ => _.ExaminationAt)
				 .ToArrayAsync();
		}

		public async Task<IList<TestsDto>> GetExaminationDtoForParticipantByIdAsync(int id)
			=> await _dbContext.Examinations
					.Include(_ => _.Participant)
					.Include(_ => _.TestCenter)
					.Include(_ => _.Campaign)
					.Where(_ => _.Participant.Id == id)
					.Select(e => new TestsDto(e))
					.ToListAsync();
	}
}
