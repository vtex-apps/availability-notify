namespace service.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AvailabilityNotify.Data;
    using AvailabilityNotify.Models;
    using AvailabilityNotify.Services;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Vtex.Api.Context;

    static class Throttle
    {
        public static int counter = 0;
    }

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

        public async Task<IActionResult> BroadcasterNotification(string account, string workspace)
        {
            var incremented_counter = Interlocked.Increment(ref Throttle.counter);
            if (incremented_counter > 10)
            {
                // Throttling -- event system will retry the event later
                Interlocked.Decrement(ref Throttle.counter);
                return StatusCode(429);
            }

            try
            {
                BroadcastNotification notification = null;
                string bodyAsText = string.Empty;
                try
                {
                    bodyAsText = new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
                    notification = JsonConvert.DeserializeObject<BroadcastNotification>(bodyAsText);
                }
                catch (Exception ex)
                {
                    _context.Vtex.Logger.Error("BroadcasterNotification", null, "Error reading Notification", ex);
                    Interlocked.Decrement(ref Throttle.counter);
                    return BadRequest();
                }

                string skuId = notification.IdSku;
                if (string.IsNullOrEmpty(skuId))
                {
                    _context.Vtex.Logger.Warn("BroadcasterNotification", null, "Empty Sku");
                    Interlocked.Decrement(ref Throttle.counter);

                    // return OK so that notification is not retried
                    return Ok();
                }

                bool isActive = notification.IsActive;
                bool inventoryUpdated = notification.StockModified;
                if (!isActive || !inventoryUpdated)
                {
                    // If SKU is not active or inventory hasn't changed, notification is not relevant
                    Interlocked.Decrement(ref Throttle.counter);

                    // return OK so that notification is not retried
                    return Ok();
                }

                DateTime processingStarted = await _availabilityRepository.CheckImportLock(skuId);
                TimeSpan elapsedTime = DateTime.Now - processingStarted;
                if (elapsedTime.TotalMinutes < 1)
                {
                    // Commenting this out to reduce noise
                    //_context.Vtex.Logger.Warn("BroadcasterNotification", null, $"Sku {skuId} blocked by lock.  Processing started: {processingStarted}");
                    Interlocked.Decrement(ref Throttle.counter);
                    return Ok();
                }

                _ = _availabilityRepository.SetImportLock(DateTime.Now, skuId);

                bool processed = _vtexAPIService.ProcessNotification(notification).Result;
                _context.Vtex.Logger.Info("BroadcasterNotification", null, $"Processed Notification? {processed} : {bodyAsText}");

                _ = _availabilityRepository.ClearImportLock(skuId);
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Error("BroadcasterNotification", null, "Error processing Notification", ex);
                Interlocked.Decrement(ref Throttle.counter);
                throw;
            }

            Interlocked.Decrement(ref Throttle.counter);
            return Ok();
        }

        public async Task OnAppInstalled([FromBody] AppInstalledEvent @event)
        {
            if (@event.To.Id.Contains(Constants.APP_SETTINGS))
            {
                await _vtexAPIService.VerifySchema();
                await _vtexAPIService.CreateDefaultTemplate();
            }
        }

        public void AllStates(string account, string workspace)
        {
            try
            {
                string bodyAsText = new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
                AllStatesNotification allStatesNotification = JsonConvert.DeserializeObject<AllStatesNotification>(bodyAsText);
                _vtexAPIService.ProcessNotification(allStatesNotification);
            }
            catch (Exception ex)
            {
                _context.Vtex.Logger.Warn("AllStates", null, $"Error processing Orders Broadcaster Notification: {ex.Message}");
            }
        }
    }
}
