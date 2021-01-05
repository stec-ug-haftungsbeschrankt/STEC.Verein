using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Verein.Data;
using Verein.Models;
using Verein.Maps;

namespace Verein.Pages.Mitglieder
{
    public class CreateModel : VereinPageModel
    {
        private readonly IGeoService _geoService;


        public CreateModel(IDatabaseMediator databaseMediator, IGeoService geoService, ILogger<CreateModel> logger)
            : base(databaseMediator, logger)
        {
            _geoService = geoService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            return Page();
        }

        [BindProperty]
        public Mitglied Mitglied { get; set; }

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

            await base.Initialize().ConfigureAwait(false);
          
            Mitglied.Entfernung = await GetDistance().ConfigureAwait(false);
            await _databaseMediator.AddMitglied(Mitglied).ConfigureAwait(false);

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
