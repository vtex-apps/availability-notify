using AvailabilityNotify.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vtex.Api.Context;
using System.Linq;
using AvailabilityNotify.Models;
using System.Net;

namespace AvailabilityNotify.Services
{
    public class VtexAPIService : IVtexAPIService
    {
        private readonly IIOServiceContext _context;
        private readonly IVtexEnvironmentVariableProvider _environmentVariableProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly string _applicationName;

        public VtexAPIService(IIOServiceContext context, IVtexEnvironmentVariableProvider environmentVariableProvider, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory, IAvailabilityRepository availabilityRepository)
        {
            this._context = context ??
                            throw new ArgumentNullException(nameof(context));

            this._environmentVariableProvider = environmentVariableProvider ??
                                                throw new ArgumentNullException(nameof(environmentVariableProvider));

            this._httpContextAccessor = httpContextAccessor ??
                                        throw new ArgumentNullException(nameof(httpContextAccessor));

            this._clientFactory = clientFactory ??
                               throw new ArgumentNullException(nameof(clientFactory));

            this._availabilityRepository = availabilityRepository ??
                               throw new ArgumentNullException(nameof(availabilityRepository));

            this._applicationName =
                $"{this._environmentVariableProvider.ApplicationVendor}.{this._environmentVariableProvider.ApplicationName}";

            this.VerifySchema();
            this.CreateDefaultTemplate();
        }

        //public async Task GetShopperToNotifyBySku(string sku)
        //{
        //    try
        //    {
        //        var request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/dataentities/{Constants.DATA_ENTITY}/search?skuId={sku}")
        //        };

        //        request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
        //        string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
        //        if (authToken != null)
        //        {
        //            request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
        //            request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
        //            request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
        //        }

        //        var client = _clientFactory.CreateClient();
        //        var response = await client.SendAsync(request);
        //        string responseContent = await response.Content.ReadAsStringAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Vtex.Logger.Error("GetShopperToNotifyBySku", null, $"Error getting shoppers for sku '{sku}'", ex);
        //    }
        //}

        public async Task<InventoryBySku> ListInventoryBySku(string sku, RequestContext requestContext)
        {
            // GET https://{accountName}.{environment}.com.br/api/logistics/pvt/inventory/skus/skuId
        
            InventoryBySku inventoryBySku = new InventoryBySku();

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{requestContext.Account}.{Constants.ENVIRONMENT}.com.br/api/logistics/pvt/inventory/skus/{sku}")
                };

