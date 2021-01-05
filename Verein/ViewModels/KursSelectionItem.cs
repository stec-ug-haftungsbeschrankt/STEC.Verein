using System;
using System.ComponentModel.DataAnnotations;


namespace Verein.ViewModels
{
    public class KursSelectionItem : SelectionItem
    {
        public string Kursname { get; set; }

        [DataType(DataType.Date)]
        public DateTime Geburtstag { get; set; }
    }
}