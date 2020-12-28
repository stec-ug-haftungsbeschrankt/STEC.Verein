# STEC.Verein

Verwaltungssoftware für Hundevereine. Das umfalsst das Verwalten von

- Mitgliedern, Jahresteilnehmern und Kursteilnehmern der Welpen-Spielstunde
- Hunden inklusive Chip- und Versicherungsinformationen
- Zahlungsinformationen für die Jährlichen Beitragsabrechnungen
- Arbeitstunden
- Kursen
- Trainer-Budget für AUs und Weiterbildung der Trainer

Als Datenbank wird PostgreSql verwendet. Die Konfiguration erfolgt in einer appsettings.json-Datei.


## Tests

Um die Tests auszuführen, müssen zuerst im Test-Project (STEC.Verein.Tests) UserSecrets gesetzt werden

```
cd STEC.Verein.Tests

dotnet user-secrets set PagesHundeTestsConnectionString "<your_connection_string>"
dotnet user-secrets set TarifCalculatorTestsConnectionString "<your_connection_string>"

cd ..
```