                request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
                string authToken = requestContext.AuthToken;
                if (authToken != null)
                {
                    request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                    request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                    request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
                }

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    inventoryBySku = JsonConvert.DeserializeObject<InventoryBySku>(responseContent);
                    _context.Vtex.Logger.Debug("ListInventoryBySku", null, $"Sku '{sku}' {responseContent}");
                }
                else
                {
                    _context.Vtex.Logger.Debug("ListInventoryBySku", null, $"Sku '{sku}' [{response.StatusCode}]");
                }
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("ListInventoryBySku", null, $"Error getting inventory for sku '{sku}'", ex);
            }

            return inventoryBySku;
        }

        public async Task<long> GetTotalAvailableForSku(string sku, RequestContext requestContext)
        {
            long totalAvailable = 0;
            InventoryBySku inventoryBySku = await this.ListInventoryBySku(sku, requestContext);
            if(inventoryBySku != null && inventoryBySku.Balance != null)
            {
                try
                {
                    long totalQuantity = inventoryBySku.Balance.Where(i => !i.HasUnlimitedQuantity).Sum(i => i.TotalQuantity);
                    long totalReseved = inventoryBySku.Balance.Where(i => !i.HasUnlimitedQuantity).Sum(i => i.ReservedQuantity);
                    totalAvailable = totalQuantity - totalReseved;
                    _context.Vtex.Logger.Debug("GetTotalAvailableForSku", null, $"Sku '{sku}' {totalQuantity} - {totalReseved} = {totalAvailable}");
                }
                catch(Exception ex)
                {
                    _context.Vtex.Logger.Error("GetTotalAvailableForSku", null, $"Error calculating total available for sku '{sku}' '{JsonConvert.SerializeObject(inventoryBySku)}'", ex);
                }
            }

            return totalAvailable;
        }

        public async Task<bool> CreateOrUpdateTemplate(string jsonSerializedTemplate)
        {
            // POST: "http://hostname/api/template-render/pvt/templates"

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/template-render/pvt/templates"),
                Content = new StringContent(jsonSerializedTemplate, Encoding.UTF8, Constants.APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
            //MerchantSettings merchantSettings = await _availabilityRepository.GetMerchantSettings();
            //request.Headers.Add(Constants.AppKey, merchantSettings.AppKey);
            //request.Headers.Add(Constants.AppToken, merchantSettings.AppToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            _context.Vtex.Logger.Info("Create Template", null, $"[{response.StatusCode}] {responseContent}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateOrUpdateTemplate(EmailTemplate template)
        {
            string jsonSerializedTemplate = JsonConvert.SerializeObject(template);
            if(string.IsNullOrEmpty(jsonSerializedTemplate))
            {
                return false;
            }
            else
            {
                return await this.CreateOrUpdateTemplate(jsonSerializedTemplate);
            }
        }

        public async Task<bool> TemplateExists(string templateName)
        {
            // POST: "http://hostname/api/template-render/pvt/templates"

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.myvtex.com/api/template-render/pvt/templates/{templateName}")
            };

            //request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
            }

            //MerchantSettings merchantSettings = await _availabilityRepository.GetMerchantSettings();
            //string appKey = merchantSettings.AppKey;
            //string appToken = merchantSettings.AppToken;
            //request.Headers.Add(Constants.AppKey, appKey);
            //request.Headers.Add(Constants.AppToken, appToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return (int)response.StatusCode == StatusCodes.Status200OK;
        }

        public async Task<string> GetDefaultTemplate(string templateName)
        {
            string templateBody = string.Empty;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{Constants.GITHUB_URL}/{Constants.RESPOSITORY}/{Constants.TEMPLATE_FOLDER}/{templateName}.{Constants.TEMPLATE_FILE_EXTENSION}")
            };

            request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
            string authToken = _context.Vtex.AuthToken;
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                //request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
                //request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            _context.Vtex.Logger.Debug("GetDefaultTemplate", "Response", $"[{response.StatusCode}] {responseContent}");
            if (response.IsSuccessStatusCode)
            {
                templateBody = responseContent;
            }
            else
            {
                _context.Vtex.Logger.Info("GetDefaultTemplate", "Response", $"[{response.StatusCode}] {responseContent} [{Constants.GITHUB_URL}/{Constants.RESPOSITORY}/{Constants.TEMPLATE_FOLDER}/{templateName}.{Constants.TEMPLATE_FILE_EXTENSION}]");
            }    

            return templateBody;
        }

        public async Task<bool> VerifySchema()
        {
            return await _availabilityRepository.VerifySchema();
        }

        public async Task<bool> CreateDefaultTemplate()
        {
            bool templateExists = false;
            string templateName = Constants.DEFAULT_TEMPLATE_NAME;
            string subjectText = string.Empty;

            templateExists = await this.TemplateExists(templateName);
            if (!templateExists)
            {
                string templateBody = await this.GetDefaultTemplate(templateName);
                if (string.IsNullOrWhiteSpace(templateBody))
                {
                    _context.Vtex.Logger.Warn("SendEmail", "Create Template", $"Failed to Load Template {templateName}");
                }
                else
                {
                    EmailTemplate emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(templateBody);
                    emailTemplate.Templates.Email.Message = emailTemplate.Templates.Email.Message.Replace(@"\r", string.Empty);
                    emailTemplate.Templates.Email.Message = emailTemplate.Templates.Email.Message.Replace(@"\n", "\n");
                    templateExists = await this.CreateOrUpdateTemplate(emailTemplate);
                }
            }

            return templateExists;
        }

        public async Task<GetSkuContextResponse> GetSkuContext(string skuId, RequestContext requestContext)
        {
            // GET https://{accountName}.{environment}.com.br/api/catalog_system/pvt/sku/stockkeepingunitbyid/skuId

            GetSkuContextResponse getSkuContextResponse = null;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{requestContext.Account}.{Constants.ENVIRONMENT}.com.br/api/catalog_system/pvt/sku/stockkeepingunitbyid/{skuId}")
                };

                request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
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
                if (response.IsSuccessStatusCode)
                {
                    getSkuContextResponse = JsonConvert.DeserializeObject<GetSkuContextResponse>(responseContent);
                }
                else
                {
                    _context.Vtex.Logger.Warn("GetSkuContext", null, $"Could not get sku for id '{skuId}' [{response.StatusCode}]");
                }
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("GetSkuContext", null, $"Error getting sku for id '{skuId}'", ex);
            }

            return getSkuContextResponse;
        }

        public async Task<bool> SendEmail(NotifyRequest notifyRequest, GetSkuContextResponse skuContext, RequestContext requestContext)
        {
            bool success = false;

            string responseText = string.Empty;
            string templateName = Constants.DEFAULT_TEMPLATE_NAME;
            
            EmailMessage emailMessage = new EmailMessage
            {
                TemplateName = templateName,
                ProviderName = requestContext.Account,
                JsonData = new JsonData
                {
                    SkuContext = skuContext,
                    NotifyRequest = notifyRequest
                }
            };
            
            string accountName = requestContext.Account;
            string message = JsonConvert.SerializeObject(emailMessage);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{Constants.MAIL_SERVICE}?an={accountName}"),
                Content = new StringContent(message, Encoding.UTF8, Constants.APPLICATION_JSON)
            };
            
            request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
            string authToken = requestContext.AuthToken;
            if (authToken != null)
            {
                request.Headers.Add(Constants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(Constants.VTEX_ID_HEADER_NAME, authToken);
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }
            
            HttpClient client = _clientFactory.CreateClient();
            try
            {
                HttpResponseMessage responseMessage = await client.SendAsync(request);
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                //responseText = $"[-] SendEmail [{responseMessage.StatusCode}] {responseContent}";
                _context.Vtex.Logger.Debug("SendEmail", null, $"{message}\n[{responseMessage.StatusCode}]\n{responseContent}");
                success = responseMessage.IsSuccessStatusCode;
                if (responseMessage.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    _context.Vtex.Logger.Error("SendEmail", null, $"Template {templateName} not found.");
                }
            }
            catch (Exception ex)
            {
                //responseText = $"[-] SendEmail Failure [{ex.Message}]";
                _context.Vtex.Logger.Error("SendEmail", null, $"Failure sending {message}", ex);
                success = false;  //jic
            }
            
            return success;
        }

        public async Task<bool> AvailabilitySubscribe(string email, string sku, string name, string locale, SellerObj seller)
        {
            bool success = false;
            RequestContext requestContext = new RequestContext
            {
                Account = _context.Vtex.Account,
                AuthToken = _context.Vtex.AuthToken
            };

            //await _availabilityRepository.VerifySchema();

            NotifyRequest notifyRequest = new NotifyRequest
            {
                RequestedAt = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                Email = email,
                SkuId = sku,
                Name = name,
                NotificationSent = "false",
                Locale = locale,
                Seller = seller
            };

            success = await _availabilityRepository.SaveNotifyRequest(notifyRequest, requestContext);

            return success;
        }

        public async Task<bool> ProcessNotification(AffiliateNotification notification)
        {
            bool success = true;
            RequestContext requestContext = new RequestContext
            {
                Account = _context.Vtex.Account,
                AuthToken = _context.Vtex.AuthToken
            };

            bool isActive = notification.IsActive;
            bool inventoryUpdated = notification.StockModified;
            string skuId = notification.IdSku;
            _context.Vtex.Logger.Debug("ProcessNotification", "AffiliateNotification", $"Sku:{skuId} Active?{isActive} Inventory Changed?{inventoryUpdated}");
            success = await this.ProcessNotification(requestContext, isActive, inventoryUpdated, skuId);

            return success;
        }

        public async Task<bool> ProcessNotification(BroadcastNotification notification)
        {
            bool success = false;
            RequestContext requestContext = new RequestContext
            {
                Account = _context.Vtex.Account,
                AuthToken = _context.Vtex.AuthToken
            };

            bool isActive = notification.IsActive;
            bool inventoryUpdated = notification.StockModified;
            string skuId = notification.IdSku;
            _context.Vtex.Logger.Debug("ProcessNotification", "BroadcastNotification", $"Sku:{skuId} Active?{isActive} Inventory Changed?{inventoryUpdated}");
            success = await this.ProcessNotification(requestContext, isActive, inventoryUpdated, skuId);

            return success;
        }

        public async Task<ShopperRecord> GetShopperByEmail(string email)
        {
            // GET https://{accountName}.{environment}.com.br/api/dataentities/CL/search?email=

            ShopperRecord shopperRecord = null;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/dataentities/CL/search?email={email}")
                };

                request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
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
                    shopperRecord = JsonConvert.DeserializeObject<ShopperRecord>(responseContent);
                }
                else
                {
                    _context.Vtex.Logger.Warn("GetShopperByEmail", null, $"Could not find shopper '{email}'\n[{response.StatusCode}] '{responseContent}'");
                }
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("GetShopperByEmail", null, $"Error getting shopper '{email}'", ex);
            }

            return shopperRecord;
        }

        public async Task<ShopperAddress[]> GetShopperAddressById(string id)
        {
            // GET https://{accountName}.{environment}.com.br/api/dataentities/AD/search?userId=&_fields=

            ShopperAddress[] shopperAddress = null;
            string searchFields = "country,postalCode";

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/dataentities/AD/search?userId={id}&_fields={searchFields}")
                };

                request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
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
                if (response.IsSuccessStatusCode)
                {
                    shopperAddress = JsonConvert.DeserializeObject<ShopperAddress[]>(responseContent);
                }
                else
                {
                    _context.Vtex.Logger.Warn("GetShopperAddressById", null, $"Could not find shopper '{id}'\n[{response.StatusCode}] '{responseContent}'");
                }
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("GetShopperAddressById", null, $"Error getting shopper '{id}'", ex);
            }

            return shopperAddress;
        }

        private async Task<bool> ProcessNotification(RequestContext requestContext, bool isActive, bool inventoryUpdated, string skuId)
        {
            bool success = false;
            if (isActive && inventoryUpdated)
            {
                MerchantSettings merchantSettings = await _availabilityRepository.GetMerchantSettings();
                NotifyRequest[] requestsToNotify = await _availabilityRepository.ListRequestsForSkuId(skuId, requestContext);
                if (requestsToNotify != null && requestsToNotify.Length > 0)
                {
                    long available = await GetTotalAvailableForSku(skuId, requestContext);
                    if (available > 0)
                    {
                        GetSkuContextResponse skuContextResponse = await GetSkuContext(skuId, requestContext);
                        if (skuContextResponse != null)
                        {
                            foreach (NotifyRequest requestToNotify in requestsToNotify)
                            {
                                bool sendMail = true;
                                if(merchantSettings.DoShippingSim)
                                {
                                    sendMail = await this.CanShipToShopper(requestToNotify, requestContext);
                                }

                                if (sendMail)
                                {
                                    bool mailSent = await SendEmail(requestToNotify, skuContextResponse, requestContext);
                                    if (mailSent)
                                    {
                                        requestToNotify.NotificationSent = "true";
                                        requestToNotify.NotificationSentAt = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                        bool updatedRequest = await _availabilityRepository.SaveNotifyRequest(requestToNotify, requestContext);
                                        success = updatedRequest;
                                        if (!updatedRequest)
                                        {
                                            _context.Vtex.Logger.Error("ProcessNotification", null, $"Mail was sent but failed to update record {JsonConvert.SerializeObject(requestToNotify)}");
                                        }
                                    }
                                }
                                else
                                {
                                    _context.Vtex.Logger.Debug("ProcessNotification", null, $"SkuId '{skuId}' can not be shipped to '{requestToNotify.Email}' ");
                                }
                            }
                        }
                        else
                        {
                            _context.Vtex.Logger.Warn("ProcessNotification", null, $"Null SkuContext for skuId {skuId}");
                        }
                    }
                    else
                    {
                        _context.Vtex.Logger.Debug("ProcessNotification", null, $"SkuId '{skuId}' {available} available");
                    }
                }
                else
                {
                    _context.Vtex.Logger.Debug("ProcessNotification", null, $"No requests to be notified for {skuId}");
                }
            }

            return success;
        }

        public async Task<List<string>> ProcessAllRequests()
        {
            List<string> results = new List<string>();
            RequestContext requestContext = new RequestContext
            {
                Account = _context.Vtex.Account,
                AuthToken = _context.Vtex.AuthToken
            };

            NotifyRequest[] allRequests = await _availabilityRepository.ListNotifyRequests();
            if(allRequests != null && allRequests.Length > 0)
            {
                foreach(NotifyRequest requestToNotify in allRequests)
                {
                    bool sendMail = false;
                    bool updatedRecord = false;
                    string skuId = requestToNotify.SkuId;
                    if(requestToNotify.NotificationSent.Equals("true"))
                    {
                        results.Add($"{skuId} {requestToNotify.Email} Sent at {requestToNotify.NotificationSentAt}");
                    }
                    else
                    {
                        long available = await GetTotalAvailableForSku(skuId, requestContext);
                        if(available > 0)
                        {
                            GetSkuContextResponse skuContextResponse = await GetSkuContext(skuId, requestContext);
                            sendMail = await SendEmail(requestToNotify, skuContextResponse, requestContext);
                            if(sendMail)
                            {
                                requestToNotify.NotificationSent = "true";
                                requestToNotify.NotificationSentAt = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                updatedRecord = await _availabilityRepository.SaveNotifyRequest(requestToNotify, requestContext);
                            }
                        }

                        results.Add($"{skuId} Qnty:{available} '{requestToNotify.Email}' Sent? {sendMail} Updated? {updatedRecord}");
                    }
                }
            }
            else
            {
                results.Add("No requests to notify.");
            }

            return results;
        }

        public async Task<List<string>> ProcessUnsentRequests()
        {
            List<string> results = new List<string>();
            RequestContext requestContext = new RequestContext
            {
                Account = _context.Vtex.Account,
                AuthToken = _context.Vtex.AuthToken
            };

            NotifyRequest[] allRequests = await _availabilityRepository.ListUnsentNotifyRequests();
            if(allRequests != null && allRequests.Length > 0)
            {
                foreach(NotifyRequest requestToNotify in allRequests)
                {
                    bool sendMail = false;
                    bool updatedRecord = false;
                    string skuId = requestToNotify.SkuId;
                    long available = await GetTotalAvailableForSku(skuId, requestContext);
                    if(available > 0)
                    {
                        GetSkuContextResponse skuContextResponse = await GetSkuContext(skuId, requestContext);
                        sendMail = await SendEmail(requestToNotify, skuContextResponse, requestContext);
                        if(sendMail)
                        {
                            requestToNotify.NotificationSent = "true";
                            requestToNotify.NotificationSentAt = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                            updatedRecord = await _availabilityRepository.SaveNotifyRequest(requestToNotify, requestContext);
                        }
                    }

                    results.Add($"{skuId} Qnty:{available} '{requestToNotify.Email}' Sent? {sendMail} Updated? {updatedRecord}");
                }
            }
            else
            {
                results.Add("No requests to notify.");
            }

            return results;
        }

        public async Task<bool> CanShipToShopper(NotifyRequest requestToNotify, RequestContext requestContext)
        {
            bool sendMail = false;
            ShopperRecord shopperRecord = await this.GetShopperByEmail(requestToNotify.Email);
            if (shopperRecord != null)
            {
                ShopperAddress[] shopperAddresses = await this.GetShopperAddressById(shopperRecord.Id);
                if (shopperAddresses != null)
                {
                    string sellerId = string.Empty;
                    if (requestToNotify.Seller != null && requestToNotify.Seller.sellerId != null)
                    {
                        sellerId = requestToNotify.Seller.sellerId;
                    }

                    CartSimulationRequest cartSimulationRequest = new CartSimulationRequest
                    {
                        Items = new List<CartItem>
                        {
                            new CartItem
                            {
                                Id = requestToNotify.Id,
                                Quantity = 1,
                                Seller = sellerId
                            }
                        },
                        PostalCode = string.Empty,
                        Country = string.Empty
                    };

                    var addressList = shopperAddresses.Distinct();
                    foreach (ShopperAddress shopperAddress in addressList)
                    {
                        CartSimulationResponse cartSimulationResponse = await this.CartSimulation(cartSimulationRequest, requestContext);
                        if (cartSimulationResponse != null)
                        {
                            if (cartSimulationResponse.Items[0].Availability.Equals(Constants.Availability.Available))
                            {
                                sendMail = true;
                                break;
                            }
                        }
                    }
                }
            }

            return sendMail;
        }

        public async Task<CartSimulationResponse> CartSimulation(CartSimulationRequest cartSimulationRequest, RequestContext requestContext)
        {
            CartSimulationResponse cartSimulationResponse = null;
            string jsonSerializedData = JsonConvert.SerializeObject(cartSimulationRequest);

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/checkout/pub/orderForms/simulation"),
                    Content = new StringContent(jsonSerializedData, Encoding.UTF8, Constants.APPLICATION_JSON)
                };

                request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
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
                Console.WriteLine($"CartSimulation {cartSimulationRequest.PostalCode},{cartSimulationRequest.Country} : [{response.StatusCode}] ");
                if(response.IsSuccessStatusCode)
                {
                    cartSimulationResponse = JsonConvert.DeserializeObject<CartSimulationResponse>(responseContent);
                }
                else
                {
                    _context.Vtex.Logger.Warn("CartSimulation", null, $"[{response.StatusCode}] '{responseContent}'\n{jsonSerializedData}");
                }
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("CartSimulation", null, $"Error in Cart Simulation '{jsonSerializedData}'", ex);
            }

            return cartSimulationResponse;
        }
    }
}
