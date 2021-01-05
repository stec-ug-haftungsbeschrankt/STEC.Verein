using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using Verein.Models;
using Verein.ViewModels;

namespace Verein
{
    public class TarifListExporter
    {
        public byte[] ExportToExcel(List<TarifEvalViewModel> tarifdaten, IList<Mitglied> mitglieder)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Tarif- und Zahlungsdaten");

                worksheet.Cells[1, 1].Value = "SWHV Mitgliedsnummer";
                worksheet.Cells[1, 2].Value = "Mitgliedsnummer";
                worksheet.Cells[1, 3].Value = "Vorname";
                worksheet.Cells[1, 4].Value = "Name";
                worksheet.Cells[1, 5].Value = "Geburtstag";
                worksheet.Cells[1, 6].Value = "Details";
                worksheet.Cells[1, 7].Value = "Betrag";
                worksheet.Cells[1, 8].Value = "Kontoinhaber";
                worksheet.Cells[1, 9].Value = "Name der Bank";
                worksheet.Cells[1, 10].Value = "IBAN";
                worksheet.Cells[1, 11].Value = "BIC";

                using (var range = worksheet.Cells[1, 1, 1, 11])
                {
                    range.Style.Font.Bold = true;
                }

                int row = 2;

                foreach (var entry in tarifdaten)
                {
                    var mitglied = mitglieder.SingleOrDefault(m => m.Id == entry.MitgliedsId);

                    worksheet.Cells[row, 1].Value = mitglied.SwhvMitgliedsNummer;
                    worksheet.Cells[row, 2].Value = mitglied.MitgliedsNummer;
                    worksheet.Cells[row, 3].Value = mitglied.Vorname;
                    worksheet.Cells[row, 4].Value = mitglied.Name;
                    worksheet.Cells[row, 5].Value = mitglied.Geburtstag.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                    worksheet.Cells[row, 6].Style.WrapText = true;
                    worksheet.Cells[row, 6].Value = string.Join("\n", entry.Errors) + string.Join("\n", entry.Details);
                    worksheet.Cells[row, 7].Value = $"{entry.Beitrag}€";

                    if (mitglied.ZahlungsInfo != null)
                    {
                        worksheet.Cells[row, 8].Value = mitglied.ZahlungsInfo.KontoInhaber;
                        worksheet.Cells[row, 9].Value = mitglied.ZahlungsInfo.BankName;
                        worksheet.Cells[row, 10].Value = mitglied.ZahlungsInfo.Iban;
                        worksheet.Cells[row, 11].Value = mitglied.ZahlungsInfo.Bic;
                    }

                    worksheet.Row(row).Height = 60;
                    row++;
                }

                worksheet.Cells.AutoFitColumns(0);
                worksheet.Column(6).Width = 50;
                return excelPackage.GetAsByteArray();
            }
        }
    }
}
