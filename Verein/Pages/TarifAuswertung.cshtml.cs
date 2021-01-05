using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;
using Verein.ViewModels;

namespace Verein.Pages
{
    public class TarifAuswertungModel : VereinPageModel
    {
        private readonly VereinDbContext _context;
        public TarifAuswertungModel(IDatabaseMediator databaseMediator, ILogger<TarifAuswertungModel> logger)
            : base(databaseMediator, logger)
        {
            _context = databaseMediator.GetDbContext();
        }

        public List<TarifEvalViewModel> Tarifdaten { get; set; }


        private async Task<List<TarifEvalViewModel>> CalculateTarifdaten(IList<Mitglied> mitglieder)
        {
            var tarifdaten = new List<TarifEvalViewModel>();
            TarifCalculator calculator = new TarifCalculator(_context, _logger);

            foreach (var mitglied in mitglieder)
            {
                var result = await calculator.CalculateBeitrag(mitglied).ConfigureAwait(false);
                var entry = new TarifEvalViewModel()
                {
                    MitgliedsId = mitglied.Id,
                    FullName = $"{mitglied.Vorname} {mitglied.Name}",
                    Mitgliedsnummer = mitglied.MitgliedsNummer,
                    Details = result.Details,
                    Errors = result.Errors,
                    Beitrag = result.Beitrag
                };
                tarifdaten.Add(entry);
            }
            return tarifdaten;
        }



        public async Task<IActionResult> OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);

            var mitglieder = await _context.Mitglieder
                                           .Include(m => m.Hunde)
                                           .Include(m => m.Familie)
                                           .Include(m => m.Arbeitsstunden)
                                           .ThenInclude(h => h.Arbeitseinsatz)
                                           .ToListAsync()
                                           .ConfigureAwait(false);

            if (mitglieder == null)
            {
                return NotFound();
            }

            Tarifdaten = await CalculateTarifdaten(mitglieder).ConfigureAwait(false);
            return Page();
        }


        public async Task<IActionResult> OnPostExportExcelAsync()
        {
            await base.Initialize().ConfigureAwait(false);

            var mitglieder = await _context.Mitglieder
                                           .Include(m => m.Hunde)
                                           .Include(m => m.Familie)
                                           .Include(m => m.ZahlungsInfo)
                                           .Include(m => m.Arbeitsstunden)
                                           .ThenInclude(h => h.Arbeitseinsatz)
                                           .ToListAsync()
                                           .ConfigureAwait(false);

            if (mitglieder == null)
            {
                return NotFound();
            }

            Tarifdaten = await CalculateTarifdaten(mitglieder).ConfigureAwait(false);
            TarifListExporter exporter = new TarifListExporter();

            var buffer = exporter.ExportToExcel(Tarifdaten, mitglieder);
            return new FileContentResult(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "TarifZahlungsinformationen.xslt"
            };
        }
    }
}