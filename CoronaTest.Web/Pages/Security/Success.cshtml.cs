using System;
using System.Threading.Tasks;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoronaTest.Web.Pages.Security
{
    public class SuccessModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public Guid VerificationIdentifier { get; set; }

        [BindProperty]
        public int ParticipantId { get; set; }

        public SuccessModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(Guid verificationIdentifier, int participantId)
        {
            VerificationIdentifier = verificationIdentifier;
            VerificationToken verificationToken = await _unitOfWork.VerificationTokens.GetTokenByIdentifierAsync(verificationIdentifier);
            ParticipantId = participantId;

            if (!ModelState.IsValid)
            {
                Message = "Angaben sind nicht korrekt";
                return Page();
            }

            if (verificationToken.ValidUntil >= DateTime.Now)
            {
                return RedirectToPage("/Security/TokenError");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostExaminateAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Angaben sind nicht korrekt";
                return Page();
            }
            Participant participant;
            try
            {
                participant = await _unitOfWork.Participants
                    .GetParticipantByIdAsync(ParticipantId);
            }
            catch (Exception)
            {
                Message = "Teilnehmer nicht vorhanden";
                return Page();
            }
            return RedirectToPage("../CRUD/Untersuchung/Create");
        }

        public async Task<IActionResult> OnPostSetprofileAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Angaben sind nicht korrekt";
                return Page();
            }
            Participant participant;
            try
            {
                participant = await _unitOfWork.Participants
                    .GetParticipantByIdAsync(ParticipantId);
            }
            catch (Exception)
            {
                Message = "Teilnehmer nicht vorhanden";
                return Page();
            }
            return RedirectToPage("../CRUD/User/Details", new 
            { 
                verificationIdentifier = VerificationIdentifier, 
                id = ParticipantId} 
            );
        }

    }
}
