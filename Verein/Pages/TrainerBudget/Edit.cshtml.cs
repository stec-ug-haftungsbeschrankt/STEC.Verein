using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.TrainerBudget
{
    public class EditModel : VereinPageModel
    {

        public EditModel(IDatabaseMediator databaseMediator, ILogger<EditModel> logger, UserManager<HundevereinUser> userManager)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var context = _databaseMediator.GetDbContext();
            context.Attach(TrainerBudget).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerBudgetExists(TrainerBudget.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TrainerBudgetExists(int id)
        {
            var context = _databaseMediator.GetDbContext();
            return context.TrainerBudget.Any(e => e.Id == id);
        }
    }
}
