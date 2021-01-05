using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Kurse
{
    public class DeleteModel : VereinPageModel
    {
        public DeleteModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {

        }

        [BindProperty]
        public Kurs Kurs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await base.Initialize().ConfigureAwait(false);
            Kurs = await _databaseMediator.GetKursById(id).ConfigureAwait(false);

            if (Kurs == null)
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

            Kurs = await _databaseMediator.GetKursById(id).ConfigureAwait(false);

            if (Kurs != null)
            {
                await _databaseMediator.DeleteKurs(Kurs).ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
