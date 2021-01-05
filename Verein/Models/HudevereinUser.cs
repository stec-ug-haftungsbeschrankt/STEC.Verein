using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Verein.Models
{
    public enum BenutzerTyp
    {
        Vorstand = 1,
        [Display(Name = "Erweiterter Vorstand")]
        ErweiterterVorstand = 2,
        Trainer = 3,
        None = 999
    }

    public class HundevereinUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Vor- und Nachname")]
        public string FullName { get; set; }

        [Display(Name = "Genehmigt")]
        public bool Approved {get; set; }

        public BenutzerTyp Rolle { get; set; }
    }
}