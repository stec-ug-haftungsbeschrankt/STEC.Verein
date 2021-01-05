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
    public class TwoFactorAuthenticationModel : VereinPageModel
    {
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}";

        private readonly SignInManager<HundevereinUser> _signInManager;

        public TwoFactorAuthenticationModel(
            IDatabaseMediator databaseMediator,
            UserManager<HundevereinUser> userManager,
            SignInManager<HundevereinUser> signInManager,
            ILogger<TwoFactorAuthenticationModel> logger)
            : base(databaseMediator, logger, userManager)
        {
            _signInManager = signInManager;
        }

        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        [BindProperty]
        public bool Is2faEnabled { get; set; }

        public bool IsMachineRemembered { get; set; }

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

            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false) != null;
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user).ConfigureAwait(false);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync().ConfigureAwait(false);
            StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
            return RedirectToPage();
        }
    }
}