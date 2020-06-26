using APSmartHomeService.Business.ExternalServices.APCoreSmart.ExternalServiceObjects;
using System.Threading.Tasks;

namespace APSmartHomeService.Business.ExternalServices.APCoreSmart.ExternalService
{
    public interface IAPSmartHomeCoreTcpService
    {
        Task<bool> ConnectAndSendCommand(string ipAddress, int port, string clientID, string userName, string secret, RequestModel model);
        Task<bool> SecureConnectAndSendCommand(string ipAddress, int port, string clientID, string userName, string secret, RequestModel model);
    }
}