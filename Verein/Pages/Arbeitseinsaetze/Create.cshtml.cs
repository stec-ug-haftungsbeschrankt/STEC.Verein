using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Arbeitseinsaetze
{
    public class CreateModel : VereinPageModel
    {
        public CreateModel(IDatabaseMediator databaseMediator, ILogger<CreateModel> logger)
            : base(databaseMediator, logger)
        {

        }

        public async Task<IActionResult> OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            return Page();
        }

        [BindProperty]
        public Arbeitseinsatz ArbeitsTaetigkeit { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _databaseMediator.AddArbeitseinsatz(ArbeitsTaetigkeit).ConfigureAwait(false);
            
            return RedirectToPage("./Index");
        }
    }
}
