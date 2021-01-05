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

namespace Verein.Pages.TrainerBudget
{
    public class DetailsModel : VereinPageModel
    {
        public DetailsModel(IDatabaseMediator databaseMediator, ILogger<DetailsModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        public Models.TrainerBudget TrainerBudget { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var context = _databaseMediator.GetDbContext();
            TrainerBudget = await context.TrainerBudget.FirstOrDefaultAsync(m => m.Id == id);

            if (TrainerBudget == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
