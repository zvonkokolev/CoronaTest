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

		[BindProperty]
		public Guid VerificationIdentifier { get; set; }

		[BindProperty]
		public int ParticipantId { get; set; }
		public string Message { get; set; }

		public async Task<IActionResult> OnGetAsync(Guid verificationIdentifier, int? id)
		{
			// ---------- request cookie ------------
			var cookieValue = Request.Cookies["MyCookieId"];
			if (cookieValue == null)
			{
				Message = "Benutzer war nicht angemeldet";
				return RedirectToPage("Login", Message);
			}
			// LoggedUserId = int.Parse(cookieValue);
			// --------------------------------------
			VerificationIdentifier = verificationIdentifier;
			VerificationToken verificationToken = await _unitOfWork
				 .VerificationTokens
				 .GetTokenByIdentifierAsync(verificationIdentifier);
			ParticipantId = id.Value;

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
