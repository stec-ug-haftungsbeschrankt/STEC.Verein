using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Linq;
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

namespace Verein.Pages
{
    [AllowAnonymous]
    public class InitializeModel : VereinPageModel
    {
        private readonly IEmailSender _emailSender;

        public InitializeModel(
            IDatabaseMediator databaseMediator,
            UserManager<HundevereinUser> userManager,
            IEmailSender emailSender,
            ILogger<InitializeModel> logger)
            : base(databaseMediator, logger, userManager)
        {
            _emailSender = emailSender;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name des Vereins")]
            public string VereinsName { get; set; }

            [Required]
            [Display(Name = "Firmierung")]
            public string VereinsFirmierung { get; set; } = "e.V.";

            [Required]
            [Display(Name = "Vereinsnummer des Verbands")]
            public string VereinsNummer { get; set; }

            [Required]
            [Display(Name = "Vor- und Nachname")]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "E-Mail")]
            public string Email { get; set; }

            [Required]
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

        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync().ConfigureAwait(false);

            if (users.Any())
            {
                // Redirect to Index if a user exists
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input == null)
            {
                await base.Initialize().ConfigureAwait(false);
                return Page();
            }

            if (ModelState.IsValid)
            {
                await EnsureStammdaten().ConfigureAwait(false);
                await EnsureAdminUser().ConfigureAwait(false);
                return RedirectToPage("/Index");
            }
            await base.Initialize().ConfigureAwait(false);
            return Page();
        }

        private async Task EnsureStammdaten()
        {
            var stammdaten = await _databaseMediator.GetStammdaten().ConfigureAwait(false);

            EnsureStammdatum(stammdaten, "VereinsName", Input.VereinsName);
            EnsureStammdatum(stammdaten, "VereinsFirmierung", Input.VereinsFirmierung);
            EnsureStammdatum(stammdaten, "VereinsNummer", Input.VereinsNummer);

            try
            {
                var context = _databaseMediator.GetDbContext();
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                _logger.LogWarning(exception.Message);
            }
        }

        private void EnsureStammdatum(IList<StammdatenEintrag> stammdaten, string titel, string wert)
        {
            var entry = stammdaten.SingleOrDefault(s => s.Title == titel);

            if (entry == null)
            {
                entry = new StammdatenEintrag()
                {
                    Title = titel,
                    Value = wert,
                    Type = Datatype.String
                };
                stammdaten.Add(entry);
            }
            else
            {
                entry.Value = wert;
                var context = _databaseMediator.GetDbContext();
                context.Attach(entry).State = EntityState.Modified;
            }
        }


        private async Task EnsureAdminUser()
        {
            var user = new HundevereinUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName,
                Approved = true,
                Rolle = BenutzerTyp.Vorstand
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

                await _emailSender.SendEmailAsync(Input.Email, Input.VereinsName + " - Invite & Confirm your email",
                    $"Hallo,<br /><br />Du hast soeben in der Hundeverein Verwaltungssoftware einen neuen Verein mit dem Namen {Input.VereinsName} angelegt.<br /><br />Bitte bestätige deinen Account indem du <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier klickst</a>.").ConfigureAwait(false);
            }
        }

    }
}
