using System;
using System.ComponentModel.DataAnnotations;
using Verein.Validators;

namespace Verein.Models
{
    public class Hund
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Zwingername { get; set; }

        [Required]
        public string Rasse { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Geburtsdatum { get; set; }

        [HundChipValidator]
        [Display(Name = "Chip Nummer")]
        public string ChipNummer { get; set; }

        public bool Geimpft { get; set; }
        public bool Versichert { get; set; }

        public virtual Mitglied Besitzer { get; set; }
    }
}