using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO;
using Laboratorio.Medios.Herramientas;

namespace Laboratorio.Medios.Metodos
{
    public class DirectoryManager : FileManager
    {
        string rootUri = String.Empty;

        public DirectoryManager(string RootUri)
        {
            rootUri = RootUri;
        }

        public new Tuple<bool, string> CrearDirectorio(string Path)
        {
            var newUri = rootUri + Path;
            return new Tuple<bool, string>(base.CrearDirectorio(newUri), newUri);
        }

        public new bool MoverDirectorio(string PathOrigen, string Path)
        {
            var newUri = rootUri + Path;
            return base.MoverDirectorio(PathOrigen, newUri);
        }

        public new Tuple<bool, string> RevisarDirectorio(string Path)
        {
            var newUri = rootUri + Path;
            return new Tuple<bool, string>(base.RevisarDirectorio(newUri), newUri);
        }

        public new Tuple<bool, string> RevisarArchivo(string Path)
        {
            var newUri = rootUri + Path;
            return new Tuple<bool, string>(base.RevisarArchivo(newUri), newUri);
        }

        /**/
        public Tuple<bool, string> VerificarInventario(string Directoriobase, string Inventario)
        {
            bool tmp = false;
            List<string> uris = new List<string>();
            var dir = RevisarDirectorio(Directoriobase);
            var inventario = new Herramientas.Inventario();
            if (dir.Item1)
            {

                string[] archivos = inventario.CrearInventario(dir.Item2);

                XDocument xmlMetas = XDocument.Load(new StringReader(Inventario));
                var query = from c in xmlMetas.Descendants().Elements("Archivo")
                            select c;

                ComprobacionArchivos comprobacion = new ComprobacionArchivos();
                if (archivos.Count() == query.Count())
                {
                    if (query.Count() > 0)
                    {
                        foreach (var ObjData in query)
                        {
                            var numero = ObjData.Element("Numero").Value.ToString();
                            var nombre = ObjData.Element("Nombre").Value.ToString();
                            var Path = ObjData.Element("Path").Value.ToString();
                            var Extension = ObjData.Element("Extension").Value.ToString();
                            var Tamanio = ObjData.Element("Tamanio").Value.ToString();
                            var Suma = ObjData.Element("Suma").Value.ToString();
                            try
                            {
                                //lo buscamos en el inventario
                                var file = "";
                                foreach (var Arc in archivos)
                                {
                                    FileInfo info = new FileInfo(Arc);
                                    if (info.Name.ToLower() == nombre.ToLower())
                                    {
                                        file = Arc;
                                        break;
                                    }
                                }

                                if (file != "")
                                {
                                    var checksum = comprobacion.CalcularSum(file);
                                    if (Suma == checksum)
                                    {
                                        uris.Add(file);
                                        tmp = true;
                                    }
                                    else
                                    {
                                        tmp = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    tmp = false;
                                    break;
                                }
                            }
                            catch
                            {
                                //Si hay fallas de intregiudad se devuelve
                                tmp = false;
                                break;
                            }
                        }
                    }
                }
            }

            //sacamos los directorios
            if (tmp)
            {
                inventario.CrearInventarioDirectorio(dir.Item2);
            }

            return new Tuple<bool, string>(tmp, dir.Item2);
        }



    }

}
