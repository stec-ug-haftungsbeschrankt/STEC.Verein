using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public partial class IndexModel : VereinPageModel
    {
        private readonly SignInManager<HundevereinUser> _signInManager;

        public IndexModel(
            IDatabaseMediator databaseMediator,
            UserManager<HundevereinUser> userManager,
            SignInManager<HundevereinUser> signInManager,
            ILogger<IndexModel> logger)
            : base(databaseMediator, logger, userManager)
        {
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Vor- und Nachname")]
            public string FullName { get; set; }

            [Phone]
            [Display(Name = "Telefonnummer")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(HundevereinUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user).ConfigureAwait(false);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user).ConfigureAwait(false);

            Username = userName;

            Input = new InputModel
            {
                FullName = user.FullName,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await base.Initialize().ConfigureAwait(false);
            await LoadAsync(user).ConfigureAwait(false);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user).ConfigureAwait(false);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user).ConfigureAwait(false);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber).ConfigureAwait(false);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.FullName != user.FullName)
            {
                user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
                user.FullName = Input.FullName;

                var setFullNameResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);

                if (!setFullNameResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
