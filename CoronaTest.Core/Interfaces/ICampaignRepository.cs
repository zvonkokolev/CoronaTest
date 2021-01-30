using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Core.Interfaces
{
    public interface ICampaignRepository
    {
        Task<List<Campaign>> GetAllCampaignsAsync();
        Task<Campaign> GetCampaignByIdAsync(int id);
        Task AddCampaignAsync(Campaign newCampaign);
        Task AddCampaignsRangeAsync(List<Campaign> campaigns);
        void UpdateCampaignsData(Campaign oldCampaign);
        Task<Campaign> RemoveCampaignAsync(int campaignId);
        Task AddRangeAsync(IEnumerable<Campaign> campaigns);
        Task<int> GetCountAsync();
    }
}
