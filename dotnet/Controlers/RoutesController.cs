namespace service.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
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
            bool success = false;
            //ActionResult status = BadRequest();
            ActionResult status = Ok();
            if ("post".Equals(HttpContext.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                string bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                Console.WriteLine($"[Notification] : '{bodyAsText}'");
                AffiliateNotification notification = JsonConvert.DeserializeObject<AffiliateNotification>(bodyAsText);
                //bool sent = await _vtexAPIService.ProcessNotification(notification);
            }
            else
            {
                Console.WriteLine($"[Notification] : '{HttpContext.Request.Method}'");
            }

            return status;
        }

        public async Task<IActionResult> ListRequests()
        {
            return BadRequest();
        }
    }
}
