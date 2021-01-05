using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;
using Verein.ViewModels;


namespace Verein.Pages.Hunde
{
    public class DetailsModel : VereinPageModel
    {

        public DetailsModel(IDatabaseMediator databaseMediator, ILogger<DetailsModel> logger)
            : base(databaseMediator, logger)
        {

        }

        public Hund Hund { get; set; }

        public IList<Mitglied> Mitglieder { get; set; }

        [BindProperty]
        public string Selected { get; set; }


        private async Task<bool> InitializeFromDb(int? id)
        {
            Hund = await _databaseMediator.GetHundById(id).ConfigureAwait(false);
            Mitglieder = await _databaseMediator.GetMitgliederOrderedByName().ConfigureAwait(false);

            if (Hund == null ||
                Mitglieder == null)
            {
                return false;
            }
            return true;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await base.Initialize().ConfigureAwait(false);
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostMitgliedVerknuepfenAsync(int id, string selected)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(Selected))
            {
                var success = int.TryParse(selected, out var mitgliedsId);

                if (success)
                {
                    var mitglied = await _databaseMediator.GetMitgliedById(mitgliedsId).ConfigureAwait(false);
                    Hund.Besitzer = mitglied;
                    await _databaseMediator.UpdateHund(Hund).ConfigureAwait(false);

                }
            }

            return RedirectToPage("./Details", new { id = id });
        }


        public async Task<IActionResult> OnPostMitgliedVerknuepfungAufhebenAsync(int id)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            Hund.Besitzer = null;
            await _databaseMediator.UpdateHund(Hund).ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }
    }
}
