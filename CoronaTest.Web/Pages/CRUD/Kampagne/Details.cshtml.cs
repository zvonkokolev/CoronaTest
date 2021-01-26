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
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Campaign Campaign { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Campaign = await _unitOfWork.Campaigns.GetCampaignByIdAsync(id.Value);

            if (Campaign == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
