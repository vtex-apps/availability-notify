using System.Threading.Tasks;

namespace AvailabilityNotify.Services
{
    public interface IVtexAPIService
    {
        Task<bool> AvailabilitySubscribe(string email, string sku, string name);
    }
}