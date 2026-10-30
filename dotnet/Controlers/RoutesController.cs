namespace service.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class RoutesController : Controller
    {
        private const string DeprecatedMessage =
            "vtex.availability-notify@0.x is deprecated and no longer processes requests. Migrate to vtex.availability-notify@1.x.";

        public IActionResult ProcessNotification() => Deprecated();

        public IActionResult Initialize() => Deprecated();

        /* Action name must match the "prcocessAllRequests" route key in service.json. */
        public IActionResult PrcocessAllRequests() => Deprecated();

        public IActionResult ProcessUnsentRequests() => Deprecated();

        private IActionResult Deprecated()
        {
            Response.Headers["Cache-Control"] = "no-store";

            return new ContentResult
            {
                StatusCode = StatusCodes.Status410Gone,
                ContentType = "text/plain; charset=utf-8",
                Content = DeprecatedMessage,
            };
        }
    }
}
