using System;
using System.Collections.Generic;
using System.Text;

namespace APSmartHomeService.Business
{
   public class Cache
    {
        public static string Agent { get; set; }
        public static string Token { get; set; }
        public static string DeviceID { get; set; }

        public static string BaseRestAddress { get; set; }
    }
}
