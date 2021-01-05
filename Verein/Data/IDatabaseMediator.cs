using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verein.Models;

namespace Verein.Data
{
    public interface IDatabaseMediator
    {
        VereinDbContext GetDbContext();


        /**
         * Mitglieder
         */
        Task<IList<Mitglied>> GetMitgliederOrderedByName();

        Task<IList<Mitglied>> GetMitgliederByTypeOrderedByName(MitgliederTyp mitgliedsTyp);

        Task<IList<SelectListItem>> GetMitgliederSelectList();

        Task<IList<string>> GetMitgliesnummernByPrefix(string prefix);

        Task<Mitglied> GetMitgliedById(int? id);

        Task<bool> MitgliedExists(int id);

        Task AddMitglied(Mitglied mitglied);

        Task UpdateMitglied(Mitglied mitglied);

        Task DeleteMitglied(Mitglied mitglied);


        /**
         * Kurse
         */
        Task<IList<Kurs>> GetKurseOrderedByTitle();

        Task<Kurs> GetKursById(int? id);

        Task<bool> KursExists(int id);

        Task AddKurs(Kurs kurs);

        Task UpdateKurs(Kurs kurs);

        Task DeleteKurs(Kurs kurs);


        /**
         * Zahlungsinformationen
         */
        Task<IList<BankInformation>> GetZahlungsinformationenWithBesitzer();

        Task<BankInformation> GetZahlungsinformationById(int? id);

        Task<BankInformation> GetZahlungsinformationByIdWithBesitzer(int? id);

        Task<bool> ZahlungsinformationExists(int id);

        Task AddZahlungsinformation(BankInformation bankInformation);

        Task UpdateZahlungsinformation(BankInformation bankInformation);

        Task DeleteZahlungsinformation(BankInformation bankInformation);


        /**
         * Tarife
         */
        Task<IList<Tarif>> GetTarifeOrderedByTitle();

        Task<bool> TarifExists(int id);

        Task<Tarif> GetTarifById(int? id);

        Task AddTarif(Tarif tarif);

        Task UpdateTarif(Tarif tarif);

        Task DeleteTarif(Tarif tarif);


        /**
         * Inventar
         */
        Task<IList<Gegenstand>> GetInventarOrderedByName();

        Task<bool> InventarExists(int id);

        Task<Gegenstand> GetInventarById(int? id);

        Task AddInventar(Gegenstand gegenstand);

        Task UpdateInventar(Gegenstand gegenstand);

        Task DeleteInventar(Gegenstand gegenstand);


        /**
         * Stammdaten
         */
        Task<IList<StammdatenEintrag>> GetStammdaten();

        Task<StammdatenEintrag> GetStammdatenById(int? id);

        Task AddStammdaten(StammdatenEintrag stammdatenEintrag);

        Task UpdateStammdaten(StammdatenEintrag stammdatenEintrag);

        Task DeleteStammdaten(StammdatenEintrag stammdatenEintrag);

        Task<bool> StammdatenExists(int id);


        /**
         * Arbeitseinsätze
         */
        Task<IList<Arbeitseinsatz>> GetArbeitseinsatzOrderedByTitle();

        Task<bool> ArbeitseinsatzExists(int id);

        Task<Arbeitseinsatz> GetArbeitseinsatzById(int? id);

        Task AddArbeitseinsatz(Arbeitseinsatz arbeitseinsatz);

        Task UpdateArbeitseinsatz(Arbeitseinsatz arbeitseinsatz);

        Task DeleteArbeitseinsatz(Arbeitseinsatz arbeitseinsatz);


        /**
         * Hunde
         */
        Task<IList<Hund>> GetHunde();

        Task<bool> HundExists(int id);

        Task<Hund> GetHundById(int? id);

        Task AddHund(Hund hund);

        Task UpdateHund(Hund hund);

        Task DeleteHund(Hund hund);


        /**
         * User
         */
        Task<IList<HundevereinUser>> GetUsers();

        Task<bool> UserExists(string id);

        Task<HundevereinUser> GetUserById(string id);

        Task AddUser(HundevereinUser user);

        Task UpdateUser(HundevereinUser user);

        Task DeleteUser(HundevereinUser user);
    }
}
