using System.Threading.Tasks;
using CoronaTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Entities;

namespace CoronaTest.Web.Pages.CRUD.User
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
        public Participant Participant { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _unitOfWork.Participants.AddParticipantAsync(Participant);
            await _unitOfWork.SaveChangesAsync();

            // return RedirectToPage("./Index");
            return RedirectToPage("../../Security/Login", new { message = "Danke für Anmeldung"});
        }
    }
}
