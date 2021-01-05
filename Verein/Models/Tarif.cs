using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Verein.Models
{
    public class Tarif
    {
        public int Id { get; set; }

        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Betrag")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal Fee { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
    }
}