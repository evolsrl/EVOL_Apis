using ApiSim.Core.Services;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ApiSim.Core.Encriptacion
{
    public class RSAProvider
    {
        private readonly IDapper _dapper;
        public RSACryptoServiceProvider RSAService { get; set; }

        public RSAProvider(IDapper dapper)
        {
            this.RSAService = new RSACryptoServiceProvider();
            _dapper = dapper;
        }

        public byte[] CrearPublicKey()
        {
            string xmlPublicKey = this.RSAService.ToXmlString(false);
            return Encoding.ASCII.GetBytes(xmlPublicKey);
        }

        public byte[] CrearPrivacateKey()
        {
            string xmlPrivateKey = this.RSAService.ToXmlString(true);
            return Encoding.ASCII.GetBytes(xmlPrivateKey);
        }

        public async Task<string> ObtenerPrivateKey()
        {
            var dbparams = new DynamicParameters();
            var result = await Task.FromResult(_dapper.Get<string>("[WordPressTGEParametrosValoresSeleccionarClavePrivada]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        public async Task<string> ObtenerPublicKey()
        {
            var dbparams = new DynamicParameters();
            var result = await Task.FromResult(_dapper.Get<string>("[WordPressTGEParametrosValoresSeleccionarClavePublica]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        public static byte[] Encriptar(string texto, string PublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            rsa.FromXmlString(PublicKey);
            byte[] cadenaEncriptada = rsa.Encrypt(Encoding.ASCII.GetBytes(texto), false);
            return cadenaEncriptada;
        }

        public static byte[] Desencriptar(string textoEncriptado, string PrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            rsa.FromXmlString(PrivateKey);
            byte[] cadenaDesencriptada = rsa.Decrypt(Convert.FromBase64String(textoEncriptado), false);
            return cadenaDesencriptada;
        }

    }
}
