using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Security.Cryptography;

namespace laboratorio.Administracion.Herramientas
{
    public class Inventario
    {
        public string[] CrearInventario(string Path)
        {
            string[] archivos = Directory.GetFiles(Path, "*.*", System.IO.SearchOption.AllDirectories);
            return archivos;
        }

        public string[] CrearInventario(string Path, string Patron)
        {
            string[] archivos = Directory.GetFiles(Path, Patron, System.IO.SearchOption.AllDirectories);
            return archivos;
        }

        public string[] CrearInventarioDirectorio(string Path)
        {
            string[] archivos = Directory.GetDirectories(Path, "*.*", System.IO.SearchOption.AllDirectories);
            return archivos;
        }

        public bool ComprobarInventario(string Path, string xml)
        {
            return true;
        }

    }

    public class ComprobacionArchivos
    {
        public string CalcularSum(string Path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(Path))
                {
                    string md5Check = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    return md5Check.Trim().ToLower();
                }
            }
        }
        public string CrearMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }

}
