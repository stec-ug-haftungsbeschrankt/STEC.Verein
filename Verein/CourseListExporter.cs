using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using Verein.Models;

namespace Verein
{
    public class CourseListExporter
    {
        private readonly string Delimiter = ";";


        public byte[] ExportToCsv(Kurs kurs, IList<Mitglied> teilnehmer)
        {
            string result = string.Empty;

            // Name, Vorname, Hunde, Datumsliste
            string header = $"Name{Delimiter}Vorname{Delimiter}Hunde{Delimiter}";

            DateTime trainingDay = DateTime.Now;
            for (int i = 6; i < 14; i++)
            {
                header += trainingDay.ToString("dd.MM.");
                header += Delimiter;
                trainingDay = trainingDay.AddDays(7);
            }
            header += "\n";

            result+= header;

            foreach (var entry in teilnehmer)
            {
                result += TeilnehmerToCsv(entry, Delimiter);
            }

            return Encoding.UTF8.GetBytes(result);
        }


        private string TeilnehmerToCsv(Mitglied teilnehmer, string delimiter)
        {
            string result = string.Empty;

            if (teilnehmer.Hunde.Any())
            {
                result += $"{teilnehmer.Name}{delimiter}{teilnehmer.Vorname}{delimiter}{teilnehmer.Hunde.First().Name}{delimiter}\n";

                foreach (var hund in teilnehmer.Hunde.Skip(1))
                {
                    result += $"{delimiter}{delimiter}{hund.Name}{delimiter}\n";
                }
            }
            else
            {
                result += $"{teilnehmer.Name}{delimiter}{teilnehmer.Vorname}{delimiter}\n";
            }
            return result;
        }

        public byte[] ExportToExcel(Kurs kurs, IList<Mitglied> teilnehmer)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(kurs.Titel);

                worksheet.Cells[1, 1].Value = "Grp";
                worksheet.Cells[1, 2].Value = "M/JT";
                worksheet.Cells[1, 3].Value = "B";
                worksheet.Cells[1, 4].Value = "Vorname, Name";
                worksheet.Cells[1, 5].Value = "Hundename";

                DateTime trainingDay = DateTime.Now;
                for (int i = 6; i < 14; i++)
                {
                    worksheet.Cells[1, i].Value = trainingDay.ToString("dd.MM.");
                    trainingDay = trainingDay.AddDays(7);
                }

                using (var range = worksheet.Cells[1, 1, 1, 14])
                {
                    range.Style.Font.Bold = true;
                }

                int row = 2;

                foreach (var entry in teilnehmer)
                {
                    worksheet.Cells[row, 4].Value = $"{entry.Vorname} {entry.Name}";

                    var hundeNamen = entry.Hunde.Select(h => h.Name).ToList();
                    worksheet.Cells[row, 5].Value = string.Join(", ", hundeNamen);

                    row++;
                }

                worksheet.Cells.AutoFitColumns(0);

                //ws.Cells["A1"].LoadFromDataTable(tbl, true);
                return excelPackage.GetAsByteArray();
            }
        }
    }
}
