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
using Newtonsoft.Json.Linq;
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

        public async Task GetShopperToNotifyBySku(string sku)
        {
            // GET https://{accountName}.{environment}.com.br/api/dataentities/CL/search?email=

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/dataentities/{Constants.DATA_ENTITY}/search?skuId={sku}")
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

            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("GetShopperToNotifyBySku", null, $"Error getting shoppers for sku '{sku}'", ex);
            }
        }

        public async Task<InventoryBySku> ListInventoryBySku(string sku)
        {
            // GET https://{accountName}.{environment}.com.br/api/logistics/pvt/inventory/skus/skuId

            InventoryBySku inventoryBySku = new InventoryBySku();

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/logistics/pvt/inventory/skus/{sku}")
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
                //Console.WriteLine($"ListInventoryBySku {sku} : {responseContent}");
                if(response.IsSuccessStatusCode)
                {
                    inventoryBySku = JsonConvert.DeserializeObject<InventoryBySku>(responseContent);
                }
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("ListInventoryBySku", null, $"Error getting inventory for sku '{sku}'", ex);
            }

            return inventoryBySku;
        }

        public async Task<long> GetTotalAvailableForSku(string sku)
        {
            long totalAvailable = 0;
            InventoryBySku inventoryBySku = await this.ListInventoryBySku(sku);
            if(inventoryBySku != null && inventoryBySku.Balance != null)
            {
                try
                {
                    long totalQuantity = inventoryBySku.Balance.Where(i => !i.HasUnlimitedQuantity).Sum(i => i.TotalQuantity);
                    long totalReseved = inventoryBySku.Balance.Where(i => !i.HasUnlimitedQuantity).Sum(i => i.ReservedQuantity);
                    totalAvailable = totalQuantity - totalReseved;
                    Console.WriteLine($"Sku {sku} : {totalQuantity} - {totalReseved} = {totalAvailable}");
                }
                catch(Exception ex)
                {
                    _context.Vtex.Logger.Error("GetTotalAvailableForSku", null, $"Error calculating total available for sku '{sku}' '{JsonConvert.SerializeObject(inventoryBySku)}'", ex);
                }
            }

            Console.WriteLine($"Sku {sku} Total Available = {totalAvailable}");

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
                request.Headers.Add(Constants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
            MerchantSettings merchantSettings = await _availabilityRepository.GetMerchantSettings();
            request.Headers.Add(Constants.AppKey, merchantSettings.AppKey);
            request.Headers.Add(Constants.AppToken, merchantSettings.AppToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] CreateOrUpdateTemplate Response {response.StatusCode} Content = '{responseContent}' [-]");
            _context.Vtex.Logger.Info("Create Template", null, $"[{response.StatusCode}] {responseContent}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateOrUpdateTemplate(EmailTemplate template)
        {
            string jsonSerializedTemplate = JsonConvert.SerializeObject(template);
            return await this.CreateOrUpdateTemplate(jsonSerializedTemplate);
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

            MerchantSettings merchantSettings = await _availabilityRepository.GetMerchantSettings();
            //Console.WriteLine($"Key:[{merchantSettings.AppKey}] | Token:[{merchantSettings.AppToken}]");
            string appKey = merchantSettings.AppKey;
            string appToken = merchantSettings.AppToken;
            request.Headers.Add(Constants.AppKey, appKey);
            request.Headers.Add(Constants.AppToken, appToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"[-] TemplateExists Response {response.StatusCode} Content = '{responseContent}' [-]");
            Console.WriteLine($"[-] Template '{templateName}' Exists Response {response.StatusCode} [-]");

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
            //Console.WriteLine($"[-] GetDefaultTemplate [{response.StatusCode}] '{responseContent}' [-]");
            //_context.Vtex.Logger.Info("GetDefaultTemplate", "Response", $"[{response.StatusCode}] {responseContent}");
            if (response.IsSuccessStatusCode)
            {
                templateBody = responseContent;
            }
            else
            {
                Console.WriteLine($"[-] GetDefaultTemplate Failed [{Constants.GITHUB_URL}/{Constants.RESPOSITORY}/{Constants.TEMPLATE_FOLDER}/{templateName}.{Constants.TEMPLATE_FILE_EXTENSION}]");
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
                    Console.WriteLine($"Failed to Load Template {templateName}");
                    _context.Vtex.Logger.Info("SendEmail", "Create Template", $"Failed to Load Template {templateName}");
                }
                else
                {
                    EmailTemplate emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(templateBody);
                    emailTemplate.Templates.Email.Message = emailTemplate.Templates.Email.Message.Replace(@"\n", "\n");
                    templateExists = await this.CreateOrUpdateTemplate(emailTemplate);
                }
            }

            return templateExists;
        }

        public async Task<GetSkuContextResponse> GetSkuContext(string skuId)
        {
            // GET https://{accountName}.{environment}.com.br/api/catalog_system/pvt/sku/stockkeepingunitbyid/skuId

            GetSkuContextResponse getSkuContextResponse = null;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/catalog_system/pvt/sku/stockkeepingunitbyid/{skuId}")
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

        public async Task<bool> SendEmail(NotifyRequest notifyRequest, GetSkuContextResponse skuContext)
        {
            bool success = false;

            string responseText = string.Empty;
            string templateName = Constants.DEFAULT_TEMPLATE_NAME;
            
            EmailMessage emailMessage = new EmailMessage
            {
                TemplateName = templateName,
                ProviderName = _context.Vtex.Account,
                JsonData = new JsonData
                {
                    SkuContext = skuContext,
                    NotifyRequest = notifyRequest
                }
            };

            string accountName = _httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME].ToString();
            string message = JsonConvert.SerializeObject(emailMessage);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{Constants.MAIL_SERVICE}?an={accountName}"),
                Content = new StringContent(message, Encoding.UTF8, Constants.APPLICATION_JSON)
            };

            request.Headers.Add(Constants.USE_HTTPS_HEADER_NAME, "true");
            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL];
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
                responseText = $"[-] SendEmail [{responseMessage.StatusCode}] {responseContent}";
                _context.Vtex.Logger.Info("SendEmail", null, $"{message} [{responseMessage.StatusCode}] {responseContent}");
                success = responseMessage.IsSuccessStatusCode;
                if (responseMessage.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    _context.Vtex.Logger.Error("SendEmail", null, $"Template {templateName} not found.");
                }
            }
            catch (Exception ex)
            {
                responseText = $"[-] SendEmail Failure [{ex.Message}]";
                _context.Vtex.Logger.Error("SendEmail", null, $"Failure sending {message}", ex);
                success = false;  //jic
            }

            Console.WriteLine(responseText);
            return success;
        }

        public async Task<bool> AvailabilitySubscribe(string email, string sku, string name)
        {
            bool success = false;

            //await _availabilityRepository.VerifySchema();

            NotifyRequest notifyRequest = new NotifyRequest
            {
                RequestedAt = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                Email = email,
                SkuId = sku,
                Name = name,
                NotificationSent = "false"
            };

            success = await _availabilityRepository.SaveNotifyRequest(notifyRequest);

            return success;
        }

        public async Task<bool> ProcessNotification(AffiliateNotification notification)
        {
            bool success = false;

            bool isActive = notification.IsActive;
            bool inventoryUpdated = notification.StockModified;
            string skuId = notification.IdSku;
            _context.Vtex.Logger.Debug("ProcessNotification", null, $"Sku:{skuId} Active?{isActive} Inventory Changed?{inventoryUpdated}");
            Console.WriteLine($"Sku:{skuId} Active?{isActive} Inventory Changed?{inventoryUpdated}");
            if(isActive && inventoryUpdated)
            {
                long available = await GetTotalAvailableForSku(skuId);
                if(available > 0)
                {
                    NotifyRequest[] requests = await _availabilityRepository.ListRequestsForSkuId(skuId);
                    if(requests != null)
                    {
                        foreach(NotifyRequest request in requests)
                        {
                            GetSkuContextResponse skuContextResponse = await GetSkuContext(skuId);
                            bool sendMail = await SendEmail(request, skuContextResponse);
                            if(sendMail)
                            {
                                request.NotificationSent = "true";
                                request.NotificationSentAt = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                bool updatedRequest = await _availabilityRepository.SaveNotifyRequest(request);
                            }
                        }
                    }
                }
            }

            return success;
        }

        public async Task<bool> ProcessNotification(BroadcastNotification notification)
        {
            bool success = false;

            bool isActive = notification.IsActive;
            bool inventoryUpdated = notification.StockModified;
            string skuId = notification.IdSku;
            _context.Vtex.Logger.Debug("ProcessNotification", null, $"Sku:{skuId} Active?{isActive} Inventory Changed?{inventoryUpdated}");
            Console.WriteLine($"Sku:{skuId} Active?{isActive} Inventory Changed?{inventoryUpdated}");
            if(isActive && inventoryUpdated)
            {
                NotifyRequest[] requests = await _availabilityRepository.ListRequestsForSkuId(skuId);
                if(requests != null)
                {
                    long available = await GetTotalAvailableForSku(skuId);
                    if(available > 0)
                    {
                        foreach(NotifyRequest request in requests)
                        {
                            GetSkuContextResponse skuContextResponse = await GetSkuContext(skuId);
                            bool sendMail = await SendEmail(request, skuContextResponse);
                            if(sendMail)
                            {
                                request.NotificationSent = "true";
                                request.NotificationSentAt = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                bool updatedRequest = await _availabilityRepository.SaveNotifyRequest(request);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"No requests to be notified for {skuId}");
                }
            }

            return success;
        }
    }
}
