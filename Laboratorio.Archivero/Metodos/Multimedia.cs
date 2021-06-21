using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Laboratorio.Medios.Atributos.Items;
using Laboratorio.Usuarios.Atributos;
using Laboratorio.Usuarios.Descriptores.Atributos;
using Laboratorio.Usuarios.Metodos;
using Laboratorio.Libreria.BaseDatos;

namespace Laboratorio.Medios.Metodos
{
    public class Multimedia : Libreria.BaseClass.BaseObject
    {
        public string Dominio { get; set; }

        Laboratorio.Usuarios.Metodos.UsuarioSeguridad us = new Laboratorio.Usuarios.Metodos.UsuarioSeguridad("Data Source=25.3.253.103;Initial Catalog=Usuarios;Persist Security Info=True;User ID=sa;Password=Rush01");
        public Multimedia(string ConnectionString) : base(ConnectionString)
        {

        }

        public object CrearDirectorioFisico(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }

        public Multimedia(IBaseDatos Conexion) : base(Conexion)
        {

        }
        //////////archivo
        public int CrearArchivo(string Sumario, int ArchivoTipo, string Nombre, bool Asegurado, int OficinaId, int UsuarioId, bool Publico, string Ruta)
        {
            int idArchivo = 0;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Sumario", Sumario));
                prmtrs.Add(new SqlParameter("@ArchivoTipo", ArchivoTipo));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Asegurado", Asegurado));
                prmtrs.Add(new SqlParameter("@Oficina_Id", OficinaId));
                prmtrs.Add(new SqlParameter("@Usuario_Id", UsuarioId));
                prmtrs.Add(new SqlParameter("@Publico", Publico));
                prmtrs.Add(new SqlParameter("@Ruta", Ruta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivo_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idArchivo = Resultado.GetInt32(0);
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
        public Archivo ConsultarArchivo(long IdArchivo)
        {
            Archivo archivo = null;
            try
            {
                List<Object> prmtrs = new List<object>();
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
        public List<Archivo> ConsultarArchivoColleccion(bool Disponible)
        {
            List<Archivo> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoColeccion_get", prmtrs))
                {
                    coleccion = new List<Archivo>();
                    while (true)
                    {
                        var archivo = ConstruirArchivo(Resultado);
                        coleccion.Add(archivo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }   
            return coleccion;
        }
        private Archivo ConstruirArchivo(DataTableReader Resultado)
        {
            Archivo tmp = new Archivo();
            
            tmp.Id = long.Parse(Resultado["idArchivo"].ToString());
            tmp.Sumario = Resultado["sumarioArchivo"].ToString();
            tmp.ArchivoTipo = (ArchivoTipo)int.Parse(Resultado["idarchivoTipoArchivo"].ToString());
            tmp.Nombre = Resultado["nombreArchivo"].ToString();
            tmp.Asegurado = Resultado["aseguradoArchivo"].ToString() == "1" ? true : false;
            tmp.OficinaId = new Oficina() { Id = int.Parse(Resultado["oficinaArchivo"].ToString()) };
            tmp.Usuario = new Usuario() { Id = int.Parse(Resultado["usuarioArchivo"].ToString()) };
            tmp.Publico = Resultado["publicoArchivo"].ToString() == "1" ? true : false;
            tmp.Fecha = DateTime.Parse(Resultado["fechaArchivo"].ToString());
            tmp.FechaModificacion = DateTime.Parse(Resultado["fechaModificacionArchivo"].ToString());
            tmp.Ruta = Resultado["rutaArchivo"].ToString();
            if (Dominio != "")
            {
                tmp.Url = Dominio + tmp.Ruta.Replace("\\", "/").Replace("//", "/");
            }
            tmp.Disponible = Resultado["disponibleArchivo"].ToString() == "1" ? true : false;
            return tmp;
        }
        public bool ActualizarArchivo(int Idarchivo, string Sumario, int ArchivoTipo, string Nombre, bool Asegurado, int OficinaId, int UsuarioId, bool Publico, DateTime Fecha, DateTime FechaModificacion, string Ruta, string Url)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", Idarchivo));
                prmtrs.Add(new SqlParameter("@Sumario", Sumario));
                prmtrs.Add(new SqlParameter("@ArchivoTipo", ArchivoTipo));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Asegurado", Asegurado));
                prmtrs.Add(new SqlParameter("@Oficina_Id", OficinaId));
                prmtrs.Add(new SqlParameter("@Usuario_Id", UsuarioId));
                prmtrs.Add(new SqlParameter("@Publico", Publico));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@FechaModificacion", FechaModificacion));
                prmtrs.Add(new SqlParameter("@Ruta", Ruta));
                prmtrs.Add(new SqlParameter("@Url", Url));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivo_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarArchivo(long IdArchivo)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivo_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        /////Medio
        public int CrearMedio(int ArchivoId, string Esquema, bool Tamanio, string Checksum, int MedioTipoId, bool EnDirectorio)

        {
            int idMedio = 0;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", ArchivoId));
                prmtrs.Add(new SqlParameter("@Esquema", Esquema));
                prmtrs.Add(new SqlParameter("@Tamanio", Tamanio));
                prmtrs.Add(new SqlParameter("@CheckSum", Checksum));
                prmtrs.Add(new SqlParameter("@MedioTipo_Id", MedioTipoId));
                prmtrs.Add(new SqlParameter("@EnDirectorio", EnDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idMedio = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idMedio;
        }
        public Medio ConsultaMedio(int IdMedio)
        {
            Medio medio = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Medio_Id", IdMedio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medio_get", prmtrs))
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
            return medio;
        }
        private Medio ConstruirMedio(DataTableReader Resultado)
        {
            return new Medio()
            {
                Id = int.Parse(Resultado["idMedio"].ToString()),
                Esquema = Resultado["esquemaMedio"].ToString(),
                Tamanio = long.Parse(Resultado["tamanioMedio"].ToString()),
                CheckSum = Resultado["checkSumMedio"].ToString(),
                EnDirectorio = Resultado["enDirectorio"].ToString() == "1" ? true : false,
                Archivo = new Archivo()
                {
                    Id = int.Parse(Resultado["idArchivo"].ToString()),
                    Sumario = Resultado["sumarioArchivo"].ToString(),
                    ArchivoTipo = (ArchivoTipo)int.Parse(Resultado["idarchivoTipoArchivo"].ToString()),
                    Nombre = Resultado["nombreArchivo"].ToString(),
                    Asegurado = Resultado["aseguradoArchivo"].ToString() == "1" ? true : false,
                    OficinaId = new Oficina() { Id = int.Parse(Resultado["oficinaArchivo"].ToString()) },
            Usuario = new Usuario() { Id = int.Parse(Resultado["usuarioArchivo"].ToString())},
                    Publico = Resultado["publicoArchivo"].ToString() == "1" ? true : false,
                    Fecha = DateTime.Parse(Resultado["fechaArchivo"].ToString()),
                    FechaModificacion = DateTime.Parse(Resultado["fechaModificacionArchivo"].ToString()),
                    Ruta = Resultado["rutaArchivo"].ToString(),
                    Url = Resultado["urlArchivo"].ToString(),
                    Disponible = Resultado["disponibleArchivo"].ToString() == "1" ? true : false,
                },
                MedioTipo = new MedioTipo() {
                    Id = int.Parse(Resultado["idMedioTipo"].ToString()),
                    Nombre = Resultado["nombreMedioTipo"].ToString(),
                    Descripcion = Resultado["descripcionMedioTipo"].ToString(),
                    Mime = Resultado["mimeMedioTipo"].ToString(),
                    Extension = Resultado["extensionMedioTipo"].ToString(),
                    Disponible = Resultado["disponibleMedioTipo"].ToString() == "1" ? true : false,
                }
            };
        }
        public bool ActualizarMedio(int IdMedio, int ArchivoId, string Esquema, bool Tamanio, string Checksum, int MedioTipoId, bool EnDirectorio)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Medio_Id", IdMedio));
                prmtrs.Add(new SqlParameter("@Archivo_Id", ArchivoId));
                prmtrs.Add(new SqlParameter("@Esquema", Esquema));
                prmtrs.Add(new SqlParameter("@Tamanio", Tamanio));
                prmtrs.Add(new SqlParameter("@CheckSum", Checksum));
                prmtrs.Add(new SqlParameter("@MedioTipo_Id", MedioTipoId));
                prmtrs.Add(new SqlParameter("@EnDirectorio", EnDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medio_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        //////////////MedioAsegurado
        public int CrearMedioAsegurado(string CuerpoPrincipal, int IdMedio, DateTime Fecha, int IdUsuario, ArchivoTipo Tipo, long Referencia)
        {
            int idMedioAsegurado = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@CuerpoPrincipal", CuerpoPrincipal));
                prmtrs.Add(new SqlParameter("@Medio_Id", IdMedio));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@ArchivoTipo", Tipo));
                prmtrs.Add(new SqlParameter("@Referencia", Referencia));

                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioAsegurado_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idMedioAsegurado = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idMedioAsegurado;
        }
        public MedioAsegurado ConsultarMedioAsegurado(int IdMedioAsegurado)
        {
            MedioAsegurado medioUsuario = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@MedioAsegurado_Id", IdMedioAsegurado));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioAsegurado_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        medioUsuario = ConstruirMedioAsegurado(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return medioUsuario;
        }
        private MedioAsegurado ConstruirMedioAsegurado(DataTableReader Resultado)
        {

            return new MedioAsegurado()
            {
                Id = int.Parse(Resultado["idMedioAsegurado"].ToString()),
                CuerpoPrincipal = Resultado["cuerpoPrincipalMedioAsegurado"].ToString(),
                Fecha = DateTime.Parse(Resultado["fechaMedioAsegurado"].ToString()),
                Usuario = new Usuario(){Id = int.Parse(Resultado["idUsuario"].ToString()),},
                ArchivoTipo = (ArchivoTipo)int.Parse(Resultado["archivoTipoMedioAsegurado"].ToString()),
                Referencia = long.Parse(Resultado["referenciaMedioAsegurado"].ToString()),
                Medio = new Medio()
                {
                    Id = int.Parse(Resultado["idMedio"].ToString()),
                    Esquema = Resultado["esquemaMedio"].ToString(),
                    Tamanio = long.Parse(Resultado["tamanioMedio"].ToString()),
                    CheckSum = Resultado["checkSumMedio"].ToString(),
                    EnDirectorio = Resultado["enDirectorio"].ToString() == "1" ? true : false,
                },                
            };

        }
        public bool ActualizarMedioAsegurado(int MedioAsegurado_Id, string CuerpoPrincipal, int IdMedio, DateTime Fecha, int IdUsuario, int ArchivoTipo, long Referencia)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@MedioAsegurado_Id", MedioAsegurado_Id));
                prmtrs.Add(new SqlParameter("@CuerpoPrincipal", CuerpoPrincipal));
                prmtrs.Add(new SqlParameter("@Medio_Id", IdMedio));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@ArchivoTipo", ArchivoTipo));
                prmtrs.Add(new SqlParameter("@Referencia", Referencia));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioAsegurado_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        /////MedioTipo
        public int CrearMedioTipo(int MedioId, string Nombre, string Descripcion, string Mime, string Extension, bool Disponible)

        {
            int idMedioTipo = 0;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@Extension", Extension));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioTipo_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idMedioTipo = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idMedioTipo;
        }
        public MedioTipo ConsultaMedioTipo(int IdMedioTipo)
        {
            MedioTipo medioTipo = new MedioTipo();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@MedioTipo_Id", IdMedioTipo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioTipo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        medioTipo = ConstruirMedioTipo(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return medioTipo;
        }
        public List<MedioTipo> ConsultarMedioTipoColeccion(bool Disponible)
        {
            List<MedioTipo> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioTipoColeccion_get", prmtrs))
                {
                    coleccion = new List<MedioTipo>();
                    while (true)
                    {
                        var medioTipo = ConstruirMedioTipo(Resultado);
                        coleccion.Add(medioTipo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return coleccion;
        }
        private MedioTipo ConstruirMedioTipo(DataTableReader Resultado)
        {
            return new MedioTipo()
            {
                Id = int.Parse(Resultado["idMedioTipo"].ToString()),
                Nombre = Resultado["nombreMedioTipo"].ToString(),
                Descripcion = Resultado["descripcionMedioTipo"].ToString(),
                Mime = Resultado["mimeMedioTipo"].ToString(),
                Extension = Resultado["extensionMedioTipo"].ToString(),
                Disponible = Resultado["disponibleMedioTipo"].ToString() == "1" ? true : false,
            };
        }
        public bool ActualizarMedioTipo(int IdMedio, string Nombre, string Descripcion, string Mime, string Extension)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("idMedioTipo", IdMedio));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@Extension", Extension));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioTipo_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarMedioTipo(int IdMedioTipo)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@MedioTipo_Id", IdMedioTipo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioTipo_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        //////MedioBitacora
        public int CrearMedioBitacora(int IdMedio, DateTime FechaAcceso, string DatosAcces)

        {
            int idMedioBitacora = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Medio_id", IdMedio));
                prmtrs.Add(new SqlParameter("@FechaAcceso", FechaAcceso));
                prmtrs.Add(new SqlParameter("@DatosAcceso", DatosAcces));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioBitacora_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idMedioBitacora = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idMedioBitacora;
        }
        public MedioBitacora ConsultarMedioBitacora(int IdMedioBitacora)
        {
            MedioBitacora medioBitacora = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@MedioBitacora_Id", IdMedioBitacora));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioBitacora_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        medioBitacora = ConstruirMedioBitacora(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return medioBitacora;
        }
        private MedioBitacora ConstruirMedioBitacora(DataTableReader Resultado)
        {
            return new MedioBitacora()
            {
                Id = int.Parse(Resultado["[idMedioBitacora]"].ToString()),
                FechaAcceso = DateTime.Parse(Resultado["fechaAccesoMedioBitacora"].ToString()),
                DatosAcceso = Resultado["datosAccesoMedioBitacora"].ToString(),
                Medio = new Medio()
                {
                    Id = int.Parse(Resultado["idMedio"].ToString()),
                    Esquema = Resultado["esquemaMedio"].ToString(),
                    Tamanio = long.Parse(Resultado["tamanioMedio"].ToString()),
                    CheckSum = Resultado["checkSumMedio"].ToString(),
                    EnDirectorio = Resultado["enDirectorio"].ToString() == "1" ? true : false,
                }
            };

        }
        public bool ActualizarMedioBitacora(int IdMedioBitacora, int IdMedio, DateTime FechaAcceso, string DatosAcces)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@MedioBitacora_Id", IdMedioBitacora));
                prmtrs.Add(new SqlParameter("@Medio_id", IdMedio));
                prmtrs.Add(new SqlParameter("@FechaAcceso", FechaAcceso));
                prmtrs.Add(new SqlParameter("@DatosAcceso", DatosAcces));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioBitacora_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        //////////DireectorioMarcador
        public int CrearDirectorioMarcador(string Descripcion, string Color, string Icono)
        {
            int idDirectorioMarcador = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@Color", Color));
                prmtrs.Add(new SqlParameter("@Icono", Icono));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMarcador_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDirectorioMarcador = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDirectorioMarcador;
        }
        public DirectorioMarcador ConsultarDirectorioMarcador(int IdDirectorioMarcador)
        {
            DirectorioMarcador directorioMarcador = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioMarcado_Id", IdDirectorioMarcador));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMarcador_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        directorioMarcador = ConstruirDirectorioMarcador(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return directorioMarcador;
        }
        public List<DirectorioMarcador> ConsultarDirectorioMarcadorColeccion(bool Disponible)
        {
            List<DirectorioMarcador> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMarcadorColeccion_get", prmtrs))
                {
                    coleccion = new List<DirectorioMarcador>();
                    while (Resultado.Read())
                    {
                        var cuentaNivel = ConstruirDirectorioMarcador(Resultado);
                        coleccion.Add(cuentaNivel);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return coleccion;
        }
        private DirectorioMarcador ConstruirDirectorioMarcador(DataTableReader Resultado)
        {
            return new DirectorioMarcador()
            {
                Id = int.Parse(Resultado["idDirectorioMarcador"].ToString()),
                Descripcion = Resultado["descripcionDirectorioMarcador"].ToString(),
                Color = Resultado["colorDirectorioMarcador"].ToString(),
                Icono = Resultado["[iconoDirectorioMarcador]"].ToString(),
                Disponible = Resultado["disponibleDirectorioMarcador"].ToString() == "1" ? true : false,
            };
        }
        public bool ActualizarDirectorioMarcador(int IdDirectorioMarcador, string Descripcion, string Color, string Icono)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioMarcador_Id", IdDirectorioMarcador));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@Color", Color));
                prmtrs.Add(new SqlParameter("@Icono", Icono));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMarcador_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        //////////DirectorioTipo
        public int CrearDirectorioTipo(string Descripcion)
        {
            int idDirectorioTipo = 0;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioTipo_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDirectorioTipo = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDirectorioTipo;
        }
        public DirectorioTipo ConsultarDirectorioTipo(int IdDirectorioTipo)
        {
            DirectorioTipo directorioTipo = new DirectorioTipo();
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@DirectorioTipo_Id", IdDirectorioTipo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioTipo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        directorioTipo = ConstruirDirectorioTipo(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return directorioTipo;
        }
        public List<DirectorioTipo> ConsultarDirectorioTipoColeccion(bool Disponible)
        {
            List<DirectorioTipo> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioTipoColeccion_get", prmtrs))
                {
                    coleccion = new List<DirectorioTipo>();
                    while (true)
                    {
                        var directorioTipo = ConstruirDirectorioTipo(Resultado);
                        coleccion.Add(directorioTipo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return coleccion;
        }
        private DirectorioTipo ConstruirDirectorioTipo(DataTableReader Resultado)
        {
            return new DirectorioTipo()
            {
                Id = int.Parse(Resultado["idDirectorioTipo"].ToString()),
                Descripcion = Resultado["descripcionDirectorioTipo"].ToString(),
                Disponible = Resultado["descripcionDirectorioTipo"].ToString() == "1" ? true : false,
            };

        }
        public bool ActualizarDirectorioTipo(long IdDirectorioTipo, string Descripcion)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioTipo_Id", IdDirectorioTipo));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioTipo_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarDirectorioTipo(int IdDirectorioTipo)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@DirectorioTipo_Id", IdDirectorioTipo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioTipo_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public Tuple<long,long> CrearDirectorio(string Sumario, int Tipo, string Nombre, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
            string Referencias, int IdDirectorioTipo,int IdDirecorioMarcador,bool Raiz,bool Fisico) {
            
            //Se crea el archivo para arbol principal
            var idArchivo = CrearArchivo(Sumario,Tipo,Nombre, Asegurado, IdOficina,IdUsuario,Publico,Ruta);
            //Se crea directorio vinculado al archivo
            long idDirectorio = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
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
        //////Directorio
        public Directorio ConsultarDirectorio(long IdDirectorio)
        {
            Directorio directorio = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
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
            return directorio;
        }
        private Directorio ConstruirDirectorio(DataTableReader Resultado)
        {
            return new Directorio
            {
                Id = int.Parse(Resultado["idDirectorio"].ToString()),
                Referencias = Resultado["referenciasDirectorio"].ToString(),
                Raiz = Resultado["raizDirectorio"].ToString() == "1" ? true : false,
                Fisico = Resultado["fisicoDirectorio"].ToString() == "1" ? true : false,
                Archivo = new Archivo()
                {
                    Id = int.Parse(Resultado["idArchivo"].ToString()),
                    Sumario = Resultado["sumarioArchivo"].ToString(),
                    Nombre = Resultado["nombreArchivo"].ToString(),
                    Asegurado = Resultado["aseguradoArchivo"].ToString() == "1" ? true : false,
                    OficinaId = new Oficina() { Id = int.Parse(Resultado["oficinaArchivo"].ToString()) },
            Usuario = new Usuario() { Id = int.Parse(Resultado["usuarioArchivo"].ToString()) },
                    Publico = Resultado["publicoArchivo"].ToString() == "1" ? true : false,
                    Fecha = DateTime.Parse(Resultado["fechaArchivo"].ToString()),
                    FechaModificacion = DateTime.Parse(Resultado["fechaModificacionArchivo"].ToString()),
                    Ruta = Resultado["rutaArchivo"].ToString(),
                    Url = Resultado["urlArchivo"].ToString(),
                    Disponible = Resultado["disponibleArchivo"].ToString() == "1" ? true : false,
                    ArchivoTipo = (ArchivoTipo)int.Parse(Resultado["archivoTipoArchivo"].ToString())
                },
                DirectorioTipo = new DirectorioTipo()
                {
                    Id = int.Parse(Resultado["idDirectorioTipo"].ToString()),
                    Descripcion = Resultado["descripcionDirectorioTipo"].ToString(),
                    Disponible = Resultado["descripcionDirectorioTipo"].ToString() == "1" ? true : false,
                },
                DirectorioMarcador = new DirectorioMarcador()
                {
                    Id = int.Parse(Resultado["idDirectorioMarcador"].ToString()),
                    Descripcion = Resultado["descripcionDirectorioMarcador"].ToString(),
                    Color = Resultado["colorDirectorioMarcador"].ToString(),
                    Icono = Resultado["iconoDirectorioMarcador"].ToString(),
                    Disponible = Resultado["disponibleDirectorioMarcador"].ToString() == "1" ? true : false,
                },
            };

        }
        public bool ActualizarDirectorio(long IdDirectrio, long IdArchivo, string Referencias, int IdDirectorioTipo, int IdDirectorioMarcador, bool Raiz, bool Fisico)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectrio));
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                prmtrs.Add(new SqlParameter("@Referencias", Referencias));
                prmtrs.Add(new SqlParameter("@DirectorioTipo_Id", IdDirectorioTipo));
                prmtrs.Add(new SqlParameter("@DirectorioMarcador_Id", IdDirectorioMarcador));
                prmtrs.Add(new SqlParameter("@Raiz", Raiz));
                prmtrs.Add(new SqlParameter("@Fisico", Fisico));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorio_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        //////DirectorioControl
        public int CrearDirectorioControl(long IdDirectorio, string Descripcion, long TipoEscrituraLectura, DateTime FechaRegistro, DateTime FechaInicio, DateTime FechaUltimaActualizacion, int EstatusCarpeta)
        {
            int idDirectorioControl = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@TipoEstructuraLectura_Id", TipoEscrituraLectura));
                prmtrs.Add(new SqlParameter("@FechaRegistro", FechaRegistro));
                prmtrs.Add(new SqlParameter("@FechaInicio", FechaInicio));
                prmtrs.Add(new SqlParameter("@FechaUltimaActualizacion", FechaUltimaActualizacion));
                prmtrs.Add(new SqlParameter("@EstatusCarpeta", EstatusCarpeta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioControl_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDirectorioControl = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDirectorioControl;
        }
        public DirectorioControl ConsultarDirectorioControl(int IdDirectorioControl)
        {
            DirectorioControl directorioControl = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioControl_Id", IdDirectorioControl));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioControl_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        directorioControl = ConstruirDirectorioControl(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return directorioControl;
        }
        public List<DirectorioControl> ConsultarDirectorioControlColeccion(bool Disponible)
        {
            List<DirectorioControl> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioControlColeccion_get", prmtrs))
                {
                    coleccion = new List<DirectorioControl>();
                    while (Resultado.Read())
                    {
                        var clasificacion = ConstruirDirectorioControl(Resultado);
                        coleccion.Add(clasificacion);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return coleccion;
        }
        private DirectorioControl ConstruirDirectorioControl(DataTableReader Resultado)
        {
            return new DirectorioControl
            {
                Id = int.Parse(Resultado["idDirectorioControl"].ToString()),
                Descripcion = Resultado["descripcionDirectorioControl"].ToString(),
                TipoEscrituraLectura = int.Parse(Resultado["tipoEscrituraLecturaDirectorioControl"].ToString()),
                FechaRegistro = DateTime.Parse(Resultado["fechaRegistroDirectorioControl"].ToString()),
                FechaInicio = DateTime.Parse(Resultado["fechaInicioDirectorioControl"].ToString()),
                FechaUltimaActualizacion = DateTime.Parse(Resultado["fechaModificacionDirectorioControl"].ToString()),
                EstatusCarpeta = int.Parse(Resultado["estatusCarpetaDirectorioControl"].ToString()),
                Disponible = Resultado["disponibleDirectorioControl"].ToString() == "1" ? true : false,
                Directorio = new Directorio()
                {
                    Id = int.Parse(Resultado["idDirectorio"].ToString()),
                    Referencias = Resultado["referenciasDirectorio"].ToString(),
                    Raiz = Resultado["raizDirectorio"].ToString() == "1" ? true : false,
                    Fisico = Resultado["fisicoDirectorio"].ToString() == "1" ? true : false,
                    Archivo = new Archivo() { Id = int.Parse(Resultado["idArchivo"].ToString()) },
                    DirectorioTipo = new DirectorioTipo() { Id = int.Parse(Resultado["idDirectorioTipo"].ToString()) },
                    DirectorioMarcador = new DirectorioMarcador() { Id = int.Parse(Resultado["idDirectorioMarcador"].ToString()) }
                },

            };

        }
        public bool ActualizarDirectorioControl(long IdDirectorioControl, long IdDirectorio, string Descripcion, long TipoEscrituraLectura, DateTime FechaRegistro, DateTime FechaInicio, DateTime FechaUltimaActualizacion, int EstatusCarpeta)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioControl_Id", IdDirectorioControl));
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@TipoEstructuraLectura_Id", TipoEscrituraLectura));
                prmtrs.Add(new SqlParameter("@FechaRegistro", FechaRegistro));
                prmtrs.Add(new SqlParameter("@FechaInicio", FechaInicio));
                prmtrs.Add(new SqlParameter("@FechaUltimaActualizacion", FechaUltimaActualizacion));
                prmtrs.Add(new SqlParameter("@EstatusCarpeta", EstatusCarpeta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioControl_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarDirectorioControl(int IdDirectorioControl)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@DirectorioControl_Id", IdDirectorioControl));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioControl_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        /////ArchivoCertificacion
        public int CrearArchivoCertificacion(long IdArchivo, string Metas, DateTime Fecha, int IdUsuario)
        {
            int idArchivoCertificacion = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                prmtrs.Add(new SqlParameter("@Metas", Metas));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoCertificacion_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idArchivoCertificacion = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idArchivoCertificacion;
        }
        public ArchivoCertificacion ConsultarArchivoCertificacion(int IdArchivoCertificacion)
        {
            ArchivoCertificacion archivoCertificacion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ArchivoCertificacion", IdArchivoCertificacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoCertificacion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        archivoCertificacion = ConstruirArchivoCertificacion(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return archivoCertificacion;
        }
        public List<ArchivoCertificacion> ConsultarArchivoCertificacionColeccion(bool Disponible)
        {
            List<ArchivoCertificacion> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioControlColeccion_get", prmtrs))
                {
                    coleccion = new List<ArchivoCertificacion>();
                    while (Resultado.Read())
                    {
                        var clasificacion = ConstruirArchivoCertificacion(Resultado);
                        coleccion.Add(clasificacion);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return coleccion;
        }
        private ArchivoCertificacion ConstruirArchivoCertificacion(DataTableReader Resultado)
        {
            return new ArchivoCertificacion
            {
                Id = long.Parse(Resultado["idArchivoCertificacion"].ToString()),
                Metas = Resultado["metasArchivoCertificacion"].ToString(),
                Fecha = DateTime.Parse(Resultado["fechaArchivoCertificacion"].ToString()),
                Disponible = Resultado["disponibleDirectorioControl"].ToString() == "1" ? true : false,

                Archivo = new Archivo()
                {
                    Sumario = Resultado["sumarioArchivo"].ToString(),
                    Nombre = Resultado["nombreArchivo"].ToString(),
                    Asegurado = Resultado["aseguradoArchivo"].ToString() == "1" ? true : false,
                    OficinaId = new Oficina() { Id = int.Parse(Resultado["oficinaArchivo"].ToString()) },
            Publico = Resultado["publicoArchivo"].ToString() == "1" ? true : false,
                    Fecha = DateTime.Parse(Resultado["fechaArchivo"].ToString()),
                    FechaModificacion = DateTime.Parse(Resultado["fechaModificacionArchivo"].ToString()),
                    Ruta = Resultado["rutaArchivo"].ToString(),
                    Url = Resultado["urlArchivo"].ToString(),
                    Disponible = Resultado["disponibleArchivo"].ToString() == "1" ? true : false,
                    ArchivoTipo = (ArchivoTipo)int.Parse(Resultado["idarchivoTipoArchivo"].ToString())
                },
                Usuario = new Usuario()
                {
                    Nick = Resultado["nickUsuario"].ToString(),
                    Password = Resultado["passwordUsrUsuario"].ToString(),
                    Observacion = Resultado["observacionUsuario"].ToString(),
                    TipoUsuario = (TipoUsuario)int.Parse(Resultado["tipoUsuario"].ToString()),
                    Identificacion = Resultado["identificacionUsuario"].ToString(),
                    ClasificacionIdentidad = new ClasificacionIdentidad()
                    {
                        Id = int.Parse(Resultado["idClasificacionIdentidad"].ToString()),
                        Nombre = Resultado["nombreClasificacionIdentidad"].ToString(),
                        Disponible = Resultado["disponibleClasificacionIdentidad"].ToString() == "1" ? true : false,
                    },
                    Estatus = new Estatus()
                    {
                        Id = int.Parse(Resultado["idEstatus"].ToString()),
                        Descripcion = Resultado["descripcionEstatus"].ToString()
                    }
                }
            };

        }
        public bool ActualizarArchivoCertificacion(long IdArchivoCertificaion, long IdArchivo, string Metas, DateTime Fecha, int IdUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ArchivoCertificacion_Id", IdArchivoCertificaion));
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                prmtrs.Add(new SqlParameter("@Metas", Metas));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoCertificacion_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarArchivoCertificacion(int IdArchivoCertificacion)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@ArchivoCertificacion_Id", IdArchivoCertificacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoCertificacion_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        ///////////////DirectorioRelacion
        public int CrearDirectorioRelacion(long IdRaizDirectorio, long IdNodoDirectorio)
        {
            int idDirectorioRelacion = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@RaizDirectorio_Id", IdRaizDirectorio));
                prmtrs.Add(new SqlParameter("@NodoDirectorio_Id", IdNodoDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioRelacion_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDirectorioRelacion = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDirectorioRelacion;
        }
        public DirectorioRelacion ConsultarDirectorioRelacion(int IdDirectorioRelacion)
        {
            DirectorioRelacion directorioRelacion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioRelacion_Id", IdDirectorioRelacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoCertificacion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        directorioRelacion = ConstruirDirectorioRelacion(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return directorioRelacion;
        }
        private DirectorioRelacion ConstruirDirectorioRelacion(DataTableReader Resultado)
        {
            return new DirectorioRelacion
            {
                Id = long.Parse(Resultado["idArchivoCertificacion"].ToString()),

                DirectorioRaiz = new Directorio()
                {
                    Id = long.Parse(Resultado["raizDirectorioDirectorioRelacion"].ToString())
                },
                NodoDirectorio = new Directorio()
                {
                    Id = long.Parse(Resultado["nodoDirectorioDirectorioRelacion"].ToString())
                },

            };

        }
        public bool ActualizarDirectorioRelacion(long IdDirectorioRelacion, long IdRaizDirectorio, long IdNodoDirectorio)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioRelacion_Id", IdDirectorioRelacion));
                prmtrs.Add(new SqlParameter("@RaizDirectorio_Id", IdRaizDirectorio));
                prmtrs.Add(new SqlParameter("@NodoDirectorio_Id", IdNodoDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioRelacion_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        //////////DirectorioUsuario
        public int CrearDirectorioUsuario(long Directorio_Id, DateTime Fecha, int Usuario_Id)
        {
            int idDirectorioUsuario = 0;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", Directorio_Id));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioUsuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDirectorioUsuario = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDirectorioUsuario;
        }
        public DirectorioUsuario ConsultaDirectorioUsuario(int IdDirectorioUsuario)
        {
            DirectorioUsuario directorioUsuario = new DirectorioUsuario();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioUsuario_Id", IdDirectorioUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioUsuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        directorioUsuario = ConstruirDirectorioUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return directorioUsuario;
        }
        public List<DirectorioUsuario> ConsultarDirectorioUsuarioByDirectorio(bool Disponible)
        {
            List<DirectorioUsuario> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioUsuarioByDirectorio_get", prmtrs))
                {
                    coleccion = new List<DirectorioUsuario>();
                    while (true)
                    {
                        var directorioUsuario = ConstruirDirectorioUsuario(Resultado);
                        coleccion.Add(directorioUsuario);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return coleccion;
        }
        public List<DirectorioUsuario> ConsultarDirectorioUsuarioByUsuario(bool Disponible)
        {
            List<DirectorioUsuario> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioUsuarioByUsuario_get", prmtrs))
                {
                    coleccion = new List<DirectorioUsuario>();
                    while (true)
                    {
                        var directorioUsuario = ConstruirDirectorioUsuario(Resultado);
                        coleccion.Add(directorioUsuario);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return coleccion;
        }
        private DirectorioUsuario ConstruirDirectorioUsuario(DataTableReader Resultado)
        {
            return new DirectorioUsuario
            {
                Id = int.Parse(Resultado["idDirectorioUsuario"].ToString()),
                Fecha = DateTime.Parse(Resultado["fechaDirectorioUsuario"].ToString()),
                Usuario = new Usuario { Id =int.Parse(Resultado["UsuarioDirectorioUsuario"].ToString()) },
                Disponible = Resultado["fisicoDirectorio"].ToString() == "1" ? true : false,
                Directorio = new Directorio()
                {
                    Id = int.Parse(Resultado["idDirectorio"].ToString()),
                    Archivo = new Archivo() { Id=int.Parse( Resultado["archivoDirectorio"].ToString()) }, 
                    Referencias = Resultado["referenciasDirectorio"].ToString(),
                    DirectorioMarcador = new DirectorioMarcador() {Id= int.Parse(Resultado["directorioMarcadorDirectorio"].ToString()) },
                    Raiz = Resultado["raizDirectorio"].ToString() == "1" ? true : false,
                    Fisico = Resultado["fisicoDirectorio"].ToString() == "1" ? true : false,
                    DirectorioTipo = new DirectorioTipo { Id =int.Parse(Resultado["idDirectorioTipo"].ToString()) },
                },
            };

        }
        public bool ActualizarDirectorioUsuario(long DirectorioUsuarioId, long DirectorioId, DateTime Fecha, long UsuarioId)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioUsuario_Id", DirectorioUsuarioId));
                prmtrs.Add(new SqlParameter("@Directorio_Id", DirectorioId));
                prmtrs.Add(new SqlParameter("@Directorio_Id", Fecha));
                prmtrs.Add(new SqlParameter("@Usuario_Id", UsuarioId));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("directorioUsuario_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarDirectorioUsuario(long IdDirectorioUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@DirectorioUsuario_Id", IdDirectorioUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioUsuario_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        //////////DirectorioMedio
        public int CrearDirectorioMedio(long IdMedio, long IdDirectorio, bool Compartido)

        {
            int idDirectorioMedio = 0;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Medio_Id", IdMedio));
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                prmtrs.Add(new SqlParameter("@Compartido", Compartido));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMedio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDirectorioMedio = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDirectorioMedio;
        }
        public bool CrearDirectorioFisico(Directorio Directorio, string BaseUri, string Nombre)
        {
            bool resultado = false;
            var directorio = Directorio;
            var newUri = BaseUri + directorio.Archivo.Ruta + "\\" + Nombre;
            var DirUri = directorio.Archivo.Ruta + "\\" + Nombre;
            if (!System.IO.Directory.Exists(newUri))
            {
                System.IO.Directory.CreateDirectory(newUri);
            }
            resultado = true;
            return resultado;
        }
        public Tuple<long, long> CrearDirectorioDirectorio(long IdDirectorio, string Sumario, int Tipo, string Nombre, bool Asegudado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
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
                long idDirectorioMedio = CrearDirectorioRelacion(idDirectorio, idDirectorio);
            }
            return resultado;
        }
        public bool ActivarDesactivarArchivo(long IdArchivo, bool Disponible)
        {
            bool tmp = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
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

        public bool ActualizarMedioSumario(long IdArchivo)
        {
            var medio = ConsultarMedioArchivo(IdArchivo);
            bool tmp = false;
            //var sumario = JsonConvert.SerializeObject(medio);
            //ActualizarSumario(IdArchivo, sumario);
            return tmp;
        }
        public bool ActualizarDirectorioSumario(long IdArchivo)
        {
            var dir = ConsultarDirectorioArchivo(IdArchivo);
            bool tmp = false;
            //var sumario = JsonConvert.SerializeObject(dir);
           // ActualizarSumario(IdArchivo, sumario);

            return tmp;
        }
        public bool ActualizarDirectorioMedio(long IdDirectorioMedio, long IdMedio, long IdDirectorio)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DirectorioMedio_Id", IdDirectorioMedio));
                prmtrs.Add(new SqlParameter("@Medio_Id", IdMedio));
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioMedio_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public List<Archivo> ConsultarArchivosDirectorio(long IdArchivo)
        {
            List<Archivo> archivos = new List<Archivo>();
            try
            {
                List<Object> prmtrs = new List<Object>();
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
        public Tuple<long, long> CrearMedioDirectorioVirtual(Directorio Directorio, string BaseUri, string PathFTP, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, bool EnDirectorio)
        {
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            if (System.IO.File.Exists(PathFTP))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(PathFTP);
               // var suma = new Herramientas.ComprobacionArchivos().CalcularSum(info.FullName);
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
                var newUri = directorio.Archivo.Ruta + "\\" + info.Name;
                //Se crea
                //resultado = CrearMedioDirectorio(directorio.Id, "", ArchivoTipo.MEDIO, info.Name, Asegurado, IdOficina, IdUsuario, Publico, newUri, "", info.Length, suma, tipo.Id, EnDirectorio);

                System.IO.File.Move(PathFTP, destino);

            }
            else
            {
                throw new Exception("No encontrado: " + PathFTP);
            }

            return resultado;
        }
        public Tuple<long, long> CrearDirectorioDirectorioReal(Laboratorio.Medios.Atributos.Items.Directorio Directorio, string BaseUri, string Nombre, int IdOficina, int IdUsuario)
        {
            Tuple<long, long> resultado = new Tuple<long, long>(0, 0);
            var directorio = Directorio;
            var newUri = BaseUri + directorio.Archivo.Ruta + "\\" + Nombre;
            var DirUri = directorio.Archivo.Ruta + "\\" + Nombre;
            resultado = CrearDirectorioDirectorio(directorio.Id, "", (int)Laboratorio.Medios.Atributos.Items.ArchivoTipo.DIRECTORIO, Nombre, false, IdOficina, IdUsuario, true, DirUri, "", 1, 1, true, true);
            if (!System.IO.Directory.Exists(newUri))
            {
                System.IO.Directory.CreateDirectory(newUri);
            }
            return resultado;
        }
        public Tuple<long, long> CrearMedioDirectorio(long IdDirectorio, string Sumario, ArchivoTipo Tipo, string Nombre, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
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
                   // ActualizarMedio(validacion.Item3, Esquema, Tamanio, CheckSum, IdMediaTipo, EnDirectorio);
                }
                idMedio = validacion.Item5;
                //Se reactiva
                ActivarDesactivarArchivo(validacion.Item3, true);
            }
            else
            {
                resultado = CrearMedio(Sumario, Tipo, Nombre, Asegurado, IdOficina, IdUsuario, Publico, Ruta,
                 Esquema, Tamanio, CheckSum, IdMediaTipo, EnDirectorio);
                idMedio = resultado.Item2;

                long idDirectorioMedio = 0;
                try
                {
                    List<Object> prmtrs = new List<Object>();
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
        public Tuple<long, long> CrearMedio(string Sumario, ArchivoTipo Tipo, string Nombre, bool Asegurado, int IdOficina, int IdUsuario, bool Publico, string Ruta,
    string Esquema, long Tamanio, string CheckSum, int IdMediaTipo, bool EnDirectorio)
        {
            var idArchivo = CrearArchivo(Sumario, (int)Tipo, Nombre, Asegurado, IdOficina, IdUsuario, Publico, Ruta);
            long idMedio = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
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
        public Medio ConsultarMedioArchivo(long IdArchivo)
        {
            var archivo = ConsultarArchivo(IdArchivo);
            Medio medio = new Medio();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioByArchivo_get", prmtrs))
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
            //return CompletarMedio(archivo, medio);
            return medio;
        }
        public Directorio ConsultarDirectorioArchivo(long IdArchivo)
        {
            Directorio directorio = new Directorio();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioByArchivo_get", prmtrs))
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
            return directorio;
        }
        public List<Archivo> ConsultarDirectorioDirectorio(long IdArchivo)
        {
            List<Archivo> archivos = new List<Archivo>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoDirectorioRaizbyArchivo_get", prmtrs))
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
        private void LimpiarDirectorio(long IdArchivo)
        {
            var archivos = ConsultarArchivosDirectorio(IdArchivo);
            foreach (var Archivo in archivos)
            {
                var res = EliminarArchivo(Archivo.Id);
            }
        }
        private Tuple<bool, long, long, long, long> ValidarDirectorioDirectorio(long IdDirectorio, string NombreArchivo)
        {

            Tuple<bool, long, long, long, long> tmp = new Tuple<bool, long, long, long, long>(false, 0, 0, 0, 0);
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Directorio_Id", IdDirectorio));
                prmtrs.Add(new SqlParameter("@Nombre", NombreArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.directorioValidarbyDirectorio_get", prmtrs))
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
        private DirectorioMedio ConstruirDirectorioMedio(DataTableReader Resultado)
        {
            return new DirectorioMedio
            {
                Id = int.Parse(Resultado["idDirectorioMedio"].ToString()),
                Compartido = Resultado["compartidoDirectorioMedio"].ToString() == "1" ? true : false,
                Medio = new Medio()
                {
                    Id = int.Parse(Resultado["idMedio"].ToString()),
                    Archivo = new Archivo() { Id = int.Parse(Resultado["idArchivoMedio"].ToString()) },
                    Esquema = Resultado["esquemaMedio"].ToString(),
                    Tamanio = long.Parse(Resultado["tamanioMedio"].ToString()),
                    CheckSum = Resultado["checkSumMedio"].ToString(),
                    MedioTipo = new MedioTipo() { Id = int.Parse(Resultado["idMedioTipoMedio"].ToString()) },
                    EnDirectorio = Resultado["enDirectorio"].ToString() == "1" ? true : false,
                },
                Directorio = new Directorio()
                {
                    Id = int.Parse(Resultado["idDirectorio"].ToString()),
                    Archivo = new Archivo() { Id = int.Parse(Resultado["archivoDirectorio"].ToString()) },
                    Referencias = Resultado["referenciasDirectorio"].ToString(),
                    DirectorioMarcador = new DirectorioMarcador() { Id = int.Parse(Resultado["directorioMarcadorDirectorio"].ToString()) },
                    Raiz = Resultado["raizDirectorio"].ToString() == "1" ? true : false,
                    Fisico = Resultado["fisicoDirectorio"].ToString() == "1" ? true : false,
                    DirectorioTipo = new DirectorioTipo() { Id = int.Parse(Resultado["idDirectorioTipo"].ToString()) },
                }
            };

        }
        private void ActualizarSumario(long IdArchivo, string Sumario)
        {
            try
            {
                List<Object> prmtrs = new List<Object>();
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
        private MedioTipo DeterminarTipoMedio(string Extension)
        {
           MedioTipo tipo = new MedioTipo();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Extension", Extension));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medioTipoExtension_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipo = ConstruirMedioTipo(Resultado);
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
    }
}