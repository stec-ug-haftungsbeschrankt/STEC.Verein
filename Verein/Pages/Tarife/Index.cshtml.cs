using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Tarife
{
    public class IndexModel : VereinPageModel
    {
        public IndexModel(IDatabaseMediator databaseMediator, ILogger<IndexModel> logger, UserManager<HundevereinUser> userManager)
            : base (databaseMediator, logger, userManager)
        {
        }

        public IList<Tarif> Tarif { get;set; }

        public async Task OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            Tarif = await _databaseMediator.GetTarifeOrderedByTitle().ConfigureAwait(false);
        }
    }
}
