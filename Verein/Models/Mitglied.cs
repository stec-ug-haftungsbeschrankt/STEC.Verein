using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Verein.Validators;

namespace Verein.Models
{
    public enum MitgliederTyp
    {
        Ehrenmitglied,
        Mitglied,
        Kursteilnehmer,
        Jahresteilnahme,
        WelpenLernSpielstunde
    }

    public class Mitglied
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Mitgliedsnummer")]
        public string MitgliedsNummer { get; set; } // Leading zeroes

        [Display(Name = "SWHV Mitgliedsnummer")]
        [MitgliedSwhvNummerValidator]
        public string SwhvMitgliedsNummer { get; set; } // Leading zeroes

        public int? Year { get; set; } // Rolling Entry, every Year, all Mitglieder get copied and the Year gets incremented to the current year

        [Required]
        public string Name { get; set; }

        [Required]
        public string Vorname { get; set; }

        [Required]
        [MitgliedTypValidator]
        public MitgliederTyp Typ { get; set; }

        public bool Passiv { get; set; }

        public bool Familienmitgliedschaft { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Geburtstag { get; set; }

        [Phone]
        [Display(Name = "Telefon")]
        public string Telefonnummer { get; set; }

        [Phone]
        [Display(Name = "Mobil")]
        public string HandyNummer { get; set; }

        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string EMail { get; set; }

        [Required]
        [Display(Name = "Straße")]
        public string Strasse { get; set; }

        [Required]
        public string Hausnummer { get; set; }

        [Required]
        public string Postleitzahl { get; set; }

        [Required]
        public string Ort { get; set; }

        [DataType(DataType.Date)]
        public DateTime Eintrittsdatum { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Austrittsdatum { get; set; }

        public double Entfernung { get; set; }

        public string Bemerkung { get; set; }

        public virtual IList<KursTeilnehmer> Kurse { get; set; }
        public virtual IList<Hund> Hunde { get; set; }
        public virtual BankInformation ZahlungsInfo { get; set; }
        public virtual IList<Helfer> Arbeitsstunden { get; set; }
        public virtual Familie Familie { get; set; }
    }
}