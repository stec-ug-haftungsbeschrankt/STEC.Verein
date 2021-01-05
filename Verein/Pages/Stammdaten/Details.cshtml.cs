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
    public class DetailsModel : VereinPageModel
    {
        public DetailsModel(IDatabaseMediator databaseMediator, ILogger<DetailsModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        public StammdatenEintrag StammdatenEintrag { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            await base.Initialize().ConfigureAwait(false);
            StammdatenEintrag = await _databaseMediator.GetStammdatenById(id).ConfigureAwait(false);

            if (StammdatenEintrag == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
