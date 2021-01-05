using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;
using Verein.ViewModels;

namespace Verein.Pages.ZahlungsInformationen
{
    public class DetailsModel : VereinPageModel
    {

        public DetailsModel(IDatabaseMediator databaseMediator, ILogger<DetailsModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        public BankInformation BankInformation { get; set; }

        public IList<Mitglied> Mitglieder { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> LinkMitgliederModalObjects { get; set; }

        [BindProperty]
        public IList<MitgliedSelectionItem> UnlinkMitgliederModalObjects { get; set; }


        private async Task<bool> InitializeFromDb(int? id)
        {
            BankInformation = await _databaseMediator.GetZahlungsinformationByIdWithBesitzer(id).ConfigureAwait(false);
            Mitglieder = await _databaseMediator.GetMitgliederOrderedByName().ConfigureAwait(false);

            UnlinkMitgliederModalObjects = BankInformation.Besitzer.Select(m =>
                new MitgliedSelectionItem()
                {
                    Id = m.Id.ToString(),
                    Selected = false,
                    Vorname = m.Vorname,
                    Name = m.Name,
                    Geburtstag = m.Geburtstag
                }
            ).ToList();

            LinkMitgliederModalObjects = Mitglieder.Select(m =>
                new MitgliedSelectionItem()
                {
                    Id = m.Id.ToString(),
                    Selected = (BankInformation.Besitzer.Contains(m)) ? true : false,
                    Vorname = m.Vorname,
                    Name = m.Name,
                    Geburtstag = m.Geburtstag
                }
            ).ToList();

            if (BankInformation == null ||
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

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
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

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            var selectedIds = GetSelectedIds(LinkMitgliederModalObjects);

            foreach (var selectedid in selectedIds)
            {
                var mitglied = Mitglieder.SingleOrDefault(m => m.Id == selectedid);

                if (mitglied != null)
                {
                    BankInformation.Besitzer.Add(mitglied);
                } 
            }
            var context = _databaseMediator.GetDbContext();
            await context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }


        public async Task<IActionResult> OnPostMitgliedVerknuepfungAufhebenAsync(int id, IEnumerable<SelectionItem> UnlinkMitgliederModalObjects)
        {
            var isInitialized = await InitializeFromDb(id).ConfigureAwait(false);

            if (isInitialized == false)
            {
                return NotFound();
            }

            if (!(await IsAuthorized(BenutzerTyp.ErweiterterVorstand).ConfigureAwait(false)))
            {
                return RedirectToPage("/AccessDenied");
            }

            var selectedIds = GetSelectedIds(UnlinkMitgliederModalObjects);

            foreach (var selectedId in selectedIds)
            {
                var mitglied = BankInformation.Besitzer.SingleOrDefault(m => m.Id == selectedId);

                if (mitglied != null)
                {
                    BankInformation.Besitzer.Remove(mitglied);
                }
            }
            var context = _databaseMediator.GetDbContext();
            await context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Details", new {id = id});
        }

        private IList<int> GetSelectedIds(IEnumerable<SelectionItem> selectionItems)
        {
            var selectedIds = new List<int>();

            foreach (var item in selectionItems)
            {
                if (item.Selected == true)
                {
                    var success = int.TryParse(item.Id, out var id);

                    if (success == false)
                    {
                        continue;
                    }
                    selectedIds.Add(id);
                }
            }
            return selectedIds;
        }
    }
}
