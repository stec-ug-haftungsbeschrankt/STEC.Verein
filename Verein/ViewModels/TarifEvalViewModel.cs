using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Verein.ViewModels
{
    public class TarifEvalViewModel
    {
        public int MitgliedsId { get; set; }

        [Display(Name = "Mitgliedsnummer")]
        public string Mitgliedsnummer { get; set; }

        [Display(Name = "Vor- und Nachname")]
        public string FullName { get; set; }

        [Display(Name = "Details")]
        public List<string> Details { get; set; }

        public List<string> Errors { get; set; }

        [Display(Name = "Betrag")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal Beitrag { get; set; }

    }
}




