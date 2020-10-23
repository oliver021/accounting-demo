using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OliWorkshop.AccountingSys.Components;

namespace OliWorkshop.AccountingSys.Pages.Groups
{
    public class IndexModel : PageModel
    {
        public IndexModel(GroupsService controller)
        {
        }

        public void OnGetAsync()
        {
        }
    }
}
