using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Books;
using Nop.Services.Books;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Books;
using System.Threading.Tasks;

namespace Nop.Web.Controllers
{
    public partial class BookController : BasePublicController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IBookModelFactory _BookModelFactory;
        private readonly IBookService _BookService;

        #endregion

        #region Ctor

        public BookController(

            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IUrlRecordService urlRecordService,
            IBookModelFactory BookModelFactory,
            IBookService BookService
            )
        {

            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _urlRecordService = urlRecordService;
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