using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Verein.Data;
using Verein.Models;

namespace Verein
{
    public class YearChangeActions
    {
        private VereinDbContext _context;
        /*
         * Upon a year change, we want to copy all Mitglieder to a new Year.
         * Everything from the old year has to be set to read only
         */

        public YearChangeActions(VereinDbContext context)
        {
            _context = context;
        }

        /*
         * Sets the given year property to each Mitglieder Entry where it is not set.
         */
        public async Task EnsureMitgliederYearPropertyIsSet(int year)
        {
            foreach (var mitglied in _context.Mitglieder)
            {
                if (mitglied.Year.HasValue)
                {
                    continue;
                }

                mitglied.Year = year;
                _context.Attach(mitglied).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }


        public async Task CopyMitgliederToYear(int oldYear, int newYear)
        {
            var mitglieder = await _context.Mitglieder.Where(m => m.Typ == MitgliederTyp.Mitglied && m.Year == oldYear).ToListAsync();

            foreach (var mitglied in mitglieder)
            {
                var newMitglied = CopyToNewYear(mitglied, newYear);

                _context.Mitglieder.Add(newMitglied);
            }
            await _context.SaveChangesAsync();
        }

        private Mitglied CopyToNewYear(Mitglied oldMitglied, int newYear)
        {
            Mitglied mitglied = new Mitglied()
            {
                Year = newYear,
                Arbeitsstunden = oldMitglied.Arbeitsstunden,
                Austrittsdatum = oldMitglied.Austrittsdatum,
                Bemerkung = oldMitglied.Bemerkung,
                Eintrittsdatum = oldMitglied.Eintrittsdatum,
                EMail = oldMitglied.EMail,
                Entfernung = oldMitglied.Entfernung,
                Familie = oldMitglied.Familie,
                Familienmitgliedschaft = oldMitglied.Familienmitgliedschaft,
                Geburtstag = oldMitglied.Geburtstag,
                HandyNummer = oldMitglied.HandyNummer,
                Hausnummer = oldMitglied.Hausnummer,
                Hunde = oldMitglied.Hunde,
                Kurse = oldMitglied.Kurse,
                MitgliedsNummer = oldMitglied.MitgliedsNummer,
                Name = oldMitglied.Name,
                Ort = oldMitglied.Ort,
                Passiv = oldMitglied.Passiv,
                Postleitzahl = oldMitglied.Postleitzahl,
                Strasse = oldMitglied.Strasse,
                SwhvMitgliedsNummer = oldMitglied.SwhvMitgliedsNummer,
                Telefonnummer = oldMitglied.Telefonnummer,
                Typ = oldMitglied.Typ,
                Vorname = oldMitglied.Vorname,
                ZahlungsInfo = oldMitglied.ZahlungsInfo,
                // Leave ID Blan, because we want to add a new entry
                // Id = 0,
            };
            return mitglied;
        }

    }
}