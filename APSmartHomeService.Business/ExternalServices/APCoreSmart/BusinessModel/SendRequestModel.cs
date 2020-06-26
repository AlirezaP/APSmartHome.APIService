using System;
using System.Collections.Generic;
using System.Text;

namespace APSmartHomeService.Business.ExternalServices.APCoreSmart.BusinessObject
{
  public  class SendRequestModel
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string ClientID { get; set; }
        public ExternalServiceObjects.RequestModel RequestData { get; set; }
    }
}
