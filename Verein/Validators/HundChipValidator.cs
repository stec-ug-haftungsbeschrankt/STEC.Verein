using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Verein.Models;

namespace Verein.Validators
{
    public class HundChipValidator : ValidationAttribute
    {
        public HundChipValidator()
        {

        }

        public string GetErrorMessage() =>
            $"Chipnummer ist ungültig. Bitte prüfen sie die Eingabe.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var chipnummer = ((Hund)validationContext.ObjectInstance).ChipNummer;

            if (chipnummer == null)
            {
                return ValidationResult.Success;
            }

            var isValid = ValidateChipnummer(chipnummer);

            if (!isValid)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }

        private bool ValidateChipnummer(string chipnummer)
        {
            if (chipnummer.Length != 15) // Chipcode has a length of 15 digits
            {
                return false;
            }

            if (!chipnummer.All(char.IsDigit))
            {
                return false;
            }
            return true;
        }
    }
}