using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Data
{
    public class Constants
    {
        public const string AppToken = "X-Vtex-Api-AppToken";
        public const string AppKey = "X-Vtex-Api-AppKey";
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

        public const string BUCKET = "availability-notify";

        public const string DATA_ENTITY = "notify";
        public const string SCHEMA = "notify";
        public const string SCHEMA_JSON = "{\"name\": \"notify\", \"properties\": {\"skuId\": {\"type\": \"string\", \"title\": \"skuId\"}, \"sendAt\": {\"type\": \"string\", \"title\": \"sendAt\"}, \"name\": {\"type\": \"string\", \"title\": \"name\"}, \"email\": {\"type\": \"string\", \"title\": \"email\"}, \"createdAt\": {\"type\": \"string\", \"title\": \"createdAt\"}, \"notificationSend\": {\"type\": \"string\", \"title\": \"notificationSend\"}}, \"v-indexed\": [\"skuId\", \"notificationSend\"], \"v-security\": {\"allowGetAll\": true}}";
        public const string FIELDS = "id,email,skuId,notificationSend,sendAt,name,createdAt";

        public const string MAIL_SERVICE = "http://mailservice.vtex.com.br/api/mail-service/pvt/sendmail";
        public const string ACQUIERER = "AvailabilityNotify";

        public const string GITHUB_URL = "https://raw.githubusercontent.com";
        public const string RESPOSITORY = "vtex-apps/availability-notify/master";
        public const string TEMPLATE_FOLDER = "templates";
        public const string TEMPLATE_FILE_EXTENSION = "json";
    }
}
