using AvailabilityNotify.Models;
using System;
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
        Task SetImportLock(DateTime importStartTime);
        Task<DateTime> CheckImportLock();
        Task ClearImportLock();

        Task<bool> SaveNotifyRequest(NotifyRequest notifyRequest, RequestContext requestContext);
        Task<bool> DeleteNotifyRequest(string documentId);
        Task<NotifyRequest[]> ListRequestsForSkuId(string skuId, RequestContext requestContext);
        Task<NotifyRequest[]> ListNotifyRequests();
        Task<NotifyRequest[]> ListUnsentNotifyRequests();
    }
}