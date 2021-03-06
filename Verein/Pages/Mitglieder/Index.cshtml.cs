using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Mitglieder
{
    public class IndexModel : VereinPageModel
    {
        public IndexModel(IDatabaseMediator databaseMediator, ILogger<IndexModel> logger)
            : base(databaseMediator, logger)
        {

        }

        public IList<Mitglied> Mitglieder { get; set; }

        public async Task OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            Mitglieder = await _databaseMediator.GetMitgliederOrderedByName().ConfigureAwait(false);
        }


        public async Task OnGetFilterAsync(string query)
        {
            await base.Initialize().ConfigureAwait(false);
            var success = Enum.TryParse(query, out MitgliederTyp mitgliedsTyp);

            if (success)
            {
                Mitglieder = await _databaseMediator.GetMitgliederByTypeOrderedByName(mitgliedsTyp).ConfigureAwait(false);
            }
            else
            {
                Mitglieder = await _databaseMediator.GetMitgliederOrderedByName().ConfigureAwait(false);
            }
        }
    }
}
