using System;
using System.ComponentModel.DataAnnotations;

namespace Verein.Models
{
    public enum Datatype
    {
        Integer,
        Double,
        String
    }

    public class StammdatenEintrag
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Wert")]
        public string Value { get; set; }

        [Required]
        [Display(Name = "Typ")]
        public Datatype Type { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
    }
}