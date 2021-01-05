using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.ViewModels;

namespace Verein.Pages
{
    public class ArbeitsstundenAuswertungModel : VereinPageModel
    {
        private readonly VereinDbContext _context;

        public ArbeitsstundenAuswertungModel(IDatabaseMediator databaseMediator, ILogger<ArbeitsstundenAuswertungModel> logger)
            : base(databaseMediator, logger)
        {
            _context = databaseMediator.GetDbContext();
        }

        public List<ArbeitsstundenEvalViewModel> Arbeitsdaten { get; set; }

        public IList<int> Years { get; set; }

        public int Year;



        public async Task<IActionResult> OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);

            Year = DateTime.Now.Year;

            var success = await InitializeByYear(Year).ConfigureAwait(false);

            if (!success)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnGetFilterAsync(int query)
        {
            await base.Initialize().ConfigureAwait(false);

            Year = query;
            var success = await InitializeByYear(Year).ConfigureAwait(false);

            if (!success)
            {
                return NotFound();
            }

            return Page();
        }

        private async Task<bool> InitializeByYear(int year)
        {
            var mitglieder = await _context.Mitglieder
                                           .Include(m => m.Arbeitsstunden)
                                           .ThenInclude(h => h.Arbeitseinsatz)
                                           .ToListAsync()
                                           .ConfigureAwait(false);

            if (mitglieder == null)
            {
                return false;
            }

            Arbeitsdaten = new List<ArbeitsstundenEvalViewModel>();
            double erwarteteStunden = GetDoublePropertyValue("Arbeitsstunden");

            foreach (var mitglied in mitglieder)
            {
                double geleisteteStunden = 0.0;

                if (mitglied.Arbeitsstunden != null)
                {
                    geleisteteStunden = mitglied.Arbeitsstunden.Where(h => h.Arbeitseinsatz.Datum.Year == year)
                                                               .Sum(h => h.Dauer.TimeOfDay.TotalMinutes) / 60.0;
                }

                var entry = new ArbeitsstundenEvalViewModel()
                {
                    MitgliedsId = mitglied.Id,
                    FullName = $"{mitglied.Vorname} {mitglied.Name}",
                    Mitgliedsnummer = mitglied.MitgliedsNummer,
                    GeleisteteStunden = geleisteteStunden,
                    ErwarteteStunden = erwarteteStunden
                };
                Arbeitsdaten.Add(entry);
            }

            Arbeitsdaten = Arbeitsdaten.OrderBy(a => a.GeleisteteStunden).ToList();

            Years = await _context.Arbeitseinsaetze.Select(a => a.Datum.Year)
                                                   .Distinct()
                                                   .OrderByDescending(y => y)
                                                   .ToListAsync()
                                                   .ConfigureAwait(false);
            return true;
        }

    }
}