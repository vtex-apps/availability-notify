using AvailabilityNotify.Models;
using System;
using System.Threading.Tasks;

namespace AvailabilityNotify.Services
{
    public interface IAvailabilityRepository
    {
        Task<MerchantSettings> GetMerchantSettings();
        Task SetMerchantSettings(MerchantSettings merchantSettings);

        Task<bool> VerifySchema();
        Task SetImportLock(DateTime importStartTime, string sku);
        Task<DateTime> CheckImportLock(string sku);
        Task ClearImportLock(string sku);
        Task<DateTime> GetLastUnsentCheck();
        Task SetLastUnsentCheck(DateTime lastCheck);

        Task<bool> SaveNotifyRequest(NotifyRequest notifyRequest, RequestContext requestContext);
        Task<bool> DeleteNotifyRequest(string documentId);
        Task<NotifyRequest[]> ListRequestsForSkuId(string skuId, RequestContext requestContext);
        Task<NotifyRequest[]> ListNotifyRequests();
        Task<NotifyRequest[]> ListUnsentNotifyRequests();
    }
}