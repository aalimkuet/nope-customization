using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.Stripe
{
    /// <summary>
    /// Represents settings of the Stripe Standard payment plugin
    /// </summary>
    public class StripePaymentSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use sandbox (testing environment)
        /// </summary>
        public bool UseSandbox { get; set; }

        /// <summary>
        /// Gets or sets a business email
        /// </summary>
        public string BusinessEmail { get; set; }

        /// <summary>
        /// Gets or sets PDT identity token
        /// </summary>
        public string PdtToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to pass info about purchased items to Stripe
        /// </summary>
        public bool PassProductNamesAndTotals { get; set; }

        /// <summary>
        /// Gets or sets an additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }

        // <summary>
        /// Gets or sets OAuth2 application identifier
        /// </summary>
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }

        
    }
}
