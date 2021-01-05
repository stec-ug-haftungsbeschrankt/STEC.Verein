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
    public class MitgliedEinAustritteModel : VereinPageModel
    {
        private readonly VereinDbContext _context;
        public MitgliedEinAustritteModel(IDatabaseMediator databaseMediator, ILogger<MitgliedEinAustritteModel> logger)
            : base(databaseMediator, logger)
        {
            _context = databaseMediator.GetDbContext();
        }

        public IList<Mitglied> Mitglieder { get; set; }

        public IList<Mitglied> MitgliederEintritte { get; set; }

        public IList<Mitglied> MitgliederAustritte { get; set; }


        private async Task<bool> InitializeFromDb()
        {
            Mitglieder = await _context.Mitglieder.Where(m =>
                (m.Eintrittsdatum != null && m.Eintrittsdatum.Year == DateTime.Now.Year) ||
                (m.Austrittsdatum.HasValue && m.Austrittsdatum.Value.Year == DateTime.Now.Year)).ToListAsync().ConfigureAwait(false);

            MitgliederEintritte = await _context.Mitglieder.Where(m =>
                m.Eintrittsdatum != null &&
                m.Eintrittsdatum.Year == DateTime.Now.Year &&
                (m.Typ == MitgliederTyp.Ehrenmitglied || m.Typ == MitgliederTyp.Mitglied)).ToListAsync().ConfigureAwait(false);

            MitgliederAustritte = await _context.Mitglieder.Where(m =>
                m.Austrittsdatum.HasValue &&
                m.Austrittsdatum.Value.Year == DateTime.Now.Year &&
                (m.Typ == MitgliederTyp.Ehrenmitglied || m.Typ == MitgliederTyp.Mitglied)).ToListAsync().ConfigureAwait(false);

            if (Mitglieder == null ||
                MitgliederEintritte == null ||
                MitgliederAustritte == null)
            {
                return false;
            }
            return true;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);

            var success = await InitializeFromDb().ConfigureAwait(false);

            if (success == false)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostExportExcelAsync()
        {
            await base.Initialize().ConfigureAwait(false);

            var success = await InitializeFromDb().ConfigureAwait(false);

            if (success == false)
            {
                return NotFound();
            }

            MitgliederListExporter exporter = new MitgliederListExporter();
            exporter.AddWorksheet("Eintritte", MitgliederEintritte);
            exporter.AddWorksheet("Austritte", MitgliederAustritte);

            var buffer = exporter.ExportToExcel();
            return new FileContentResult(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "MitgliederEinAustritte.xslt"
            };
        }
    }
}