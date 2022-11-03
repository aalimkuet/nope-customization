using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Factories;
using Nop.Plugin.Widgets.Customer.Models;
using Nop.Plugin.Widgets.CustomerTrackers.Services;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Customer.Components
{

    [ViewComponent(Name = "CustomerTracker")]
    public class CustomerTrackerViewComponent : NopViewComponent
    {

        private readonly ICustomerTrackerService _customerTrackerService;
        private readonly ICustomerTrackerModelFactory _customerTrackerModelFactory;
   
        public CustomerTrackerViewComponent(IStoreContext storeContext, ISettingService settingService, ICustomerTrackerModelFactory customerTrackerModelFactory,
            ICustomerTrackerService customerTrackerService)
        {
            _customerTrackerService = customerTrackerService;
            _customerTrackerModelFactory = customerTrackerModelFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            //var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            // var MyWidgetSettings = await _settingService.LoadSettingAsync<MyWidgetSettings>(storeScope);
            //var model = new CustomerTrackerModel
            //{

            //};

            //var model = await _customerTrackerModelFactory.PrepareCustomerTrackerListModelAsync(new CustomerTrackerSearchModel());

            var model = await _customerTrackerService.GetAllCustomerTrackerList(new CustomerTracker());
            //return Content("Hello");
            return View("~/Plugins/Widgets.Customer/Views/CustomerList.cshtml", model);
        }
    }

}
