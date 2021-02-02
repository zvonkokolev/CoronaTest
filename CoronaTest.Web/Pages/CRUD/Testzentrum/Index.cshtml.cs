using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;

namespace CoronaTest.Web.Pages.CRUD.Testzentrum
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<TestCenter> TestCenters { get;set; }

        public async Task OnGetAsync()
        {
            TestCenters = await _unitOfWork.TestCenters
                .GetAllTestCentersAsync();
        }
    }
}
