using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Books;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Books;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Books;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class BookController : BaseAdminController
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
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                return AccessDeniedView();

            //prepare model
            var model = await _BookModelFactory.PrepareBookSearchModelAsync(new BookSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(BookSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                 return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _BookModelFactory.PrepareBookListModelAsync(searchModel);

            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                return AccessDeniedView();

            //prepare model
            var model = await _BookModelFactory.PrepareBookModelAsync(new BookModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Create(BookModel model, bool continueEditing, IFormCollection form)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                 return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var book = model.ToEntity<Book>();
                await _BookService.InsertBookAsync(book);

                // await _BookService.UpdateBookAsync(Book);                

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Books.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = book.Id });
            }

            //prepare model
            model = await _BookModelFactory.PrepareBookModelAsync(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                 return AccessDeniedView();

            //try to get a Book with the specified id
            var Book = await _BookService.GetBookByIdAsync(id);
            if (Book == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _BookModelFactory.PrepareBookModelAsync(null, Book);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(BookModel model, bool continueEditing, IFormCollection form)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                 return AccessDeniedView();

            //try to get a Book with the specified id
            var Book = await _BookService.GetBookByIdAsync(model.Id);
            if (Book == null)
                return RedirectToAction("List");


            if (ModelState.IsValid)
            {
                Book = model.ToEntity(Book);
                await _BookService.UpdateBookAsync(Book);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Books.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = Book.Id });
            }

            //prepare model
            model = await _BookModelFactory.PrepareBookModelAsync(model, Book, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageBooks))
                return AccessDeniedView();

            //try to get a Book with the specified id
            var Book = await _BookService.GetBookByIdAsync(id);
            if (Book == null)
                return RedirectToAction("List");

            //delete a Book
            await _BookService.DeleteBookAsync(Book);

            ////activity log
            //await _customerActivityService.InsertActivityAsync("DeleteBook",
            //    string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteBook"), Book.Id), Book);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Books.Deleted"));

            return RedirectToAction("List");
        }

        #endregion


    }
}