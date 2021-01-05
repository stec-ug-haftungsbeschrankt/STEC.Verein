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

namespace Verein.Pages.Mitglieder
{
    public class DetailsModel : VereinPageModel
    {
        private readonly VereinDbContext _context;

        public DetailsModel(IDatabaseMediator databaseMediator, ILogger<DetailsModel> logger)
            : base(databaseMediator, logger)
        {
            _context = databaseMediator.GetDbContext();
        }

        public Mitglied Mitglied { get; set; }

        public IList<Kurs> Kurse { get; set; }

        public IList<Hund> Hunde { get; set; }

        public IList<Kurs> AlleKurse { get; set; }

        public IList<Mitglied> AlleMitglieder { get; set; }


        [BindProperty]
        public IList<HundSelectionItem> LinkHundModalObjects { get; set; }

        [BindProperty]
        public IList<HundSelectionItem> UnlinkHundModalObjects { get; set; }

        [BindProperty]
        public IList<KursSelectionItem> LinkKursModalObjects { get; set; }

        [BindProperty]
        public IList<KursSelectionItem> UnlinkKursModalObjects { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> LinkFamilieModalObjects { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> UnlinkFamilieModalObjects { get; set; }


        private async Task<bool> InitializeFromDb(int? id)
        {
            Mitglied = await _context.Mitglieder
                                     .Include(m => m.Hunde)
                                     .Include(m => m.Kurse)
                                     .ThenInclude(k => k.Kurse)
                                     .Include(m => m.ZahlungsInfo)
                                     .Include(m => m.Arbeitsstunden)
                                     .Include(m => m.Familie)
                                     .ThenInclude(f => f.Mitglieder)
                                     .FirstOrDefaultAsync(m => m.Id == id)
                                     .ConfigureAwait(false);

            Kurse = await _context.KursTeilnehmer
                                  .Where(kt => kt.Teilnehmer == Mitglied)
                                  .Select(kt => kt.Kurse)
                                  .ToListAsync()
                                  .ConfigureAwait(false);

            Hunde = await _context.Hunde.ToListAsync().ConfigureAwait(false);

            AlleKurse = await _context.Kurse.ToListAsync().ConfigureAwait(false);

            AlleMitglieder = await _context.Mitglieder.OrderBy(m => m.Name).ToListAsync().ConfigureAwait(false);

            if (Mitglied == null ||
                Kurse == null ||
                Hunde == null ||
                AlleKurse == null ||
                AlleMitglieder == null)
            {
                return false;
            }

            LinkHundModalObjects = Hunde.Select(h =>
                new HundSelectionItem()
                {
                    Id = h.Id.ToString(),
                    Hundename = h.Name,
                    Rasse = h.Rasse,
                    Geburtstag = h.Geburtsdatum,
                    Selected = false
                }
            ).ToList();

            UnlinkHundModalObjects = Mitglied.Hunde.Select(h =>
                new HundSelectionItem()
                {
                    Id = h.Id.ToString(),
                    Hundename = h.Name,
                    Rasse = h.Rasse,
                    Geburtstag = h.Geburtsdatum,
                    Selected = false
                }
            ).ToList();

            LinkKursModalObjects = AlleKurse.Select(k =>
                new KursSelectionItem()
                {
                    Id = k.Id.ToString(),
                    Kursname = k.Titel,
                    Selected = false
                }
            ).ToList();

            UnlinkKursModalObjects = Kurse.Select(k =>
                new KursSelectionItem()
                {
                    Id = k.Id.ToString(),
                    Kursname = k.Titel,
                    Selected = false
                }
            ).ToList();

            LinkFamilieModalObjects = AlleMitglieder.Select(m =>
                new MitgliedSelectionItem()
                {
                    Id = m.Id.ToString(),
                    Vorname = m.Vorname,
                    Name = m.Name,
                    Geburtstag = m.Geburtstag,
                    Selected = IsFamilyMember(Mitglied, m)
                }
            ).ToList();

            if (Mitglied.Familie != null)
            {
                UnlinkFamilieModalObjects = Mitglied.Familie.Mitglieder.Select(m =>
                    new MitgliedSelectionItem()
                    {
                        Id = m.Id.ToString(),
                        Vorname = m.Vorname,
                        Name = m.Name,
                        Geburtstag = m.Geburtstag,
                        Selected = false
                    }
                ).ToList();
            }

            return true;
        }

        private bool IsFamilyMember(Mitglied mitglied, Mitglied testMitglied)
        {
            if (mitglied.Familie == null)
            {
                return false;
            }
            return mitglied.Familie.Mitglieder.Contains(testMitglied);
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

        public async Task<IActionResult> OnPostHundVerknuepfenAsync(int id, IEnumerable<HundSelectionItem> LinkHundModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in LinkHundModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var hundId);
                    if (success)
                    {
                        var hund = Hunde.SingleOrDefault(h => h.Id == hundId);
                        Mitglied.Hunde.Add(hund);
                    }

                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }


        public async Task<IActionResult> OnPostHundVerknuepfungAufhebenAsync(int id, IEnumerable<HundSelectionItem> UnlinkHundModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in UnlinkHundModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var hundId);

                    if (success)
                    {
                        var hund = Mitglied.Hunde.SingleOrDefault(h => h.Id == hundId);
                        if (hund != null)
                        {
                            Mitglied.Hunde.Remove(hund);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }

        public async Task<IActionResult> OnPostKursVerknuepfenAsync(int id, IEnumerable<KursSelectionItem> LinkKursModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in LinkKursModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var kursId);
                    if (success)
                    {
                        _context.KursTeilnehmer.Add(
                            new KursTeilnehmer()
                            {
                                Teilnehmer = Mitglied,
                                Kurse = AlleKurse.SingleOrDefault(k => k.Id == kursId)
                            }
                        );
                    }

                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }

        public async Task<IActionResult> OnPostKursVerknuepfungAufhebenAsync(int id, IEnumerable<KursSelectionItem> UnlinkKursModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in UnlinkKursModalObjects)
            {
                if (item.Selected == false)
                {
                    continue;
                }
                var success = int.TryParse(item.Id, out var kursId);
                _logger.LogWarning($"1");

                if (success)
                {
                    _logger.LogWarning($"2 {kursId}");
                    var kursTeilnehmer = Mitglied.Kurse.SingleOrDefault(k => k.Kurse.Id == kursId);

                    if (kursTeilnehmer != null)
                    {
                        _logger.LogWarning($"3 {kursId}");
                        Mitglied.Kurse.Remove(kursTeilnehmer);
                    }
                    else
                    {
                        _logger.LogWarning($"Kurs not found");
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }

        public async Task<IActionResult> OnPostFamilieVerknuepfenAsync(int id, IEnumerable<SelectionItem> LinkFamilieModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in LinkFamilieModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var familienMitgliedsId);

                    if (success)
                    {
                        var mitglied = _context.Mitglieder.SingleOrDefault(m => m.Id == id);
                        var familienMitglied = _context.Mitglieder.SingleOrDefault(m => m.Id == familienMitgliedsId);

                        if (familienMitglied == null)
                        {
                            continue;
                        }

                        if (Mitglied.Familie == null)
                        {
                            _context.Familien.Add(
                                new Familie()
                                {
                                    Name = familienMitglied.Name,
                                    Mitglieder = new List<Mitglied>()
                                    {
                                        mitglied,
                                        familienMitglied
                                    }
                                });
                        }
                        else
                        {
                            var familien = await _context.Familien.Include(f => f.Mitglieder).ToListAsync().ConfigureAwait(false);
                            var familie = familien.SingleOrDefault(f => f.Mitglieder.Contains(mitglied));

                            if (familie != null)
                            {
                                familie.Mitglieder.Add(familienMitglied);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Unable to parse Mitglieder ID");
                    }
                }
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }

        public async Task<IActionResult> OnPostFamilieVerknuepfungAufhebenAsync(int id, IEnumerable<SelectionItem> UnlinkFamilieModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in UnlinkFamilieModalObjects)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var familienMitgliedsId);

                    if (success == false)
                    {
                        continue;
                    }

                    var familienMitglied = Mitglied.Familie.Mitglieder.SingleOrDefault(m => m.Id == familienMitgliedsId);

                    if (familienMitglied != null)
                    {
                        Mitglied.Familie.Mitglieder.Remove(familienMitglied);
                    }
                }
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }
    }
}
