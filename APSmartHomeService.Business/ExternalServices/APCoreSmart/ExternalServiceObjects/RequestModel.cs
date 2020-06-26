using System;
using System.Collections.Generic;
using System.Text;

namespace APSmartHomeService.Business.ExternalServices.APCoreSmart.ExternalServiceObjects
{
   public class RequestModel
    {
        public string Topic { get; set; }
        public ExternalServiceObjects.CommandModel[] Commands { get; set; }
    }
}
