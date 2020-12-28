using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Verein.Validators;
using System.ComponentModel.DataAnnotations;
using Verein.Pages;

namespace STEC.Verein.Tests
{
    public class PasswordValidationTest
    {
        [Theory]
        [InlineData("Teea+f#3123")]
        [InlineData("Test#/123")]
        [InlineData("Test#+123")]
        [InlineData("Test#123")]
        public void PasswordValidationSuccessTest(string password)
        {
            InitializeModel.InputModel model = new InitializeModel.InputModel()
            {
                VereinsName = "HSV Breitenaurach",
                VereinsFirmierung = "e.V.",
                VereinsNummer = "0789",
                FullName = "Stefan Schick",
                Email = "some.name@stecug.de",
                Password = password,
                ConfirmPassword = password
            };
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);

            Assert.True(result);
        }


        [Theory]
        [InlineData("Test123")]
        [InlineData("test#123")]
        public void PasswordValidationFailTest(string password)
        {
            InitializeModel.InputModel model = new InitializeModel.InputModel()
            {
                VereinsName = "HSV Breitenaurach",
                VereinsFirmierung = "e.V.",
                VereinsNummer = "0789",
                FullName = "Stefan Schick",
                Email = "some.name@stecug.de",
                Password = password,
                ConfirmPassword = password
            };
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);

            Assert.False(result);
        }
    }
}
