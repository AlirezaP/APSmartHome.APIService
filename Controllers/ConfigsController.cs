using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APSmartHomeService.Mid;
using APSmartHomeService.Models.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace APSmartHomeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigsController : ControllerBase
    {
        private IMemoryCache _cache;
        public ConfigsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpPost("SetConfig")]
        public string SetIPAddress(ConfigModel model)
        {
            try
            {
                if (model != null 
                    && model.Secret == "APISECRETKEY1654656546546546546546" 
                    && model.Secret2 == "APISECRETKEY2654656546546546546546")
                {
                    model.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _cache.Set("CoreInfo", model);
                    return "112";
                }

                return "111";
            }
            catch (Exception ex)
            {
                return "113";
            }
        }

        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        [HttpGet("GetConfig")]
        public string GetConfig()
        {
            try
            {
                var tmp = _cache.Get<ConfigModel>("CoreInfo");

                return System.Text.Json.JsonSerializer.Serialize(tmp);
            }
            catch (Exception ex)
            {
                return "Null";
            }
        }
    }
}
