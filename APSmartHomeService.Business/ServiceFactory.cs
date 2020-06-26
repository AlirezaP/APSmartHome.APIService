using APSmartHomeService.Business.ExternalServices.APCoreSmart;
using System;
using System.Collections.Generic;
using System.Text;

namespace APSmartHomeService.Business
{
    public class ServiceFactory
    {
        public IAPSmartHomeCoreService GetAPSmartHomeCoreService()
        {
            return new APSmartHomeCoreService();
        }
    }
}
