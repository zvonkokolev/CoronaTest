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

namespace CoronaTest.Web.Pages.CRUD.Testzentrum
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public TestCenter TestCenter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TestCenter = await _unitOfWork.TestCenters.GetTestCenterByIdAsync(id.Value);

            if (TestCenter == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TestCenter = await _unitOfWork.TestCenters.GetTestCenterByIdAsync(id.Value);

            if (TestCenter != null)
            {
                await _unitOfWork.TestCenters.RemoveTestCenterAsync(TestCenter.Id);
                await _unitOfWork.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
