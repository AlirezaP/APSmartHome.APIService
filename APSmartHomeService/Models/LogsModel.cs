using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APSmartHomeService.Models
{
    public class LogsModel
    {
        public Guid UID { get; set; }
        public DateTime CreateDate { get; set; }
        public string RawReq { get; set; }
        public string RawResponse { get; set; }
        public string ClientIPAddress { get; set; }
        public string RawEception { get; set; }
    }
}
