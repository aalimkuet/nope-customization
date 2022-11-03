using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Books;
using Nop.Services.Books;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Books;
using System.Threading.Tasks;

namespace Nop.Web.Controllers
{
    public partial class BookController : BasePublicController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IBookModelFactory _BookModelFactory;
        private readonly ICustomerTrackerService _BookService;

        #endregion

        #region Ctor

        public BookController(
                         
            IPermissionService permissionService,
            IBookModelFactory BookModelFactory,
            ICustomerTrackerService BookService

            )
        {           
            _permissionService = permissionService;
            _BookModelFactory = BookModelFactory;
            _BookService = BookService;
        }

        #endregion

        #region Books

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public virtual async Task<IActionResult> List()
        {
            
             var model = await _BookService.GetAllBookList(new Book());

            return View(model);
        }
        [HttpPost]
        public virtual async Task<IActionResult> List(BookSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                ;// return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _BookModelFactory.PrepareBookListModelAsync(searchModel);

            return Json(model);
        }
        #endregion
    }
}