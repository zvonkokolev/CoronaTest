using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using StringRandomizer;
using StringRandomizer.Options;

namespace CoronaTest.Web.Pages.CRUD.Untersuchung
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;

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

        public CreateModel(IUnitOfWork unitOfWork, ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
        }

        public async Task<IActionResult> OnGetAsync(Guid verificationIdentifier, int participantId)
        {
            // ---------- request cookie ------------
            var cookieValue = Request.Cookies["MyCookieId"];
            if (cookieValue == null)
            {
                Message = "Benutzer ist nicht angemeldet";
                return RedirectToPage("/Login", Message);
            }
            // --------------------------------------
            VerificationIdentifier = verificationIdentifier;
            ParticipantId = int.Parse(cookieValue);

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
            // ---------- request cookie ------------
            var cookieValue = Request.Cookies["MyCookieId"];
            if (cookieValue == null)
            {
                Message = "Benutzer ist nicht angemeldet";
                return RedirectToPage("/Login", Message);
            }
            // --------------------------------------
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
            if(Testzentrum.SlotCapacity > 0)
            {
                Testzentrum.SlotCapacity -= 1;
            }
            else
            {
                Message = "Es ist kein freie Termin vorhanden";
                return Page();
            }
            Examination.ExaminationAt = DateTime;
            // stringRandomizer nuGet packet
            var randomizer = new Randomizer(6, new DefaultRandomizerOptions(hasNumbers: false, hasLowerAlphabets: true, hasUpperAlphabets: true));
            var examIdent = randomizer.Next();

            Examination.Identifier = examIdent;

            await _unitOfWork.Examinations
                .AddExaminationAsync(Examination);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                Message = "Es ist kein freie Termin vorhanden";
                return Page();
            }
            try
            {
                Participant participant = await _unitOfWork.Participants.GetParticipantByIdAsync(ParticipantId);
                _smsService.SendSms(participant.Mobilephone, $"Id Nummer Corona-Test: { examIdent }");
            }
            catch (Exception)
            {
                Message = "Es ist kein Teilnehmer vorhanden";
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
