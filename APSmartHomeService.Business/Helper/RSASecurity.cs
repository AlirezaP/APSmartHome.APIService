using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace APSmartHomeService.Business.Helper
{
    public class RSASecurity
    {
        public enum KeyType
        {
            PrivateKey,
            PublicKey
        }

        private RSA rsaInstance;

        public RSASecurity(RSA instance)
        {
            rsaInstance = instance;
        }

        public RSASecurity(string key, KeyType kType)
        {
            rsaInstance = RSA.Create();

            if (kType == KeyType.PrivateKey)
            {
                rsaInstance.ImportPkcs8PrivateKey(Convert.FromBase64String(key), out _);
            }

            if (kType == KeyType.PublicKey)
            {
                rsaInstance.ImportSubjectPublicKeyInfo(Convert.FromBase64String(key), out _);
            }
        }

        public byte[] RSAEncryption(byte[] rawData, RSAEncryptionPadding padding)
        {
            var enTxt2 = rsaInstance.Encrypt(rawData, padding);
            return enTxt2;
        }

        public byte[] RSADecryption(byte[] encData, RSAEncryptionPadding padding)
        {
            var deTxt2 = rsaInstance.Decrypt(encData, padding);
            return deTxt2;
        }


        public byte[] RSASign(byte[] rawData, HashAlgorithmName alg , RSASignaturePadding padding)
        {
            var si1 = rsaInstance.SignData(rawData, alg, padding);
            return si1;
        }


        public bool RSAVerifyData(byte[] data,byte[] signData, HashAlgorithmName alg, RSASignaturePadding padding)
        {
           return rsaInstance.VerifyData(data, signData, alg, padding);
        }
    }
}
