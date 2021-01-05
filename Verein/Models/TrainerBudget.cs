using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Verein.Models
{
    public class TrainerBudget
    {
        public int Id { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Betrag { get; set; }

        public virtual Trainer Trainer { get; set; }
    }
}
