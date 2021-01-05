using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Verein.Models;

namespace Verein.Validators
{
    public class MitgliedSwhvNummerValidator : ValidationAttribute
    {
        public MitgliedSwhvNummerValidator()
        {

        }

        public string GetErrorMessage() =>
            $"Länge der SWHV Nummer ist falsch. Bitte prüfen sie die Eingabe.";

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
            if (string.IsNullOrEmpty(mitglied.SwhvMitgliedsNummer))
            {
                return true;
            }

            var length = mitglied.SwhvMitgliedsNummer.Length;
            /*
             * Consists of SWHV Vereinsnummer (4 Digits) (0610)
             * Consists of Mitgliedsnummer (5 Digits)
             */
            if (length == 9)
            {
                return true;
            }
            return false;
        }

    }
}