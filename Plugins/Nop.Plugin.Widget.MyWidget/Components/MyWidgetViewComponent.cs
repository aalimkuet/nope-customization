using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widget.MyWidget.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nop.Plugin.Widget.MyWidget.Components
{

    [ViewComponent(Name = "MyWidget")]
    public class MyWidgetViewComponent : NopViewComponent
    {

        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        public MyWidgetViewComponent(IStoreContext storeContext, ISettingService settingService   )
        {
            _storeContext = storeContext;
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var MyWidgetSettings = await _settingService.LoadSettingAsync<MyWidgetSettings>(storeScope);
            var model = new ConfigurationModel
            {
                CustomText = MyWidgetSettings.CustomText,
                ActiveStoreScopeConfiguration = storeScope
            };

            //return Content("Hello World");
            return View("~/Plugins/Widget.MyWidget/Views/Hello.cshtml", model);
        }
    }

}
