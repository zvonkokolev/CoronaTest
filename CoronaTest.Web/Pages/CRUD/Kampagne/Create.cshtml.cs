using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoronaTest.Core.Entities;
using CoronaTest.Persistence;
using CoronaTest.Core.Interfaces;

namespace CoronaTest.Web.Pages.CRUD.Kampagne
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Campaign Campaign { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _unitOfWork.Campaigns.AddCampaignAsync(Campaign);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
