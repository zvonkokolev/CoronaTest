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
    public class CampaignRepository : ICampaignRepository
    {
        private ApplicationDbContext _dbContext;

        public CampaignRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCampaignAsync(Campaign newCampaign) =>
            await _dbContext.Campaigns
            .AddAsync(newCampaign);

        public async Task AddCampaignsRangeAsync(List<Campaign> campaigns) =>
            await _dbContext.Campaigns
            .AddRangeAsync(campaigns);

        public async Task<Campaign[]> GetAllCampaignsAsync() =>
            await _dbContext.Campaigns
            .ToArrayAsync();

        public async Task<Campaign> GetCampaignByIdAsync(int id) =>
            await _dbContext.Campaigns
                .Include(a => a.AvailableTestCenters)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Campaign> RemoveCampaignAsync(int campaignId)
        {
            Campaign cmp = await _dbContext.Campaigns
                 .Where(c => c.Id == campaignId)
                 .FirstOrDefaultAsync();

            _dbContext.Campaigns.Remove(cmp);
            return cmp;
        }

        public void UpdateCampaignsData(Campaign oldCampaign) =>
            _dbContext.Campaigns
                .Attach(oldCampaign)
                .State = EntityState.Modified;
    }
}
