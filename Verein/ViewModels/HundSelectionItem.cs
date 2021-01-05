using System;
using System.ComponentModel.DataAnnotations;


namespace Verein.ViewModels
{
    public class HundSelectionItem : SelectionItem
    {
        public string Hundename { get; set; }

        public string Rasse { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Geburtstag { get; set; }
    }
}