using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.Stripe.Models;
using Nop.Services.Common;
using Nop.Services.Localization;

namespace Nop.Plugin.Payments.Stripe.Components
{
    [ViewComponent(Name = StripePaymentDefaults.ViewComponentName)]
    public class PaymentStripeViewComponent : ViewComponent
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IAddressService _addressService;

        //private readonly StripePaymentManager _stripePaymentManager;

        #endregion

        #region Ctor

        public PaymentStripeViewComponent(IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IWorkContext workContext, IAddressService addressService)
        {
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _workContext = workContext;
            _addressService = addressService;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            var customer = _workContext.GetCurrentCustomerAsync().Result;
            var address = _addressService.GetAddressByIdAsync((int)customer.BillingAddressId).Result;
            PaymentInfoModel model = new PaymentInfoModel
            {
                //whether current customer is guest

                IsGuest = customer.IsSystemAccount,
                PostalCode = address?.ZipPostalCode ?? address?.ZipPostalCode,
            };

            return View("~/Plugins/Payments.Stripe/Views/PaymentInfo.cshtml", model);
        }

        #endregion
    }
}