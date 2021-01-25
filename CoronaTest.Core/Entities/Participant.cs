using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoronaTest.Core.Entities
{
    public class Participant : BaseEntity
    {
        [Display(Name = "Vorname")]
        [Required(ErrorMessage = "Vorname ist verpflichtend.")]
        [StringLength(20, ErrorMessage = "Vorname darf maximal 20 Stellen lang sein.")]
        public string Firstname { get; set; }

        [Display(Name = "Familienname")]
        [Required(ErrorMessage = "Familienname ist verpflichtend.")]
        public string Lastname { get; set; }

        [DisplayName("SVN")]
        [Required(ErrorMessage = "Sozialversicherungsnummer ist verpflichtend.")]
        [StringLength(10, ErrorMessage = "SVN hat genau 10 Ziffer.", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Nur Zahlen sind erlaubt.")]
        public string SocialSecurityNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Handynummer")]
        [Required(ErrorMessage = "Handynummer ist verpflichtend.")]
        [StringLength(16, ErrorMessage = "Die {0} muss zw. {2} und {1} Zeichen lang sein!", MinimumLength = 7)]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Bitte geben Sie eine gültige Telefonnummer ein.")]
        public string Mobilephone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Festnetz Tel.Nr.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Geschlecht")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Geburtsdatum")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Postleitzahl")]
        public string Postalcode { get; set; }

        [Display(Name = "Stadt")]
        public string City { get; set; }

        [Display(Name = "Strasse")]
        public string Street { get; set; }

        [Display(Name = "Hausnummer")]
        public string HouseNumber { get; set; }

        [Display(Name = "Stockwerk")]
        public string Stair { get; set; }

        [Display(Name = "Türnummer")]
        public string Door { get; set; }

        public List<VerificationToken> Verifications { get; set; }
    }
}
