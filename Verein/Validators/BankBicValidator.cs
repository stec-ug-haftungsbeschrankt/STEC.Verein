using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Verein.Models;

namespace Verein.Validators
{
    public class BankBicValidator : ValidationAttribute
    {
        private readonly string Pattern = "^[A-Z]{6}[2-9A-Z][0-9A-NP-Z](XXX|[0-9A-WYZ][0-9A-Z]{2})?$";

        public BankBicValidator()
        {

        }

        public string GetErrorMessage() =>
            $"BIC ist ungültig. Bitte prüfen sie die Eingabe.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bic = ((BankInformation)validationContext.ObjectInstance).Bic;

            if (bic == null)
            {
                return ValidationResult.Success;
            }

            var isValid = ValidateBic(bic);

            if (!isValid)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }

        private bool ValidateBic(string bic)
        {
            Regex regex = new Regex(Pattern);

            return regex.IsMatch(bic);
        }
    }
}