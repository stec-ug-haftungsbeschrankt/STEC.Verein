using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Verein.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : VereinPageModel
    {
        public ConfirmEmailModel(IDatabaseMediator databaseMediator, UserManager<HundevereinUser> userManager, ILogger<ConfirmEmailModel> logger)
            : base(databaseMediator, logger, userManager)
        {

        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            await base.Initialize().ConfigureAwait(false);
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
