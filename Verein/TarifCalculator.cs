using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein
{
    public class TarifResult
    {
        public decimal Beitrag { get; set; }

        public List<string> Details { get; set; } = new List<string>();

        public List<string> Errors { get; set; } = new List<string>();
    }


    public class TarifCalculator
    {
        private readonly ILogger _logger;
        private readonly VereinDbContext _context;

        private List<Tarif> Tarife;

        private TarifResult tarifResult;

        public TarifCalculator(VereinDbContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<TarifResult> CalculateBeitrag(Mitglied mitglied)
        {
            if (Tarife == null)
            {
                Tarife = await _context.Tarife.ToListAsync().ConfigureAwait(false);
            }

            tarifResult = new TarifResult();

            switch (mitglied.Typ)
            {
                case MitgliederTyp.Ehrenmitglied:
                    tarifResult.Beitrag = 0.0m;
                    tarifResult.Details.Add("Ehrenmitglied");
                    break;
                case MitgliederTyp.Mitglied:
                    tarifResult.Beitrag = (await CalculateMitgliederBeitrag(mitglied).ConfigureAwait(false)).Value;
                    break;
                case MitgliederTyp.Kursteilnehmer:
                    tarifResult.Beitrag = CalculateKursteilnehmerBeitrag(mitglied).Value;
                    break;
                case MitgliederTyp.Jahresteilnahme:
                    tarifResult.Beitrag = CalculateJahresteilnahmeBeitrag(mitglied).Value;
                    break;
                case MitgliederTyp.WelpenLernSpielstunde:
                    tarifResult.Beitrag = CalculateWelpenLernSpielstundeBeitrag(mitglied).Value;
                    tarifResult.Details.Add("Welpen Lern- und Spielstunde");
                    break;
                default:
                    _logger.LogWarning($"Unknown Mitglieder Typ: {mitglied.Typ}");
                    return null;
            }
            return tarifResult;
        }

        private async Task<decimal?> CalculateMitgliederBeitrag(Mitglied mitglied)
        {
            var baseFee = GetMitgliedBaseFee(mitglied);

            if (mitglied.Eintrittsdatum.Year == DateTime.Now.Year)
            {
                tarifResult.Details.Add("Beitrittsgebühr");
                baseFee += GetTarifFee("Beitrittsgebühr");
            }

            if (mitglied.Passiv)
            {
                return baseFee;
            }

            var isTrainer = await IsTrainer(mitglied).ConfigureAwait(false);

            if (isTrainer)
            {
                tarifResult.Details.Add("Trainerrabatt");
                baseFee = Math.Max(0.0m, baseFee.Value - GetTarifFee("TrainerRabatt").Value);
            }

            // Hunde + Platzpauschale
            var hunde = mitglied.Hunde.Count;
            decimal? hundeFee = 0.0m;

            for (int i = 0; i < hunde; i++)
            {
                switch (i)
                {
                    case 0:
                        tarifResult.Details.Add("Platzpauschale 1. Hund");
                        hundeFee += GetTarifFee("PlatzpauschaleHund1");
                        break;
                    case 1:
                        tarifResult.Details.Add("Platzpauschale 2. Hund");
                        hundeFee += GetTarifFee("PlatzpauschaleHund2");
                        break;
                    case 2:
                        tarifResult.Details.Add("Platzpauschale 3. Hund");
                        hundeFee += GetTarifFee("PlatzpauschaleHund3");
                        break;
                    case 3:
                        tarifResult.Details.Add("Platzpauschale 4. Hund");
                        hundeFee += GetTarifFee("PlatzpauschaleHund4");
                        break;
                    case 4:
                        tarifResult.Details.Add("Platzpauschale 5. Hund");
                        hundeFee += GetTarifFee("PlatzpauschaleHund5");
                        break;
                    default:
                        tarifResult.Details.Add("Platzpauschale weiterer Hund");
                        hundeFee += GetTarifFee("PlatzpauschaleHund5");
                        break;
                }
            }

            // Arbeitsstunden
            decimal? arbeitsstundenFee = CalculateArbeitsstundenFee(mitglied);

            return baseFee + hundeFee + arbeitsstundenFee;
        }


        private decimal? CalculateKursteilnehmerBeitrag(Mitglied mitglied)
        {
            throw new NotImplementedException();
        }

        private decimal? CalculateWelpenLernSpielstundeBeitrag(Mitglied mitglied)
        {
            return GetTarifFee("Welpenkurs");
        }


        private decimal? GetMitgliedBaseFee(Mitglied mitglied)
        {
            if (mitglied.Familienmitgliedschaft == true && mitglied.ZahlungsInfo != null)
            {
                tarifResult.Details.Add($"Familienmitgliedschaft");
                return GetTarifFee("Familienmitgliedschaft");
            }
            if (mitglied.Familienmitgliedschaft == true && mitglied.ZahlungsInfo == null)
            {
                tarifResult.Details.Add($"Familienmitgliedschaft als weiteres Familienmitglied");
                return 0.0m;
            }

            if (mitglied.Passiv)
            {
                tarifResult.Details.Add($"Einzelmitgliedschaft Passiv");
                return GetTarifFee("EinzelmitgliedschaftPassiv");
            }
            else
            {
                tarifResult.Details.Add($"Einzelmitgliedschaft");
                return GetTarifFee("Einzelmitgliedschaft");
            }
        }

        private decimal? CalculateJahresteilnahmeBeitrag(Mitglied mitglied)
        {
            decimal? baseFee = 0.0m;
            int factor = GetQuartalsToPay(mitglied);
            var hunde = mitglied.Hunde.Count;

            if (hunde == 1)
            {
                tarifResult.Details.Add($"Jahresteilnahme ab Quartal {5 - factor} für ersten Hund");
                baseFee += GetTarifFee("JahresteilnahmeQuartalHund1");
            }
            for (int i = 1; i < hunde; i++)
            {
                tarifResult.Details.Add($"Jahresteilnahme ab Quartal {5 - factor} für weiteren Hund");
                baseFee += GetTarifFee("JahresteilnahmeQuartalHund2");
            }
            return baseFee * factor;
        }


        private decimal? GetTarifFee(string key)
        {
            var tarif = Tarife.SingleOrDefault(t => t.Title == key);

            if (tarif != null)
            {
                return tarif.Fee;
            }
            _logger.LogWarning($"Tarif key not found {key}");
            tarifResult.Errors.Add($"Berechnung ist falsch!");
            tarifResult.Errors.Add($"Tarif mit dem Key {key} nicht gefunden");
            return 0.0m;
        }

        private int GetQuartalsToPay(Mitglied mitglied)
        {
            var eintrittsmonat = mitglied.Eintrittsdatum.Month;

            if (eintrittsmonat >= 1 && eintrittsmonat <= 3)
            {
                return 4;
            }
            else if (eintrittsmonat >= 4 && eintrittsmonat <= 6)
            {
                return 3;
            }
            else if (eintrittsmonat >= 7 && eintrittsmonat <= 9)
            {
                return 2;
            }
            else if (eintrittsmonat >= 10 && eintrittsmonat <= 12)
            {
                return 1;
            }
            else
            {
                _logger.LogWarning($"Invalid Eintrittsmonat: {eintrittsmonat}");
                return 0;
            }
        }


        private async Task<bool> IsTrainer(Mitglied mitglied)
        {
            var trainer = await _context.Trainer.Select(t => t.KursTrainer).ToListAsync().ConfigureAwait(false);

            if (trainer.Contains(mitglied))
            {
                return true;
            }
            return false;
        }


        private decimal? CalculateArbeitsstundenFee(Mitglied mitglied)
        {
            decimal? arbeitsstundenFee = 0.0m;

            // Passive Mitglieder sowie Kinder und Jugendliche müssen keine Arbeitsstunden leisten
            var stichtag = DateTime.Now.AddYears(-16);
            if (mitglied.Passiv || mitglied.Geburtstag > stichtag)
            {
                _logger.LogWarning($"{mitglied.Vorname} {mitglied.Name}: Stichtag {stichtag} Geburtstag {mitglied.Geburtstag}");
                return arbeitsstundenFee;
            }

            var stammArbeitsstunden = _context.Stammdaten.SingleOrDefault(s => s.Title == "Arbeitsstunden");
            var arbeitsstunden = double.Parse(stammArbeitsstunden.Value);

            double arbeitsstundenGeleistet = 0.0;

            if (mitglied.Arbeitsstunden != null)
            {
                arbeitsstundenGeleistet = mitglied.Arbeitsstunden.Sum(h => h.Dauer.TimeOfDay.TotalMinutes) / 60.0;
            }

            var fehlendeArbeitsstunden = arbeitsstunden - arbeitsstundenGeleistet;

            if (mitglied.Eintrittsdatum.Year == DateTime.Now.Year)
            {
                fehlendeArbeitsstunden = 0;
            }

            if (fehlendeArbeitsstunden > 0)
            {
                tarifResult.Details.Add($"Nicht geleistete Arbeitsstunden ({fehlendeArbeitsstunden}h)");
                arbeitsstundenFee = Convert.ToDecimal(fehlendeArbeitsstunden) * GetTarifFee("ArbeitsstundenVerrechnungssatz");
            }
            return arbeitsstundenFee;
        }
    }
}