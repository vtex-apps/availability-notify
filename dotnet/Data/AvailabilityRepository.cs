using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AvailabilityNotify.Data;
using AvailabilityNotify.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vtex.Api.Context;
using System.Net;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;
using System.Security.Principal;

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
            MerchantSettings merchantSettings = new MerchantSettings();
            string url = $"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/apps/{Constants.APP_SETTINGS}/settings";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Get);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("GetMerchantSettings", null, $"Failed to get merchant settings '{responseWrapper.Message}' ");
            }
            else
            {
                merchantSettings = JsonConvert.DeserializeObject<MerchantSettings>(responseWrapper.ResponseText);
            }

            return merchantSettings;
        }

        public async Task SetMerchantSettings(MerchantSettings merchantSettings)
        {
            if (merchantSettings == null)
            {
                merchantSettings = new MerchantSettings();
            }

            string url = $"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/apps/{Constants.APP_SETTINGS}/settings";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Put, merchantSettings);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("SetMerchantSettings", null, $"Failed to set merchant settings '{responseWrapper.Message}' ");
            }
        }

        public async Task<bool> VerifySchema()
        {
            // https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/schemas/{{schema_name}}

            string url = $"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/schemas/{Constants.SCHEMA}";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Get);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("VerifySchema", null, $"Failed to get schema '{responseWrapper.Message}' ");
            }
            else if (!responseWrapper.ResponseText.Equals(Constants.SCHEMA_JSON))
            {
                url = $"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/schemas/{Constants.SCHEMA}";
                responseWrapper = await this.SendRequest(url, HttpMethod.Put, null, null, null, Constants.SCHEMA_JSON);
                _context.Vtex.Logger.Debug("VerifySchema", null, $"Applying Schema [{responseWrapper.IsSuccess}] '{responseWrapper.ResponseText}' ");
            }

            return responseWrapper.IsSuccess;
        }

        public async Task<bool> SaveNotifyRequest(NotifyRequest notifyRequest, RequestContext requestContext)
        {
            // PATCH https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/documents

            string url = $"http://{requestContext.Account}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/documents";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Put, notifyRequest);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("SaveNotifyRequest", null, $"Failed to save '{JsonConvert.SerializeObject(notifyRequest)}' ");
            }

            return responseWrapper.IsSuccess;
        }

        public async Task<bool> DeleteNotifyRequest(string documentId)
        {
            // DELETE https://{accountName}.{environment}.com.br/api/dataentities/data_entity_name/documents/id

            string url = $"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/documents/{documentId}";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Delete);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("DeleteNotifyRequest", null, $"Failed to delete '{documentId}' ");
            }

            return responseWrapper.IsSuccess;
        }

        public async Task<NotifyRequest[]> ListNotifyRequests()
        {
            NotifyRequest[] notifyRequests = new NotifyRequest[0];
            RequestContext requestContext = new RequestContext
            {
                Account = _context.Vtex.Account,
                AuthToken = _context.Vtex.AuthToken
            };

            try
            {
                notifyRequests = await this.ScrollRequests(requestContext);
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("ListNotifyRequests", null, null, ex);
            }

            return notifyRequests;
        }

        public async Task<NotifyRequest[]> ListUnsentNotifyRequests()
        {
            NotifyRequest[] notifyRequests = new NotifyRequest[0];
            RequestContext requestContext = new RequestContext
            {
                Account = _context.Vtex.Account,
                AuthToken = _context.Vtex.AuthToken
            };

            try
            {
                notifyRequests = await this.SearchRequests(requestContext, "notificationSend=false");
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("ListUnsentNotifyRequests", null, null, ex);
            }

            return notifyRequests;
        }

        public async Task<NotifyRequest[]> ListRequestsForSkuId(string skuId, RequestContext requestContext)
        {
            NotifyRequest[] notifyRequests = new NotifyRequest[0];

            try
            {
                notifyRequests = await this.SearchRequests(requestContext, $"notificationSend=false&skuId={skuId}");
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("ListRequestsForSkuId", null, $"Sku '{skuId}' ", ex);
            }

            return notifyRequests;
        }

        public async Task<NotifyRequest[]> SearchRequests(RequestContext requestContext, string searchString, int? searchFrom = null)
        {
            List<NotifyRequest> notifyRequestsAll = new List<NotifyRequest>();
            if (searchFrom == null)
            {
                searchFrom = 0;
            }

            int searchTo = (searchFrom ?? 0) + 99;

            string url = $"http://{requestContext.Account}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/search?_fields={Constants.FIELDS}&_schema={Constants.SCHEMA}&{searchString}";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Get, null, searchFrom.ToString(), searchTo.ToString());
            if (responseWrapper.IsSuccess)
            {
                NotifyRequest[] notifyRequests = JsonConvert.DeserializeObject<NotifyRequest[]>(responseWrapper.ResponseText);
                notifyRequestsAll.AddRange(notifyRequests);
                int from = 0;
                int to = 0;
                int total = 0;
                if (!string.IsNullOrEmpty(responseWrapper.To) && !string.IsNullOrEmpty(responseWrapper.From) && int.TryParse(responseWrapper.To, out to) && int.TryParse(responseWrapper.From, out from) && int.TryParse(responseWrapper.Total, out total) && to < total)
                {
                    int newFrom = to + 1;
                    notifyRequests = await this.SearchRequests(requestContext, searchString, newFrom);
                    notifyRequestsAll.AddRange(notifyRequests);
                }
            }

            return notifyRequestsAll.ToArray();
        }

        public async Task<NotifyRequest[]> ScrollRequests(RequestContext requestContext)
        {
            List<NotifyRequest> notifyRequestsAll = new List<NotifyRequest>();
            string url = $"http://{requestContext.Account}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/scroll?_fields={Constants.FIELDS}";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Get);
            if (responseWrapper.IsSuccess)
            {
                NotifyRequest[] notifyRequests = JsonConvert.DeserializeObject<NotifyRequest[]>(responseWrapper.ResponseText);
                notifyRequestsAll.AddRange(notifyRequests);
                int returnedRecords = notifyRequests.Length;
                while (!string.IsNullOrEmpty(responseWrapper.MasterDataToken) && returnedRecords > 0)
                {
                    url = $"http://{requestContext.Account}.vtexcommercestable.com.br/api/dataentities/{Constants.DATA_ENTITY}/scroll?_token={responseWrapper.MasterDataToken}";
                    responseWrapper = await this.SendRequest(url, HttpMethod.Get);
                    if (responseWrapper.IsSuccess)
                    {
                        notifyRequests = JsonConvert.DeserializeObject<NotifyRequest[]>(responseWrapper.ResponseText);
                        returnedRecords = notifyRequests.Length;
                        if (returnedRecords > 0)
                        {
                            notifyRequestsAll.AddRange(notifyRequests);
                        }
                    }
                    else
                    {
                        _context.Vtex.Logger.Error("ScrollRequests", null, responseWrapper.ResponseText);
                    }
                }
            }

            return notifyRequestsAll.ToArray();
        }

        public async Task SetImportLock(DateTime importStartTime, string sku)
        {
            var processingLock = new Lock
            {
                ProcessingStarted = importStartTime,
            };

            string url = BuildLockMasterDataUrl(sku);
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Put, processingLock);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("SetImportLock", null, responseWrapper.Message);
            }
        }
        public async Task ClearImportLock(string sku)
        {
            var processingLock = new Lock
            {
                ProcessingStarted = new DateTime(),
            };

            string url = BuildLockMasterDataUrl(sku);
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Put, processingLock);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("ClearImportLock", null, $"Failed to clear lock. {responseWrapper.Message} Sku: {sku}");
            }
        }


        public async Task<DateTime> CheckImportLock(string sku)
        {
            Lock processingLock = new Lock
            {
                ProcessingStarted = new DateTime()
            };
            string url = BuildLockMasterDataUrl(sku);
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Get);

            if (responseWrapper.IsSuccess)
            {
                processingLock = JsonConvert.DeserializeObject<Lock>(responseWrapper.ResponseText);
            }

            return processingLock.ProcessingStarted;
        }

        private string BuildLockMasterDataUrl(string sku)
        {
            var account = _httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME];
            var workspace = _httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE];
            string url = $"http://{account}.vtexcommercestable.com.br/api/dataentities/{Constants.LOCK}-{workspace}/documents/{sku}";
            return url;
        }

        public async Task<DateTime> GetLastUnsentCheck()
        {
            DateTime lastCheck = DateTime.Now;
            string url = $"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/buckets/{this._applicationName}/{Constants.BUCKET}/files/{Constants.UNSENT_CHECK}";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Get);
            if (responseWrapper.IsSuccess)
            {
                if (!string.IsNullOrEmpty(responseWrapper.ResponseText))
                {
                    lastCheck = JsonConvert.DeserializeObject<DateTime>(responseWrapper.ResponseText);
                }
                else
                {
                    await this.SetLastUnsentCheck(lastCheck);
                }
            }
            else
            {
                await this.SetLastUnsentCheck(lastCheck.AddDays(-7));
            }

            return lastCheck;
        }

        public async Task SetLastUnsentCheck(DateTime lastCheck)
        {
            string url = $"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_WORKSPACE]}/buckets/{this._applicationName}/{Constants.BUCKET}/files/{Constants.UNSENT_CHECK}";
            ResponseWrapper responseWrapper = await this.SendRequest(url, HttpMethod.Put, lastCheck);
            if (!responseWrapper.IsSuccess)
            {
                _context.Vtex.Logger.Error("SetLastUnsentCheck", null, $"Failed to set last check. {responseWrapper.Message}");
            }
        }

        public async Task<ResponseWrapper> SendRequest(string url, HttpMethod httpMethod, object requestObject = null, string from = null, string to = null, string jsonSerializedData = null)
        {
            ResponseWrapper responseWrapper = null;
            string jsonSerializedRequest = string.Empty;

            var request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(url)
            };

            if (requestObject != null)
            {
                try
                {
                    jsonSerializedRequest = JsonConvert.SerializeObject(requestObject);
                    request.Content = new StringContent(jsonSerializedRequest, Encoding.UTF8, Constants.APPLICATION_JSON);
                }
                catch (Exception ex)
                {
                    _context.Vtex.Logger.Error("SendRequest", null, $"Error Serializing Request Object", ex);
                }
            }
            else if (!string.IsNullOrEmpty(jsonSerializedData))
            {
                request.Content = new StringContent(jsonSerializedData, Encoding.UTF8, Constants.APPLICATION_JSON);
                jsonSerializedRequest = jsonSerializedData;  // for logging
            }

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                request.Headers.Add("REST-Range", $"resources={from}-{to}");
            }

            request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
            string authToken = _context.Vtex.AuthToken;
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();

            try
            {
                HttpResponseMessage responseMessage = await client.SendAsync(request);
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                responseWrapper = new ResponseWrapper
                {
                    IsSuccess = responseMessage.IsSuccessStatusCode,
                    ResponseText = responseContent
                };

                if (!responseWrapper.IsSuccess)
                {
                    _context.Vtex.Logger.Warn("SendRequest", null, $"Problem Sending Request '{request.RequestUri}'.\n'{responseWrapper.ResponseText}' {jsonSerializedRequest}");
                }

                HttpHeaders headers = responseMessage.Headers;
                IEnumerable<string> values;
                if (headers.TryGetValues("REST-Content-Range", out values))
                {
                    // resources 0-10/168
                    string resources = values.First();
                    string[] split = resources.Split(' ');
                    string ranges = split[1];
                    string[] splitRanges = ranges.Split('/');
                    string fromTo = splitRanges[0];
                    string total = splitRanges[1];
                    string[] splitFromTo = fromTo.Split('-');
                    string responseFrom = splitFromTo[0];
                    string responseTo = splitFromTo[1];

                    responseWrapper.Total = total;
                    responseWrapper.From = responseFrom;
                    responseWrapper.To = responseTo;

                    // _context.Vtex.Logger.Debug("SendRequest", "REST-Content-Range", resources);
                }

                if (headers.TryGetValues("X-VTEX-MD-TOKEN", out values))
                {
                    string token = values.First();
                    responseWrapper.MasterDataToken = token;
                }
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("SendRequest", null, $"Error Sending Request to {request.RequestUri}\n{jsonSerializedRequest}", ex);
                responseWrapper = new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

            return responseWrapper;
        }
    }
}
