using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;
using Verein.ViewModels;

namespace Verein.Pages.Benutzer
{
    public class DeleteModel : VereinPageModel
    {

        public DeleteModel(IDatabaseMediator databaseMediator, ILogger<DeleteModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        [BindProperty]
        public UserViewModel HundevereinUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            await base.Initialize().ConfigureAwait(false);
            var user = await _databaseMediator.GetUserById(id).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            HundevereinUser = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Approved = user.Approved,
                Rolle = user.Rolle
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            var user = await _databaseMediator.GetUserById(id).ConfigureAwait(false);

            if (user != null)
            {
                await _databaseMediator.DeleteUser(user).ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}