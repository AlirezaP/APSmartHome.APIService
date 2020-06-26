using APSmartHomeService.Business.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace APSmartHomeService.Services
{
    public class RsaManager
    {
        private RSASecurity _clientInstance;
        private RSASecurity _serverInstance;

        public RsaManager(RSASecurity client, RSASecurity device)
        {
            _clientInstance = client;
            _serverInstance = device;
        }

        internal RSASecurity GetClientRsaInstance()
        {
            return _clientInstance;
        }

        internal RSASecurity GetServerRsaInstance()
        {
            return _serverInstance;
        }
    }
}
