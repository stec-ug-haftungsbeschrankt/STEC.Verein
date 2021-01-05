using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Verein.Models;

namespace Verein.Validators
{
    public class MitgliedTypValidator : ValidationAttribute
    {
        public MitgliedTypValidator()
        {

        }

        public string GetErrorMessage() =>
            $"Welpenstunde und Jahresteilnahme können weder passive noch Familienmitgliedschaft haben. Bitte prüfen sie die Eingabe.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var mitglied = ((Mitglied)validationContext.ObjectInstance);

            if (mitglied == null)
            {
                return ValidationResult.Success;
            }

            var isValid = ValidateMitglied(mitglied);

            if (!isValid)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }

        private bool ValidateMitglied(Mitglied mitglied)
        {
            if (mitglied.Typ != MitgliederTyp.WelpenLernSpielstunde &&
                mitglied.Typ != MitgliederTyp.Jahresteilnahme)
            {
                return true;
            }

            if (mitglied.Passiv ||
                mitglied.Familienmitgliedschaft)
            {
                return false;
            }
            return true;
        }

    }
}