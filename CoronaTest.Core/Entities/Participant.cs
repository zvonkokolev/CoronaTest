using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoronaTest.Core.Entities
{
    public class Participant
    {
        [Display(Name = "Vorname")]
        [Required(ErrorMessage = "Vorname ist verpflichtend.")]
        [StringLength(20, ErrorMessage = "Vorname darf maximal 20 Stellen lang sein.")]
        public string Firstname { get; set; }

        [Display(Name = "Familienname")]
        [Required(ErrorMessage = "Familienname ist verpflichtend.")]
        public string Lastname { get; set; }

        [Display(Name = "SVN")]
        [Required(ErrorMessage = "Sozialversicherungsnummer ist verpflichtend.")]
        [StringLength(10, ErrorMessage = "SVN hat genau 10 Ziffer."), MinLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Nur Zahlen sind erlaubt.")]
        public string SocialSecurityNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Handynummer")]
        [Required(ErrorMessage = "Handynummer ist verpflichtend.")]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Bitte geben Sie eine gültige Telefonnummer ein.")]
        [RegularExpression(@"^([0][6][0-9][0-9])([0-9]{14})$", ErrorMessage = "Bitte geben Sie eine gültige Telefonnummer ein.")]
        public string Mobilephone { get; set; }

        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Postalcode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Stair { get; set; }
        public string Door { get; set; }
        public List<VerificationToken> Verifications { get; set; }
    }
}
