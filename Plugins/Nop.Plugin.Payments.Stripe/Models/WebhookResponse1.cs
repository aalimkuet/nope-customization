using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.Stripe.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Address
    //{
    //    public object City { get; set; }
    //    public string Country { get; set; }
    //    public object Line1 { get; set; }
    //    public object Line2 { get; set; }
    //    public object PostalCode { get; set; }
    //    public object State { get; set; }
    //}

    public class AutomaticTax
    {
        public bool? Enabled { get; set; }
        public object Status { get; set; }
    }

    public class CustomerDetails
    {
        public Address Address { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public object Phone { get; set; }
        public string Tax_exempt { get; set; }
        public List<object> Tax_ids { get; set; }
    }

    public class CustomText
    {
        public object shipping_address { get; set; }
        public object submit { get; set; }
    }

    public class ResponseData
    {
        [JsonProperty("object")]
        public ResponseObject ResponseObject { get; set; }
    }

    //public class Metadata
    //{
    //}

    public class ResponseObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("after_expiration")]
        public object After_expiration { get; set; }
        [JsonProperty("allow_promotion_codes")]
        public object Allow_promotion_codes { get; set; }
        [JsonProperty("amount_subtotal")]
        public int? Amount_subtotal { get; set; }
        [JsonProperty("amount_total")]
        public int? Amount_total { get; set; }
        [JsonProperty("automatic_tax")]
        public AutomaticTax Automatic_tax { get; set; }
        [JsonProperty("billing_address_collection")]
        public object Billing_address_collection { get; set; }
        [JsonProperty("cancel_url")]
        public string Cancel_url { get; set; }
        [JsonProperty("client_reference_id")]
        public string Client_reference_id { get; set; }
        [JsonProperty("consent")]
        public object Consent { get; set; }
        [JsonProperty("consent_collection")]
        public object Consent_collection { get; set; }
        [JsonProperty("created")]
        public int? Created { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("custom_text")]
        public CustomText Custom_text { get; set; }
        [JsonProperty("customer")]
        public object Customer { get; set; }
        [JsonProperty("customer_creation")]
        public string Customer_creation { get; set; }
        [JsonProperty("customer_details")]
        public CustomerDetails Customer_details { get; set; }
        [JsonProperty("customer_email")]
        public object Customer_email { get; set; }
        [JsonProperty("expires_at")]
        public int? Expires_at { get; set; }
        [JsonProperty("livemode")]
        public bool? Livemode { get; set; }
        [JsonProperty("locale")]
        public object Locale { get; set; }
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("payment_intent")]
        public string Payment_intent { get; set; }
        [JsonProperty("payment_link")]
        public object Payment_link { get; set; }
        [JsonProperty("payment_method_collection")]
        public string Payment_method_collection { get; set; }
        [JsonProperty("payment_method_options")]
        public PaymentMethodOptions Payment_method_options { get; set; }
        [JsonProperty("payment_method_types")]
        public List<string> Payment_method_types { get; set; }
        [JsonProperty("payment_status")]
        public string Payment_status { get; set; }
        [JsonProperty("phone_number_collection")]
        public PhoneNumberCollection Phone_number_collection { get; set; }
        [JsonProperty("recovered_from")]
        public object Recovered_from { get; set; }
        [JsonProperty("setup_intent")]
        public object Setup_intent { get; set; }
        [JsonProperty("shipping_address_collection")]
        public object Shipping_address_collection { get; set; }
        [JsonProperty("shipping_cost")]
        public object Shipping_cost { get; set; }
        [JsonProperty("shipping_details")]
        public object Shipping_details { get; set; }
        [JsonProperty("shipping_options")]
        public List<object> Shipping_options { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("submit_type")]
        public object Submit_type { get; set; }
        [JsonProperty("subscription")]
        public object Subscription { get; set; }
        [JsonProperty("success_url")]
        public string Success_url { get; set; }
        [JsonProperty("total_details")]
        public TotalDetails Total_details { get; set; }
        [JsonProperty("url")]
        public object Url { get; set; }
    }

    //public class PaymentMethodOptions
    //{
    //}

    public class PhoneNumberCollection
    {
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
    }

    //public class Request
    //{
    //    [JsonProperty("id")]
    //    public object Id { get; set; }
    //    [JsonProperty("idempotency_key")]
    //    public object Idempotency_key { get; set; }
    //}

    public class WebhookResponse1
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }
        [JsonProperty("created")]
        public int? Created { get; set; }
        [JsonProperty("Data")]
        public ResponseData ResponseData { get; set; }
        [JsonProperty("livemode")]
        public bool? Livemode { get; set; }
        [JsonProperty("pending_webhooks")]
        public int? Pending_webhooks { get; set; }
        [JsonProperty("request")]
        public Request Request { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class TotalDetails
    {
        [JsonProperty("amount_discount")]
        public int? Amount_discount { get; set; }
        [JsonProperty("amount_shipping")]
        public int? Amount_shipping { get; set; }
        [JsonProperty("amount_tax")]
        public int? Amount_tax { get; set; }
    }

}
