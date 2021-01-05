using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Verein.Data;

namespace Verein.Pages
{
    public class AccessDeniedModel : VereinPageModel
    {

        public AccessDeniedModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {

        }


        public async Task OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
        }
    }
}
