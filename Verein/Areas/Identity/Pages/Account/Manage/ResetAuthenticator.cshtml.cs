using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : VereinPageModel
    {
        private readonly SignInManager<HundevereinUser> _signInManager;

        public ResetAuthenticatorModel(
            IDatabaseMediator databaseMediator,
            UserManager<HundevereinUser> userManager,
            SignInManager<HundevereinUser> signInManager,
            ILogger<ResetAuthenticatorModel> logger)
            : base(databaseMediator, logger, userManager)
        {
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await base.Initialize().ConfigureAwait(false);
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
            await _userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

            await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
            StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

            return RedirectToPage("./EnableAuthenticator");
        }
    }
}