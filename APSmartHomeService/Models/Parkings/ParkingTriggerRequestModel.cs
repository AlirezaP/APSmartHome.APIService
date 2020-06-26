using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APSmartHomeService.Models.Parkings
{
    public enum ActionCode
    {
        Open = 1,
        Close = 2
    }
    public class ParkingTriggerRequestModel
    {
        public string ClientID { get; set; }
        public int ActionCode { get; set; }
        public long Tick { get; set; }

        public string Sign { get; set; }

        public long Seq { get; set; }

        public string Tcode { get; set; }
    }
}
