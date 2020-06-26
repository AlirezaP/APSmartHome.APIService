using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APSmartHomeService.Models
{
    public enum StatusType { OK=0,Forbiden = 403,Error=2, Duplicate =4}
    public class BaseResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
