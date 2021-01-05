using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;
using Verein.ViewModels;

namespace Verein.Pages.Arbeitseinsaetze
{
    public class DetailsModel : VereinPageModel
    {
        private readonly VereinDbContext _context;

        public DetailsModel(IDatabaseMediator databaseMediator, ILogger<DetailsModel> logger)
            : base(databaseMediator, logger)
        {
            _context = databaseMediator.GetDbContext();
        }

        public Arbeitseinsatz ArbeitsTaetigkeit { get; set; }

        public IList<Helfer> Helfer { get; set; }

        public IList<Mitglied> Mitglieder { get; set; }

        public class InputModel : MitgliedSelectionItem
        {
            [Required]
            [DataType(DataType.Time)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
            public DateTime Dauer { get; set; }
        }

        [BindProperty]
        public IList<InputModel> LinkMitgliederModalObjects { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> UnlinkMitgliederModalObjects { get; set; }


        private async Task<bool> InitializeFromDb(int? id)
        {
            ArbeitsTaetigkeit = await _databaseMediator.GetArbeitseinsatzById(id).ConfigureAwait(false);

            Helfer = await _context.Helfer
                                   .Where(h => h.Arbeitseinsatz.Id == id)
                                   .ToListAsync()
                                   .ConfigureAwait(false);

            Mitglieder = await _databaseMediator.GetMitgliederOrderedByName().ConfigureAwait(false);

            LinkMitgliederModalObjects = Mitglieder.Select(m =>
                new InputModel()
                {
                    Id = m.Id.ToString(),
                    Selected = false,
                    Vorname = m.Vorname,
                    Name = m.Name,
                    Geburtstag = m.Geburtstag
                }
            ).ToList();

            UnlinkMitgliederModalObjects = Helfer.Select(h =>
                new MitgliedSelectionItem()
                {
                    Id = h.Teilnehmer.Id.ToString(),
                    Selected = false,
                    Vorname = h.Teilnehmer.Vorname,
                    Name = h.Teilnehmer.Name,
                    Geburtstag = h.Teilnehmer.Geburtstag
                }
            ).ToList();

            if (ArbeitsTaetigkeit == null ||
                Helfer == null ||
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



        public async Task<IActionResult> OnPostMitgliedVerknuepfenAsync(int id, IEnumerable<InputModel> LinkMitgliederModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in LinkMitgliederModalObjects)
            {
                if (item.Selected == false)
                {
                    continue;
                }
                var success = int.TryParse(item.Id, out var mitgliedsId);

                if (success)
                {
                    var mitglied = Mitglieder.SingleOrDefault(m => m.Id == mitgliedsId);
                    var helfer = Helfer.SingleOrDefault(h => h.Arbeitseinsatz.Id == ArbeitsTaetigkeit.Id && h.Teilnehmer.Id == mitgliedsId);

                    if (mitglied != null && helfer == null)
                    {
                        _context.Helfer.Add(
                            new Helfer()
                            {
                                Teilnehmer = mitglied,
                                Arbeitseinsatz = ArbeitsTaetigkeit,
                                Dauer = item.Dauer
                            }
                        );
                    }

                    if (mitglied != null && helfer != null)
                    {
                        helfer.Dauer = item.Dauer;
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }


        public async Task<IActionResult> OnPostMitgliedVerknuepfungAufhebenAsync(int id, IEnumerable<MitgliedSelectionItem> UnlinkMitgliederModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            foreach (var item in UnlinkMitgliederModalObjects)
            {
                if (item.Selected == false)
                {
                    continue;
                }
                var success = int.TryParse(item.Id, out var mitgliedsId);

                if (success)
                {
                    var helfer = Helfer.SingleOrDefault(h => h.Arbeitseinsatz.Id == ArbeitsTaetigkeit.Id && h.Teilnehmer.Id == mitgliedsId);

                    if (helfer != null)
                    {
                        _context.Helfer.Remove(helfer);
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }
    }
}
