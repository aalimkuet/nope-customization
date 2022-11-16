using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Payments.Stripe.Services;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Services.Tax;
using Stripe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Stripe
{
    /// <summary>
    /// Stripe payment processor
    /// </summary>
    public class StripePaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAddressService _addressService;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderService _orderService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IProductService _productService;
        private readonly ISettingService _settingService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ITaxService _taxService;
        private readonly IWebHelper _webHelper;
        private readonly StripeHttpClient _StripeHttpClient;
        private readonly StripePaymentSettings _StripePaymentSettings;
        private readonly IPaymentService _paymentService;

        #endregion

        #region Ctor

        public StripePaymentProcessor(CurrencySettings currencySettings,
            IAddressService addressService,
            ICheckoutAttributeParser checkoutAttributeParser,
            ICountryService countryService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IOrderService orderService,
            IOrderTotalCalculationService orderTotalCalculationService,
            IProductService productService,
            ISettingService settingService,
            IStateProvinceService stateProvinceService,
            ITaxService taxService,
            IWebHelper webHelper,
            StripeHttpClient StripeHttpClient,
            StripePaymentSettings StripePaymentSettings,
            IPaymentService paymentService)
        {
            _currencySettings = currencySettings;
            _addressService = addressService;
            _checkoutAttributeParser = checkoutAttributeParser;
            _countryService = countryService;
            _currencyService = currencyService;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
            _orderService = orderService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _productService = productService;
            _settingService = settingService;
            _stateProvinceService = stateProvinceService;
            _taxService = taxService;
            _webHelper = webHelper;
            _StripeHttpClient = StripeHttpClient;
            _StripePaymentSettings = StripePaymentSettings;
            _paymentService = paymentService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Convert a NopCommere address to a Stripe API address
        /// </summary>
        /// <param name="nopAddress"></param>
        /// <returns></returns>
        private AddressOptions MapNopAddressToStripe(Core.Domain.Common.Address nopAddress)
        {
            var state = _stateProvinceService.GetStateProvinceByIdAsync(nopAddress?.StateProvinceId ?? 0).Result;
            var country = _countryService.GetCountryByIdAsync(nopAddress?.CountryId ?? 0).Result;
            return new AddressOptions
            {
                Line1 = nopAddress.Address1,
                City = nopAddress.City,
                State = state.Abbreviation,
                PostalCode = nopAddress.ZipPostalCode,
                Country = country.ThreeLetterIsoCode
            };
        }

        /// <summary>
        /// Set up for a call to the Stripe API
        /// </summary>
        /// <returns></returns>
        private RequestOptions GetStripeApiRequestOptions()
        {
            return new RequestOptions
            {
                ApiKey = _StripePaymentSettings.SecretKey,
                IdempotencyKey = Guid.NewGuid().ToString()
            };
        }

        /// <summary>
        /// Perform a shallow validation of a stripe token
        /// </summary>
        /// <param name="stripeTokenObj"></param>
        /// <returns></returns>
        private bool IsStripeTokenID(string token)
        {
            return token.StartsWith("tok_");
        }

        private bool IsChargeID(string chargeID)
        {
            return chargeID.StartsWith("ch_");
        }

        #endregion

        #region Methods


        public override string GetConfigurationPageUrl() => $"{_webHelper.GetStoreLocation()}Admin/PaymentStripe/Configure";

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return Task.FromResult(new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } });
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public Task<bool> CanRePostProcessPaymentAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //let's ensure that at least 5 seconds passed after order is placed
            //P.S. there's no any particular reason for that. we just do it
            if ((DateTime.UtcNow - order.CreatedOnUtc).TotalSeconds < 5)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            throw new NotImplementedException();
        }

        //public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        //{
        //    var result = _paymentService.CalculateAdditionalFee(cart, _stripePaymentSettings.AdditionalFee, _stripePaymentSettings.AdditionalFeePercentage);

        //    return result;
        //}

        public async Task<ProcessPaymentRequest> GetPaymentInfo(IFormCollection form)
        {
            var paymentRequest = new ProcessPaymentRequest();

            if (form.TryGetValue("stripeToken", out StringValues stripeToken) && !StringValues.IsNullOrEmpty(stripeToken))
                paymentRequest.CustomValues.Add(await _localizationService.GetResourceAsync("Plugins.Payments.Stripe.Fields.StripeToken.Key"), stripeToken.ToString());

            return await Task.FromResult(paymentRequest);
        }

        public string GetPublicViewComponentName()
        {
            return StripePaymentDefaults.ViewComponentName;
        }

        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - hide; false - display.
        /// </returns>
        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return Task.FromResult(false);
        }

        public Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            return Task.FromResult(new PostProcessPaymentRequest());
        }

        public async Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            //get customer
            var customer = await _customerService.GetCustomerByIdAsync(processPaymentRequest?.CustomerId ?? 0);
            if (customer == null)
                throw new NopException("Customer cannot be loaded");

            string tokenKey = await _localizationService.GetResourceAsync("Plugins.Payments.Stripe.Fields.StripeToken.Key");
            if (!processPaymentRequest.CustomValues.TryGetValue(tokenKey, out object stripeTokenObj) || !(stripeTokenObj is string) || !IsStripeTokenID((string)stripeTokenObj))
            {
                throw new NopException("Card token not received");
            }
            string stripeToken = stripeTokenObj.ToString();
            var service = new ChargeService();
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (long)(processPaymentRequest.OrderTotal * 100),
                Currency = "usd",
                Description = string.Format(StripePaymentDefaults.PaymentNote, processPaymentRequest.OrderGuid),
                Source = stripeToken
            };

            var address = _addressService.GetAddressByIdAsync(customer?.ShippingAddressId ?? 0).Result;
            if (customer != null)
            {
                chargeOptions.Shipping = new ChargeShippingOptions
                {
                    Address = MapNopAddressToStripe(address),
                    Phone = address.PhoneNumber,
                    Name = address.FirstName + ' ' + address.LastName
                };
            }

            var charge = service.Create(chargeOptions, GetStripeApiRequestOptions());

            var result = new ProcessPaymentResult();
            if (charge.Status == "succeeded")
            {
                result.NewPaymentStatus = PaymentStatus.Paid;
                result.AuthorizationTransactionId = charge.Id;
                result.AuthorizationTransactionResult = $"Transaction was processed by using {charge?.Source.Object}. Status is {charge.Status}";
                return await Task.FromResult(result);
            }
            else
            {
                throw new NopException($"Charge error: {charge.FailureMessage}");
            }
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the process payment result
        /// </returns>
        public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } });
        }

        /// <summary>
        /// Full or partial refund
        /// </summary>
        /// <param name="refundPaymentRequest"></param>
        /// <returns></returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            string chargeID = refundPaymentRequest.Order.AuthorizationTransactionId;
            var orderAmtRemaining = refundPaymentRequest.Order.OrderTotal - refundPaymentRequest.AmountToRefund;
            bool isPartialRefund = orderAmtRemaining > 0;

            if (!IsChargeID(chargeID))
            {
                throw new NopException($"Refund error: {chargeID} is not a Stripe Charge ID. Refund cancelled");
            }
            var service = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                Charge = chargeID,
                Amount = (long)(refundPaymentRequest.AmountToRefund * 100),
                Reason = RefundReasons.RequestedByCustomer
            };
            var refund = service.Create(refundOptions, GetStripeApiRequestOptions());

            RefundPaymentResult result = new RefundPaymentResult();

            switch (refund.Status)
            {
                case "succeeded":
                    result.NewPaymentStatus = isPartialRefund ? PaymentStatus.PartiallyRefunded : PaymentStatus.Refunded;
                    break;

                case "pending":
                    result.NewPaymentStatus = PaymentStatus.Pending;
                    result.AddError($"Refund failed with status of ${refund.Status}");
                    break;

                default:
                    throw new NopException("Refund returned a status of ${refund.Status}");
            }
            return result;
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            IList<string> errors = new List<string>();
            if (!(form.TryGetValue("stripeToken", out StringValues stripeToken) || stripeToken.Count != 1 || !IsStripeTokenID(stripeToken[0])))
            {
                errors.Add("Token was not supplied or invalid");
            }
            return errors;
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Install the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            //settings
            //settings
           await _settingService.SaveSettingAsync(new StripePaymentSettings
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false
            });


            //locales            
           await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Fields.SecretKey", "Secret key, live or test (starts with sk_)");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Fields.PublishableKey", "Publishable key, live or test (starts with pk_)");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Fields.AdditionalFee", "Additional fee");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Fields.AdditionalFee.Hint", "Enter additional fee to charge your customers.");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Fields.AdditionalFeePercentage", "Additional fee. Use percentage");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Fields.StripeToken.Key", "Stripe Token");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Fields.AdditionalFeePercentage.Hint", "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.");
           await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Stripe.Instructions", @"
                <p>
                     For plugin configuration follow these steps:<br />
                    <br />
                    1. If you haven't already, create an account on Stripe.com and sign in<br />
                    2. In the Developers menu (left), choose the API Keys option.
                    3. You will see two keys listed, a Publishable key and a Secret key. You will need both. (If you'd like, you can create and use a set of restricted keys. That topic isn't covered here.)
                    <em>Stripe supports test keys and production keys. Use whichever pair is appropraite. There's no switch between test/sandbox and proudction other than using the appropriate keys.</em>
                    4. Paste these keys into the configuration page of this plug-in. (Both keys are required.) 
                    <br />
                    <em>Note: If using production keys, the payment form will only work on sites hosted with HTTPS. (Test keys can be used on http sites.) If using test keys, 
                    use these <a href='https://stripe.com/docs/testing'>test card numbers from Stripe</a>.</em><br />
                </p>");

      
            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<StripePaymentSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Payments.Stripe");

            await base.UninstallAsync();
        }

        /// <summary>
        /// Gets a payment method description that will be displayed on checkout pages in the public store
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return await _localizationService.GetResourceAsync("Plugins.Payments.Stripe.PaymentMethodDescription");
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the additional handling fee
        /// </returns>
        public async Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            return await _orderTotalCalculationService.CalculatePaymentAdditionalFeeAsync(cart,
                _StripePaymentSettings.AdditionalFee, _StripePaymentSettings.AdditionalFeePercentage);
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the capture payment result
        /// </returns>
        public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
        {
            return Task.FromResult(new CapturePaymentResult { Errors = new[] { "Capture method not supported" } });
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
        {
            return Task.FromResult(new RefundPaymentResult { Errors = new[] { "Refund method not supported" } });
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            return Task.FromResult(new VoidPaymentResult { Errors = new[] { "Void method not supported" } });
        }

        /// <summary>
        /// Validate payment form
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of validating errors
        /// </returns>
        public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            return Task.FromResult<IList<string>>(new List<string>());
        }

        /// <summary>
        /// Get payment information
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the payment info holder
        /// </returns>
        public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            return Task.FromResult(new ProcessPaymentRequest());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture => false;

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund => false;

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund => false;

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid => false;

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo => false;

        #endregion
    }
}