using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoronaTest.Core.Entities;
using CoronaTest.Persistence;
using CoronaTest.Core.Interfaces;

namespace CoronaTest.Web.Pages.CRUD.Kampagne
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Campaign> Campaign { get;set; }

        public async Task OnGetAsync()
        {
            Campaign = await _unitOfWork.Campaigns.GetAllCampaignsAsync();
        }
    }
}
