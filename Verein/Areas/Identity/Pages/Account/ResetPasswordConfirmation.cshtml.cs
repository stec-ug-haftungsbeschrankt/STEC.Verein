using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Verein.Models;
using Verein.Data;

namespace Verein.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : VereinPageModel
    {
        public ResetPasswordConfirmationModel(IDatabaseMediator databaseMediator, UserManager<HundevereinUser> userManager, ILogger<ResetPasswordConfirmationModel> logger)
            : base(databaseMediator, logger, userManager)
        {

        }


        public async Task OnGetAsync()
        {
            await base.Initialize().ConfigureAwait(false);
        }
    }
}
