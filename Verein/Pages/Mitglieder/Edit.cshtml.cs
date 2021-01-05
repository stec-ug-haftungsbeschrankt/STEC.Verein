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
using Verein.Maps;

namespace Verein.Pages.Mitglieder

{
    public class EditModel : VereinPageModel
    {
        private readonly IGeoService _geoService;


        public EditModel(IDatabaseMediator databaseMediator, ILogger<EditModel> logger, UserManager<HundevereinUser> userManager, IGeoService geoService)
            : base(databaseMediator, logger, userManager)
        {
            _geoService = geoService;
        }

        [BindProperty]
        public Mitglied Mitglied { get; set; }

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
            Mitglied = await _databaseMediator.GetMitgliedById(id).ConfigureAwait(false);

            if (Mitglied == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {           
            // Generate Mitgliedsnummer für JT und WS
            if (string.IsNullOrEmpty(Mitglied.MitgliedsNummer))
            {
                MitgliedsnummerGenerator generator = new MitgliedsnummerGenerator(_databaseMediator);
                Mitglied.MitgliedsNummer = await generator.GenerateMitgliedsnummer(Mitglied.Typ).ConfigureAwait(false);
                ModelState.Remove("Mitglied.MitgliedsNummer");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            await base.Initialize().ConfigureAwait(false);
            
            try
            {
                Mitglied.Entfernung = await GetDistance().ConfigureAwait(false);
                await _databaseMediator.UpdateMitglied(Mitglied).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _databaseMediator.MitgliedExists(Mitglied.Id).ConfigureAwait(false)))
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

        private async Task<double> GetDistance()
        {
            var location = new Position()
            {
                Latitude = GetDoublePropertyValue("VereinLatitude"),
                Longitude = GetDoublePropertyValue("VereinLongitude")
            };
            var vereinAdresse = GetPropertyValue("VereinAdresse");
            var mitgliedAdresse = $"{Mitglied.Strasse} {Mitglied.Hausnummer}, {Mitglied.Postleitzahl} {Mitglied.Ort}";

            var distance = await _geoService.GetKmDistance(location, mitgliedAdresse).ConfigureAwait(false);
            return distance;
        }
    }
}
