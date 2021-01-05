using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verein.Models;
using Verein.Data;

namespace Verein
{
    public class VereinPageModel : PageModel
    {
        public readonly IDatabaseMediator _databaseMediator;

        public readonly ILogger _logger;

        public readonly UserManager<HundevereinUser> _userManager;

        public VereinPageModel(IDatabaseMediator databaseMediator, ILogger logger = null, UserManager<HundevereinUser> userManager = null)
            : base()
        {
            _databaseMediator = databaseMediator;
            _logger = logger;
            _userManager = userManager;
        }

        private IList<StammdatenEintrag> Stammdaten { get; set; }


        public async Task<bool> IsAuthorized(BenutzerTyp neededRole)
        {
            var user =  await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user.Rolle <= neededRole)
            {
                return true;
            }
            return false;
        }

        public async Task Initialize()
        {
            Stammdaten = await _databaseMediator.GetStammdaten().ConfigureAwait(false);

            foreach (var entry in Stammdaten)
            {
                ViewData[entry.Title] = GetPropertyValue(entry.Title);
            }
        }

        public string GetPropertyValue(string name)
        {
            var entry = Stammdaten.SingleOrDefault(e => e.Title == name);

            if (entry.Type == Datatype.String)
            {
                return entry.Value;
            }
            else
            {
                return entry.Value.ToString();
            }
        }

        public double GetDoublePropertyValue(string name)
        {
            var entry = Stammdaten.SingleOrDefault(e => e.Title == name);
            return double.Parse(entry.Value);
        }

    }
}