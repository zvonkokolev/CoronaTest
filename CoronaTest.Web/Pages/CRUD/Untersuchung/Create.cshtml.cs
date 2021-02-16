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
using CoronaTest.Utils;

namespace CoronaTest.Web.Pages.CRUD.Untersuchung
{
    public class CreateModel : PageModel
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;
        #endregion

        #region properties
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
        #endregion

        #region constructor
        public CreateModel(IUnitOfWork unitOfWork, ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
        }
        #endregion


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
            Testzentrum = TestCenters.FirstOrDefault();

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
                ParticipantId = int.Parse(cookieValue);
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
                Testzentrum = TestCenters.FirstOrDefault();
                return Page();
            }
            Kampagne = await _unitOfWork.Campaigns
                .GetCampaignByIdAsync(Kampagne.Id);
            Testzentrum = await _unitOfWork.TestCenters.
                GetTestCenterByIdAsync(Testzentrum.Id);
            Participant p = await _unitOfWork.Participants
                .GetParticipantByIdAsync(ParticipantId);

            Examination = Examination.CreateNew();
            Examination.TestCenter = Testzentrum;
            Examination.Campaign = Kampagne;
            Examination.Participant = p;

            DateTime dt = ParseTimeSlot.SetExaminationAtTime(DateTime);
            try
            {
                var check = await _unitOfWork.Examinations.GetExaminationsByDateTimeAsync(dt);

                if (check.Count < Examination.TestCenter.SlotCapacity)
                {
                    Examination.ExaminationAt = dt;
                }
                else
                {
                    Message = "Es ist kein freie Termin vorhanden";
                    return Page();
                }
            }
            catch (Exception)
            {
                Message = "Datenbank derzeit nicht erreichbar";
                return Page();
            }

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

            return RedirectToPage("Index");
        }
    }
}
