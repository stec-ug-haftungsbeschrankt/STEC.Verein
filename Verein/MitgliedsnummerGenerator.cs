using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verein.Data;
using Verein.Models;

namespace Verein
{
    public class MitgliedsnummerGenerator
    {
        private readonly IDatabaseMediator _databaseMediator;

        public MitgliedsnummerGenerator(IDatabaseMediator databaseMediator)
        {
            _databaseMediator = databaseMediator;
        }


        public async Task<string> GenerateMitgliedsnummer(MitgliederTyp typ)
        {
            switch (typ)
            {
                case MitgliederTyp.Kursteilnehmer:
                    return await GenerateMitgliedsnummerWithPrefix("KT").ConfigureAwait(false);
                case MitgliederTyp.Jahresteilnahme:
                    return await GenerateMitgliedsnummerWithPrefix("JT").ConfigureAwait(false);
                case MitgliederTyp.WelpenLernSpielstunde:
                    return await GenerateMitgliedsnummerWithPrefix("WS").ConfigureAwait(false);
                default:
                    return string.Empty;
            }
        }

        private async Task<string> GenerateMitgliedsnummerWithPrefix(string prefix)
        {
            if (prefix != "KT" && prefix != "JT" && prefix != "WS")
            {
                throw new ArgumentOutOfRangeException("Invalid Mitgliedsnummern prefix");
            }

            var nummer = "001";
            var mitgliedsNummern = await _databaseMediator.GetMitgliesnummernByPrefix(prefix).ConfigureAwait(false);

            if (mitgliedsNummern.Any())
            {
                var maxNumber = GetMaxMitgliedsnummer(mitgliedsNummern);
                nummer = (maxNumber + 1).ToString("D3");
            }
            return prefix + nummer;
        }



        private int GetMaxMitgliedsnummer(IList<string> numbers)
        {
            var maxNumber = 0;

            foreach (var number in numbers)
            {
                var numberPart = number.Substring(2);

                var success = int.TryParse(numberPart, out var parsedNumber);

                if (success && parsedNumber > maxNumber)
                {
                    maxNumber = parsedNumber;
                }
            }
            return maxNumber;
        }
    }
}
