using System;
using System.ComponentModel.DataAnnotations;
using Verein.Models;

namespace Verein.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Vor- und Nachname")]
        public string FullName { get; set; }

        [Display(Name = "Benutzername")]
        public string UserName { get; set; }

        [Display(Name = "Rolle")]
        public BenutzerTyp Rolle { get; set; }

        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Genehmigt")]
        public bool Approved { get; set; }
    }
}