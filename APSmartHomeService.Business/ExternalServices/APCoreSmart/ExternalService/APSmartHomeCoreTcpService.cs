using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APSmartHomeService.Business.ExternalServices.APCoreSmart.ExternalService
{
    public class APSmartHomeCoreTcpService : IAPSmartHomeCoreTcpService
    {
        private string shKey = @"ocxVBBKudPc2fZWP0R93BtLYfi2foBsSE/YU26JCvKY=";
        private string shIv = @"UUoSAQTqoqGmhqud/LC3Tw==";


        private bool firstUse = true;
        private IMqttClient mqttClient = null;
        public Action<MqttApplicationMessageReceivedEventArgs> handlerRevice;
        public APSmartHomeCoreTcpService()
        {
            mqttClient = new MqttFactory().CreateMqttClient();
        }

        private async Task ConnectToCore(string ipaddress, int port, string clientID, string userName, string secret)
        {
            //...........................
            // Create TCP based options using the builder.
            var options = new MqttClientOptionsBuilder()
            .WithClientId(clientID)
            .WithTcpServer(ipaddress, port)
            .WithCredentials(userName, secret)
            //.WithTls(new MqttClientOptionsBuilderTlsParameters
            //{
            //    UseTls = true,
            //    CertificateValidationCallback = (X509Certificate x, X509Chain y, SslPolicyErrors z, IMqttClientOptions o) =>
            //    {
            //        // TODO: Check conditions of certificate by using above parameters.
            //        return true;
            //    }
            //})
            .WithCleanSession()
            .Build();


            mqttClient = new MqttFactory().CreateMqttClient();

            if (handlerRevice != null)
            {
                mqttClient.UseApplicationMessageReceivedHandler(handlerRevice);
            }

            await mqttClient.ConnectAsync(options, CancellationToken.None); // Since 3.0.5 with CancellationToken
        }

        private async Task<bool> SendCommand(string topic, ExternalServiceObjects.CommandModel[] commands)
        {
            try
            {
                string offAll = System.Text.Json.JsonSerializer.Serialize(commands);

                var message = new MqttApplicationMessageBuilder()
                              .WithTopic(topic)
                              .WithPayload(offAll)
                              .WithExactlyOnceQoS()
                              .WithRetainFlag()
                              .Build();

                var res = await mqttClient.PublishAsync(message, CancellationToken.None);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> SendSecureCommand(string topic, ExternalServiceObjects.CommandModel[] commands)
        {
            try
            {
                byte[] jsonData = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(commands));

                var secureData = Helper.SecurityHelper.EncryptStringToBytes_Aes(
                    Encoding.UTF8.GetString(jsonData),
                    Convert.FromBase64String(shKey),
                    Convert.FromBase64String(shIv));


                ExternalServiceObjects.SecureCommandModel secureModel = new ExternalServiceObjects.SecureCommandModel
                {
                    Data = Convert.ToBase64String(secureData),
                    Sign = "Soon",
                };

                string offAll = System.Text.Json.JsonSerializer.Serialize(secureModel);

                var message = new MqttApplicationMessageBuilder()
                              .WithTopic(topic)
                              .WithPayload(offAll)
                              .WithExactlyOnceQoS()
                              .WithRetainFlag()
                              .Build();

                var res = await mqttClient.PublishAsync(message, CancellationToken.None);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ConnectAndSendCommand(
            string ipAddress,
            int port,
            string clientID,
            string userName,
            string secret,
            ExternalServiceObjects.RequestModel model)
        {
            try
            {

                if (!mqttClient.IsConnected)
                {
                    await ConnectToCore(ipAddress, port, clientID, userName, secret);
                    firstUse = false;
                }

                var res = await SendCommand(model.Topic, model.Commands);

                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SecureConnectAndSendCommand(
string ipAddress,
int port,
string clientID,
string userName,
string secret,
ExternalServiceObjects.RequestModel model)
        {
            try
            {

                if (!mqttClient.IsConnected)
                {
                    await ConnectToCore(ipAddress, port, clientID, userName, secret);
                    firstUse = false;
                }

                var res = await SendSecureCommand(model.Topic, model.Commands);

                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
