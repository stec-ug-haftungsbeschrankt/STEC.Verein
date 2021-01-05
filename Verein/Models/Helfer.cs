using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Verein.Models
{
    public class Helfer
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Dauer { get; set; }

        public virtual Arbeitseinsatz Arbeitseinsatz { get; set; }

        public virtual Mitglied Teilnehmer { get; set; }

    }
}
