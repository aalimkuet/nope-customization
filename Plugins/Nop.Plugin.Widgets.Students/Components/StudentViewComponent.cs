using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.Students.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Students.Components
{

    [ViewComponent(Name = "StudentWidget")]
    public class StudentWidgetViewComponent : NopViewComponent
    {

        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        public StudentWidgetViewComponent(IStoreContext storeContext, ISettingService settingService   )
        {
            _storeContext = storeContext;
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var StudentSettings = await _settingService.LoadSettingAsync<StudentWidgetSettings>(storeScope);
            var model = new ConfigurationModel
            {
                CustomText = StudentSettings.CustomText,
                ActiveStoreScopeConfiguration = storeScope
            };

            //return Content("Hello World");
            return View("~/Plugins/Widget.MyWidget/Views/Hello.cshtml", model);
        }
    }

}
