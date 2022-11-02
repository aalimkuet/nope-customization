using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Security;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Stores;

namespace Nop.Plugin.Widgets.BookTracker.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class BookTrackerController : BasePluginController
    {
        public BookTrackerController()
        {

        }
        public IActionResult Configure()
        {
            return View("~/Plugins/Widgets.BookTracker/Views/Configure.cshtml");
        }
    }
}