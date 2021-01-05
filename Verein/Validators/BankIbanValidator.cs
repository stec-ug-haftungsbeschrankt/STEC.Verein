using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Verein.Models;

namespace Verein.Validators
{
    public class BankIbanValidator : ValidationAttribute
    {
        public BankIbanValidator()
        {

        }

        public string GetErrorMessage() =>
            $"IBAN ist ungültig. Bitte prüfen sie die Eingabe.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var iban = ((BankInformation)validationContext.ObjectInstance).Iban;

            if (iban == null)
            {
                return ValidationResult.Success;
            }

            var isValid = ValidateIban(iban);

            if (!isValid)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }

        private bool ValidateIban(string iban)
        {
            string ibanCleared = iban.ToUpper().Replace(" ","").Replace("-","");
            string ibanSwapped = ibanCleared.Substring(4)+ibanCleared.Substring(0,4);
            string sum = ibanSwapped.Aggregate("", (current, c) => current + (char.IsLetter(c) ? (c - 55).ToString() : c.ToString()));

            var d = decimal.Parse(sum);
            return ((d % 97) == 1);
        }
    }
}

