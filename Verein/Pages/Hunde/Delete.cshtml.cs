using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Hunde
{
    public class DeleteModel : VereinPageModel
    {

        public DeleteModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {

        }

        [BindProperty]
        public Hund Hund { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await base.Initialize().ConfigureAwait(false);
            Hund = await _databaseMediator.GetHundById(id).ConfigureAwait(false);

            if (Hund == null)
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

            Hund = await _databaseMediator.GetHundById(id).ConfigureAwait(false);

            if (Hund != null)
            {
                await _databaseMediator.DeleteHund(Hund).ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
