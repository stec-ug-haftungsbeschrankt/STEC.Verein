using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages
{
    public class IndexModel : VereinPageModel
    {
        public IndexModel(IDatabaseMediator databaseMediator, ILogger<IndexModel> logger) :
            base(databaseMediator, logger)
        {

        }

        public IList<Mitglied> Mitglieder { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            Mitglieder = await _databaseMediator.GetMitgliederOrderedByName().ConfigureAwait(false);
            return Page();
        }
    }
}
