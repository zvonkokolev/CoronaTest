using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;

namespace CoronaTest.Web.Pages.CRUD.Testzentrum
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
    }
}
