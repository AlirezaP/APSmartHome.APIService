using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APSmartHomeService.Models.Configs
{
    public class ConfigModel
    {
        public string IPAddress { get; set; }
        public long Tick { get; set; }
        public string Secret { get; set; }
        public string Secret2 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
