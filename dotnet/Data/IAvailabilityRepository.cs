using AvailabilityNotify.Models;
using System.Threading.Tasks;

namespace AvailabilityNotify.Services
{
    public interface IAvailabilityRepository
    {
        Task<MerchantSettings> GetMerchantSettings();
        Task SetMerchantSettings(MerchantSettings merchantSettings);
        Task<bool> IsInitialized();
        Task SetInitialized();

        Task<bool> VerifySchema();

        Task<bool> SaveNotifyRequest(NotifyRequest notifyRequest);
        Task<NotifyRequest[]> ListRequestsForSkuId(string skuId);
    }
}