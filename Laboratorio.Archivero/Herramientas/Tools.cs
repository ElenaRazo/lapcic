using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Security.Cryptography;

namespace Laboratorio.Medios.Herramientas
{
    public class Empaquetado
    {
        public string Empaquetar(string Path, string Etiqueta, string TemporalPath, CompressionLevel Nivel)
        {
            //Ruta de la carpeta que sera a zip
            string carpeta = Path;
            //Ruta donde sera creado el archivo zip
            string archivoZip = TemporalPath + @"\" + Etiqueta + ".zip";
            //Verificacion de archivo
            if (File.Exists(archivoZip))
            {
                File.Delete(archivoZip);
            }
            //Se crea el archivo
           // ZipFile.CreateFromDirectory(carpeta, archivoZip, Nivel, false);
            return archivoZip;
        }

        public string Desempaquetar(string Path, string Etiqueta, string TemporalPath)
        {
            string carpeta = TemporalPath + @"\" + Etiqueta;
            return Desempaquetar(Path, carpeta);
        }

        public string Desempaquetar(string Path, string TemporalPath)
        {
            //Ruta del archivo zip a desempaquetar
            string archivoZip = Path;
            //Ruta donde sera desempaquetado
            string carpeta = TemporalPath;
            //Verificacion de directorio
            if (!Directory.Exists(carpeta))
            //{
            //    Directory.Delete(carpeta, true);
            //}
            //else
            {
                Directory.CreateDirectory(TemporalPath);
            }
            //Se extrae la informacion del zip
            //ZipFile.ExtractToDirectory(archivoZip, carpeta);
            return carpeta;
        }
    }
    
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
