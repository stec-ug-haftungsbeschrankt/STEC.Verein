using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Arbeitseinsaetze
{
    public class DeleteModel : VereinPageModel
    {
        public DeleteModel(IDatabaseMediator databaseMediator)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArbeitsTaetigkeit = await _databaseMediator.GetArbeitseinsatzById(id).ConfigureAwait(false);

            if (ArbeitsTaetigkeit != null)
            {
                await _databaseMediator.DeleteArbeitseinsatz(ArbeitsTaetigkeit).ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
