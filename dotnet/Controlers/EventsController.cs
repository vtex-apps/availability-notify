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
        private readonly IAvailabilityRepository _availabilityRepository;

        public EventsController(IVtexAPIService vtexAPIService, IIOServiceContext context, IAvailabilityRepository availabilityRepository)
        {
            this._vtexAPIService = vtexAPIService ?? throw new ArgumentNullException(nameof(vtexAPIService));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._availabilityRepository = availabilityRepository ?? throw new ArgumentNullException(nameof(availabilityRepository));
        }

        public void BroadcasterNotification(string account, string workspace)
        {
            DateTime processingStarted = _availabilityRepository.CheckImportLock().Result;
            TimeSpan elapsedTime = DateTime.Now - processingStarted;
            if (elapsedTime.TotalMinutes < 1)
            {
                _context.Vtex.Logger.Warn("BroadcasterNotification", null, $"Blocked by lock.  Processing started: {processingStarted}");
                throw new Exception("Blocked by lock.");
            }

            _availabilityRepository.SetImportLock(DateTime.Now);

            BroadcastNotification notification = null;
            string bodyAsText = string.Empty;
            try
            {
                bodyAsText = new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
                notification = JsonConvert.DeserializeObject<BroadcastNotification>(bodyAsText);
            }
            catch(Exception ex)
            {
                _context.Vtex.Logger.Error("BroadcasterNotification", null, "Error reading Notification", ex);
            }

            bool processed = _vtexAPIService.ProcessNotification(notification).Result;
            _context.Vtex.Logger.Info("BroadcasterNotification", null, $"Processed Notification? {processed} : {bodyAsText}");

            _availabilityRepository.ClearImportLock();
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
