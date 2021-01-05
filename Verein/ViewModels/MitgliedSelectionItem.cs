using System;
using System.ComponentModel.DataAnnotations;


namespace Verein.ViewModels
{
    public class MitgliedSelectionItem : SelectionItem
    {
        public string Vorname { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Geburtstag { get; set; }
    }
}