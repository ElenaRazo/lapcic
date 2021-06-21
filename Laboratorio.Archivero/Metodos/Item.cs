using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Laboratorio.Libreria.BaseDatos;
using System.Security.Cryptography;
using Laboratorio.Medios.Herramientas;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Xml;
using Laboratorio.Medios.Items;

namespace Laboratorio.Medios.Metodos
{
    public class Item : Libreria.BaseClass.BaseObject
    {
        public string Dominio { get; set; }
        public Item(string ConnectionString) : base(ConnectionString)
        {
            Dominio = "Directorio/";
        }

        public Item(IBaseDatos Conexion) : base(Conexion)
        {
            Dominio = "Directorio/";
        }
        private long CrearAchivo(string Sumario, Items.TipoArchivo Tipo, string Nombre, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, string Ruta, string Tag)
        {
            long idArchivo = 0;

            //Creamos un codigo unico
            if (Tag == "")
                Tag = Guid.NewGuid().ToString();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Sumario", Sumario));
                prmtrs.Add(new SqlParameter("@ArchivoTipo", Tipo));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Codigo", Tag));
                prmtrs.Add(new SqlParameter("@Asegurado", Asegurado));
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Publico", Publico));
                prmtrs.Add(new SqlParameter("@Ruta", Ruta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivo_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idArchivo = Resultado.GetInt64(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idArchivo;
        }

        public Tuple<long, long> CrearMedio(string Sumario, Items.TipoArchivo Tipo, string Nombre, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
            string Esquema, long Tamanio, string CheckSum, int IdMediaTipo, bool EnDirectorio, string Tag)
        {
            var idArchivo = CrearAchivo(Sumario, Tipo, Nombre, Asegurado, IdOficina, IdUsuario, Publico, Ruta,Tag);
            long idMedio = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", idArchivo));
                prmtrs.Add(new SqlParameter("@Esquema", Esquema));
                prmtrs.Add(new SqlParameter("@Tamanio", Tamanio));
                prmtrs.Add(new SqlParameter("@CheckSum", CheckSum));
                prmtrs.Add(new SqlParameter("@MediaTipo_Id", IdMediaTipo));
                prmtrs.Add(new SqlParameter("@EnDirectorio", EnDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idMedio = Resultado.GetInt64(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            ActualizarMedioSumario(idArchivo);
            return new Tuple<long, long>(idArchivo, idMedio);
        }

        public Tuple<long, long> CrearDirectorio(string Sumario, Items.TipoArchivo Tipo, string Nombre, bool Asegudado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
            string Referencias, int IdDirectorioTipo, int IdDirecorioMarcador, bool Raiz, bool Fisico)
        {

            //Se crea el archivo para arbol principal
            var idArchivo = CrearAchivo(Sumario, Tipo, Nombre, Asegudado, IdOficina, IdUsuario, Publico, Ruta,"");
            //Se crea directorio vinculado al archivo
            long idDirectorio = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", idArchivo));
                prmtrs.Add(new SqlParameter("@Referencias", SqlDbType.Xml) { Value = Referencias });
                prmtrs.Add(new SqlParameter("@DirectorioTipo_Id", IdDirectorioTipo));
                prmtrs.Add(new SqlParameter("@DirectorioMarcador_Id", IdDirecorioMarcador));
                prmtrs.Add(new SqlParameter("@Raiz", Raiz));
                prmtrs.Add(new SqlParameter("@Fisico", Fisico));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDirectorio = Resultado.GetInt64(0);
                    }
                }
                
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            ActualizarDirectorioSumario(idArchivo);
            return new Tuple<long, long>(idArchivo, idDirectorio);
        }

        public long ActualizarMedio(long IdArchivo, string Esquema, long Tamanio, string CheckSum, int IdMediaTipo, bool EnDirectorio)
        {
            //
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id ", IdArchivo));
                prmtrs.Add(new SqlParameter("@Esquema", Esquema));
                prmtrs.Add(new SqlParameter("@Tamanio", Tamanio));
                prmtrs.Add(new SqlParameter("@CheckSum ", CheckSum));
                prmtrs.Add(new SqlParameter("@MediaTipo_Id ", IdMediaTipo));
                prmtrs.Add(new SqlParameter("@EnDirectorio ", EnDirectorio ? "1" : "0"));
                 base.conexion.EjecutarStoreProcedure("dbo.medio_upd", prmtrs);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }

            ActualizarMedioSumario(IdArchivo);
            return IdArchivo;
        }

        /*Actualizar*/
        public bool ActivarDesactivarArchivo(long IdArchivo, bool Disponible)
        {
            bool tmp = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? "1" : "0"));
                 base.conexion.EjecutarStoreProcedure("dbo.archivoDesactivar_upd", prmtrs);
                tmp = true;
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }

            //si se cambia se actualiza
            if (Disponible)
            {
                ActualizarMedioSumario(IdArchivo);
            }

            return tmp;
        }
        public bool EliminarArchivo(long IdArchivo, bool Disponible)
        {
            bool tmp = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? "1" : "0"));
                 base.conexion.EjecutarStoreProcedure("dbo.archivo_del", prmtrs);
                tmp = true;
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }

            //si se cambia se actualiza
            if (Disponible)
            {
                ActualizarMedioSumario(IdArchivo);
            }

            return tmp;
        }

        public bool ActualizarMedioSumario(long IdArchivo)
        {
            var medio = ConsultarMedioArchivo(IdArchivo);
            bool tmp = false;
            var sumario = JsonConvert.SerializeObject(medio);
            ActualizarSumario(IdArchivo, sumario);
            return tmp;
        }

        public bool ActualizarDirectorioSumario(long IdArchivo)
        {
            var dir = ConsultarDirectorioArchivo(IdArchivo);
            bool tmp = false;
            var sumario = JsonConvert.SerializeObject(dir);
            ActualizarSumario(IdArchivo, sumario);

            return tmp;
        }

        private void ActualizarSumario(long IdArchivo, string Sumario)
        {
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                prmtrs.Add(new SqlParameter("@Sumario", Sumario));
                 base.conexion.EjecutarStoreProcedure("dbo.archivoSumario_upd", prmtrs);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }

        /*vinculados*/
        public Tuple<long, long> CrearMedioDirectorio(long IdDirectorio, string Sumario, Items.TipoArchivo Tipo, string Nombre, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
           string Esquema, long Tamanio, string CheckSum, int IdMediaTipo, bool EnDirectorio)
        {
            //Se tiene que comprobar si el archivo ya existe
            var validacion = ValidarDirectorioMedio(IdDirectorio, Nombre, CheckSum, IdMediaTipo);
            long idMedio = 0;
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            if (validacion.Item1)
            {
                /*Solo si difiere cheksum*/
                if (CheckSum != validacion.Item6)
                {
                    //Se actualiza al archivo mas reciente
                    ActualizarMedio(validacion.Item3, Esquema, Tamanio, CheckSum, IdMediaTipo, EnDirectorio);
                }
                idMedio = validacion.Item5;
                //Se reactiva
                ActivarDesactivarArchivo(validacion.Item3, true);
            }
            else
            {
                resultado = CrearMedio(Sumario, Tipo, Nombre, Asegurado, IdOficina, IdUsuario, Publico, Ruta,
                 Esquema, Tamanio, CheckSum, IdMediaTipo, EnDirectorio,"");
                idMedio = resultado.Item2;

                long idDirectorioMedio = 0;
                try
                {
                    List<object> prmtrs = new List<object>();
                    prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                    prmtrs.Add(new SqlParameter("@Medio_Id", idMedio));
                    prmtrs.Add(new SqlParameter("@Compartido", "0"));
                    using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMedio_set", prmtrs))
                    {
                        while (Resultado.Read())
                        {
                            idDirectorioMedio = Resultado.GetInt64(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Dispose();
                    throw ex;
                }

            }

            return resultado;
        }

        public Tuple<long, long> CrearMedioDirectorioTag(long IdDirectorio, string Sumario, Items.TipoArchivo Tipo, string Nombre, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
          string Esquema, long Tamanio, string CheckSum, int IdMediaTipo, bool EnDirectorio, string Tag)
        {
            //Se tiene que comprobar si el archivo ya existe
            var validacion = ValidarDirectorioMedio(IdDirectorio, Nombre, CheckSum, IdMediaTipo);
            long idMedio = 0;
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            if (validacion.Item1)
            {
                /*Solo si difiere cheksum*/
                if (CheckSum != validacion.Item6)
                {
                    //Se actualiza al archivo mas reciente
                    ActualizarMedio(validacion.Item3, Esquema, Tamanio, CheckSum, IdMediaTipo, EnDirectorio);
                }
                idMedio = validacion.Item5;
                //Se reactiva
                ActivarDesactivarArchivo(validacion.Item3, true);
            }
            else
            {
                //aqui
                resultado = CrearMedio(Sumario, Tipo, Nombre, Asegurado, IdOficina, IdUsuario, Publico, Ruta,
                 Esquema, Tamanio, CheckSum, IdMediaTipo, EnDirectorio, Tag);
                idMedio = resultado.Item2;

                long idDirectorioMedio = 0;
                try
                {
                    List<object> prmtrs = new List<object>();
                    prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                    prmtrs.Add(new SqlParameter("@Medio_Id", idMedio));
                    prmtrs.Add(new SqlParameter("@Compartido", "0"));
                    using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMedio_set", prmtrs))
                    {
                        while (Resultado.Read())
                        {
                            idDirectorioMedio = Resultado.GetInt64(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Dispose();
                    throw ex;
                }

            }

            return resultado;
        }

        public Tuple<long, long> CrearDirectorioDirectorio(long IdDirectorio, string Sumario, Items.TipoArchivo Tipo, string Nombre, bool Asegudado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
            string Referencias, int IdDirectorioTipo, int IdDirecorioMarcador, bool Raiz, bool Fisico)
        {
            //Se tiene que comprobar si el archivo ya existe
            var validacion = ValidarDirectorioDirectorio(IdDirectorio, Nombre);

            long idDirectorio = 0;
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);

            if (validacion.Item1)
            {
                //Se limpia
                LimpiarDirectorio(validacion.Item4);

                //Se activa
                ActivarDesactivarArchivo(validacion.Item4, true);
                idDirectorio = validacion.Item5;
                resultado = new Tuple<long, long>(validacion.Item4, idDirectorio);
            }
            else
            {
                resultado = CrearDirectorio(Sumario, Tipo, Nombre, Asegudado, IdOficina, IdUsuario, Publico, Ruta, Referencias, IdDirectorioTipo, IdDirecorioMarcador, Raiz, Fisico);
                idDirectorio = resultado.Item2;
                long idDirectorioMedio = 0;
                try
                {
                    List<object> prmtrs = new List<object>();
                    prmtrs.Add(new SqlParameter("@DirectorioRaiz_Id", IdDirectorio));
                    prmtrs.Add(new SqlParameter("@DirectorioNodo_Id", idDirectorio));
                    using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioRelacion_set", prmtrs))
                    {
                        while (Resultado.Read())
                        {
                            idDirectorioMedio = Resultado.GetInt64(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Dispose();
                    throw ex;
                }
            }
            return resultado;
        }

        public Items.Archivo ConsultarArchivo(long IdArchivo)
        {
            Items.Archivo archivo = new Items.Archivo();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        archivo = ConstruirArchivo(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return archivo;
        }
        public List<Items.Archivo> ConsultarArchivoNombre(string Nombre, int IdOficina)
        {
            List<Items.Archivo> _archivos;
            Items.Archivo archivo = new Items.Archivo();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoNombre_get", prmtrs))
                {
                    _archivos = new List<Items.Archivo>();
                    while (Resultado.Read())
                    {
                        archivo = ConstruirArchivo(Resultado);
                        _archivos.Add(archivo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return _archivos;
        }

        public Items.Archivo ConsultarArchivoDirectorio(long IdDirectorio)
        {
            Items.Archivo archivo = new Items.Archivo();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoDirectorioDirectorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        archivo = ConstruirArchivo(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return archivo;
        }

        public List<Items.Archivo> ConsultarArchivosDirectorio(long IdArchivo)
        {
            List<Items.Archivo> archivos = new List<Items.Archivo>();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoDirectorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        archivos.Add(ConstruirArchivo(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }

            //Consultamos Directorio
            var directorios = ConsultarDirectorioDirectorio(IdArchivo);
            foreach (var Dir in directorios)
            {
                archivos.Add(Dir);
            }

            return archivos;
        }

        public List<Items.Archivo> ConsultarDirectorioDirectorio(long IdArchivo)
        {
            List<Items.Archivo> archivos = new List<Items.Archivo>();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.DirectorioDirectorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        archivos.Add(ConstruirArchivo(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return archivos;
        }

        public Items.Directorio ConsultarDirectorioArchivo(long IdArchivo)
        {
            var archivo = ConsultarArchivo(IdArchivo);
            Items.Directorio directorio = new Items.Directorio();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioArchivo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        directorio = ConstruirDirectorio(Resultado);
                    }
                }
                if (!System.IO.Directory.Exists(ConfigurationManager.ConnectionStrings["Directorio"].ToString() + directorio.Ruta))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.ConnectionStrings["Directorio"].ToString() + directorio.Ruta);
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return CompletarDirectorio(archivo, directorio);
        }

        public Items.Directorio ConsultarDirectorio(long IdDirectorio)
        {
            var archivo = ConsultarArchivoDirectorio(IdDirectorio);
            Items.Directorio directorio = new Items.Directorio();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        directorio = ConstruirDirectorio(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return CompletarDirectorio(archivo, directorio);
        }

        public Items.Medio ConsultarMedioArchivo(long IdArchivo)
        {
            var archivo = ConsultarArchivo(IdArchivo);
            Items.Medio medio = new Items.Medio();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioArchivo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        medio = ConstruirMedio(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return CompletarMedio(archivo, medio);
        }

        private Items.Archivo ConsultarArchivoBase(long IdArchivo)
        {
            Items.Archivo archivo = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        archivo = ConstruirArchivo(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return archivo;
        }

        private Items.Archivo ConstruirArchivo(DataTableReader Resultado)
        {
            //

            string baseUri = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
            Items.Archivo tmp = new Items.Archivo();
            tmp.IdArchivo = long.Parse(Resultado["idArchivo"].ToString());
            tmp.Sumario = Resultado["idSumario"].ToString();
            tmp.TipoArchivo = (Items.TipoArchivo)int.Parse(Resultado["tipoArchivoArchivo"].ToString());
            tmp.NombreOrigen = Resultado["nombreArchivo"].ToString();
            tmp.NombreCodigo = Resultado["codigoArchivo"].ToString();
            tmp.Asegurado = Resultado["archivoAsegudaro"].ToString() == "1";

            //Aqui validamos
            tmp.Oficina = new Usuarios.Descriptores.Atributos.OficinaBase();
            tmp.Oficina.Id = int.Parse(Resultado["idOficina"].ToString());
            //Validacion para el nodo
            if (Resultado.FieldCount > 15)
            {
                tmp.Oficina.Nombre = Resultado["nombreOficina"].ToString();
                tmp.Oficina.Descripcion = Resultado["descripcionOficina"].ToString();
                tmp.Oficina.Municipio = new Usuarios.Descriptores.Atributos.Municipio()
                {
                    Id = int.Parse(Resultado["idMunicipio"].ToString()),
                    Nombre = Resultado["nombreMunicipio"].ToString()
                };
            }
            tmp.Usuario = new Usuarios.Atributos.UsuarioBase()
            {
                Id = int.Parse(Resultado["idUsuario"].ToString())
            };
            tmp.Publico = Resultado["publicoArchivo"].ToString() == "1";
            tmp.FechaCreacion = DateTime.Parse(Resultado["fechaArchivo"].ToString());
            tmp.FechaModificacion = DateTime.Parse(Resultado["fechaModificacionArchivo"].ToString());
            tmp.Ruta = Resultado["rutaArchivo"].ToString();
            // tmp.URL= Resultado["urlArchivo"].ToString();
            //Contruimos uri
            if (Dominio != "")
            {
                tmp.URL = Dominio + (tmp.Ruta.Replace("\\", "/").Replace("//", "/"));
            }
            if (tmp.TipoArchivo == TipoArchivo.MEDIO) { }
            else
            {
                if (!System.IO.Directory.Exists(baseUri + tmp.Ruta))
                {
                    System.IO.Directory.CreateDirectory(baseUri + tmp.Ruta);
                }
            }
            tmp.Disponible = Resultado["disponibleArchivo"].ToString() == "1";
            return tmp;
        }

        private Items.Medio ConstruirMedio(DataTableReader Resultado)
        {
            return new Items.Medio()
            {
                IdMedio = int.Parse(Resultado["Id"].ToString()),
                Esquema = Resultado["Esquema"].ToString(),
                Suma = Resultado["Checksum"].ToString(),
                EnDirectorio = Resultado["EnDirectorio"].ToString() == "1",
                TamanioBytes = long.Parse(Resultado["Tamanio"].ToString()),
                TipoMedio = new Items.TipoMedio()
                {
                    IdTipoMedio = int.Parse(Resultado["MedioTipo_Id"].ToString()),
                    Nombre = Resultado["Nombre"].ToString(),
                    Descripcion = Resultado["Descripcion"].ToString(),
                    Mime = Resultado["Mime"].ToString(),
                    Extension = Resultado["Extension"].ToString(),
                    Disponible = Resultado["Disponible"].ToString() == "1"
                }
            };
        }

        private Items.Directorio ConstruirDirectorio(DataTableReader Resultado)
        {
            return new Items.Directorio()
            {
                IdDirectorio = long.Parse(Resultado["idDirectorio"].ToString()),
                Referencias = Resultado["referenciasDirectorio"].ToString(),
                Raiz = Resultado["referenciasDirectorio"].ToString() == "1",
                Fisico = Resultado["fisicoArchivo"].ToString() == "1",
                TipoDirectorio = new Items.TipoDirectorio()
                {
                    IdTipoDirectorio = int.Parse(Resultado["idDirectorioTipo"].ToString()),
                    Descripcion = Resultado["descripcionDirectorioTipo"].ToString(),
                    Disponible = Resultado["disponibleDirectorioTipo"].ToString() == "1"
                },
                MarcadorDirectorio = new Items.MarcadorDirectorio()
                {
                    IdMarcadorDirectorio = int.Parse(Resultado["idDirectorioMarcador"].ToString()),
                    Descripcion = Resultado["descripcionDirectorioMarcador"].ToString(),
                    Icono = Resultado["iconoDirectorioMarcador"].ToString(),
                    Color = Resultado["colorDirectorioMarcador"].ToString(),
                    Disponible = Resultado["disponibleDirectorioMarcador"].ToString() == "1"
                }
            };
        }

        private Items.Medio CompletarMedio(Items.Archivo Archivo, Items.Medio Medio)
        {
            Medio.Asegurado = Archivo.Asegurado;
            Medio.Disponible = Archivo.Disponible;
            Medio.FechaCreacion = Archivo.FechaCreacion;
            Medio.FechaModificacion = Archivo.FechaModificacion;
            Medio.IdArchivo = Archivo.IdArchivo;
            Medio.NombreCodigo = Archivo.NombreCodigo;
            Medio.NombreOrigen = Archivo.NombreOrigen;
            Medio.Oficina = Archivo.Oficina;
            Medio.Publico = Archivo.Publico;
            Medio.Ruta = Archivo.Ruta;
            Medio.Sumario = Archivo.Sumario;
            Medio.TipoArchivo = Archivo.TipoArchivo;
            Medio.Usuario = Archivo.Usuario;
            return Medio;
        }

        private Items.Directorio CompletarDirectorio(Items.Archivo Archivo, Items.Directorio Directorio)
        {
            Directorio.Asegurado = Archivo.Asegurado;
            Directorio.Disponible = Archivo.Disponible;
            Directorio.FechaCreacion = Archivo.FechaCreacion;
            Directorio.FechaModificacion = Archivo.FechaModificacion;
            Directorio.IdArchivo = Archivo.IdArchivo;
            Directorio.NombreCodigo = Archivo.NombreCodigo;
            Directorio.NombreOrigen = Archivo.NombreOrigen;
            Directorio.Oficina = Archivo.Oficina;
            Directorio.Publico = Archivo.Publico;
            Directorio.Ruta = Archivo.Ruta;
            Directorio.Sumario = Archivo.Sumario;
            Directorio.TipoArchivo = Archivo.TipoArchivo;
            Directorio.Usuario = Archivo.Usuario;
            return Directorio;
        }

        /*Validacion**/
        private Tuple<bool, long, long, string, int, string> ValidarDirectorioMedio(long IdDirectorio, string NombreArchivo, string Checksum, int MedioTipo)
        {

            Tuple<bool, long, long, string, int, string> tmp = new Tuple<bool, long, long, string, int, string>(false, 0, 0, "", 0, "");
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                prmtrs.Add(new SqlParameter("@NombreArchivo", NombreArchivo));
                prmtrs.Add(new SqlParameter("@CheckSum", Checksum));
                prmtrs.Add(new SqlParameter("@MedioTipo_Id", MedioTipo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioArchivoValidar_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var idDirectorio = long.Parse(Resultado["idDirectorio"].ToString());
                        var idArchivo = int.Parse(Resultado["idArchivo"].ToString());
                        var nombreArchivo = Resultado["NombreArchivo"].ToString();
                        var idMedio = int.Parse(Resultado["idMedio"].ToString());
                        var checksum = Resultado["checksumMedio"].ToString();
                        tmp = new Tuple<bool, long, long, string, int, string>(true, idDirectorio, idArchivo, nombreArchivo, idMedio, checksum);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tmp;
        }

        private Tuple<bool, long, long, long, long> ValidarDirectorioDirectorio(long IdDirectorio, string NombreArchivo)
        {

            Tuple<bool, long, long, long, long> tmp = new Tuple<bool, long, long, long, long>(false, 0, 0, 0, 0);
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                prmtrs.Add(new SqlParameter("@Nombre", NombreArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directoriodirectorioValidar_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var idArchivo = int.Parse(Resultado["idArchivo"].ToString());
                        var idDirectorio = long.Parse(Resultado["idDirectorio"].ToString());
                        var idArchivoNodo = int.Parse(Resultado["idArchivoNodo"].ToString());
                        var idDirectorioNodo = long.Parse(Resultado["idDirectorioNodo"].ToString());
                        tmp = new Tuple<bool, long, long, long, long>(true, idDirectorio, idArchivo, idArchivoNodo, idDirectorioNodo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tmp;
        }

        /*Inventario*/
        public Tuple<bool, string> InventariarDirectorio(string BaseUri, long IdArchivo, string InventarioOriginal)
        {
            var directorio = ConsultarDirectorioArchivo(IdArchivo);
            bool exito = false;
            string mensaje = "";
            DirectoryManager dir = new DirectoryManager(BaseUri);
            //Se verifica la integreidad de los archico
            var resultado = dir.VerificarInventario(directorio.Ruta, InventarioOriginal);
            if (resultado.Item1)
            {
                //Se tiene que eliminar el historial anterior en el caso de que exista
                LimpiarDirectorio(IdArchivo);
                AnalisisDirectorio(resultado.Item2, IdArchivo, BaseUri, InventarioOriginal);
                exito = true;
            }
            else
            {
                //Error no se puede inventariar
                mensaje = "El inventario original y la caperta destino no coinciden";
            }
            return new Tuple<bool, string>(exito, mensaje);
        }

        public Tuple<bool, string> InventariarDirectorio(string BaseUri, string Path, long IdArchivo)
        {
            var directorio = ConsultarDirectorioArchivo(IdArchivo);
            bool exito = false;
            string mensaje = "";
            DirectoryManager dir = new DirectoryManager(BaseUri);

            //Se tiene que eliminar el historial anterior en el caso de que exista
            LimpiarDirectorio(IdArchivo);
            AnalisisDirectorio(Path, IdArchivo, BaseUri);
            exito = true;

            return new Tuple<bool, string>(exito, mensaje);
        }


        /*Medio tipos*/
        public Items.TipoMedio DeterminarTipoMedio(string Extension)
        {
            Items.TipoMedio tipo = new Items.TipoMedio();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Extension", Extension));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.mediaTipoExtension_getset", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipo = ConstruirTipoMedio(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipo;
        }




        private Items.TipoMedio ConstruirTipoMedio(DataTableReader Resultado)
        {
            return new Items.TipoMedio()
            {
                IdTipoMedio = int.Parse(Resultado["Id"].ToString()),
                Nombre = Resultado["Nombre"].ToString(),
                Descripcion = Resultado["Descripcion"].ToString(),
                Mime = Resultado["Mime"].ToString(),
                Extension = Resultado["Extension"].ToString(),
                Disponible = Resultado["Disponible"].ToString() == "1"
            };
        }

        private void LimpiarDirectorio(long IdArchivo)
        {
            var archivos = ConsultarArchivosDirectorio(IdArchivo);
            foreach (var Archivo in archivos)
            {
                ActivarDesactivarArchivo(Archivo.IdArchivo, false);
            }
        }

        /*Iteracion de sistema de archivos*/
        private void AnalisisDirectorio(string Uri, long IdArchivo, string BaseUri, string InventarioOriginal)
        {

            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(Uri);
            System.IO.DirectoryInfo[] dirs = info.GetDirectories();
            System.IO.FileInfo[] archs = info.GetFiles();
            ComprobacionArchivos comprobacion = new ComprobacionArchivos();
            bool esJvl = false;
            string dataJvl = "";

            var directorio = ConsultarDirectorioArchivo(IdArchivo);

            //Analizamos si es una caperta de javs
            var jvl = from x in archs
                      where x.Extension == ".jvl"
                      select x;

            if (jvl.Count() > 0)
            {
                var resultado = AnalizarJvl(jvl.First());
                esJvl = resultado.Item1;
                dataJvl = resultado.Item2;
            }
            //Fin de validacion javs

            foreach (var File in archs)
            {
                var tipo = DeterminarTipoMedio(File.Extension.ToLower().Trim());

                //Parse uri
                Uri baseuri = new System.Uri(BaseUri, UriKind.RelativeOrAbsolute);

                var newUri = File.FullName.ToLower().Replace(baseuri.LocalPath.ToLower().Replace("\\\\", "\\"), "");
                var sumario = "";

                //Carpeta javs y solo si es encontrado
                if (esJvl && File.Extension == ".mp4")
                {
                    sumario = BuscarSumarioJvl(dataJvl, File.Name, archs, InventarioOriginal);
                    if (sumario.Trim() == "")
                    {
                        //Lo parsea como un medio normal
                        sumario = BuscarSumario(File.Name, InventarioOriginal);
                    }
                }
                else if (File.Extension == ".mp4")
                {
                    sumario = BuscarSumario(File.Name, InventarioOriginal);
                }

                CrearMedioDirectorio(directorio.IdDirectorio, sumario, Items.TipoArchivo.MEDIO, File.Name, true, directorio.Oficina.Id, directorio.Usuario.Id, false, newUri, sumario, File.Length, comprobacion.CalcularSum(File.FullName), tipo.IdTipoMedio, true);
            }

            foreach (var Dir in dirs)
            {
                //Se crea el directorio
                var newUri = Dir.FullName.ToLower().Replace(BaseUri.ToLower(), "");
                var resultado = CrearDirectorioDirectorio(directorio.IdDirectorio, "", Items.TipoArchivo.DIRECTORIO, Dir.Name, false, directorio.Oficina.Id, directorio.Usuario.Id, false, newUri, "", 2, 1, true, true);
                AnalisisDirectorio(Dir.FullName, resultado.Item1, BaseUri, InventarioOriginal);
            }
        }

        private void AnalisisDirectorio(string Uri, long IdArchivo, string BaseUri)
        {

            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(BaseUri + Uri);
            System.IO.FileInfo[] archs = info.GetFiles();
            ComprobacionArchivos comprobacion = new ComprobacionArchivos();
            bool esJvl = false;
            string dataJvl = "";

            var directorio = ConsultarDirectorioArchivo(IdArchivo);

            foreach (var File in archs)
            {
                var tipo = DeterminarTipoMedio(File.Extension.ToLower().Trim());
                var newUri = File.FullName.ToLower().Replace(new DirectoryInfo(BaseUri).FullName.ToLower(), "");
                var sumario = "";

                CrearMedioDirectorio(directorio.IdDirectorio, sumario, Items.TipoArchivo.MEDIO, File.Name, true, directorio.Oficina.Id, directorio.Usuario.Id, false, newUri, sumario, File.Length, comprobacion.CalcularSum(File.FullName), tipo.IdTipoMedio, true);
            }
        }

        /*Manejo de archivos*/
        public bool CrearDirectorioFisico(Items.Directorio Directorio, string BaseUri, string Nombre)
        {
            bool resultado = false;
            var directorio = Directorio;
            var newUri = BaseUri + directorio.Ruta + "\\" + Nombre;
            var DirUri = directorio.Ruta + "\\" + Nombre;
            if (!System.IO.Directory.Exists(newUri))
            {
                System.IO.Directory.CreateDirectory(newUri);
            }
            resultado = true;
            return resultado;
        }
        public Tuple<long, long> CrearDirectorioDirectorioReal(Items.Directorio Directorio, string BaseUri, string Nombre, int IdOficina, int IdUsuario)
        {
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            var directorio = Directorio;
            var newUri = BaseUri + directorio.Ruta + "\\" + Nombre;
            var DirUri = directorio.Ruta + "\\" + Nombre;
            resultado = CrearDirectorioDirectorio(directorio.IdDirectorio, "", Items.TipoArchivo.DIRECTORIO, Nombre, false, IdOficina, IdUsuario, true, DirUri, "", 1, 1, true, true);
            if (!System.IO.Directory.Exists(newUri))
            {
                System.IO.Directory.CreateDirectory(newUri);
            }
            return resultado;
        }
        public Tuple<long, long> CrearMedioDirectorioReal(Items.Directorio Directorio, string BaseUri, string PathFTP, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, bool EnDirectorio)
        {
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            if (System.IO.File.Exists(PathFTP))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(PathFTP);
                var suma = new ComprobacionArchivos().CalcularSum(info.FullName);
                var tipo = DeterminarTipoMedio(info.Extension.ToLower().Trim());
                var directorio = Directorio;
                var destino = BaseUri + directorio.Ruta + "\\" + info.Name;
                var newUri = directorio.Ruta + "\\" + info.Name;

                if (!System.IO.Directory.Exists(BaseUri + directorio.Ruta))
                {
                    System.IO.Directory.CreateDirectory(BaseUri + directorio.Ruta);
                }
                //Se crea
                resultado = CrearMedioDirectorio(directorio.IdDirectorio, "", Items.TipoArchivo.MEDIO, info.Name, Asegurado, IdOficina, IdUsuario, Publico, newUri, "", info.Length, suma, tipo.IdTipoMedio, EnDirectorio);
                System.IO.File.Move(PathFTP, destino);
            }
            return resultado;
        }
        public Tuple<long, long> CrearMedioDirectorioRealSolicitudes(Items.Directorio Directorio, string BaseUri, string PathFTP, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, bool EnDirectorio)
        {
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            if (System.IO.File.Exists(PathFTP))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(PathFTP);
                var suma = new ComprobacionArchivos().CalcularSum(info.FullName);
                var tipo = DeterminarTipoMedio(info.Extension.ToLower().Trim());
                var directorio = Directorio;
                var destino = BaseUri + directorio.Ruta + "\\" + info.Name;
                var newUri = directorio.Ruta + "\\" + info.Name;

                if (!System.IO.Directory.Exists(BaseUri + directorio.Ruta))
                {
                    System.IO.Directory.CreateDirectory(BaseUri + directorio.Ruta);
                }
                Guid guid = new Guid();
                bool existe = false;
                if (System.IO.File.Exists(destino))
                {
                    existe = true;
                    newUri = directorio.Ruta + "\\" + guid + "_" + info.Name;
                    destino = BaseUri + directorio.Ruta + "\\" + guid + "_" + info.Name;
                }
                //Se crea
                resultado = CrearMedioDirectorio(directorio.IdDirectorio, "", Items.TipoArchivo.MEDIO, (existe == true ? guid + "_" + info.Name : info.Name), Asegurado, IdOficina, IdUsuario, Publico, newUri, "", info.Length, suma, tipo.IdTipoMedio, EnDirectorio);
                System.IO.File.Move(PathFTP, destino);
            }
            return resultado;
        }

        public Tuple<long, long> CrearDirectorioDirectorioVirtual(Items.Directorio Directorio, string BaseUri, string Nombre, int IdOficina, int IdUsuario)
        {
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            var directorio = Directorio;
            var newUri = BaseUri + directorio.Ruta + "\\" + Nombre;
            var DirUri = directorio.Ruta + "\\" + Nombre;
            resultado = CrearDirectorioDirectorio(directorio.IdDirectorio, "", Items.TipoArchivo.DIRECTORIO, Nombre, false, IdOficina, IdUsuario, true, DirUri, "", 1, 1, true, false);
            return resultado;
        }

        public Tuple<long, long> CrearMedioDirectorioVirtual(Items.Directorio Directorio, string BaseUri, string PathFTP, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, bool EnDirectorio)
        {
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            if (System.IO.File.Exists(PathFTP))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(PathFTP);
                var suma = new ComprobacionArchivos().CalcularSum(info.FullName);
                var tipo = DeterminarTipoMedio(info.Extension.ToLower().Trim());
                var directorio = Directorio;

                /*se creara un id por id de oficia*/
                var destino = BaseUri + "\\" + IdOficina + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month.ToString("00");
                if (!System.IO.Directory.Exists(destino))
                {
                    System.IO.Directory.CreateDirectory(destino);
                }

                //var destino = BaseUri + directorio.Ruta +"\\" + info.Name;
                destino = destino + "\\" + info.Name;
                var newUri = directorio.Ruta + "\\" + info.Name;
                //Se crea
                resultado = CrearMedioDirectorio(directorio.IdDirectorio, "", Items.TipoArchivo.MEDIO, info.Name, Asegurado, IdOficina, IdUsuario, Publico, newUri, "", info.Length, suma, tipo.IdTipoMedio, EnDirectorio);

                System.IO.File.Move(PathFTP, destino);

            }
            else
            {
                throw new Exception("No encontrado: " + PathFTP);
            }

            return resultado;
        }

        /*Analizar jvl (SOLO JAVS)*/
        private Tuple<bool, string> AnalizarJvl(System.IO.FileInfo Jvl)
        {
            string dataJvl = "";
            bool exito = false;
            try
            {
                string data = System.IO.File.ReadAllText(Jvl.FullName);
                XDocument xmlMetas = XDocument.Load(new StringReader(data));
                var query = from c in xmlMetas.Descendants().Elements("Sessions")
                            select c;


                if (query.Count() > 0)
                {
                    exito = true;
                    dataJvl = "<Sesiones>";
                    foreach (var ObjData in query)
                    {
                        dataJvl += ObjData.ToString();
                    }
                    dataJvl += "</Sesiones>";
                }
            }
            catch { dataJvl = ""; /* do nothing */ }
            return new Tuple<bool, string>(exito, dataJvl);
        }

        private string BuscarSumarioJvl(string JvlData, string NombreVideo, System.IO.FileInfo[] Archs, string InventarioOriginal)
        {
            var provider = CultureInfo.CurrentCulture;
            string sumario = "";
            try
            {
                string estampa = NombreVideo.ToLower().Replace(".mp4", "").Replace("_", "T").Replace(".", ":");
                var splitter = estampa.Split(Convert.ToChar("T"));
                if (splitter.Count() > 1)
                {
                    estampa = splitter[0] + "T" + (splitter[1].Length > 7 ? splitter[1].Substring(0, 8) : splitter[1]);
                }

                DateTime tstm = DateTime.ParseExact(estampa, "yyyy-MM-ddTHH:mm:ss", provider);
                DateTime limitTstm = DateTime.MaxValue;

                for (int cont = 0; cont <= Archs.Count() - 1; cont++)
                {
                    if (Archs[cont].Name.ToLower() == NombreVideo)
                    {
                        if (cont < Archs.Count() - 1)
                        {
                            if (Archs[cont + 1].Extension == ".mp4")
                            {
                                string limtEstampa = Archs[cont + 1].Name.ToLower().Replace(".mp4", "").Replace("_", "T").Replace(".", ":");
                                splitter = limtEstampa.Split(Convert.ToChar("T"));

                                if (splitter.Count() > 1)
                                {
                                    limtEstampa = splitter[0] + "T" + (splitter[1].Length > 7 ? splitter[1].Substring(0, 8) : splitter[1]);
                                }

                                limitTstm = DateTime.ParseExact(limtEstampa, "yyyy-MM-ddTHH:mm:ss", provider);
                                break;
                            }
                            else
                            {
                                limitTstm = DateTime.MaxValue;
                            }
                        }
                    }
                }

                XDocument xmlMetas = XDocument.Load(new StringReader(JvlData));
                var query = from c in xmlMetas.Descendants().Elements("Event")
                            select c;

                if (query.Count() > 0)
                {
                    sumario += "<Eventos>";
                    sumario += "<Duracion>" + BuscarDuracion(NombreVideo, InventarioOriginal) + "</Duracion>";
                    foreach (var ObjData in query)
                    {
                        var Name = ObjData.Element("Name").Value.ToString();
                        var TimeStamp = ObjData.Element("TimeStamp").Value.ToString();
                        var SystemEvent = ObjData.Element("IsSystemEvent").Value.ToString();
                        DateTime newtstm = DateTime.Parse(TimeStamp);
                        if ((newtstm.Ticks >= tstm.Ticks) && (newtstm.Ticks < limitTstm.Ticks))
                        {
                            sumario += "<Evento>";
                            sumario += "<Nombre>" + Name + "</Nombre>";
                            sumario += "<Sistema>" + SystemEvent + "</Sistema>";
                            sumario += "<Estampa>" + newtstm.Ticks + "</Estampa>";
                            sumario += "</Evento>";
                        }
                    }
                    sumario += "</Eventos>";

                    //Convierte a json
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(sumario);
                    sumario = JsonConvert.SerializeXmlNode(doc);
                }

            }
            catch { sumario = ""; /* do nothing */ }
            return sumario;
        }

        /*Todos los demas*/
        private string BuscarSumario(string NombreVideo, string InventarioOriginal)
        {
            string sumario = "";
            try
            {
                sumario += "<Eventos>";
                sumario += "<Duracion>" + BuscarDuracion(NombreVideo, InventarioOriginal) + "</Duracion>";
                sumario += "</Eventos>";
                //Convierte a json
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sumario);
                sumario = JsonConvert.SerializeXmlNode(doc);
            }
            catch { sumario = ""; /* do nothing */ }
            return sumario;
        }

        private string BuscarDuracion(string NombreArchivo, string InventarioOriginal)
        {
            string duracion = "0";
            try
            {
                XDocument Archivos = XDocument.Load(new StringReader(InventarioOriginal));
                var query = from c in Archivos.Descendants().Elements("Archivo")
                            select c;
                if (query.Count() > 0)
                {
                    foreach (var Archivo in query)
                    {
                        var Name = Archivo.Element("Nombre").Value.ToString();
                        if (NombreArchivo.ToLower() == Name.ToLower())
                        {
                            if (Archivo.HasAttributes)
                            {
                                duracion = Archivo.Attribute("Duracion").Value.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            catch { duracion = "0"; /* do nothing */ }
            return duracion;
        }
    }
}
