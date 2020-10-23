using Microsoft.AspNetCore.Mvc.RazorPages;
using OliWorkshop.AccountingSys.Components;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Pages.Groups
{
    public class IndexModel : PageModel
    {
        public IndexModel(GroupsService groupService)
        {
            GroupService = groupService ?? throw new System.ArgumentNullException(nameof(groupService));
        }

        public IEnumerable<GroupMetricResult> GroupMetrics { get; set; }
        public GroupsService GroupService { get; }

        public async Task OnGetAsync(int page = 0, int length = 25)
        {
            GroupMetrics = await GroupService.GetGroupWithMetrics(HttpContext.User.GetUserId(), page, length);
        }
    }
}
