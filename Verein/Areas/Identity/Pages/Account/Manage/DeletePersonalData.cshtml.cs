using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : VereinPageModel
    {
        private readonly SignInManager<HundevereinUser> _signInManager;

        public DeletePersonalDataModel(
            IDatabaseMediator databaseMediator,
            UserManager<HundevereinUser> userManager,
            SignInManager<HundevereinUser> signInManager,
            ILogger<DeletePersonalDataModel> logger)
            : base(databaseMediator, logger, userManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await base.Initialize().ConfigureAwait(false);
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password).ConfigureAwait(false))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
            var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync().ConfigureAwait(false);

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
