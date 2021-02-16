using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Interfaces;
using CoronaTest.Core.DTOs;

namespace CoronaTest.Web.Pages.CRUD.Untersuchung
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<TestsDto> Examination { get;set; }

        public async Task OnGetAsync()
        {
            Examination = await _unitOfWork.Examinations
                .GetAllExaminationsDtosAsync();
        }
    }
}
