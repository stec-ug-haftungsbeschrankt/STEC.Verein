using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using Verein.Models;

namespace Verein.Data
{
    public class DbInitializer
    {
        public void Initialize(VereinDbContext context)
        {
            InitializeMitglieder(context);
            InitializeHunde(context);
            InitializeKurse(context);

            var mitglied = context.Mitglieder.SingleOrDefault(x => x.Name == "Schick");
            var hund = context.Hunde.SingleOrDefault(x => x.Name == "Marley B.");
            mitglied.Hunde.Add(hund);

            InitializeStammdaten(context);
            InitializeTarife(context);

            context.SaveChanges();
        }

        /*
        private void InitializeUser(UserManager<HundevereinUser> userManager)
        {
            var user = new HundevereinUser 
            { 
                UserName = "stefan.schick@stecug.de", 
                Email = "stefan.schick@stecug.de", 
                FullName = "Stefan Schick" 
            };
            var result = userManager.CreateAsync(user, "test123").Result;

            var userId = userManager.GetUserIdAsync(user).Result;
            var code = userManager.GenerateEmailConfirmationTokenAsync(user).Result;
            var confirmationResult = userManager.ConfirmEmailAsync(user, code).Result;
        }
        */

        private void InitializeMitglieder(VereinDbContext context)
        {
            if (context.Mitglieder.Any())
            {
                return;   // DB has been seeded
            }

            var mitglieder = new Mitglied[]
            {
                new Mitglied {
                    Vorname = "Stefan",
                    Name = "Schick",
                    MitgliedsNummer = "0536",
                    Geburtstag = DateTime.Parse("11.09.1983"),
                    Strasse = "Mozartstraße",
                    Hausnummer = "6",
                    Postleitzahl = "78120",
                    Ort = "Mannheim"
                },
                new Mitglied {
                    Vorname = "Sarah",
                    Name = "Leitner",
                    MitgliedsNummer = "0820",
                    Geburtstag = DateTime.Parse("09.06.1988"),
                    Strasse = "Beethovenstraße",
                    Hausnummer = "87",
                    Postleitzahl = "76819",
                    Ort = "Berlin"
                },
                new Mitglied {
                    Vorname = "Felix",
                    Name = "Mustermann",
                    MitgliedsNummer = "0687",
                    Geburtstag = DateTime.Parse("09.04.1989"),
                    Strasse = "Brahmstraße",
                    Hausnummer = "89",
                    Postleitzahl = "79509",
                    Ort = "Hof"
                }
            };
            foreach (Mitglied s in mitglieder)
            {
                context.Mitglieder.Add(s);
            }
            context.SaveChanges();
        }

        private void InitializeHunde(VereinDbContext context)
        {
            if (context.Hunde.Any())
            {
                return;   // DB has been seeded
            }

            var hunde = new Hund[]
            {
                new Hund() {
                    Name = "Marley B.",
                    Zwingername = "of secular Dirtpaws",
                    Rasse = "Siberian Husky",
                    Geburtsdatum = DateTime.Parse("05.11.2010"),
                    ChipNummer = "4367245657824362",
                    Geimpft = true,
                    Versichert = true,
                    Besitzer = context.Mitglieder.SingleOrDefault(x => x.Name == "Schick")
                }
            };

            foreach (Hund s in hunde)
            {
                context.Hunde.Add(s);
            }
            context.SaveChanges();
        }

        private void InitializeKurse(VereinDbContext context)
        {
            if (context.Kurse.Any())
            {
                return;   // DB has been seeded
            }

            var kurse = new Kurs[]
            {
                new Kurs() {
                    Titel = "Unterordnung",
                    Beschreibung = "Sonntagsunterordnung",
                    Startdatum = DateTime.Parse("01.01.2017"),
                    Enddatum = DateTime.Parse("31.12.2020")
                }
            };

            foreach (Kurs s in kurse)
            {
                context.Kurse.Add(s);
            }
            context.SaveChanges();
        }

