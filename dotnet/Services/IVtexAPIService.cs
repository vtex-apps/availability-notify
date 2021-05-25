using System.Threading.Tasks;
using AvailabilityNotify.Models;

namespace AvailabilityNotify.Services
{
    public interface IVtexAPIService
    {
        Task<bool> AvailabilitySubscribe(string email, string sku, string name);
        Task<bool> ProcessNotification(AffiliateNotification notification);
    }
}