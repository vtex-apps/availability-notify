using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Data
{
    public class Constants
    {
        public const string AppToken = "X-VTEX-API-AppToken";
        public const string AppKey = "X-VTEX-API-AppKey";
        public const string EndPointKey = "availability-notify";
        public const string AppName = "availability-notify";

        public const string FORWARDED_HEADER = "X-Forwarded-For";
        public const string FORWARDED_HOST = "X-Forwarded-Host";
        public const string APPLICATION_JSON = "application/json";
        public const string HEADER_VTEX_CREDENTIAL = "X-Vtex-Credential";
        public const string AUTHORIZATION_HEADER_NAME = "Authorization";
        public const string PROXY_AUTHORIZATION_HEADER_NAME = "Proxy-Authorization";
        public const string USE_HTTPS_HEADER_NAME = "X-Vtex-Use-Https";
        public const string PROXY_TO_HEADER_NAME = "X-Vtex-Proxy-To";
        public const string VTEX_ACCOUNT_HEADER_NAME = "X-Vtex-Account";
        public const string ENVIRONMENT = "vtexcommercestable";
        public const string LOCAL_ENVIRONMENT = "myvtex";
        public const string VTEX_ID_HEADER_NAME = "VtexIdclientAutCookie";
        public const string HEADER_VTEX_WORKSPACE = "X-Vtex-Workspace";
        public const string APP_SETTINGS = "vtex.availability-notify";
        public const string ACCEPT = "Accept";
        public const string CONTENT_TYPE = "Content-Type";
        public const string MINICART = "application/vnd.vtex.checkout.minicart.v1+json";
        public const string HTTP_FORWARDED_HEADER = "HTTP_X_FORWARDED_FOR";
        public const string API_VERSION_HEADER = "'x-api-version";

        public const string VTEX_USER_AGENT_KEY = "X-Vtex-user-agent";

        public const string BUCKET = "availability-notify";
        public const string LOCK = "availability-notify-lock";
        public const string UNSENT_CHECK = "check-unsent";

        public const string DATA_ENTITY = "notify";
        public const string SCHEMA = "notify";
        public const string SCHEMA_JSON = "{\"name\":\"notify\",\"properties\":{\"skuId\":{\"type\":\"string\",\"title\":\"skuId\"},\"sendAt\":{\"type\":\"string\",\"title\":\"sendAt\"},\"name\":{\"type\":\"string\",\"title\":\"name\"},\"email\":{\"type\":\"string\",\"title\":\"email\"},\"createdAt\":{\"type\":\"string\",\"title\":\"createdAt\"},\"notificationSend\":{\"type\":\"string\",\"title\":\"notificationSend\"},\"locale\":{\"type\":\"string\",\"title\":\"locale\"},\"seller\":{\"type\":\"object\",\"title\":\"seller\"}},\"v-indexed\":[\"skuId\",\"notificationSend\"],\"v-security\":{\"allowGetAll\":true}}";
        public const string FIELDS = "id,email,skuId,notificationSend,sendAt,name,createdAt,locale,seller";

        public const string MAIL_SERVICE = "http://mailservice.vtex.com.br/api/mail-service/pvt/sendmail";
        public const string ACQUIERER = "AvailabilityNotify";

        public const string GITHUB_URL = "https://raw.githubusercontent.com";
        public const string RESPOSITORY = "vtex-apps/availability-notify/master";
        public const string TEMPLATE_FOLDER = "templates";
        public const string TEMPLATE_FILE_EXTENSION = "json";
        public const string DEFAULT_TEMPLATE_NAME = "back-in-stock";

        public class Availability
        {
            public const string CannotBeDelivered = "cannotBeDelivered";
            public const string Available = "available";
        }

        public static class Domain
        {
            public const string Fulfillment = "Fulfillment";
            public const string Marketplace = "Marketplace";
        }

        public static class VtexOrderStatus
        {
            public const string OrderCreated = "order-created";
            public const string OrderCompleted = "order-completed";
            public const string OnOrderCompleted = "on-order-completed";
            public const string PaymentPending = "payment-pending";
            public const string WaitingForOrderAuthorization = "waiting-for-order-authorization";
            public const string ApprovePayment = "approve-payment";
            public const string PaymentApproved = "payment-approved";
            public const string PaymentDenied = "payment-denied";
            public const string RequestCancel = "request-cancel";
            public const string WaitingForSellerDecision = "waiting-for-seller-decision";
            public const string AuthorizeFullfilment = "authorize-fulfillment";
            public const string OrderCreateError = "order-create-error";
            public const string OrderCreationError = "order-creation-error";
            public const string WindowToCancel = "window-to-cancel";
            public const string ReadyForHandling = "ready-for-handling";
            public const string StartHanding = "start-handling";
            public const string Handling = "handling";
            public const string InvoiceAfterCancellationDeny = "invoice-after-cancellation-deny";
            public const string OrderAccepted = "order-accepted";
            public const string Invoice = "invoice";
            public const string Invoiced = "invoiced";
            public const string Replaced = "replaced";
            public const string CancellationRequested = "cancellation-requested";
            public const string Cancel = "cancel";
            public const string Canceled = "canceled";
            public const string Cancelled = "cancelled";
        }
    }
}
