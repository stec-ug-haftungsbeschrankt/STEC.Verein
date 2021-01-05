using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Verein.Models;
using Verein.Data;
using STEC.Services.Mailing;

namespace Verein.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : VereinPageModel
    {
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(IDatabaseMediator databaseMediator, UserManager<HundevereinUser> userManager, ILogger<ForgotPasswordModel> logger, IEmailSender emailSender)
            : base(databaseMediator, logger, userManager)
        {
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email).ConfigureAwait(false);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
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
                    await _emailSender.SendEmailAsync(
                        Input.Email,
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

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
