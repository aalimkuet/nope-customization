using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Factories;
using Nop.Plugin.Widgets.Customer.Mapper;
using Nop.Plugin.Widgets.Customer.Models;
using Nop.Plugin.Widgets.CustomerTrackers.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Customer.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class CustomerTrackerController : BasePluginController
    {

        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ICustomerTrackerModelFactory _customerTrackerModelFactory;
        private readonly ICustomerTrackerService _customerTrackerService;
        private readonly Lazy<IPermissionService> _permissionService;
        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor

        public CustomerTrackerController(
            ILocalizationService localizationService,
            INotificationService notificationService,
           Lazy<IPermissionService> permissionService,
            ICustomerTrackerModelFactory customerTrackerModelFactory,
            ICustomerTrackerService customerTrackerService,
            IPictureService pictureService
            )
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _customerTrackerModelFactory = customerTrackerModelFactory;
            _customerTrackerService = customerTrackerService;
            _pictureService = pictureService;
        }

        #endregion

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.Value.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
                return AccessDeniedView();

            //prepare model
            var model = await _customerTrackerModelFactory.PrepareCustomerTrackerSearchModelAsync(new CustomerTrackerSearchModel());

            //return View(model);
            return View("~/Plugins/Widgets.Customer/Views/List.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(CustomerTrackerSearchModel searchModel)
        {
            if (!await _permissionService.Value.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _customerTrackerModelFactory.PrepareCustomerTrackerListModelAsync(searchModel);

            return Json(model);
        }
        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.Value.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
                return AccessDeniedView();

            //prepare model
            var model = await _customerTrackerModelFactory.PrepareCustomerTrackerModelAsync(new CustomerTrackerModel(), null);

            //return View(model);
            return View("~/Plugins/Widgets.Customer/Views/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Create(CustomerTrackerModel model, bool continueEditing, IFormCollection form)
        {
            if (!await _permissionService.Value.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var customerTracker = model.ToEntity<CustomerTracker>();

                //try to get a picture with the specified id
                var picture = await _pictureService.GetPictureByIdAsync(model.PictureId)
                    ?? throw new ArgumentException("No picture found with the specified id");

                await _pictureService.UpdatePictureAsync(picture.Id,
                    await _pictureService.LoadPictureBinaryAsync(picture),
                    picture.MimeType,
                    picture.SeoFilename,
                    model.OverrideAltAttribute,
                   model.OverrideTitleAttribute

                   );

                ///await _pictureService.SetSeoFilenameAsync(model.PictureId, await _pictureService.GetPictureSeNameAsync(product.Name));

                //await _productService.InsertProductPictureAsync(new ProductPicture
                //{
                //    PictureId = pictureId,
                //    ProductId = productId,
                //    DisplayOrder = displayOrder
                //});

                await _customerTrackerService.InsertCustomerTrackerAsync(customerTracker);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.CustomerTrackers.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = customerTracker.Id });
            }

            //prepare model
            model = await _customerTrackerModelFactory.PrepareCustomerTrackerModelAsync(model, null, true);

            //if we got this far, something failed, redisplay form
            return View("~/Plugins/Widgets.Customer/Views/Create.cshtml", model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.Value.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
                return AccessDeniedView();

            //try to get a CustomerTracker with the specified id
            var CustomerTracker = await _customerTrackerService.GetCustomerTrackerByIdAsync(id);
            if (CustomerTracker == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _customerTrackerModelFactory.PrepareCustomerTrackerModelAsync(null, CustomerTracker);

            // return View(model);
            return View("~/Plugins/Widgets.Customer/Views/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(CustomerTrackerModel model, bool continueEditing, IFormCollection form)
        {
            if (!await _permissionService.Value.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
                return AccessDeniedView();

            //try to get a CustomerTracker with the specified ids
            var CustomerTracker = await _customerTrackerService.GetCustomerTrackerByIdAsync(model.Id);
            if (CustomerTracker == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                CustomerTracker = model.ToEntity(CustomerTracker);
                await _customerTrackerService.UpdateCustomerTrackerAsync(CustomerTracker);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.CustomerTrackers.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = CustomerTracker.Id });
            }

            //prepare model
            model = await _customerTrackerModelFactory.PrepareCustomerTrackerModelAsync(model, CustomerTracker, true);

            //if we got this far, something failed, redisplay form
            return View("~/Plugins/Widgets.Customer/Views/Edit.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.Value.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
                return AccessDeniedView();

            //try to get a CustomerTracker with the specified id
            var CustomerTracker = await _customerTrackerService.GetCustomerTrackerByIdAsync(id);
            if (CustomerTracker == null)
                return RedirectToAction("List");

            //delete a CustomerTracker
            await _customerTrackerService.DeleteCustomerTrackerAsync(CustomerTracker);

            ////activity log
            //await _customerActivityService.InsertActivityAsync("DeleteCustomerTracker",
            //    string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteCustomerTracker"), CustomerTracker.Id), CustomerTracker);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.CustomerTrackers.Deleted"));

            return RedirectToAction("List");
        }

        public IActionResult Configure()
        {
            return View("~/Plugins/Widgets.CustomerTrackerTracker/Views/Configure.cshtml");
        }
    }
}