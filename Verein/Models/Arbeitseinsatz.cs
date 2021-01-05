using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Verein.Models
{
    public class Arbeitseinsatz
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Titel { get; set; }

        [Required]
        [Display(Name = "Tätigkeit")]
        public string Taetigkeit { get; set; }

        [Display(Name = "Helfer")]
        public virtual IList<Helfer> Helfer { get; set; }

    }
}
