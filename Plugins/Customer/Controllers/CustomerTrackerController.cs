using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.Customer.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class CustomerTrackerController : BasePluginController
    {
        public CustomerTrackerController()
        {

        }
        public IActionResult Configure()
        {
            return View("~/Plugins/Widgets.BookTracker/Views/Configure.cshtml");
        }
    }
}