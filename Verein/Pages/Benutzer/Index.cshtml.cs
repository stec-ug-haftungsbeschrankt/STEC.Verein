using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using STEC.Services.Mailing;
using Verein.Data;
using Verein.Models;
using Verein.ViewModels;
using Verein.Validators;

namespace Verein.Pages.Benutzer
{
    public class IndexModel : VereinPageModel
    {
        private readonly IEmailSender _emailSender;

        public IndexModel(
            IDatabaseMediator databaseMediator,
            UserManager<HundevereinUser> userManager,
            ILogger<IndexModel> logger,
            IEmailSender emailSender)
            : base(databaseMediator, logger, userManager)
        {
            _emailSender = emailSender;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Vor- und Nachname")]
            public string FullName { get; set; }

            [Required]
            public BenutzerTyp Rolle { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "E-Mail")]
            public string Email { get; set; }

            [Verein.Validators.PasswordValidator]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Passwort wiederholen")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<UserViewModel> HundevereinUsers { get;set; }

        private async Task<bool> InitializeFromDb()
        {
            var users = await _databaseMediator.GetUsers().ConfigureAwait(false);
            HundevereinUsers = users.Select(u =>
                new UserViewModel() {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Approved = u.Approved,
                    Rolle = u.Rolle
                }).ToList();

            if (HundevereinUsers == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }
            await base.Initialize().ConfigureAwait(false);
            var success = await InitializeFromDb().ConfigureAwait(false);

            if (success == false)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(string id)
        {
            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            var user = await _databaseMediator.GetUserById(id).ConfigureAwait(false);

            if (user != null)
            {
                user.Approved = true;
                await _databaseMediator.UpdateUser(user).ConfigureAwait(false);
            }

            await base.Initialize().ConfigureAwait(false);
            var success = await InitializeFromDb().ConfigureAwait(false);

            if (success == false)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostResetPasswordAsync(string id)
        {
            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            var dbUser = await _databaseMediator.GetUserById(id).ConfigureAwait(false);

            if (dbUser is object)
            {

                var user = await _userManager.FindByEmailAsync(dbUser.Email).ConfigureAwait(false);
                if (user is null || !(await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false)))
                {
                    _logger.LogInformation($"User with E-Mail Address {dbUser.Email} not found with UserManager");
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./Index");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                try
                {
                    _logger.LogWarning($"Found User with E-Mail Address {dbUser.Email}. Sending Password Reset E-Mail");

                    await _emailSender.SendEmailAsync(
                        dbUser.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);
                }
                catch (System.Exception exception)
                {
                    _logger.LogWarning(exception.Message);
                    _logger.LogWarning(exception.StackTrace);

                    if (exception.InnerException is object)
                    {
                        _logger.LogWarning(exception.Message);
                        _logger.LogWarning(exception.StackTrace);
                    }
                }
            }

            await base.Initialize().ConfigureAwait(false);
            var success = await InitializeFromDb().ConfigureAwait(false);

            if (success == false)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostInviteAsync()
        {
            if (Input == null)
            {
                await base.Initialize().ConfigureAwait(false);

                if (await InitializeFromDb().ConfigureAwait(false) == false)
                {
                    return NotFound();
                }
                return Page();
            }

            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            await base.Initialize().ConfigureAwait(false);

            // Generate Password
            PasswordGenerator passwordGenerator = new PasswordGenerator();
            var password = passwordGenerator.Generate(12, PasswordRules.AlphabeticCharacters | PasswordRules.NumericCharacters | PasswordRules.SpecialChararcters);
            _logger.LogWarning(password);

            Input.Password = password;
            Input.ConfirmPassword = password;

            ModelState.Remove("Input.Password");
            ModelState.Remove("Input.ConfirmPassword");

            if (ModelState.IsValid)
            {
                var user = new HundevereinUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FullName = Input.FullName,
                    Rolle = Input.Rolle
                };

                var result = await _userManager.CreateAsync(user, Input.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    var vereinsName = GetPropertyValue("VereinsName");

                    await _emailSender.SendEmailAsync(Input.Email, vereinsName + " - Invite & Confirm your email",
                    $"Hallo {Input.FullName},<br /><br />Du wurdest zur Verwaltungssoftware vom Hundeverein {vereinsName} eingeladen.<br /><br />Bitte bestätige deinen Account indem du <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier klickst</a>.<br /><br />Dein Benutzername lautet: {Input.Email}<br />Dein Passwort lautet: {Input.Password}").ConfigureAwait(false);

                    ViewData["UserInviteSuccess"] = $"Benutzer {Input.FullName} mit E-Mail {Input.Email} erfolgreich eingeladen.";
                }
                else
                {
                    ViewData["UserInviteError"] = "Das Anlegen des Benutzers ist fehlgeschlagen";
                }
            }
            else
            {
                ViewData["UserInviteError"] = "Passwort muss Buchstaben, Zahlen und Sonderzeichen enthalten";
                ModelState.AddModelError("Password", "Passwort muss Sonderzeichen enthalten");
            }

            var success = await InitializeFromDb().ConfigureAwait(false);

            if (success == false)
            {
                return NotFound();
            }
            return Page();
        }

    }
}
