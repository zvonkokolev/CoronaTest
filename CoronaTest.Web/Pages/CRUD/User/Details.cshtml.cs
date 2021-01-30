using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;

namespace CoronaTest.Web.Pages.CRUD.User
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Participant Participant { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid verificationIdentifier, int? id)
        {
            VerificationToken verificationToken = await _unitOfWork.VerificationTokens
                                    .GetTokenByIdentifierAsync(verificationIdentifier);
            if (verificationToken.ValidUntil < DateTime.Now)
            {
                return RedirectToPage("../Security/TokenError");
            }

            if (id == null)
            {
                return NotFound();
            }

            Participant = await _unitOfWork.Participants.GetParticipantByIdAsync(id.Value);

            if (Participant == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
