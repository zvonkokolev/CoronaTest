using System;
using System.ComponentModel.DataAnnotations;

namespace CoronaTest.Web.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SsnValidation : ValidationAttribute
    {

        public new ValidationResult GetValidationResult(object value, ValidationContext validationContext)
        {
            if (!(validationContext.ObjectInstance is string svn))
            {
                throw new ArgumentException("c");
            }

            if (!IsValideSsn(svn))
            {
                return new ValidationResult("SVN is nicht valide");
            }
            return ValidationResult.Success;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(validationContext.ObjectInstance is string svn))
            {
                throw new ArgumentException("Das zu validierende Objekt ist kein SVN");
            }

            if (!IsValideSsn(svn))
            {
                return new ValidationResult("SVN is nicht valide");
            }
            return ValidationResult.Success;
        }

        public static bool IsValideSsn(string svn)
        {
            bool isValid = false;
            int svnNumber, checkDigit;
            if (string.IsNullOrEmpty(svn) || string.IsNullOrWhiteSpace(svn) || svn.Length != 10)
            {
                isValid = false;
            }
            else
            {
                svnNumber = ((int)svn[0] - '0' ) * 3 +
                            ((int)svn[1] - '0' ) * 7 +                  
                            ((int)svn[2] - '0' ) * 9 +
                            ((int)svn[3] - '0' ) * 0 +
                            ((int)svn[4] - '0' ) * 5 +
                            ((int)svn[5] - '0' ) * 8 +
                            ((int)svn[6] - '0' ) * 4 +
                            ((int)svn[7] - '0' ) * 2 +
                            ((int)svn[8] - '0' ) * 1 +
                            ((int)svn[9] - '0' ) * 6;
                checkDigit = svnNumber % 11;
                if (checkDigit != 10 && (int)svn[3] - '0' == checkDigit)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
    }
}
