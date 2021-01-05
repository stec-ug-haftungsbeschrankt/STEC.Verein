using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.Kurse
{
    public class IndexModel : VereinPageModel
    {

        public IndexModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {

        }

        public IList<Kurs> Kurse { get;set; }

        public async Task OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            Kurse = await _databaseMediator.GetKurseOrderedByTitle().ConfigureAwait(false);
        }
    }
}
