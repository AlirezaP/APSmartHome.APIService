using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using APSmartHomeService.Mid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace APSmartHomeService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            Settings.DevicePort = int.Parse(Configuration["APDeviceCore:DevicePort"]);
           // Settings.DeviceSecret = Configuration[""];
            Settings.DeviceUserName = Configuration["APDeviceCore:DeviceUserName"];
            Settings.ParkingCommandDuartion = int.Parse(Configuration["APDeviceCore:ParkingCommandDuartion"]);
            Settings.ParkingPinNum = int.Parse(Configuration["APDeviceCore:ParkingPinNum"]);
            Settings.ParkingPinVal = bool.Parse(Configuration["APDeviceCore:ParkingPinVal"]);


            services.AddControllers();
            services.AddMemoryCache();


            //RSA
            //...................................

            var clientPK = System.IO.File.ReadAllText(@".\pData\sk\xamPrivate.key");
            clientPK = clientPK.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "");
            clientPK = clientPK.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "");
            var clientRsaInstance = RSA.Create();
            clientRsaInstance.ImportRSAPrivateKey(Convert.FromBase64String(clientPK), out _);


            var serverPK = System.IO.File.ReadAllText(@".\pData\sk\APSmartExchangeServerCertPrivate.key");
            serverPK = serverPK.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "");
            serverPK= serverPK.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "");
            var serverRsaInstance = RSA.Create();
            serverRsaInstance.ImportPkcs8PrivateKey(Convert.FromBase64String(serverPK), out _);


            services.AddSingleton(new Services.RsaManager(
                new Business.Helper.RSASecurity(clientRsaInstance), new Business.Helper.RSASecurity(serverRsaInstance)));

            //...................................

            services.AddSingleton<Business.ExternalServices.APCoreSmart.IAPSmartHomeCoreService,
                Business.ExternalServices.APCoreSmart.APSmartHomeCoreService>();

            services.AddSwaggerDocument();

            services.AddScoped<ClientIpCheckActionFilter>(container =>
            {
                var loggerFactory = container.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

                return new ClientIpCheckActionFilter(
                    Configuration["AdminSafeList"], logger);
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
