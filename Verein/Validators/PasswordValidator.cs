using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Verein.Models;
using Verein.Pages.Benutzer;
using Verein.Pages;

namespace Verein.Validators
{
    public class PasswordValidator : ValidationAttribute
    {
        private readonly string Pattern = @"(?=.*\d)(?=(.*\W))(?=.*[a-z])(?=.*[A-Z])(?=.*\S).{8,}";

        public PasswordValidator()
        {

        }

        public string GetErrorMessage() =>
            $"Passwort ist ungülltig. Es muss mindestens 8 Zeichen haben und aus Groß- und Klein-Buchstaben, Zahlen und Sonderzeichen bestehen";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = GetPasswordFromContext(validationContext);

            if (password == null)
            {
                return ValidationResult.Success;
            }

            var isValid = ValidatePassword(password);

            if (!isValid)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }

        private static string GetPasswordFromContext(ValidationContext validationContext)
        {
            var password = string.Empty;

            if (validationContext.ObjectInstance is Areas.Identity.Pages.Account.Manage.ChangePasswordModel.InputModel changePasswordModel)
            {
                password = changePasswordModel.NewPassword;
            }
            else if (validationContext.ObjectInstance is Areas.Identity.Pages.Account.ResetPasswordModel.InputModel resetPasswordModel)
            {
                password = resetPasswordModel.Password;
            }
            else if (validationContext.ObjectInstance is Pages.Benutzer.IndexModel.InputModel indexModel)
            {
                password = indexModel.Password;
            }
            else if (validationContext.ObjectInstance is InitializeModel.InputModel initializeModel)
            {
                password = initializeModel.Password;
            }

            return password;
        }

        private bool ValidatePassword(string password)
        {
            Regex regex = new Regex(Pattern);

            return regex.IsMatch(password);
        }
    }
}