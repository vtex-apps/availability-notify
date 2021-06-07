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

        public string OnAppsLinked(string account, string workspace)
        {
            return $"OnAppsLinked event detected for {account}/{workspace}";
        } 

        public void BroadcasterNotification(string account, string workspace)
        {
            BroadcastNotification notification = null;
            try
            {
                string bodyAsText = new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
                _context.Vtex.Logger.Info("BroadcasterNotification", null, $"Notification: {bodyAsText}");
                notification = JsonConvert.DeserializeObject<BroadcastNotification>(bodyAsText);
            }
            catch(Exception ex)
            {
                _context.Vtex.Logger.Error("BroadcasterNotification", null, "Error reading Notification", ex);
            }

            _vtexAPIService.ProcessNotification(notification);
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
