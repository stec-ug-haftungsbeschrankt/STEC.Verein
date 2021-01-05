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
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : VereinPageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorModel(IDatabaseMediator databaseMediator)
            : base(databaseMediator)
        {

        }


        public async Task OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
