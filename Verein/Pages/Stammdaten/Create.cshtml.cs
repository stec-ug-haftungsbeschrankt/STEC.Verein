using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Stammdaten
{
    public class CreateModel : VereinPageModel
    {
        public CreateModel(IDatabaseMediator databaseMediator, ILogger<CreateModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            await base.Initialize().ConfigureAwait(false);
            return Page();
        }

        [BindProperty]
        public StammdatenEintrag StammdatenEintrag { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!(await IsAuthorized(BenutzerTyp.Vorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            await _databaseMediator.AddStammdaten(StammdatenEintrag).ConfigureAwait(false);

            return RedirectToPage("./Index");
        }
    }
}
