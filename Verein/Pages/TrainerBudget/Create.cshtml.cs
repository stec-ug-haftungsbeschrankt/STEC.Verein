using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Verein.Data;
using Verein.Models;

namespace Verein.Pages.TrainerBudget
{
    public class CreateModel : VereinPageModel
    {

        public CreateModel(IDatabaseMediator databaseMediator, ILogger<CreateModel> logger, UserManager<HundevereinUser> userManager)
            : base(databaseMediator, logger, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.TrainerBudget TrainerBudget { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var context = _databaseMediator.GetDbContext();
            context.TrainerBudget.Add(TrainerBudget);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
