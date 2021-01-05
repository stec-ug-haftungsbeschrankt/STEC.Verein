using System;
using System.ComponentModel.DataAnnotations;

namespace Verein.ViewModels
{
    public class EntfernungEvalViewModel
    {
        public string Gruppe { get; set; }

        [Display(Name = "Einträge")]
        public int Eintraege { get; set; }
    }
}