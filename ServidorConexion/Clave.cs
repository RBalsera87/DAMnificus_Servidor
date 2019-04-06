using System;
using System.Security.Cryptography;

namespace ServidorConexion
{
    public class Clave
    {
        public Clave()
        {

        }

        //Coge la Clave encriptada y extrae la "sal" y la retorna
        public static string getSal(string hash)
        {
            // Convierte al hash a arreglo de bytes
            byte[] hashWithSaltBytes = Convert.FromBase64String(hash);

            // Tamaño del hash en bits
            int hashSizeInBits, hashSizeInBytes;
            hashSizeInBits = 512;

            // Convierte el tamaño a bytes
            hashSizeInBytes = hashSizeInBits / 8;

            // Verifica que el valor es lo suficientemente largo
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return null;

            // Arreglo para almacenar la sal
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            // Copia la sal al arreglo
            for (int i = 0; i < saltBytes.Length; i++)
            {
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];
            }

            return Convert.ToBase64String(saltBytes);

            //return saltBytes;
        }

        public static bool validarClave(string pass, string hash)
        {
            if (pass.Equals(hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}