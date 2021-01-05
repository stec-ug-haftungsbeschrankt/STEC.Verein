using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : VereinPageModel
    {

        public PersonalDataModel(
            IDatabaseMediator databaseMediator,
            UserManager<HundevereinUser> userManager,
            ILogger<PersonalDataModel> logger)
            : base(databaseMediator, logger, userManager)
        {

        }

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
    }
}