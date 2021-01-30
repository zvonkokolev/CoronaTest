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
    public class TestCenterRepository : ITestCenterRepository
    {
        private ApplicationDbContext _dbContext;

        public TestCenterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTestCenterAsync(TestCenter newTestCenter) =>
            await _dbContext.TestCenters
            .AddAsync(newTestCenter);

        public async Task AddTestCentersRangeAsync(List<TestCenter> testCenters) =>
            await _dbContext.TestCenters
            .AddRangeAsync(testCenters);

        public async Task<List<TestCenter>> GetAllTestCentersAsync() =>
            await _dbContext.TestCenters
            .ToListAsync();

        public async Task<TestCenter> GetTestCenterByIdAsync(int id) =>
            await _dbContext.TestCenters
                .SingleOrDefaultAsync(a => a.Id == id);

        public async Task<TestCenter> RemoveTestCenterAsync(int testCenterId)
        {
            TestCenter cmp = await _dbContext.TestCenters
                 .Where(c => c.Id == testCenterId)
                 .FirstOrDefaultAsync();

            _dbContext.TestCenters.Remove(cmp);
            return cmp;
        }

        public void UpdateTestCentersData(TestCenter oldTestCenter) =>
            _dbContext.TestCenters
                .Attach(oldTestCenter)
                .State = EntityState.Modified;

        public async Task<int> GetCountAsync() =>
            await _dbContext.TestCenters.CountAsync();

        public async Task AddRangeAsync(List<TestCenter> testCenters) =>
            await _dbContext.TestCenters
                .AddRangeAsync(testCenters);

    }
}
