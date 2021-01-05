using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Arbeitseinsaetze
{
    public class IndexModel : VereinPageModel
    {
        public IndexModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {
           
        }

        public IList<Arbeitseinsatz> ArbeitsTaetigkeit { get;set; }

        public async Task OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            var context = _databaseMediator.GetDbContext();
            ArbeitsTaetigkeit = await context.Arbeitseinsaetze.Include(a => a.Helfer)
                                                               .OrderByDescending(a => a.Datum)
                                                               .ToListAsync()
                                                               .ConfigureAwait(false);
        }
    }
}
