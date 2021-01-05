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

namespace Verein.Pages.Stammdaten
{
    public class DeleteModel : VereinPageModel
    {
        public DeleteModel(IDatabaseMediator databaseMediator, ILogger<DeleteModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        [BindProperty]
        public StammdatenEintrag StammdatenEintrag { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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
            StammdatenEintrag = await _databaseMediator.GetStammdatenById(id).ConfigureAwait(false);

            if (StammdatenEintrag is null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            StammdatenEintrag = await _databaseMediator.GetStammdatenById(id).ConfigureAwait(false);

            if (StammdatenEintrag is object)
            {
                await _databaseMediator.DeleteStammdaten(StammdatenEintrag).ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
