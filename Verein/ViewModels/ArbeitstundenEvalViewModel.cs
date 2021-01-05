using System;
using System.ComponentModel.DataAnnotations;

namespace Verein.ViewModels
{
    public class ArbeitsstundenEvalViewModel
    {
        public int MitgliedsId { get; set; }

        [Display(Name = "Mitgliedsnummer")]
        public string Mitgliedsnummer { get; set; }

        [Display(Name = "Vor- und Nachname")]
        public string FullName { get; set; }

        [Display(Name = "Geleistete Stunden")]
        public double GeleisteteStunden { get; set; }

        [Display(Name = "Erwartete Stunden")]
        public double ErwarteteStunden { get; set; }

    }
}