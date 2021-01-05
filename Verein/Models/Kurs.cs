using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Verein.Models
{
    public enum Wochentag {
        Sonntag,
        Montag,
        Dienstag,
        Mittwoch,
        Donnerstag,
        Freitag,
        Samstag
    }

    public class Kurs
    {
        public int Id { get; set; }

        [Required]
        public string Titel { get; set; }

        public string Beschreibung { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Startdatum { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Enddatum { get; set; }


        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Von { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Bis { get; set; }


        public Wochentag Wochentag { get; set; }

        public virtual IList<KursTeilnehmer> Teilnehmer { get; set; }

        public virtual IList<Trainer> Trainer { get; set; }
    }
}
