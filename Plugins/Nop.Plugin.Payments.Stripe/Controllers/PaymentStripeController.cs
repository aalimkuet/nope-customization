using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.Stripe.Models;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Stripe;
using System.IO;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Stripe.Controllers
{
    public class PaymentStripeController : BasePaymentController
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentPluginManager _paymentPluginManager;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly StripePaymentSettings _stripePaymentSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;


        #endregion

        #region Ctor

        public PaymentStripeController(IGenericAttributeService genericAttributeService,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IPaymentPluginManager paymentPluginManager,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            ILogger logger,
            INotificationService notificationService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWebHelper webHelper,
            IWorkContext workContext,
            ShoppingCartSettings shoppingCartSettings,
            StripePaymentSettings stripePaymentSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _genericAttributeService = genericAttributeService;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _paymentPluginManager = paymentPluginManager;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _logger = logger;
            _notificationService = notificationService;
            _settingService = settingService;
            _storeContext = storeContext;
            _webHelper = webHelper;
            _workContext = workContext;
            _shoppingCartSettings = shoppingCartSettings;
            _stripePaymentSettings = stripePaymentSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            //prepare model
            ConfigurationModel model = new ConfigurationModel
            {
                SecretKey = _stripePaymentSettings.SecretKey,
                PublishableKey = _stripePaymentSettings.PublishableKey,
                AdditionalFee = _stripePaymentSettings.AdditionalFee,
                AdditionalFeePercentage = _stripePaymentSettings.AdditionalFeePercentage
            };

            return View("~/Plugins/Payments.Stripe/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();


            if (!ModelState.IsValid)
                return await Configure();

            //save settings

            _stripePaymentSettings.SecretKey = model.SecretKey;
            _stripePaymentSettings.PublishableKey = model.PublishableKey;
            _stripePaymentSettings.AdditionalFee = model.AdditionalFee;
            _stripePaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            await _settingService.SaveSettingAsync(_stripePaymentSettings);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        // This is your Stripe CLI webhook secret for testing your endpoint locally.

        const string endpointSecret = "whsec_8157644fd875bff8eae20647c5a3c91173bc78f19ecdc37868e8a1575dec2350";

        [HttpPost("WebhookRequest")]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(_httpContextAccessor.HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, _httpContextAccessor.HttpContext.Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                //Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        #endregion
    }
}