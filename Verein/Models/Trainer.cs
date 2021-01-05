using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Verein.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        public virtual Mitglied KursTrainer { get; set; }

        public virtual Kurs Kurse { get; set; }

        public virtual IList<TrainerBudget> TrainerBudget { get; set; }
    }
}