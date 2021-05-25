namespace service.Controllers
{
  using System;
  using Microsoft.AspNetCore.Mvc;

    public class EventsController : Controller
    {
        public string OnAppsLinked(string account, string workspace)
        {
            return $"OnAppsLinked event detected for {account}/{workspace}";
        } 

        public void BroadcasterCatalog(string account, string workspace)
        {
            Console.WriteLine($"BroadcasterCatalog event detected for {account}/{workspace}");
            string bodyAsText = new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
            Console.WriteLine($"[BroadcasterCatalog Notification] : '{bodyAsText}'");
        }
    }
}
