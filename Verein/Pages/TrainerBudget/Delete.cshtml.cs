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
    public class DeleteModel : VereinPageModel
    {


        public DeleteModel(IDatabaseMediator databaseMediator, ILogger<DeleteModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {

        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var context = _databaseMediator.GetDbContext();
            TrainerBudget = await context.TrainerBudget.FindAsync(id);

            if (TrainerBudget != null)
            {
                context.TrainerBudget.Remove(TrainerBudget);
                await context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
