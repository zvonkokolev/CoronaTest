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

        public async Task AddRangeAsync(IEnumerable<Campaign> campaigns) =>
            await _dbContext.Campaigns
            .AddRangeAsync(campaigns);

        public async Task<List<Campaign>> GetAllCampaignsAsync() =>
            await _dbContext.Campaigns
            .Include(c => c.AvailableTestCenters)
            .ToListAsync();

        public async Task<List<KampagneDto>> GetAllCampaignsDtosAsync() =>
            await _dbContext.Campaigns
            .Include(c => c.AvailableTestCenters)
            .Select(c => new KampagneDto
            {
                Name = c.Name,
                From = c.From,
                To = c.To,
                AvailableTestCenters = c.AvailableTestCenters
            })
            .ToListAsync();

        public async Task<Campaign> GetCampaignByIdAsync(int id) =>
            await _dbContext.Campaigns
                .SingleOrDefaultAsync(m => m.Id == id);

        public async Task<KampagneDto> GetCampaignDtoByIdAsync(int id)
        {
            var campaign = await _dbContext.Campaigns
                .Include(a => a.AvailableTestCenters)
                .SingleOrDefaultAsync(a => a.Id == id);
            return new KampagneDto
            {
                Name = campaign.Name,
                From = campaign.From,
                To = campaign.To,
                AvailableTestCenters = campaign.AvailableTestCenters
            };
        }

        public async Task<int> GetCountAsync() =>
            await _dbContext.Campaigns.CountAsync();

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
