using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : VereinPageModel
    {
        private readonly SignInManager<HundevereinUser> _signInManager;

        public LogoutModel(IDatabaseMediator databaseMediator, SignInManager<HundevereinUser> signInManager, ILogger<LogoutModel> logger)
            : base(databaseMediator, logger)
        {
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await base.Initialize().ConfigureAwait(false);
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
