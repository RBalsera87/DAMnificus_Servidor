using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServidorConexion
{
    class GeneradorTokens
    {
        public static string GenerarToken(int size)
        {
            var charSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var chars = charSet.ToCharArray();
            var datos = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(datos);
            datos = new byte[size];
            crypto.GetNonZeroBytes(datos);
            var resultado = new StringBuilder(size);
            foreach (var b in datos)
            {
                resultado.Append(chars[b % (chars.Length)]);
            }
            return resultado.ToString();
        }
    }
}
