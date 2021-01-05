using System;
using System.ComponentModel.DataAnnotations;

namespace Verein.Models
{
    public class Gegenstand
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Name { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Required]
        public string Ort { get; set; }
    }
}