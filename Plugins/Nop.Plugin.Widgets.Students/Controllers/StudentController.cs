using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.Students.Domains;
using Nop.Plugin.Widgets.Students.Factories;
using Nop.Plugin.Widgets.Students.Mapping;
using Nop.Plugin.Widgets.Students.Models;
using Nop.Plugin.Widgets.Students.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Students.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class StudentController : BasePluginController
    {

        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly StudentWidgetSettings _studentSettings;
        private readonly IStudentModelFactory _StudentModelFactory;
        private readonly IStudentService _StudentService;

        #endregion

        #region Ctor

        public StudentController(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            StudentWidgetSettings myWidgetSettings)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _studentSettings = myWidgetSettings;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var myWidgetSettings = await _settingService.LoadSettingAsync<StudentWidgetSettings>(storeScope);

            var model = new ConfigurationModel
            {
                CustomText = myWidgetSettings.CustomText,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.CustomText_OverrideForStore = await _settingService.SettingExistsAsync(myWidgetSettings, x => x.CustomText, storeScope);
               
            }
            return View("~/Plugins/Widget.MyWidget/Views/Configure.cshtml", model);
           
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var myWidgetSettings = await _settingService.LoadSettingAsync<StudentWidgetSettings>(storeScope);

            myWidgetSettings.CustomText = model.CustomText;
            

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(myWidgetSettings, x => x.CustomText, model.CustomText_OverrideForStore, storeScope, false); 

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion

        #region Students

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            //prepare model
            var model = await _StudentModelFactory.PrepareStudentSearchModelAsync(new StudentSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(StudentSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _StudentModelFactory.PrepareStudentListModelAsync(searchModel);

            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            //prepare model
            var model = await _StudentModelFactory.PrepareStudentModelAsync(new StudentModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Create(StudentModel model, bool continueEditing, IFormCollection form)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var student = model.ToEntity<Student>();
               // await _StudentService.InsertStudentAsync(Student);

                // await _StudentService.UpdateStudentAsync(Student);                

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Students.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id =1 });
            }

            //prepare model
            model = await _StudentModelFactory.PrepareStudentModelAsync(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            //try to get a Student with the specified id
            var Student = await _StudentService.GetStudentByIdAsync(id);
            if (Student == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _StudentModelFactory.PrepareStudentModelAsync(null, Student);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(StudentModel model, bool continueEditing, IFormCollection form)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            //try to get a Student with the specified id
            var Student = await _StudentService.GetStudentByIdAsync(model.Id);
            if (Student == null)
                return RedirectToAction("List");


            if (ModelState.IsValid)
            {
                Student = model.ToEntity(Student);
                await _StudentService.UpdateStudentAsync(Student);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Students.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = Student.Id });
            }

            //prepare model
            model = await _StudentModelFactory.PrepareStudentModelAsync(model, Student, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            //try to get a Student with the specified id
            var Student = await _StudentService.GetStudentByIdAsync(id);
            if (Student == null)
                return RedirectToAction("List");

            //delete a Student
            await _StudentService.DeleteStudentAsync(Student);

            ////activity log
            //await _customerActivityService.InsertActivityAsync("DeleteStudent",
            //    string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteStudent"), Student.Id), Student);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Students.Deleted"));

            return RedirectToAction("List");
        }

        #endregion
    }
}