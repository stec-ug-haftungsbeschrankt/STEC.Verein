using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Verein.Models
{
    public class KursTeilnehmer
    {
        public int Id { get; set; }

        public virtual Mitglied Teilnehmer { get; set; }

        public virtual Kurs Kurse { get; set; }
    }
}