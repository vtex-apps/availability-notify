namespace service.Controllers
{
  using System;
  using System.Threading.Tasks;
  using AvailabilityNotify.Data;
  using AvailabilityNotify.Models;
  using AvailabilityNotify.Services;
  using Microsoft.AspNetCore.Mvc;
  using Newtonsoft.Json;
  using Vtex.Api.Context;

  public class EventsController : Controller
    {
        private readonly IVtexAPIService _vtexAPIService;
        private readonly IIOServiceContext _context;

        public EventsController(IVtexAPIService vtexAPIService, IIOServiceContext context)
        {
            this._vtexAPIService = vtexAPIService ?? throw new ArgumentNullException(nameof(vtexAPIService));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void BroadcasterNotification(string account, string workspace)
        {
            BroadcastNotification notification = null;
            string bodyAsText = string.Empty;
            try
            {
                bodyAsText = new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
                //_context.Vtex.Logger.Info("BroadcasterNotification", null, $"Notification: {bodyAsText}");
                notification = JsonConvert.DeserializeObject<BroadcastNotification>(bodyAsText);
            }
            catch(Exception ex)
            {
                _context.Vtex.Logger.Error("BroadcasterNotification", null, "Error reading Notification", ex);
            }

            bool processed = _vtexAPIService.ProcessNotification(notification).Result;
            _context.Vtex.Logger.Info("BroadcasterNotification", null, $"Processed Notification? {processed} : {bodyAsText}");
        }

        public async Task OnAppInstalled([FromBody] AppInstalledEvent @event)
        {
            if (@event.To.Id.Contains(Constants.APP_SETTINGS))
            {
                await _vtexAPIService.VerifySchema();
                await _vtexAPIService.CreateDefaultTemplate();
            }
        }
    }
}
