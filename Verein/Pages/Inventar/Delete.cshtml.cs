using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Inventar
{
    public class DeleteModel : VereinPageModel
    {
        public DeleteModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {

        }

        [BindProperty]
        public Gegenstand Gegenstand { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await base.Initialize().ConfigureAwait(false);
            Gegenstand = await _databaseMediator.GetInventarById(id).ConfigureAwait(false);

            if (Gegenstand == null)
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

            Gegenstand = await _databaseMediator.GetInventarById(id).ConfigureAwait(false);

            if (Gegenstand != null)
            {
                await _databaseMediator.DeleteInventar(Gegenstand).ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
