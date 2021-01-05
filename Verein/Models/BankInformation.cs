using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Verein.Validators;

namespace Verein.Models
{
    public class BankInformation
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name der Bank")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "Kontoinhaber")]
        public string KontoInhaber { get; set; }

        [Required]
        [BankIbanValidator]
        [Display(Name = "IBAN")]
        public string Iban { get; set; }

        [Required]
        [BankBicValidator]
        [Display(Name = "BIC")]
        public string Bic { get; set; }

        [Display(Name = "Mitglied")]
        public virtual IList<Mitglied> Besitzer { get; set; }
    }
}