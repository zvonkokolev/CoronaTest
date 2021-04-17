using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Interfaces;
using CoronaTest.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using CoronaTest.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoronaTest.Web.Pages.CRUD.Untersuchung
{
	public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IList<TestsDto> Examination { get; set; }
		[BindProperty]
		public int ParticipantId { get; set; }

		public async Task<ActionResult> OnGetAsync()
		{
			// ------- request cookie ---------------------
			var cookieValue = Request.Cookies["MyCookieId"];

			if (cookieValue == null)
			{
				return RedirectToPage("../../Security/Login");
			}
			ParticipantId = int.Parse(cookieValue);
			Participant participant = await _unitOfWork.Participants
				.GetParticipantByIdAsync(ParticipantId);

			if (participant == null)
			{
				return RedirectToPage("../../Security/Login");
			}
			// --------------------------------------------
			//Examination = await _unitOfWork.Examinations
			//       .GetAllExaminationsDtosAsync();
			Examination = await _unitOfWork.Examinations
				.GetExaminationDtoForParticipantByIdAsync(participant.Id);

			return Page();
		}
	}
}
