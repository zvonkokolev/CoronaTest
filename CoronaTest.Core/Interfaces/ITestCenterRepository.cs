using CoronaTest.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoronaTest.Core.Interfaces
{
    public interface ITestCenterRepository
    {
        Task<TestCenter[]> GetAllTestCentersAsync();
        Task<TestCenter> GetTestCenterByIdAsync(int id);
        Task AddTestCenterAsync(TestCenter newTestCenter);
        Task AddTestCentersRangeAsync(List<TestCenter> testCenters);
        void UpdateTestCentersData(TestCenter oldTestCenter);
        Task<TestCenter> RemoveTestCenterAsync(int testCenterId);
        Task<int> GetCountAsync();
        Task AddRangeAsync(List<TestCenter> testCenters);
    }
}
