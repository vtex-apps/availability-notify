using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AvailabilityNotify.Data;
using AvailabilityNotify.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vtex.Api.Context;

namespace AvailabilityNotify.Services
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly IVtexEnvironmentVariableProvider _environmentVariableProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IIOServiceContext _context;
        private readonly string _applicationName;


        public AvailabilityRepository(IVtexEnvironmentVariableProvider environmentVariableProvider, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory, IIOServiceContext context)
        {
            this._environmentVariableProvider = environmentVariableProvider ??
                                                throw new ArgumentNullException(nameof(environmentVariableProvider));

            this._httpContextAccessor = httpContextAccessor ??
                                        throw new ArgumentNullException(nameof(httpContextAccessor));

            this._clientFactory = clientFactory ??
                               throw new ArgumentNullException(nameof(clientFactory));

            this._context = context ??
                            throw new ArgumentNullException(nameof(context));

            this._applicationName =
                $"{this._environmentVariableProvider.ApplicationVendor}.{this._environmentVariableProvider.ApplicationName}";
        }

        public async Task<MerchantSettings> GetMerchantSettings()
        {
            // Load merchant settings
            // 'http://apps.${region}.vtex.io/${account}/${workspace}/apps/${vendor.appName}/settings'
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/apps/{Constants.APP_SETTINGS}/settings"),
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MerchantSettings>(responseContent);
        }

        public async Task SetMerchantSettings(MerchantSettings merchantSettings)
        {
            if (merchantSettings == null)
            {
                merchantSettings = new MerchantSettings();
            }

            var jsonSerializedMerchantSettings = JsonConvert.SerializeObject(merchantSettings);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/apps/{Constants.APP_SETTINGS}/settings"),
                Content = new StringContent(jsonSerializedMerchantSettings, Encoding.UTF8, Constants.APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
            }

            request.Headers.Add(Constants.AppKey, merchantSettings.AppKey);
            request.Headers.Add(Constants.AppToken, merchantSettings.AppToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsInitialized()
        {
            bool isInitialized = false;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/buckets/{this._applicationName}/{Constants.AppName}/files/initialized"),
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                if (responseContent.Equals("true"))
                {
                    isInitialized = true;
                }
            }

            return isInitialized;
        }

        public async Task SetInitialized()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/buckets/{this._applicationName}/{Constants.AppName}/files/initialized"),
                Content = new StringContent("true", Encoding.UTF8, "text/plain")
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> VerifySchema()
        {
            // https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/schemas/{{schema_name}}
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/schemas/{Constants.SCHEMA}")
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            _context.Vtex.Logger.Debug("VerifySchema", null, $"[{response.StatusCode}] {responseContent}");

            if (response.IsSuccessStatusCode && !responseContent.Equals(Constants.SCHEMA_JSON))
            {
                request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/schemas/{Constants.SCHEMA}"),
                    Content = new StringContent(Constants.SCHEMA_JSON, Encoding.UTF8, Constants.APPLICATION_JSON)
                };

                if (authToken != null)
                {
                    request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                    request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                    request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
                }

                response = await client.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();

                _context.Vtex.Logger.Debug("VerifySchema", null, $"Applying Schema [{response.StatusCode}] {responseContent}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SaveNotifyRequest(NotifyRequest notifyRequest, RequestContext requestContext)
        {
            // PATCH https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/documents

            var jsonSerializedListItems = JsonConvert.SerializeObject(notifyRequest);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://{requestContext.Account}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/documents"), //?_schema={Constants.SCHEMA}"),
                Content = new StringContent(jsonSerializedListItems, Encoding.UTF8, Constants.APPLICATION_JSON)
            };

            string authToken = requestContext.AuthToken;
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            _context.Vtex.Logger.Debug("SaveNotifyRequest", null, $"[{response.StatusCode}] '{responseContent}'\n{jsonSerializedListItems}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteNotifyRequest(string documentId)
        {
            // DELETE https://{accountName}.{environment}.com.br/api/dataentities/data_entity_name/documents/id

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/documents/{documentId}")
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }

        public async Task<NotifyRequest[]> ListNotifyRequests()
        {
            NotifyRequest[] notifyRequests = null;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/scroll?_fields={Constants.FIELDS}")
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                notifyRequests = JsonConvert.DeserializeObject<NotifyRequest[]>(responseContent);
            }
            
            return notifyRequests;
        }

        public async Task<NotifyRequest[]> ListUnsentNotifyRequests()
        {
            NotifyRequest[] notifyRequests = null;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/search?_fields={Constants.FIELDS}&_schema=notify&_where=notificationSend=false")
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                notifyRequests = JsonConvert.DeserializeObject<NotifyRequest[]>(responseContent);
            }
            
            return notifyRequests;
        }

        public async Task<NotifyRequest[]> ListRequestsForSkuId(string skuId, RequestContext requestContext)
        {
            NotifyRequest[] notifyRequests = null;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://{requestContext.Account}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/search?_fields={Constants.FIELDS}&_schema={Constants.SCHEMA}&notificationSend=false")
            };

            string authToken = requestContext.AuthToken;
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                notifyRequests = JsonConvert.DeserializeObject<NotifyRequest[]>(responseContent);
            }

            return notifyRequests;
        }
    }
}
