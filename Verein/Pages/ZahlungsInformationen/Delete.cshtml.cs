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
    public class DeleteModel : VereinPageModel
    {

        public DeleteModel(IDatabaseMediator databaseMediator, ILogger<DeleteModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        [BindProperty]
        public BankInformation BankInformation { get; set; }

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
            BankInformation = await _databaseMediator.GetZahlungsinformationByIdWithBesitzer(id).ConfigureAwait(false);

            if (BankInformation == null)
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

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            BankInformation = await _databaseMediator.GetZahlungsinformationById(id).ConfigureAwait(false);

            if (BankInformation != null)
            {
                await _databaseMediator.DeleteZahlungsinformation(BankInformation).ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
