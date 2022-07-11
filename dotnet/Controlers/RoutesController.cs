namespace service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AvailabilityNotify.Models;
    using AvailabilityNotify.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Vtex.Api.Context;

    public class RoutesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIOServiceContext _context;
        private readonly IVtexAPIService _vtexAPIService;

        public RoutesController(IHttpContextAccessor httpContextAccessor, IIOServiceContext context, IVtexAPIService vtexAPIService)
        {
            this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._vtexAPIService = vtexAPIService ?? throw new ArgumentNullException(nameof(vtexAPIService));
        }

        public async Task<IActionResult> ProcessNotification()
        {
            Response.Headers.Add("Cache-Control", "private");
            ActionResult status = BadRequest();
            if ("post".Equals(HttpContext.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                string bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                AffiliateNotification notification = JsonConvert.DeserializeObject<AffiliateNotification>(bodyAsText);
                bool sent = await _vtexAPIService.ProcessNotification(notification);
                if(sent)
                {
                    status = Ok();
                }
            }

            return status;
        }

        public async Task<IActionResult> Initialize()
        {
            Response.Headers.Add("Cache-Control", "private");
            ActionResult status = BadRequest();
            bool schema = await _vtexAPIService.VerifySchema();
            bool template = await _vtexAPIService.CreateDefaultTemplate();
            if(schema && template)
            {
                status = Ok();
            }

            return status;
        }

        public async Task<IActionResult> PrcocessAllRequests()
        {            
            List<string> results = await _vtexAPIService.ProcessAllRequests();
            _context.Vtex.Logger.Info("PrcocessAllRequests", null, string.Join( ", ", results));
            return Ok();
        }

        public async Task<IActionResult> ProcessUnsentRequests()
        {
            ProcessingResult[] results = await _vtexAPIService.ProcessUnsentRequests();
            _context.Vtex.Logger.Info("ProcessUnsentRequests", null, JsonConvert.SerializeObject(results));
            return Json(results);
        }

        public async Task<IActionResult> ListNotifyRequests()
        {
            NotifyRequest[] results = await _vtexAPIService.ListNotifyRequests();
            return Json(results);
        }
    }
}
