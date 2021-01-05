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

namespace Verein.Pages.Tarife
{
    public class EditModel : VereinPageModel
    {

        public EditModel(IDatabaseMediator databaseMediator, ILogger<EditModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        [BindProperty]
        public Tarif Tarif { get; set; }

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
            Tarif = await _databaseMediator.GetTarifById(id).ConfigureAwait(false);

            if (Tarif == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            try
            {
                await _databaseMediator.UpdateTarif(Tarif).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _databaseMediator.TarifExists(Tarif.Id).ConfigureAwait(false)))
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
