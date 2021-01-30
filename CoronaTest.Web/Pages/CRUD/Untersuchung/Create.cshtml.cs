using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CoronaTest.Web.Pages.CRUD.Untersuchung
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public DateTime DateTime { get; set; }

        [BindProperty]
        public Campaign Kampagne { get; set; }

        [BindProperty]
        public List<Campaign> Campaigns { get; set; }

        [BindProperty]
        public TestCenter Testzentrum { get; set; }

        [BindProperty]
        public List<TestCenter> TestCenters { get; set; } = new List<TestCenter>();

        [BindProperty]
        public Examination Examination { get; set; }

        [BindProperty]
        public Guid VerificationIdentifier { get; set; }

        [BindProperty]
        public int ParticipantId { get; set; }

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(Guid verificationIdentifier, int participantId)
        {
            VerificationIdentifier = verificationIdentifier;
            ParticipantId = participantId;

            Campaigns = await _unitOfWork.Campaigns
                .GetAllCampaignsAsync();
            if(Campaigns == null)
            {
                Message = "Es ist keine Kampagne vorhanden";
            }
            Kampagne = Campaigns.FirstOrDefault();
            TestCenters.AddRange(Kampagne.AvailableTestCenters);

            if (TestCenters == null)
            {
                Message = "Es ist kein Testzentrum vorhanden";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Campaigns = await _unitOfWork.Campaigns
                    .GetAllCampaignsAsync();
                if (Campaigns == null)
                {
                    Message = "Es ist keine Kampagne vorhanden";
                }
                Kampagne = Campaigns.FirstOrDefault();
                TestCenters.AddRange(Kampagne.AvailableTestCenters);

                if (TestCenters == null)
                {
                    Message = "Es ist kein Testzentrum vorhanden";
                }
                return Page();
            }

            Testzentrum = await _unitOfWork.TestCenters.
                GetTestCenterByIdAsync(Testzentrum.Id); 
            
            Examination = Examination.CreateNew();
            Examination.ExaminationAt = Testzentrum;
            Examination.Identifier = DateTime.ToString();

            await _unitOfWork.Examinations
                .AddExaminationAsync(Examination);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
