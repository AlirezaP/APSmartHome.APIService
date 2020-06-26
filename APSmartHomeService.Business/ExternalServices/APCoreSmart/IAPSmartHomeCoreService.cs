
using APSmartHomeService.Business.ExternalServices.APCoreSmart.BusinessObject;
using System.Threading.Tasks;

namespace APSmartHomeService.Business.ExternalServices.APCoreSmart
{
    public interface IAPSmartHomeCoreService
    {
        Task<bool> SendCommand(SendRequestModel model);

        Task<bool> SendSecureCommand(BusinessObject.SendRequestModel model);
    }
}