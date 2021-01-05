using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Arbeitseinsaetze
{
    public class EditModel : VereinPageModel
    {
        public EditModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {

        }

        [BindProperty]
        public Arbeitseinsatz ArbeitsTaetigkeit { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await base.Initialize().ConfigureAwait(false);
            ArbeitsTaetigkeit = await _databaseMediator.GetArbeitseinsatzById(id).ConfigureAwait(false);

            if (ArbeitsTaetigkeit == null)
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

            try
            {
                await _databaseMediator.UpdateArbeitseinsatz(ArbeitsTaetigkeit).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _databaseMediator.ArbeitseinsatzExists(ArbeitsTaetigkeit.Id).ConfigureAwait(false)))
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
