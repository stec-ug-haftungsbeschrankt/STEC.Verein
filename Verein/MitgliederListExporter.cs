using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using Verein.Models;

namespace Verein
{
    public class MitgliederListExporter : IDisposable
    {
        private ExcelPackage _excelPackage;

        public MitgliederListExporter()
        {
            _excelPackage = new ExcelPackage();
        }

        public void AddWorksheet(string title, IList<Mitglied> mitglieder)
        {
            ExcelWorksheet worksheet = _excelPackage.Workbook.Worksheets.Add(title);

            worksheet.Cells[1, 1].Value = "SWHV Mitgliedsnummer";
            worksheet.Cells[1, 2].Value = "Mitgliedsnummer";
            worksheet.Cells[1, 3].Value = "Vorname";
            worksheet.Cells[1, 4].Value = "Nachname";
            worksheet.Cells[1, 5].Value = "Geburtsdatum";
            worksheet.Cells[1, 6].Value = "Adresse";
            worksheet.Cells[1, 7].Value = "Telefon";
            worksheet.Cells[1, 8].Value = "Mobil";
            worksheet.Cells[1, 9].Value = "E-Mail";
            worksheet.Cells[1, 10].Value = "Eintrittsdatum";
            worksheet.Cells[1, 11].Value = "Austrittsdatum";

            using (var range = worksheet.Cells[1, 1, 1, 11])
            {
                range.Style.Font.Bold = true;
            }

            int row = 2;

            foreach (var entry in mitglieder)
            {
                worksheet.Cells[row, 1].Value = entry.SwhvMitgliedsNummer;
                worksheet.Cells[row, 2].Value = entry.MitgliedsNummer;
                worksheet.Cells[row, 3].Value = entry.Vorname;
                worksheet.Cells[row, 4].Value = entry.Name;
                worksheet.Cells[row, 5].Value = entry.Geburtstag.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                worksheet.Cells[row, 6].Value = $"{entry.Strasse} {entry.Hausnummer}, {entry.Postleitzahl} {entry.Ort}";
                worksheet.Cells[row, 7].Value = entry.Telefonnummer;
                worksheet.Cells[row, 8].Value = entry.HandyNummer;
                worksheet.Cells[row, 9].Value = entry.EMail;
                worksheet.Cells[row, 10].Value = entry.Eintrittsdatum.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                if (entry.Austrittsdatum.HasValue)
                {
                    worksheet.Cells[row, 11].Value = entry.Austrittsdatum.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                }

                row++;
            }

            worksheet.Cells.AutoFitColumns(0);
        }

        public byte[] ExportToExcel()
        {
            return _excelPackage.GetAsByteArray();
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _excelPackage.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
