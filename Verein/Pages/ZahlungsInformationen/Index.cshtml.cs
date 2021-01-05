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

namespace Verein.Pages.ZahlungsInformationen
{
    public class IndexModel : VereinPageModel
    {

        public IndexModel(IDatabaseMediator databaseMediator, ILogger<IndexModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        public IList<BankInformation> BankInformation { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            await base.Initialize().ConfigureAwait(false);
            BankInformation = await _databaseMediator.GetZahlungsinformationenWithBesitzer().ConfigureAwait(false);

            return Page();
        }
    }
}
