using APSmartHomeService.Business.ExternalServices.APCoreSmart.ExternalService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APSmartHomeService.Business.ExternalServices.APCoreSmart
{
    public class APSmartHomeCoreService : IAPSmartHomeCoreService
    {
        private IAPSmartHomeCoreTcpService apSmartCoreTcp;

        public APSmartHomeCoreService()
        {
            apSmartCoreTcp = new ExternalService.APSmartHomeCoreTcpService();
        }

        public async Task<bool> SendCommand(BusinessObject.SendRequestModel model)
        {
            try
            {
                var res = await apSmartCoreTcp.ConnectAndSendCommand(
                    model.IPAddress,
                    model.Port,
                    model.ClientID,
                    model.UserName,
                    model.Pass,
                    model.RequestData);

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SendSecureCommand(BusinessObject.SendRequestModel model)
        {
            try
            {
                var res = await apSmartCoreTcp.SecureConnectAndSendCommand(
                    model.IPAddress,
                    model.Port,
                    model.ClientID,
                    model.UserName,
                    model.Pass,
                    model.RequestData);

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
