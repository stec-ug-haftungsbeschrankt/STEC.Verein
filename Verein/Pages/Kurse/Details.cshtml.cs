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

namespace Verein.Pages.Kurse
{
    public class DetailsModel : VereinPageModel
    {
        private readonly VereinDbContext _context;
        public DetailsModel(IDatabaseMediator databaseMediator, ILogger<Startup> logger)
            : base(databaseMediator, logger)
        {
            _context = databaseMediator.GetDbContext();
        }

        public Kurs Kurs { get; set; }

        public IList<KursTeilnehmer> KursTeilnehmer { get; set; }

        public IList<Trainer> Trainer { get; set; }

        public IList<Mitglied> Mitglieder { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> LinkMitgliederModalObjects { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> UnlinkMitgliederModalObjects { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> LinkTrainerModalObjects { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> UnlinkTrainerModalObjects { get; set; }


        private async Task<bool> InitializeFromDb(int? id)
        {
            Kurs = await _context.Kurse.FirstOrDefaultAsync(k => k.Id == id).ConfigureAwait(false);

            KursTeilnehmer = await _context.KursTeilnehmer
                                           .Include(kt => kt.Teilnehmer)
                                           .Where(kt => kt.Kurse.Id == id)
                                           .ToListAsync()
                                           .ConfigureAwait(false);

            Trainer = await _context.Trainer.Include(t => t.KursTrainer)
                                            .Where(t => t.Kurse.Id == id)
                                            .ToListAsync()
                                            .ConfigureAwait(false);

            Mitglieder = await _context.Mitglieder.ToListAsync().ConfigureAwait(false);

            LinkMitgliederModalObjects = Mitglieder.Select(m =>
                new MitgliedSelectionItem()
                {
                    Id = m.Id.ToString(),
                    Selected = false,
                    Vorname = m.Vorname,
                    Name = m.Name,
                    Geburtstag = m.Geburtstag,
                }
            ).ToList();

            UnlinkMitgliederModalObjects = KursTeilnehmer.Select(m =>
                new MitgliedSelectionItem()
                {
                    Id = m.Teilnehmer.Id.ToString(),
                    Selected = false,
                    Vorname = m.Teilnehmer.Vorname,
                    Name = m.Teilnehmer.Name,
                    Geburtstag = m.Teilnehmer.Geburtstag,
                }
            ).ToList();

            LinkTrainerModalObjects = Mitglieder.Select(m =>
                new MitgliedSelectionItem()
                {
                    Id = m.Id.ToString(),
                    Selected = (Trainer.Any(t => t.KursTrainer.Id == m.Id)) ? true : false,
                    Vorname = m.Vorname,
                    Name = m.Name,
                    Geburtstag = m.Geburtstag,
                }
            ).ToList();

            UnlinkTrainerModalObjects = Trainer.Select(m =>
                new MitgliedSelectionItem()
                {
                    Id = m.KursTrainer.Id.ToString(),
                    Selected = false,
                    Vorname = m.KursTrainer.Vorname,
                    Name = m.KursTrainer.Name,
                    Geburtstag = m.KursTrainer.Geburtstag,
                }
            ).ToList();

            if (Kurs == null ||
                KursTeilnehmer == null ||
                Trainer == null ||
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


        public async Task<IActionResult> OnPostMitgliedVerknuepfenAsync(int id, IEnumerable<SelectionItem> LinkMitgliederModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in LinkMitgliederModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var mitgliedsId);

                    if (success)
                    {
                        var mitglied = Mitglieder.SingleOrDefault(m => m.Id == mitgliedsId);
                        var teilnehmerExists = KursTeilnehmer.Any(kt => kt.Kurse.Id == Kurs.Id && kt.Teilnehmer.Id == mitgliedsId);

                        if (mitglied != null && teilnehmerExists == false)
                        {
                            _context.KursTeilnehmer.Add(
                                new KursTeilnehmer()
                                {
                                    Teilnehmer = mitglied,
                                    Kurse = Kurs
                                }
                            );
                        }
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }


        public async Task<IActionResult> OnPostMitgliedVerknuepfungAufhebenAsync(int id, IEnumerable<SelectionItem> UnlinkMitgliederModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in UnlinkMitgliederModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var mitgliedsId);

                    if (success)
                    {
                        var kursTeilnehmer = KursTeilnehmer.SingleOrDefault(kt => kt.Kurse.Id == Kurs.Id && kt.Teilnehmer.Id == mitgliedsId);
                        if (kursTeilnehmer != null)
                        {
                            _context.KursTeilnehmer.Remove(kursTeilnehmer);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }



        public async Task<IActionResult> OnPostTrainerVerknuepfenAsync(int id, IEnumerable<SelectionItem> LinkTrainerModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in LinkTrainerModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var mitgliedsId);

                    if (success)
                    {
                        var mitglied = Mitglieder.SingleOrDefault(m => m.Id == mitgliedsId);
                        var trainerExists = Trainer.Any(t => t.Kurse.Id == Kurs.Id && t.KursTrainer.Id == mitgliedsId);

                        if (mitglied != null && trainerExists == false)
                        {
                            _context.Trainer.Add(
                                new Trainer()
                                {
                                    KursTrainer = mitglied,
                                    Kurse = Kurs
                                }
                            );
                        }
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }


        public async Task<IActionResult> OnPostTrainerVerknuepfungAufhebenAsync(int id, IEnumerable<SelectionItem> UnlinkTrainerModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in UnlinkTrainerModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var mitgliedsId);

                    if (success)
                    {
                        var trainer = Trainer.SingleOrDefault(t => t.Kurse.Id == Kurs.Id && t.KursTrainer.Id == mitgliedsId);
                        if (trainer != null)
                        {
                            _context.Trainer.Remove(trainer);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }


        public async Task<IActionResult> OnPostExportExcelAsync(int id)
        {
            await InitializeFromDb(id).ConfigureAwait(false);
            CourseListExporter exporter = new CourseListExporter();

            var kursTeilnehmer = KursTeilnehmer.Select(kt => kt.Teilnehmer).ToList();
            var mitglieder = await _context.Mitglieder.Include(m => m.Hunde)
                                                      .Include(m => m.Kurse)
                                                      .ToListAsync().ConfigureAwait(false);

            if (mitglieder == null)
            {
                return NotFound();
            }

            List<Mitglied> teilnehmer = new List<Mitglied>();

            foreach (var mitglied in mitglieder)
            {
                var exists = kursTeilnehmer.Any(m => m.Id == mitglied.Id);

                if (exists)
                {
                    teilnehmer.Add(mitglied);
                }
            }

            var buffer = exporter.ExportToExcel(Kurs, teilnehmer);
            return new FileContentResult(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = Kurs.Titel + ".xslt"
            };
        }
    }
}
