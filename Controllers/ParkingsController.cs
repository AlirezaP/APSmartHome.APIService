using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using APSmartHomeService.Business.ExternalServices.APCoreSmart.ExternalServiceObjects;
using APSmartHomeService.Models;
using APSmartHomeService.Models.Configs;
using APSmartHomeService.Models.Parkings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace APSmartHomeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingsController : ControllerBase
    {
        private IMemoryCache _cache;
        private ILogger<ParkingsController> _logger;
        private Business.ExternalServices.APCoreSmart.IAPSmartHomeCoreService _aPSmartHomeCoreService;
        private Services.RsaManager _rsaMgr;
        private static long lastTickReq = 0;

        public ParkingsController(
                        ILogger<ParkingsController> logger,
                        IMemoryCache cache,
                        Services.RsaManager rsaMgr,
                        Business.ExternalServices.APCoreSmart.IAPSmartHomeCoreService aPSmartHomeCoreService)
        {
            _logger = logger;
            _cache = cache;
            _rsaMgr = rsaMgr;
            _aPSmartHomeCoreService = aPSmartHomeCoreService;
        }

        [HttpPost("Trigger")]
        public async Task<ParkingTriggerResultModel> Trigger(ParkingTriggerSecureRequestModel raw)
        {

            ParkingTriggerResultModel response = new ParkingTriggerResultModel()
            {
                Status = (int)Models.StatusType.Error
            };

            byte[] buf = Convert.FromBase64String(raw.Data);
            byte[] signData = Convert.FromBase64String(raw.Sign);

            var clearData =
                _rsaMgr
                .GetServerRsaInstance()
                .RSADecryption(buf, RSAEncryptionPadding.Pkcs1);

            var isVerified =
                _rsaMgr
                .GetClientRsaInstance()
                .RSAVerifyData(clearData, signData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            if (!isVerified)
            {
                response.Status = (int)Models.StatusType.Forbiden;
                return response;
            }

            var model = System.Text.Json.JsonSerializer.Deserialize<ParkingTriggerRequestModel>(clearData);

            if (lastTickReq > 0 && lastTickReq >= model.Tick)
            {
                response.Status = (int)Models.StatusType.Duplicate;
                return response;
            }

            if (model.Tcode != "DeviceSecretForCallApi8768767827SDFD%$^$")
            {
                response.Status = (int)Models.StatusType.Forbiden;
                return response;
            }

            // ParkingTriggerResultModel response = new ParkingTriggerResultModel { Status = (int)Models.StatusType.Error };

            string clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            try
            {
                SetLog(System.Text.Json.JsonSerializer.Serialize(response), null, "NewRequest", clientIP);

                var target = _cache.Get<ConfigModel>("CoreInfo");

                if (target == null)
                {
                    response.Message = "IP NotFound!";
                    return response;
                }

                lastTickReq = model.Tick;

                if (model.ActionCode == (int)ActionCode.Open)
                {
                    CommandModel command = new CommandModel();
                    List<PinCommand> pinsAct = new List<PinCommand>();
                    pinsAct.Add(new PinCommand
                    {
                        PinNumber = Settings.ParkingPinNum,
                        PinMode = 1,
                        PinVal = Settings.ParkingPinVal
                    });

                    command.PinActions = pinsAct.ToArray();
                    command.HasReversePinVal = true;
                    command.ReversePinVal = false;
                    command.DelayForReversePinVal = Settings.ParkingCommandDuartion;



                    //..........

                    command.Tcode = model.Tcode;
                    command.Tick = DateTime.Now.Ticks;

                    //............

                    var resDevice = await _aPSmartHomeCoreService.SendSecureCommand(new Business.ExternalServices.APCoreSmart.BusinessObject.SendRequestModel
                    {
                        ClientID = model.ClientID,
                        IPAddress = target.IPAddress,
                        Pass = Settings.DeviceSecret,
                        Port = Settings.DevicePort,
                        UserName = Settings.DeviceUserName,
                        RequestData = new RequestModel
                        {
                            Topic = "Segment",
                            Commands = new CommandModel[] { command }
                        }
                    });

                    if (!resDevice)
                    {
                        response.Message = "عملیات نا موفق!";

                        model.ClientID = "*****";
                        SetLog(
                            System.Text.Json.JsonSerializer.Serialize(response),
                            System.Text.Json.JsonSerializer.Serialize(model),
                            clientIP,
                            "",
                            true);

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error";

                model.ClientID = "*****";
                SetLog(
                    System.Text.Json.JsonSerializer.Serialize(response),
                    System.Text.Json.JsonSerializer.Serialize(model),
                    clientIP,
                    ex.Message,
                    true);
            }

            response.Status = (int)Models.StatusType.OK;
            return response;
        }

        private void SetLog(string rawResponse, string rawReq, string clientIP, string detail, bool isError = false)
        {
            var l = new LogsModel
            {
                RawReq = rawReq,
                RawResponse = rawResponse,
                CreateDate = DateTime.Now,
                UID = Guid.NewGuid(),
                ClientIPAddress = clientIP,
                RawEception = detail
            };

            if (!isError)
            {
                _logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(l));
            }
            else
            {
                _logger.LogError(System.Text.Json.JsonSerializer.Serialize(l));
            }
        }
    }
}
