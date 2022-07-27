using System.Collections.Generic;
using System.Threading.Tasks;
using AvailabilityNotify.Models;
using System.Net;

namespace AvailabilityNotify.Services
{
    public interface IVtexAPIService
    {
        Task<bool> AvailabilitySubscribe(string email, string sku, string name, string locale, SellerObj sellerObj);
        Task<bool> ProcessNotification(AffiliateNotification notification);
        Task<bool> ProcessNotification(BroadcastNotification notification);
        Task ProcessNotification(AllStatesNotification notification);
        Task<bool> VerifySchema();
        Task<bool> CreateDefaultTemplate();
        Task<List<string>> ProcessAllRequests();
        Task<ProcessingResult[]> ProcessUnsentRequests();
        Task CheckUnsentNotifications();
        Task<CartSimulationResponse> CartSimulation(CartSimulationRequest cartSimulationRequest, RequestContext requestContext);
        Task<bool> CanShipToShopper(NotifyRequest requestToNotify, RequestContext requestContext);
        Task<NotifyRequest[]> ListNotifyRequests();
        Task<HttpStatusCode> IsValidAuthUser();
        Task<ValidatedUser> ValidateUserToken(string token);
    }
}