        private void InitializeStammdaten(VereinDbContext context)
        {
            if (context.Stammdaten.Any())
            {
                return;   // DB has been seeded
            }

            var stammdaten = new StammdatenEintrag[]
            {
                new StammdatenEintrag() {
                    Title = "VereinsName",
                    Value = "HSV Oppenheim",
                    Type = Datatype.String
                },
                new StammdatenEintrag() {
                    Title = "VereinsNummer",
                    Value = "0830",
                    Type = Datatype.String
                },
                new StammdatenEintrag() {
                    Title = "VereinsFirmierung",
                    Value = "e.V.",
                    Type = Datatype.String
                },
                new StammdatenEintrag() {
                    Title = "Vorstand1",
                    Value = "Sarah Leitner",
                    Type = Datatype.String
                },
                new StammdatenEintrag() {
                    Title = "Vorstand2",
                    Value = "Stefan Schick",
                    Type = Datatype.String
                },
                new StammdatenEintrag() {
                    Title = "Arbeitsstunden",
                    Value = "10,0",
                    Type = Datatype.Double,
                    Description = "Anzahl zu erbringender Arbeitsstunden pro Kalenderjahr"
                },
                new StammdatenEintrag() {
                    Title = "VereinAdresse",
                    Value = "Hundesportverein Oppenheim e.V., Am Weinberg, 78120 Villingen-Schwenningen",
                    Type = Datatype.String,
                    Description = "Adresse vom Hundeplatz für Navigation"
                },
                new StammdatenEintrag() {
                    Title = "VereinLatitude",
                    Value = "48,250316",
                    Type = Datatype.Double,
                    Description = "Verein Standort Latitude"
                },
                new StammdatenEintrag() {
                    Title = "VereinLongitude",
                    Value = "8,379323",
                    Type = Datatype.Double,
                    Description = "Verein Standort Longitude"
                },
                new StammdatenEintrag() {
                    Title = "TrainerBudget",
                    Value = "150",
                    Type = Datatype.Integer,
                    Description = "Jährliches Trainerbudget für Ausbildungen und Seminare"
                },
            };

            foreach (StammdatenEintrag e in stammdaten)
            {
                context.Stammdaten.Add(e);
            }
            context.SaveChanges();
        }

        private void InitializeTarife(VereinDbContext context)
        {
            if (context.Tarife.Any())
            {
                return;   // DB has been seeded
            }

            var tarife = new Tarif[]
            {
                new Tarif() {
                    Title = "Beitrittsgebühr",
                    Fee = 50.0m,
                    Description = "Einmalige Eintrittsgebühr im Verein"
                },
                new Tarif() {
                    Title = "Einzelmitgliedschaft",
                    Fee = 30.0m,
                    Description = "Einzelmitgliedschaft im Verein"
                },
                new Tarif() {
                    Title = "EinzelmitgliedschaftPassiv",
                    Fee = 15.0m,
                    Description = "Einzelmitgliedschaft passiv im Verein"
                },
                new Tarif() {
                    Title = "Familienmitgliedschaft",
                    Fee = 50.0m,
                    Description = "Familienmitgliedschaft im Verein"
                },
                new Tarif() {
                    Title = "PlatzpauschaleHund1",
                    Fee = 15.0m,
                    Description = "Platz Nutzungspauschale für den ersten Hund"
                },
                new Tarif() {
                    Title = "PlatzpauschaleHund2",
                    Fee = 15.0m,
                    Description = "Platz Nutzungspauschale für den zweiten Hund"
                },
                new Tarif() {
                    Title = "PlatzpauschaleHund3",
                    Fee = 5.0m,
                    Description = "Platz Nutzungspauschale für den dritten Hund"
                },
                new Tarif() {
                    Title = "PlatzpauschaleHund4",
                    Fee = 5.0m,
                    Description = "Platz Nutzungspauschale für den vierten Hund"
                },
                new Tarif() {
                    Title = "PlatzpauschaleHund5",
                    Fee = 5.0m,
                    Description = "Platz Nutzungspauschale für den fünften Hund"
                },
                new Tarif() {
                    Title = "ArbeitsstundenVerrechnungssatz",
                    Fee = 10.0m,
                    Description = "Verechnungssatz für nicht geleistete Arbeitsstunden"
                },
                new Tarif() {
                    Title = "JahresteilnahmeQuartalHund1",
                    Fee = 40.0m,
                    Description = "Gebühr pro Quartal für die Jahresteilnahme für den ersten Hund"
                },
                new Tarif() {
                    Title = "JahresteilnahmeQuartalHund2",
                    Fee = 15.0m,
                    Description = "Gebühr pro Quartal für die Jahresteilnahme für den zweiten Hund"
                },
                new Tarif() {
                    Title = "Welpenkurs",
                    Fee = 80.0m,
                    Description = "Gebühr für die Welpen Lern- und Spielstunde bis zum 7.Monat"
                },
                new Tarif() {
                    Title = "TrainerRabatt",
                    Fee = 15.0m,
                    Description = "Trainerermäßigung"
                },
            };

            foreach (Tarif e in tarife)
            {
                context.Tarife.Add(e);
            }
            context.SaveChanges();
        }

    }
}