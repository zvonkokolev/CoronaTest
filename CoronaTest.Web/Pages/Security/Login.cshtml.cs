using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using CoronaTest.Web.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoronaTest.Web.Pages.Security
{
    public class LoginModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;

        [BindProperty]
        [Required(ErrorMessage = "Sozialversicherungsnummer ist verpflichtend")]
        [StringLength(10, ErrorMessage = "SVN hat genau 10 Ziffer.", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Nur Zahlen sind erlaubt.")]
        [SsnValidation]
        [DisplayName("SVN")]
        public string SocialSecurityNumber { get; set; }

        [BindProperty]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Handynummer")]
        [Required(ErrorMessage = "Handynummer ist verpflichtend")]
        [StringLength(16, ErrorMessage = "Die {0} muss zw. {2} und {1} Zeichen lang sein!", MinimumLength = 7)]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Bitte geben Sie eine gültige Telefonnummer ein.")]
        public string Mobilenumber { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public LoginModel(
            IUnitOfWork unitOfWork,
            ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Angaben sind nicht korrekt";
                return Page();
            }

            if (!SsnValidation.IsValideSsn(SocialSecurityNumber))
            {
                ModelState.AddModelError(nameof(SocialSecurityNumber), "Diese SVNr ist unbekannt!");
                Message = "SVN Angabe ist nicht korrekt";
                return Page();
            }
/*
            if (SocialSecurityNumber != "0000080384")
            {
                ModelState.AddModelError(nameof(SocialSecurityNumber), "Diese SVNr ist unbekannt!");
                return Page();
            }
*/
            // analog für HandyNr

            VerificationToken verificationToken = VerificationToken.NewToken();

            await _unitOfWork.VerificationTokens.AddAsync(verificationToken);
            await _unitOfWork.SaveChangesAsync();

            _smsService.SendSms(Mobilenumber, $"CoronaTest - Token: {verificationToken.Token} !");
            Participant participant;
            try
            {
                participant = await _unitOfWork.Participants.GetParticipantByPhoneAsync(Mobilenumber);
            }
            catch(Exception)
            {
                Message = "Teilnehmer nicht vorhanden";
                return Page();
            }
             
            return RedirectToPage("/Security/Verification", new { verificationIdentifier = verificationToken.Identifier, participantId = participant.Id });
        }
    }
}
