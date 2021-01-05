using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Verein.Models
{
    public class Familie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<Mitglied> Mitglieder { get; set; }
    }
}
