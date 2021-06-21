using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TribunalAdministrativo.Medios.Herramientas;
using System.Xml.Linq;

namespace TribunalAdministrativo.Medios
{
    public class FileManager
    {
        public FileManager()
        {
        }

        public bool CrearDirectorio(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            return Directory.Exists(Path);
        }

        public bool CrearMedio(string Path, byte[] Data)
        {
            if (!File.Exists(Path))
            {
                File.WriteAllBytes(Path, Data);
            }
            return File.Exists(Path);
        }

        public bool MoverDirectorio(string Path, string NuevoPath)
        {
            if (Directory.Exists(Path))
            {
                Directory.Move(Path, NuevoPath);
            }
            return Directory.Exists(NuevoPath);
        }

        public bool MoverMedio(string Path, string NuevoPath)
        {
            if (File.Exists(Path))
            {
                File.Move(Path, NuevoPath);
            }
            return File.Exists(NuevoPath);
        }

        public bool RevisarDirectorio(string Path)
        {
            return System.IO.Directory.Exists(Path);
        }

        public bool RevisarArchivo(string Path)
        {
            return System.IO.File.Exists(Path);
        }

    }
}
