using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verein.Models;

namespace Verein.Data
{
    public class DatabaseMediator : IDatabaseMediator
    {
        private readonly ILogger _logger;
        private readonly VereinDbContext _context;

        public DatabaseMediator(VereinDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public VereinDbContext GetDbContext()
        {
            return _context;
        }

        /**
         * Mitglieder
         */
        public async Task<IList<Mitglied>> GetMitgliederOrderedByName()
        {
            return await _context.Mitglieder.OrderBy(m => m.Name)
                                                      .ToListAsync()
                                                      .ConfigureAwait(false);
        }

        public async Task<IList<Mitglied>> GetMitgliederByTypeOrderedByName(MitgliederTyp mitgliedsTyp)
        {
            return await _context.Mitglieder.Where(m => m.Typ == mitgliedsTyp)
                                                      .OrderBy(m => m.Name)
                                                      .ToListAsync()
                                                      .ConfigureAwait(false);
        }

        public async Task<IList<SelectListItem>> GetMitgliederSelectList()
        {
            return await _context.Mitglieder.Select(m => new SelectListItem { Text = $"{m.Vorname} {m.Name}", Value = m.Id.ToString() })
                                            .ToListAsync()
                                            .ConfigureAwait(false);
        }

        public async Task<IList<string>> GetMitgliesnummernByPrefix(string prefix)
        {
            return await _context.Mitglieder.Where(m => m.MitgliedsNummer.Contains(prefix))
                                               .Select(m => m.MitgliedsNummer)
                                               .ToListAsync()
                                               .ConfigureAwait(false);
        }

        public async Task<Mitglied> GetMitgliedById(int? id)
        {
            return await _context.Mitglieder.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<bool> MitgliedExists(int id)
        {
            return await _context.Mitglieder.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task AddMitglied(Mitglied mitglied)
        {
            _context.Mitglieder.Add(mitglied);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateMitglied(Mitglied mitglied)
        {
            _context.Attach(mitglied).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteMitglied(Mitglied mitglied)
        {
            _context.Mitglieder.Remove(mitglied);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /**
         * Kurse
         */
        public async Task<IList<Kurs>> GetKurseOrderedByTitle()
        {
            return await _context.Kurse.OrderBy(k => k.Titel)
                                                      .ToListAsync()
                                                      .ConfigureAwait(false);
        }

        public async Task<Kurs> GetKursById(int? id)
        {
            return await _context.Kurse.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<bool> KursExists(int id)
        {
            return await _context.Kurse.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task AddKurs(Kurs kurs)
        {
            _context.Kurse.Add(kurs);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateKurs(Kurs kurs)
        {
            _context.Attach(kurs).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteKurs(Kurs kurs)
        {
            _context.Kurse.Remove(kurs);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /**
         * Tarife
         */
        public async Task<IList<Tarif>> GetTarifeOrderedByTitle()
        {
            return await _context.Tarife.OrderBy(t => t.Title).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> TarifExists(int id)
        {
            return await _context.Tarife.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<Tarif> GetTarifById(int? id)
        {
            return await _context.Tarife.FindAsync(id).ConfigureAwait(false);
        }

        public async Task AddTarif(Tarif tarif)
        {
            _context.Tarife.Add(tarif);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateTarif(Tarif tarif)
        {
            _context.Attach(tarif).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteTarif(Tarif tarif)
        {
            _context.Tarife.Remove(tarif);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /**
         * Zahlungsinformationen
         */
        public async Task<IList<BankInformation>> GetZahlungsinformationenWithBesitzer()
        {
            return await _context.Zahlungsinformationen
                .Include(b => b.Besitzer).ToListAsync().ConfigureAwait(false);
        }


        public async Task<BankInformation> GetZahlungsinformationById(int? id)
        {
            return await _context.Zahlungsinformationen.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<BankInformation> GetZahlungsinformationByIdWithBesitzer(int? id)
        {
            return await _context.Zahlungsinformationen
                .Include(b => b.Besitzer).FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
        }

        public async Task<bool> ZahlungsinformationExists(int id)
        {
            return await _context.Zahlungsinformationen.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task AddZahlungsinformation(BankInformation bankInformation)
        {
            _context.Zahlungsinformationen.Add(bankInformation);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateZahlungsinformation(BankInformation bankInformation)
        {
            _context.Attach(bankInformation).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteZahlungsinformation(BankInformation bankInformation)
        {
            _context.Zahlungsinformationen.Remove(bankInformation);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /**
         * Stammdaten 
         */
        public async Task<IList<StammdatenEintrag>> GetStammdaten()
        {
            return await _context.Stammdaten.ToListAsync().ConfigureAwait(false);
        }

        public async Task<StammdatenEintrag> GetStammdatenById(int? id)
        {
            return await _context.Stammdaten.FindAsync(id).ConfigureAwait(false);
        }

        public async Task AddStammdaten(StammdatenEintrag stammdatenEintrag)
        {
            _context.Stammdaten.Add(stammdatenEintrag);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateStammdaten(StammdatenEintrag stammdatenEintrag)
        {
            _context.Attach(stammdatenEintrag).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteStammdaten(StammdatenEintrag stammdatenEintrag)
        {
            _context.Stammdaten.Remove(stammdatenEintrag);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> StammdatenExists(int id)
        {
            return await _context.Stammdaten.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }


        /**
         * Inventar
         */
        public async Task<IList<Gegenstand>> GetInventarOrderedByName()
        {
            return await _context.Inventar.OrderBy(t => t.Name).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> InventarExists(int id)
        {
            return await _context.Inventar.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<Gegenstand> GetInventarById(int? id)
        {
            return await _context.Inventar.FindAsync(id).ConfigureAwait(false);
        }

        public async Task AddInventar(Gegenstand gegenstand)
        {
            _context.Inventar.Add(gegenstand);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateInventar(Gegenstand gegenstand)
        {
            _context.Attach(gegenstand).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteInventar(Gegenstand gegenstand)
        {
            _context.Inventar.Remove(gegenstand);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /**
         * Arbeitseinsätze
         */
        public async Task<IList<Arbeitseinsatz>> GetArbeitseinsatzOrderedByTitle()
        {
            return await _context.Arbeitseinsaetze.OrderBy(t => t.Titel).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> ArbeitseinsatzExists(int id)
        {
            return await _context.Arbeitseinsaetze.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<Arbeitseinsatz> GetArbeitseinsatzById(int? id)
        {
            return await _context.Arbeitseinsaetze.FindAsync(id).ConfigureAwait(false);
        }

        public async Task AddArbeitseinsatz(Arbeitseinsatz arbeitseinsatz)
        {
            _context.Arbeitseinsaetze.Add(arbeitseinsatz);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateArbeitseinsatz(Arbeitseinsatz arbeitseinsatz)
        {
            _context.Attach(arbeitseinsatz).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteArbeitseinsatz(Arbeitseinsatz arbeitseinsatz)
        {
            _context.Arbeitseinsaetze.Remove(arbeitseinsatz);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /**
         * Hunde
         */
        public async Task<IList<Hund>> GetHunde()
        {
            return await _context.Hunde.ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> HundExists(int id)
        {
            return await _context.Hunde.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<Hund> GetHundById(int? id)
        {
            return await _context.Hunde.Include(h => h.Besitzer)
                                        .SingleOrDefaultAsync(h => h.Id == id)
                                        .ConfigureAwait(false);
        }

        public async Task AddHund(Hund hund)
        {
            _context.Hunde.Add(hund);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateHund(Hund hund)
        {
            _context.Attach(hund).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteHund(Hund hund)
        {
            _context.Hunde.Remove(hund);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /** 
         * Users
         */
        public async Task<IList<HundevereinUser>> GetUsers()
        {
            return await _context.Users.ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> UserExists(string id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<HundevereinUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id).ConfigureAwait(false);
        }

        public async Task AddUser(HundevereinUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateUser(HundevereinUser user)
        {
            _context.Attach(user).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteUser(HundevereinUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
