using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.ZahlungsInformationen
{
    public class EditModel : VereinPageModel
    {

        public EditModel(IDatabaseMediator databaseMediator, ILogger<EditModel> logger, UserManager<HundevereinUser> userManager)
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
            ViewData["BesitzerId"] = await _databaseMediator.GetMitgliederSelectList().ConfigureAwait(false);
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await base.Initialize().ConfigureAwait(false);
                ViewData["BesitzerId"] = await _databaseMediator.GetMitgliederSelectList().ConfigureAwait(false);
                return Page();
            }

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            try
            {
                await _databaseMediator.UpdateZahlungsinformation(BankInformation).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _databaseMediator.ZahlungsinformationExists(BankInformation.Id).ConfigureAwait(false)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

    }
}
