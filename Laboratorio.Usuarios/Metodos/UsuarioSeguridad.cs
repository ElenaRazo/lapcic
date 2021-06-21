using System;
using System.Collections.Generic;

using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Laboratorio.Usuarios.Atributos;
using Laboratorio.Libreria.BaseDatos;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Configuration;
using System.Drawing.Imaging;

namespace Laboratorio.Usuarios.Metodos
{
    public class UsuarioSeguridad: Libreria.BaseClass.BaseObject
    {
        public UsuarioSeguridad(string ConnectionString) : base(ConnectionString)
        {
            
        }

        public UsuarioSeguridad(IBaseDatos Conexion) : base(Conexion)
        {
           
        }
        //Clasificacion Cuenta
        public int CrearClasificacionCuenta(string Nombre, string Descripcion)
        {
            int idClasificacion = 0;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionCuenta_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idClasificacion = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idClasificacion;
        }
        public ClasificacionCuenta ConsultarClasificacionCuenta(int IdClasificacionCuenta)
        {
            ClasificacionCuenta clasificacion = null;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", IdClasificacionCuenta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionCuenta_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        clasificacion = ConstruirClasificacionCuenta(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return clasificacion;
        }
        public List<ClasificacionCuenta> ConsultarClasificacionCuentaColeccion(bool Disponible)
        {
            List<ClasificacionCuenta> clasificaciones = new List<ClasificacionCuenta>(); ;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionCuentaColeccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var clasificacion = ConstruirClasificacionCuenta(Resultado);
                        clasificaciones.Add(clasificacion);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return clasificaciones;
        }
        private ClasificacionCuenta ConstruirClasificacionCuenta(DataTableReader Resultado)
        {
            return new ClasificacionCuenta()
            {
                Id = int.Parse(Resultado["idClasificacionCuenta"].ToString()),
                Nombre = Resultado["nombreClasificacionCuenta"].ToString(),
                Descripcion = Resultado["descripcionClasificacionCuenta"].ToString(),
                Disponible = Resultado["disponibleClasificacionCuenta"].ToString() == "1" ? true : false
            };
        }
        public bool EliminarClasificacionCuenta(int IdClasificacionCuenta)
        {
            bool respuesta = false;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", IdClasificacionCuenta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionCuenta_del", prmtrs))
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
        public bool ActualizarClasificacionCuenta(int IdClasificacionCuenta, string Nombre, string Descripcion)
        {
            bool respuesta = false;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", IdClasificacionCuenta));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionCuenta_upd", prmtrs))
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

        //Puestos
        public Puesto ConsultarPuesto(int IdPuesto)
        {
            Puesto puesto = null;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Puesto_Id", IdPuesto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.puesto_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        puesto = ConstruirPuesto(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return puesto;
        }
        public List<Puesto> ConsultarPuestosColeccion(bool Disponible)
        {
            List<Puesto> puestos = null;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.puestoColeccion_get", prmtrs))
                {
                    puestos = new List<Puesto>();
                    while (Resultado.Read())
                    {
                        var puesto = ConstruirPuesto(Resultado);
                        puestos.Add(puesto);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return puestos;
        }  public List<Puesto> ConsultarPuestosTipoOficinaColeccion(int TipoOficina)
        {
            List<Puesto> puestos = null;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@TipoOficina_Id", TipoOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.puestoTipoOficinaColeccion_get", prmtrs))
                {
                    puestos = new List<Puesto>();
                    while (Resultado.Read())
                    {
                        var puesto = ConstruirPuesto(Resultado);
                        puestos.Add(puesto);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return puestos;
        }
        private Puesto ConstruirPuesto(DataTableReader Resultado)
        {
            return new Puesto()
            {
                Id = int.Parse(Resultado["idPuesto"].ToString()),
                Nombre = Resultado["nombrePuesto"].ToString(),
                ClasificacionCuenta = new ClasificacionCuenta()
                {
                    Id = int.Parse(Resultado["idClasificacionCuenta"].ToString()),
                    Nombre = Resultado["nombreClasificacionCuenta"].ToString(),
                    Descripcion = Resultado["descripcionClasificacionCuenta"].ToString(),
                    Disponible = Resultado["disponibleClasificacionCuenta"].ToString() == "1" ? true : false
                },
                Disponible = Resultado["disponiblePuesto"].ToString() == "1" ? true : false
            };
        }

        //Clasificacion Identidad
        public int CrearClasificacionIdentidad(string Nombre)
        {
            int idClasificacion = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionIdentidad_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idClasificacion = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idClasificacion;
        }
        public ClasificacionIdentidad ConsultarClasificacionIdentidad(int IdClasificacionIdentidad)
        {
            ClasificacionIdentidad clasificacion = null;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionIdentidad_Id", IdClasificacionIdentidad));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionIdentidad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        clasificacion = ConstruirClasificacionIdentidad(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return clasificacion;
        }
        public List<ClasificacionIdentidad> ConsultarClasificacionIdentidadColeccion(bool Disponible)
        {
            List<ClasificacionIdentidad> coleccion = null;
            try
            {
               List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionIdentidadColeccion_get", prmtrs))
                {
                    coleccion = new List<ClasificacionIdentidad>();
                    while (Resultado.Read())
                    {
                        var clasificacion = ConstruirClasificacionIdentidad(Resultado);
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
        private ClasificacionIdentidad ConstruirClasificacionIdentidad(DataTableReader Resultado)
        {
            return new ClasificacionIdentidad()
            {
                Id = int.Parse(Resultado["Id"].ToString()),
                Nombre = Resultado["Nombre"].ToString(),
                Disponible = Resultado["Disponible"].ToString() == "1" ? true : false
            };
        }

       

        public void Dispose()
        {
            if (base.conexion != null && !base.conexion.ObtenerEsreferencia())
            {
                base.Dispose();
            }
        }

        //Domicilio Usuario

        public int CrearDomicilioUsuario(int Usuario_Id, string Calle, string Codigo, int NumeroInt, int NumeroExt, string Referencia, int Municipio_Id, string Puntos, int Colonia_Id)
        {
            int idDomicilioUsuario = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Calle", Calle));
                prmtrs.Add(new SqlParameter("@Puntos", Puntos));
                prmtrs.Add(new SqlParameter("@Codigo", Codigo));
                prmtrs.Add(new SqlParameter("@NumeroInt", NumeroInt));
                prmtrs.Add(new SqlParameter("@NumeroExt", NumeroExt));
                prmtrs.Add(new SqlParameter("@Referencia", Referencia));
                prmtrs.Add(new SqlParameter("@Municipio_Id", Municipio_Id));
                prmtrs.Add(new SqlParameter("@Colonia_Id", Colonia_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.domicilioUsuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDomicilioUsuario = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDomicilioUsuario;
        }

        public DomicilioUsuario ConsultarDomicilioUsuario(int IdDomicilioUsuario)
        {
            DomicilioUsuario domicilioUsuario = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DomicilioUsuario_Id", IdDomicilioUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.domicilioUsuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        domicilioUsuario = ConstruirDomicilioUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return domicilioUsuario;
        }

        public List<DomicilioUsuario> ConsultarDomicilioUsuarioColeccion(bool Disponible)
        {
            List<DomicilioUsuario> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.domicilioUsuarioColeccion_get", prmtrs))
                {
                    coleccion = new List<DomicilioUsuario>();
                    while (Resultado.Read())
                    {
                        var domicilioUsuario = ConstruirDomicilioUsuario(Resultado);
                        coleccion.Add(domicilioUsuario);
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

        public DomicilioUsuario ConsultarDomicilioUsuarioPorKardex(int IdKardex)
        {
            DomicilioUsuario domicilioUsuario = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Kardex_Id", IdKardex));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.domicilioUsuarioByKardex_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        domicilioUsuario = ConstruirDomicilioUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return domicilioUsuario;
        }

        private DomicilioUsuario ConstruirDomicilioUsuario(DataTableReader Resultado)
        {
            if (Resultado["idDomicilioUsuario"].ToString() != "")
            {
                var domicilio = new DomicilioUsuario()
                {
                    Id = int.Parse(Resultado["idDomicilioUsuario"].ToString()),
                    Calle = Resultado["calleDomicilioUsuario"].ToString(),
                    Colonia = new Colonia() { Id = int.Parse(Resultado["coloniaDomicilioUsuario"].ToString()), Nombre = Resultado["nombreColoniaDomicilioUsuario"].ToString() },
                    Codigo = Resultado["codigoDomicilioUsuario"].ToString(),
                    NumeroInt = int.Parse(Resultado["numeroIntDomicilioUsuario"].ToString()),
                    NumeroExt = int.Parse(Resultado["numeroExtDomicilioUsuario"].ToString()),
                    Referencia = Resultado["referenciaDomicilioUsuario"].ToString(),
                    Municipio = new Descriptores.Atributos.Municipio()
                    {
                        Nombre = Resultado["nombreCiudad"].ToString(),
                        Clave = int.Parse(Resultado["claveCiudad"].ToString()),
                        Estado = new Descriptores.Atributos.Estado()
                        {
                            Id = int.Parse(Resultado["idEstado"].ToString()),
                            Nombre = Resultado["nombreEstado"].ToString()

                        },
                        Id = int.Parse(Resultado["idMunicipioDomicilioUsuario"].ToString())
                    },
                    Disponible = Resultado["disponibleDomicilioUsuario"].ToString() == "1" ? true : false
                };
                return domicilio;
            }
            else
                return null;
        }

        public bool ActualizarDomicilioUsuario(int IdDomicilioUsuario, string Calle, int Colonia, string Codigo, int NumeroInt, int NumeroExt, string Referencia, int Municipio_Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DomicilioUsuario_Id", IdDomicilioUsuario));              
                prmtrs.Add(new SqlParameter("@Calle", Calle));
                prmtrs.Add(new SqlParameter("@Colonia_Id", Colonia));
                prmtrs.Add(new SqlParameter("@Codigo", Codigo));
                prmtrs.Add(new SqlParameter("@NumeroInt", NumeroInt));
                prmtrs.Add(new SqlParameter("@NumeroExt", NumeroExt));
                prmtrs.Add(new SqlParameter("@Referencias", Referencia));
                prmtrs.Add(new SqlParameter("@Municipio_Id", Municipio_Id));
            
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.domicilioUsuario_upd", prmtrs))
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

        


        public bool EliminarDomicilioUsuario(int IdDomicilioUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@DomicilioUsuario_Id", IdDomicilioUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.domicilioUsuario_del", prmtrs))
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

        //Kardex

        public int CrearKardex(int Usuario_Id, int Puesto_Id, DateTime Fecha, int Oficicina_Id, int ClasificacionCuenta_Id, long Archivo_Id, int Estatus_Id)
        {
            int idKardex = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Puesto_Id", Puesto_Id));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Oficina_Id", Oficicina_Id));
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", ClasificacionCuenta_Id));
                prmtrs.Add(new SqlParameter("@Archivo_Id", Archivo_Id));
                prmtrs.Add(new SqlParameter("@Estatus_Id", Estatus_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.kardex_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idKardex = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idKardex;
        }

        public Kardex ConsultarKardex(int IdKardex)
        {
            Kardex kardex = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Kardex_Id", IdKardex));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.kardex_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        kardex = ConstruirKardex(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return kardex;
        }
        public Kardex ConsultarKardexUsuario(int IdUsuario)
        {
            Kardex kardex = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.kardexByUsuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        kardex = ConstruirKardex(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return kardex;
        }


        private Kardex ConstruirKardex(DataTableReader Resultado)
        {
            var kardex = new Kardex()
            {
                Id = int.Parse(Resultado["idKardex"].ToString()),
                Fecha = DateTime.Parse(Resultado["fechaKardex"].ToString()),
                Oficina = new Descriptores.Atributos.OficinaBase() {
                    Id = int.Parse(Resultado["idOficina"].ToString()),
                    Nombre = Resultado["nombreOficina"].ToString(),
                    Descripcion = Resultado["descripcionOficina"].ToString()
                },
                Puesto = new Puesto() {
                    Id = int.Parse(Resultado["idPuesto"].ToString()),
                    Nombre = Resultado["nombrePuesto"].ToString(),
                    Disponible = Resultado["disponiblePuesto"].ToString() == "1" ? true : false
                },
                ClasificacionCuenta = new ClasificacionCuenta() {
                    Id = int.Parse(Resultado["idClasificacionCuenta"].ToString()),
                    Nombre = Resultado["nombreClasificacionCuenta"].ToString(),
                    Descripcion = Resultado["descripcionClasificacionCuenta"].ToString(),
                    Disponible = Resultado["disponibleClasificacionCuenta"].ToString() == "1" ? true : false
                },
                Estatus = new Estatus() {
                    Id = int.Parse(Resultado["idEstatus"].ToString()),
                    Descripcion = Resultado["descripcionEstatus"].ToString()
                }
            };
          
            return kardex;
        }
        public bool ActualizarKardex(int IdKardex, int Usuario_Id, int Puesto_Id, DateTime Fecha, int Oficicina_Id, int ClasificacionCuenta_Id, long Archivo_Id, int Estatus_Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Kardex_Id", IdKardex));
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Puesto_Id", Puesto_Id));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Oficina_Id", Oficicina_Id));
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", ClasificacionCuenta_Id));
                prmtrs.Add(new SqlParameter("@Archivo_Id", Archivo_Id));
                prmtrs.Add(new SqlParameter("@Estatus_Id", Estatus_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.kardex_upd", prmtrs))
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

        public bool ActualizarKardexUsuario(int Usuario_Id, int Puesto_Id,int Oficicina_Id, int ClasificacionCuenta_Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Puesto_Id", Puesto_Id));
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", ClasificacionCuenta_Id));
                prmtrs.Add(new SqlParameter("@Oficina_Id", Oficicina_Id));

                
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.kardexByUsuario_upd", prmtrs))
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


        //Telefono Usuario

        public int CrearTelefonoUsuario(int Kardex_Id, int ClasificacionTelefono_Id, string Numero)
        {
            int idTelefonoUsuario = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Kardex_Id", Kardex_Id));
                prmtrs.Add(new SqlParameter("@ClasificacionTelefono_Id", ClasificacionTelefono_Id));
                prmtrs.Add(new SqlParameter("@Numero", Numero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.telefonoUsuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idTelefonoUsuario = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idTelefonoUsuario;
        }

        public TelefonoUsuario ConsultarTelefonoUsuario(int IdTelefonoUsuario)
        {
            TelefonoUsuario telefonoUsuario = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@TelefonoUsuario_Id", IdTelefonoUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.telefonoUsuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        telefonoUsuario = ConstruirTelefonoUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return telefonoUsuario;
        }

        public List<TelefonoUsuario> ConsultarTelefonoUsuarioColeccion(bool Disponible)
        {
            List<TelefonoUsuario> coleccion = null;
            try //en el mismo de la semana pasada para que al final se lance la siguiente expresión 
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.telefonoUsuarioColeccion_get", prmtrs))
                {
                    coleccion = new List<TelefonoUsuario>();
                    while (Resultado.Read())
                    {
                        var telefonoUsuario = ConstruirTelefonoUsuario(Resultado);
                        coleccion.Add(telefonoUsuario);
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

        private TelefonoUsuario ConstruirTelefonoUsuario(DataTableReader Resultado)
        {
            return new TelefonoUsuario()
            {
                Id = int.Parse(Resultado["idTelefonoUsuario"].ToString()),                 
                ClasificacionTelefono = new Descriptores.Atributos.ClasificacionTelefono() {
                   IdClasificacionTelefono = int.Parse(Resultado["idClasificacionTelefono"].ToString()),
                   Nombre = Resultado["nombreClasificacionTelefono"].ToString()
                },
                Numero = Resultado["numeroTelefonoUsuario"].ToString()                  
            };
        }

        public bool ActualizarTelefonoUsuario(int IdTelefonoUsuario, int Kardex_Id, int ClasificacionTelefono_Id, string Numero)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@TelefonoUsuario_Id", IdTelefonoUsuario));
                prmtrs.Add(new SqlParameter("@Kardex_Id", Kardex_Id));
                prmtrs.Add(new SqlParameter("@ClasificacionTelefono_Id", ClasificacionTelefono_Id));
                prmtrs.Add(new SqlParameter("@Numero", Numero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.telefonoUsuario_upd", prmtrs))
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

        public bool EliminarTelefonoUsuario(int IdTelefonoUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@TelefonoUsuario_Id", IdTelefonoUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.telefonoUsuario_del", prmtrs))
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

        //Acceso

        public int CrearAcceso(int Usuario_Id, int Plataforma_Id, int ClasificacionCuenta_Id, int Oficina_Id, DateTime Fecha, int Referencia_Id)
        {
            int idAcceso = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Plataforma_Id", Plataforma_Id));
                prmtrs.Add(new SqlParameter("@ClasificaconCuenta_Id", ClasificacionCuenta_Id));
                prmtrs.Add(new SqlParameter("@Oficina_Id", Oficina_Id));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Referencia_Id", Referencia_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.acceso_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idAcceso = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idAcceso;
        }

        public Acceso ConsultarAcceso(int IdAcceso)
        {
            Acceso acceso = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Acceso_Id", IdAcceso));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.acceso_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        acceso = ConstruirAcceso(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return acceso;
        }

        public List<Acceso> ConsultarAccesoColeccion(bool Disponible)
        {
            List<Acceso> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.accesoColeccion_get", prmtrs))
                {//mientras el resto de las aplicaciones trabajan bajo un esquema de dire
                    coleccion = new List<Acceso>();
                    while (Resultado.Read())
                    {
                        var acceso = ConstruirAcceso(Resultado);
                        coleccion.Add(acceso);
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

        private Acceso ConstruirAcceso(DataTableReader Resultado)
        {
            return new Acceso()
            {
                Plataforma = int.Parse(Resultado["idPlataformaAcceso"].ToString()),
                Oficina = int.Parse(Resultado["idOficinaAcceso"].ToString()),
                Fecha = DateTime.Parse(Resultado["fecha"].ToString()),
                Referencia = int.Parse(Resultado["idReferenciaAcceso"].ToString()),
                Disponible = Resultado["disponibleAcceso"].ToString() == "1" ? true : false,

                Usuario = new Usuario()
                {
                    Id = int.Parse(Resultado["idUsuario"].ToString()),
                    Nick = Resultado["nickUsuario"].ToString(),
                    Password = Resultado["passwordUsrUsuario"].ToString(),
                    Observacion = Resultado["observacionUsuario"].ToString(),
                    TipoUsuario = (TipoUsuario)int.Parse(Resultado["tipoUsuario"].ToString()),
                    ClasificacionIdentidad = new ClasificacionIdentidad() { Id = int.Parse(Resultado["idClasificacionIdentidadUsuario"].ToString()) },
                    Identificacion = Resultado["identificacionUsuario"].ToString(),
                    Estatus = new Estatus() { Id = int.Parse(Resultado["idEstatusUsuario"].ToString()) }
                },                
                ClasificacionCuenta = new ClasificacionCuenta()
                {
                    Id=int.Parse( Resultado["idClasificacionCuenta"].ToString()),
                    Nombre = Resultado["nombreClasificacionCuenta"].ToString(),
                    Descripcion= Resultado["descripcionClasificacionCuenta"].ToString(),
                    Disponible = Resultado["disponibleClasificacionCuenta"].ToString() == "1" ? true : false
                },               
            };
        }

        public bool ActualizarAcceso(long IdAcceso, int Usuario_Id, int Plataforma_Id, int ClasificacionCuenta_Id, int Oficina_Id, DateTime Fecha, int Referencia_Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Acceso_Id", IdAcceso));
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Plataforma_Id", Plataforma_Id));
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", ClasificacionCuenta_Id));
                prmtrs.Add(new SqlParameter("@Oficina_Id", Oficina_Id));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Referencia", Referencia_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.acceso_upd", prmtrs))
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

        public bool EliminarAcceso(int IdAcceso)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Acceso_Id", IdAcceso));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.acceso_del", prmtrs))
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

        //Cuenta Nivel

        public int CrearCuentaNivel(int ClasificacionCuenta_Id)
        {
            int idCuentaNivel = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", ClasificacionCuenta_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.cuentaNivel_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idCuentaNivel = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idCuentaNivel;
        }

        public CuentaNivel ConsultarCuentaNivel(int IdCuentaNivel)
        {
            CuentaNivel cuentaNivel = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@CuentaNivel_Id", IdCuentaNivel));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.cuentaNivel_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        cuentaNivel = ConstruirCuentaNivel(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return cuentaNivel;
        }

        public List<CuentaNivel> ConsultarCuentaNivelColeccion(bool Disponible)
        {
            List<CuentaNivel> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.cuentaNivelColeccion_get", prmtrs))
                {
                    coleccion = new List<CuentaNivel>();
                    while (Resultado.Read())
                    {
                        var cuentaNivel = ConstruirCuentaNivel(Resultado);
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

        private CuentaNivel ConstruirCuentaNivel(DataTableReader Resultado)
        {
            return new CuentaNivel()
            {
                Id = Resultado["idCuentaNivel"]!=null? int.Parse(Resultado["idCuentaNivel"].ToString()): 0,
                Disponible = Resultado["disponibleCuentaNivel"].ToString() == "1" ? true : false,
                ClasificacionCuenta = new ClasificacionCuenta()
                {
                    Id= int.Parse( Resultado["idClasificacionCuenta"].ToString()),
                    Nombre=  Resultado["nombreClasificacionCuenta"] != null ? Resultado["nombreClasificacionCuenta"].ToString() : null,
                    Descripcion = Resultado["descripcionClasificacionCuenta"]!=null? Resultado["descripcionClasificacionCuenta"].ToString():null,
                    Disponible = Resultado["disponibleClasificacionCuenta"]!=null? (Resultado["disponibleClasificacionCuenta"].ToString() == "1" ? true : false):false
                },
                
            };
        }

        public bool ActualizarCuentaNivel(int IdCuentaNivel, int ClasificacionCuenta_Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@CuentaNivel_Id", IdCuentaNivel));
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", ClasificacionCuenta_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.cuentaNivel_upd", prmtrs))
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
        public bool ActualizarArchivoUsuario(long IdArchivo, long IdUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioArchivo_upd", prmtrs))
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
        public bool AsignarArchivoUsuario(long IdArchivo, long IdUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.archivoUsuario_set", prmtrs))
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

        public bool EliminarCuentaNivel(int IdCuentaNivel)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@CuentaNivel_Id", IdCuentaNivel));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.cuentaNivel_del", prmtrs))
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

        //Sesion

        public int CrearSesion(int Usuario_Id, string Token, DateTime Fecha, bool Dispositivo, string Tag, string Nombre)
        {
            int idSesion = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Token", Token));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Dispositivo", Dispositivo));
                prmtrs.Add(new SqlParameter("@Tag", Tag));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.sesion_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idSesion = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idSesion;
        }

        public Sesion ConsultarSesion(long IdSesion)
        {
            Sesion sesion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Sesion_Id", IdSesion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.sesion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        sesion = ConstruirSesion(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return sesion;
        }

        public List<Sesion> ConsultarSesionColeccion(bool Disponible)
        {
            List<Sesion> coleccion = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.sesionColeccion_get", prmtrs))
                {
                    coleccion = new List<Sesion>();
                    while (Resultado.Read())
                    {
                        var domicilioUsuario = ConstruirSesion(Resultado);
                        coleccion.Add(domicilioUsuario);
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

        private Sesion ConstruirSesion(DataTableReader Resultado)
        {
            return new Sesion()
            {
                Id = int.Parse(Resultado["idSesion"].ToString()),
                Fecha = DateTime.Parse(Resultado["fechaSesion"].ToString()),
                Token = Resultado["tokenSesion"].ToString(),
                Dispositivo = int.Parse( Resultado["dispositivoSesion"].ToString())==1?true:false,
                Tag = Resultado["tagSesion"].ToString(),
                Nombre = Resultado["nombreSesion"].ToString(),
                Disponible = Resultado["disponibleSesion"].ToString() == "1" ? true : false,

                Usuario = new Usuario()
                {
                    Id = int.Parse(Resultado["idUsuario"].ToString()),
                    Nick = Resultado["nickUsuario"].ToString(),
                    Password = Resultado["passwordUsrUsuario"].ToString(),
                    Observacion = Resultado["observacionUsuario"].ToString(),
                    TipoUsuario = (TipoUsuario)int.Parse(Resultado["tipoUsuario"].ToString()),
                    ClasificacionIdentidad = new ClasificacionIdentidad() { Id = int.Parse(Resultado["idClasificacionIdentidadUsuario"].ToString()) },
                    Identificacion = Resultado["identificacionUsuario"].ToString(),
                    Estatus = new Estatus() { Id = int.Parse(Resultado["idEstatusUsuario"].ToString()) }
                },
            };
        }

        public bool ActualizarSesion(long IdSesion, int Usuario_Id, string Token, DateTime Fecha, bool Dispositivo, string Tag, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Sesion_Id", IdSesion));
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario_Id));
                prmtrs.Add(new SqlParameter("@Token", Token));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Dispositivo", Dispositivo));
                prmtrs.Add(new SqlParameter("@Tag", Tag));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.sesion_upd", prmtrs))
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

        public bool EliminarSesion(int IdSesion)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Sesion_Id", IdSesion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.sesion_del", prmtrs))
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

        public bool EliminarUsuario(int IdUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuario_del", prmtrs))
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

        //Usuario Fisica

        public Usuarios.Atributos.UsuarioFisica CrearUsuarioFisica(string Paterno, string Materno, string Nombre, int Genero, string Nick, string PasswordUsr, string Observacion, int TipoUsuario, int ClasificacionUsuario, int ClasificacionIdentidad_Id, string Identificacion, string Email, DomicilioUsuario Domicilio, Kardex Kardex)
        {
            int idUsuarioFisica = 0;
            int usuarioId = 0;
            UsuarioFisica usuario = null;
            try
            {
                usuarioId = CrearUsuario(Nick, PasswordUsr, Observacion, TipoUsuario, ClasificacionUsuario, ClasificacionIdentidad_Id, Identificacion, Email);
                int domicilioId = CrearDomicilioUsuario(usuarioId,Domicilio.Calle, Domicilio.Codigo, Domicilio.NumeroInt, Domicilio.NumeroExt, Domicilio.Referencia,Domicilio.Municipio.Id ,Domicilio.Puntos, Domicilio.Colonia.Id);
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", usuarioId));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFisica_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idUsuarioFisica = Resultado.GetInt32(0);
                    }
                }
                usuario = ConsultarUsuarioFisica(idUsuarioFisica);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }
        public UsuarioFisica CrearPreUsuarioFisica(string Paterno, string Materno, string Nombre, int Genero, string Nick, string PasswordUsr, string Observacion, int TipoUsuario, int ClasificacionUsuario, int ClasificacionIdentidad_Id, string Identificacion, string Email, DomicilioUsuario Domicilio, Kardex Kardex)
        {
            int idUsuarioFisica = 0;
            int usuarioId = 0;
            UsuarioFisica usuario = null;
            try
            {
                usuarioId = CrearPreUsuario(Nick, PasswordUsr, Observacion, TipoUsuario, ClasificacionUsuario, ClasificacionIdentidad_Id, Identificacion, Email);
                int domicilioId = CrearDomicilioUsuario(usuarioId, Domicilio.Calle, Domicilio.Codigo, Domicilio.NumeroInt, Domicilio.NumeroExt, Domicilio.Referencia, Domicilio.Municipio.Id, Domicilio.Puntos, Domicilio.Colonia.Id);
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", usuarioId));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFisica_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idUsuarioFisica = Resultado.GetInt32(0);
                    }
                }
                usuario = ConsultarUsuarioFisica(idUsuarioFisica);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }

        public UsuarioFisica ConsultarUsuarioFisica(int IdUsuarioFisica)
        {
            UsuarioFisica usuarioFisica = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@UsuarioFisica_Id", IdUsuarioFisica));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFisica_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarioFisica = ConstruirUsuarioFisica(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarioFisica;
        }
        public List<UsuarioBase> ConsultarUsuarioNotificable(string Nombre, int Tipo)
        {
            List<UsuarioBase> usuarios = new List<UsuarioBase>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Tipo", Tipo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificable_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioBase(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public List<UsuarioBase> ConsultarUsuarioNotificablePendientes()
        {
            List<UsuarioBase> usuarios = new List<UsuarioBase>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificablePendientes_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioBase(Resultado));
                    }
                }
                usuarios.AddRange(ConsultarUsuarioNotificablePendientesMorales());
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public List<UsuarioBase> ConsultarUsuarioNotificablePendientesConvalidar()
        {
            List<UsuarioBase> usuarios = new List<UsuarioBase>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificablePendientesConvalidar_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioBase(Resultado));
                    }
                }
                usuarios.AddRange(ConsultarUsuarioNotificablePendientesMoralesConvalidar());
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public List<UsuarioBase> ConsultarUsuarioNotificablePendientesMorales()
        {
            List<UsuarioBase> usuarios = new List<UsuarioBase>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificablePendientesMorales_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioBase(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public List<UsuarioBase> ConsultarUsuarioNotificablePendientesMoralesConvalidar()
        {
            List<UsuarioBase> usuarios = new List<UsuarioBase>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificablePendientesMoralesConvalidad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioBase(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public List<UsuarioFoto> ConsultarUsuarioFoto(string actuarios)
        {
            int actual = 0;
            List<UsuarioFoto> usuarios = new List<UsuarioFoto>();
            try
            {
                var usuarios2 = actuarios.Split(',');
                foreach (string i in usuarios2)
                {
                    if (i != "," && i != "")
                    {
                        int id = int.Parse(i);
                        if (actual == 0 || actual != id)
                        {
                            List<Object> prmtrs = new List<Object>();
                            prmtrs.Add(new SqlParameter("@Usuario_Id", id));
                            using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFoto_get", prmtrs))
                            {
                                while (Resultado.Read())
                                {
                                    var usuario = new UsuarioFoto()
                                    {
                                        Id = int.Parse(Resultado["IdUsuario"].ToString()),
                                        Foto = Resultado["fotoUsuario"].ToString() != "" ? @"/Directorio/Users/" + Resultado["fotoUsuario"].ToString() : @"/Directorio/Users/" + "User.png",
                                        Nombre = Resultado["NombreUsuario"].ToString(),
                                    };
                                    usuarios.Add(usuario);
                                }
                            }
                            actual = id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public UsuarioBase ConsultarUsuarioNotificable(int IdUsuario)
        {
            UsuarioBase usuario = new UsuarioBase();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificableId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuario = ConstruirUsuarioBase(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }
        public UsuarioBasico ConsultarUsuarioNotificableBasico(int IdUsuario)
        {
            UsuarioBasico usuario = new UsuarioBasico();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificableBasicoId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuario = ConstruirUsuarioBasico(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }
        public void EnviarMensaje(int Tipo, string Mensaje, string Token, string Cuerpo)
        {
            var mensaje = new Herramientas.Mensajeria();
            var msj = new MovilMensajeria()
            {
                notificacion = new Notificacion()
                {
                    title = Mensaje,
                    body = ""
                },

                data = new Data()
                {
                    Tipo = Tipo,
                    Mensaje = Mensaje,
                    Cuerpo = Cuerpo

                },
                to = Token
            };
            mensaje.PostMensajeAsync(msj);
        }
        public void EnviarNotificacion(int IdUsuario, string Notificacion)
        {
            UsuarioBasico usuario = new UsuarioBasico();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificableBasicoId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuario = ConstruirUsuarioBasico(Resultado);
                    }
                }
                Usuarios.Herramientas.Mensajeria tools = new Usuarios.Herramientas.Mensajeria();
                EnviarMensaje(2, "Ha recibido una nueva notificación electrónica", usuario.Token, Notificacion);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }
        public void EnviarNotificacionSolicitud(int IdUsuario, string solicitud, string expediente, bool aceptada)
        {
            UsuarioBasico usuario = new UsuarioBasico();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioNotificableBasicoId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuario = ConstruirUsuarioBasico(Resultado);
                    }
                }
                Usuarios.Herramientas.Mensajeria tools = new Usuarios.Herramientas.Mensajeria();
                EnviarMensaje(2, "Su solicitud de acceso a " + expediente + " ha sido " + (aceptada ?"aceptada" : "rechazada"), usuario.Token, solicitud);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }

        public UsuarioFisica ConsultarUsuarioFisicaporUsuario(int IdUsuario)
        {
            UsuarioFisica usuarioFisica = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFisicaByUsuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarioFisica = ConstruirUsuarioFisica(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarioFisica;
        }

        private UsuarioFisica ConstruirUsuarioFisica(DataTableReader Resultado)
        {
            UsuarioFisica usuario = new UsuarioFisica()
            {
                IdUsuarioFisica = int.Parse(Resultado["idUsuarioFisica"].ToString()),
                Paterno = Resultado["paternoUsuarioFisica"].ToString(),
                Materno = Resultado["maternoUsuarioFisica"].ToString(),
                Nombre = Resultado["nombreUsuarioFisica"].ToString(),
                Genero = int.Parse(Resultado["generoUsuarioFisica"].ToString()),
                Usuario = new Usuario()
                {
                    Id = int.Parse(Resultado["idUsuario"].ToString()),
                    Nick = Resultado["nickUsuario"].ToString(),
                    Email = Resultado["emailUsuario"].ToString(),
                    Fecha = DateTime.Parse(Resultado["fechaUsuario"].ToString()),
                    Foto = Resultado["fotoUsuario"].ToString()!= "" ? @"/Directorio/Users/" + Resultado["fotoUsuario"].ToString() : @"/Directorio/Users/" + "User.png",
                    Password = Resultado["passwordUsrUsuario"].ToString(),
                    Observacion = Resultado["observacionUsuario"].ToString(),
                    TipoUsuario = (TipoUsuario)int.Parse(Resultado["tipoUsuario"].ToString()),
                    ClasificacionUsuario = (ClasificacionUsuario)int.Parse(Resultado["clasificacionUsuario"].ToString()),
                    ClasificacionIdentidad = new ClasificacionIdentidad() { Id = int.Parse(Resultado["idClasificacionIdentidadUsuario"].ToString()) },
                    Identificacion = Resultado["identificacionUsuario"].ToString(),
                    Estatus = new Estatus() { Id = int.Parse(Resultado["idEstatusUsuario"].ToString()) }
                }
            };
            usuario.Usuario.Domicilio = ConstruirDomicilioUsuario(Resultado);
            try
            {
                if (usuario.Usuario.ClasificacionUsuario == ClasificacionUsuario.INTERNO)
                    usuario.Kardex = ConstruirKardex(Resultado);
            }
            catch (Exception Error) { };
            return usuario;
        }
        private UsuarioFisicaBase ConstruirUsuarioFisicaBase(DataTableReader Resultado)
        {
            return new UsuarioFisicaBase()
            {
                IdUsuarioFisica = int.Parse(Resultado["idUsuarioFisica"].ToString()),
                Paterno = Resultado["paternoUsuarioFisica"].ToString(),
                Materno = Resultado["maternoUsuarioFisica"].ToString(),
                Nombre = Resultado["nombreUsuarioFisica"].ToString(),
                Genero = int.Parse(Resultado["generoUsuarioFisica"].ToString())
            };
        }

        public bool ActualizarUsuarioFisica(int IdUsuarioFisica, string Paterno, string Materno, string Nombre, bool Genero)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@UsuarioFisica_Id", IdUsuarioFisica));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFisica_upd", prmtrs))
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

        public bool ActivarUsuario(int IdUsuario, string Password)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Password", Password));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioActivar_set", prmtrs))
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
        public bool AceptarUsuario(int IdUsuario, int  UsuarioAcepto)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@UsuarioAcepto_Id", UsuarioAcepto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioAceptar_set", prmtrs))
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
        public bool ConvalidarUsuario(int IdUsuario, int UsuarioConvalido)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@UsuarioConvalido_Id", UsuarioConvalido));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioConvalidar_set", prmtrs))
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

        public bool EliminarUsuarioFisica(int IdUsuarioFisica)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@UsuarioFisica_Id", IdUsuarioFisica));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFisica_del", prmtrs))
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

        //Usuario Moral

        public UsuarioMoral CrearUsuarioMoral(int TipoMoral, string Razon, string Paterno, string Materno, string Nombre, string Nick, string PasswordUsr, string Observacion, int TipoUsuario, int ClasificacionUsuario, int ClasificacionIdentidad_Id, string Identificacion, string Email)
        {
            int idUsuarioMoral = 0;
            int usuarioId = 0;
            UsuarioMoral usuario = null;
            try
            {
                usuarioId = CrearUsuario(Nick, PasswordUsr, Observacion, TipoUsuario, ClasificacionUsuario, ClasificacionIdentidad_Id, Identificacion, Email);

                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", usuarioId));
                prmtrs.Add(new SqlParameter("@TipoMoral", TipoMoral));
                prmtrs.Add(new SqlParameter("@Razon", Razon));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioMoral_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idUsuarioMoral = Resultado.GetInt32(0);
                    }
                }
                usuario = ConsultarUsuarioMoral(idUsuarioMoral);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }
        public UsuarioMoral CrearPreUsuarioMoral(int TipoMoral, string Razon, string Paterno, string Materno, string Nombre, string Nick, string PasswordUsr, string Observacion, int TipoUsuario, int ClasificacionUsuario, int ClasificacionIdentidad_Id, string Identificacion, string Email)
        {
            int idUsuarioMoral = 0;
            int usuarioId = 0;
            UsuarioMoral usuario = null;
            try
            {
                usuarioId = CrearPreUsuario(Nick, PasswordUsr, Observacion, TipoUsuario, ClasificacionUsuario, ClasificacionIdentidad_Id, Identificacion, Email);

                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", usuarioId));
                prmtrs.Add(new SqlParameter("@TipoMoral", TipoMoral));
                prmtrs.Add(new SqlParameter("@Razon", Razon));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioMoral_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idUsuarioMoral = Resultado.GetInt32(0);
                    }
                }
                usuario = ConsultarUsuarioMoral(idUsuarioMoral);
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }

        public UsuarioMoral ConsultarUsuarioMoral(int IdUsuarioMoral)
        {
            UsuarioMoral usuarioMoral = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@UsuarioMoral_Id", IdUsuarioMoral));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioMoral_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarioMoral = ConstruirUsuarioMoral(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarioMoral;
        }

        public UsuarioMoral ConsultarUsuarioMoralPorUsuario(int IdUsuario)
        {
            UsuarioMoral usuarioMoral = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioMoralByUsuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarioMoral = ConstruirUsuarioMoral(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarioMoral;
        }

        private UsuarioMoral ConstruirUsuarioMoral(DataTableReader Resultado)
        {
           UsuarioMoral usuario= new  UsuarioMoral()
            {
                IdUsuarioMoral = int.Parse(Resultado["idUsuarioMoral"].ToString()),
                TipoMoral =int.Parse( Resultado["tipoMoralUsuarioMoral"].ToString())/*==1? true : false*/,
                Razon = Resultado["razonUsuarioMoral"].ToString(),
                //----
                Nombre = Resultado["nombreUsuarioMoral"].ToString(),
                Paterno = Resultado["paternoUsuarioMoral"].ToString(),
                Materno = Resultado["maternoUsuarioMoral"].ToString(),
                //----
                Usuario = new Usuario()
                {
                    Id = int.Parse(Resultado["idUsuario"].ToString()),
                    Nick = Resultado["nickUsuario"].ToString(),
                    Email = Resultado["emailUsuario"].ToString(),
                    Fecha = DateTime.Parse(Resultado["fechaUsuario"].ToString()),
                    Foto = Resultado["fotoUsuario"].ToString()!= "" ? @"/Directorio/Users/" + Resultado["fotoUsuario"].ToString() : @"/Directorio/Users/" + "User.png",
                    Password = Resultado["passwordUsrUsuario"].ToString(),
                    Observacion = Resultado["observacionUsuario"].ToString(),
                    TipoUsuario = (TipoUsuario)int.Parse(Resultado["tipoUsuario"].ToString()),
                    ClasificacionUsuario = (ClasificacionUsuario)int.Parse(Resultado["clasificacionUsuario"].ToString()),
                    ClasificacionIdentidad = new ClasificacionIdentidad() { Id = int.Parse(Resultado["idClasificacionIdentidadUsuario"].ToString()) },
                    Identificacion = Resultado["identificacionUsuario"].ToString(),
                    Estatus = new Estatus() { Id = int.Parse(Resultado["idEstatusUsuario"].ToString()) }

                },
            };
            usuario.Usuario.Domicilio = ConstruirDomicilioUsuario(Resultado);
            return usuario;
        }
        private UsuarioMoralBase ConstruirUsuarioMoralBase(DataTableReader Resultado)
        {
            return new UsuarioMoralBase()
            {
                IdUsuarioMoral = int.Parse(Resultado["idUsuarioMoral"].ToString()),
                TipoMoral = (TipoMoral)int.Parse(Resultado["tipoMoralUsuarioMoral"].ToString()),
                Razon = Resultado["razonUsuarioMoral"].ToString(),
                Paterno = Resultado["paternoUsuarioMoral"].ToString(),
                Materno = Resultado["maternoUsuarioMoral"].ToString(),
                Nombre = Resultado["nombreUsuarioMoral"].ToString()
            };
        }

        public bool ActualizarUsuarioMoral(int IdUsuarioMoral,int Tipo, string Paterno, string Materno,string Nombre, string Razon, string Comentarios)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@UsuarioMoral_Id", IdUsuarioMoral));
                //----
                prmtrs.Add(new SqlParameter("@Tipo", Tipo));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                //----
                prmtrs.Add(new SqlParameter("@Razon", Razon));
                //----
                prmtrs.Add(new SqlParameter("@Comentarios",Comentarios));
                //----
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioMoral_upd", prmtrs))
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

        //Estatus

        public int CrearEstatus(string Descripcion)
        {
            int idEstatus = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estatus_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idEstatus = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idEstatus;
        }

        public Estatus ConsultarEstatus(int IdEstatus)
        {
            Estatus estatus = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Estatus_Id", IdEstatus));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estatus_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        estatus = ConstruirEstatus(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estatus;
        }

        private Estatus ConstruirEstatus(DataTableReader Resultado)
        {
            return new Estatus()
            {
                Id = int.Parse(Resultado["Id"].ToString()),
                Descripcion = Resultado["Descripcion"].ToString(),
            };
        }

        public bool ActualizarEstatus(int IdEstatus, string Descripcion)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Estatus_Id", IdEstatus));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estatus_upd", prmtrs))
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

        //Usuario

        public int CrearUsuario(string Nick, string PasswordUsr, string Observacion, int TipoUsuario, int ClasificacionUsuario, int ClasificacionIdentidad_Id, string Identificacion, string Email)
        {
            int idUsuario = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionUsuario", ClasificacionUsuario));
                prmtrs.Add(new SqlParameter("@Email", Email));
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@PasswordUsr", PasswordUsr));
                prmtrs.Add(new SqlParameter("@Observacion", Observacion));
                prmtrs.Add(new SqlParameter("@TipoUsuario", TipoUsuario));
                prmtrs.Add(new SqlParameter("@ClasificacionIdentidad_Id", ClasificacionIdentidad_Id));
                prmtrs.Add(new SqlParameter("@Identificacion", Identificacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idUsuario = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idUsuario;
        }

        public int CrearPreUsuario(string Nick, string PasswordUsr, string Observacion, int TipoUsuario, int ClasificacionUsuario, int ClasificacionIdentidad_Id, string Identificacion, string Email)
        {
            int idUsuario = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionUsuario", ClasificacionUsuario));
                prmtrs.Add(new SqlParameter("@Email", Email));
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@PasswordUsr", PasswordUsr));
                prmtrs.Add(new SqlParameter("@Observacion", Observacion));
                prmtrs.Add(new SqlParameter("@TipoUsuario", TipoUsuario));
                prmtrs.Add(new SqlParameter("@ClasificacionIdentidad_Id", ClasificacionIdentidad_Id));
                prmtrs.Add(new SqlParameter("@Identificacion", Identificacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.preUsuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idUsuario = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idUsuario;
        }

        public Usuario ConsultarUsuario(int IdUsuario)
        {
            Usuario usuario = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuario = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }
        public UsuarioBasico ConsultarUsuarioBasico(int IdUsuario)
        {
            UsuarioBasico usuario = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioBasico_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuario = ConstruirUsuarioBasico(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuario;
        }
        public List<UsuarioBase> ConsultarUsuarioPorClasificacion(int Clasificacion)
        {
            List<UsuarioBase> usuarios = new List<UsuarioBase>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClasificacionUsuario", Clasificacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioClasificacionColeccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioBase(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }

        public List<UsuarioBasico> ConsultarUsuarioPorOficina(int IdOficina)
        {
            List<UsuarioBasico> usuarios = new List<UsuarioBasico>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioOficina_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioBasico(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
          public List<UsuarioKardex> ConsultarUsuarioKardexPorOficina(int IdOficina)
        {
            List<UsuarioKardex> usuarios = new List<UsuarioKardex>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioKardexOficina_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioKardex(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public List<UsuarioKardex> ConsultarUsuarioKardexProyectistas()
        {
            List<UsuarioKardex> usuarios = new List<UsuarioKardex>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioKardexProyectista_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioKardex(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }
        public List<UsuarioKardex> ConsultarUsuarioKardexMagistrados()
        {
            List<UsuarioKardex> usuarios = new List<UsuarioKardex>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioKardexMagistrados_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        usuarios.Add(ConstruirUsuarioKardex(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return usuarios;
        }


        private Usuario ConstruirUsuario(DataTableReader Resultado)
        {
            return new Usuario()
            {
                Id = int.Parse(Resultado["idUsuario"].ToString()),
                Nick = Resultado["nickUsuario"].ToString(),

                Email = Resultado["emailUsuario"].ToString(),
                Fecha = DateTime.Parse(Resultado["fechaUsuario"].ToString()),
                Foto = Resultado["fotoUsuario"].ToString()!= "" ? @"/Directorio/Users/" + Resultado["fotoUsuario"].ToString() : @"/Directorio/Users/" + "User.png",
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
                    Id = int.Parse( Resultado["idEstatus"].ToString()),
                    Descripcion = Resultado["descripcionEstatus"].ToString()
                }
            };

        }

        private UsuarioBasico ConstruirUsuarioBasico(DataTableReader Resultado)
        {
            var usuario = new UsuarioBasico()
            {
                Id = int.Parse(Resultado["idUsuario"].ToString()),
                Nick = Resultado["nickUsuario"].ToString(),
                Foto = Resultado["fotoUsuario"].ToString() != "" ? @"/Directorio/Users/" + Resultado["fotoUsuario"].ToString() : @"/Directorio/Users/" + "User.png",
                Estatus = new Estatus()
                {
                    Id = int.Parse(Resultado["idEstatus"].ToString()),
                    Descripcion = Resultado["descripcionEstatus"].ToString()
                }
            };
            if (Resultado["idUsuarioFisica"].ToString() != "")
            {
                usuario.IdUsuarioFisica = int.Parse(Resultado["idUsuarioFisica"].ToString());
                usuario.Paterno = Resultado["paternoUsuarioFisica"].ToString();
                usuario.Materno = Resultado["maternoUsuarioFisica"].ToString();
                usuario.Nombre = Resultado["nombreUsuarioFisica"].ToString();
                usuario.Genero = int.Parse(Resultado["generoUsuarioFisica"].ToString());
            }
            else {
                usuario.IdUsuarioMoral = int.Parse(Resultado["idUsuarioMoral"].ToString());
                usuario.Razon = Resultado["razonUsuarioMoral"].ToString();
                usuario.Paterno = Resultado["paternoUsuarioMoral"].ToString();
                usuario.Materno = Resultado["maternoUsuarioMoral"].ToString();
                usuario.Nombre = Resultado["nombreUsuarioMoral"].ToString();
            }
            try { usuario.Token = Resultado["usuarioToken"].ToString(); } catch (Exception error) { }
            try { usuario.Convalidado= int.Parse(Resultado["usuarioConvalidado"].ToString()); } catch (Exception error) { }
            return usuario;
        }
        private UsuarioKardex ConstruirUsuarioKardex(DataTableReader Resultado)
        {
            return new UsuarioKardex()
            {
                Id = int.Parse(Resultado["idUsuario"].ToString()),
                Nick = Resultado["nickUsuario"].ToString(),
                Kardex = ConstruirKardex(Resultado),
                Foto = Resultado["fotoUsuario"].ToString() != "" ? @"/Directorio/Users/" + Resultado["fotoUsuario"].ToString() : @"/Directorio/Users/" + "User.png",
                Estatus = new Estatus()
                {
                    Id = int.Parse(Resultado["idEstatusUsuario"].ToString()),
                    Descripcion = Resultado["descripcionEstatusUsuario"].ToString()
                },
                TipoUsuario = (TipoUsuario)int.Parse(Resultado["tipoUsuario"].ToString()),
                IdUsuarioFisica = int.Parse(Resultado["idUsuarioFisica"].ToString()),
                Paterno = Resultado["paternoUsuarioFisica"].ToString(),
                Materno = Resultado["maternoUsuarioFisica"].ToString(),
                Nombre = Resultado["nombreUsuarioFisica"].ToString(),
                Genero = int.Parse(Resultado["generoUsuarioFisica"].ToString())
            };

        }
        private UsuarioBase ConstruirUsuarioBase(DataTableReader Resultado)
        {
            var usuario = new UsuarioBase()
            {
                Id = int.Parse(Resultado["idUsuario"].ToString()),
                Nick = Resultado["nickUsuario"].ToString(),

                Email = Resultado["emailUsuario"].ToString(),
                Fecha = DateTime.Parse(Resultado["fechaUsuario"].ToString()),
                Foto = Resultado["fotoUsuario"].ToString()!= "" ? @"/Directorio/Users/" + Resultado["fotoUsuario"].ToString() : @"/Directorio/Users/" + "User.png",
                //Password = Resultado["passwordUsrUsuario"].ToString(),
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
                },

            };
            try {
                usuario.Domicilio = ConstruirDomicilioUsuario(Resultado);
                usuario.Url = Resultado["idArchivo"].ToString();
            }
            catch(Exception error) { }
            try { usuario.Telefono = ConstruirTelefonoUsuario(Resultado); }
            catch (Exception error) { }
            if (Resultado["idUsuarioFisica"].ToString() != "")
                usuario.Fisica = ConstruirUsuarioFisicaBase(Resultado);
            else if (Resultado["idUsuarioMoral"].ToString() != "")
                usuario.Moral = ConstruirUsuarioMoralBase(Resultado);
            return usuario;
        }

        public bool ActualizarUsuario(int IdUsuario, string Nombre, string Nick, string PasswordUsr, string Observacion, bool TipoUsuario, int ClasificacionIdentidad_Id, string Identificacion, int Estatus_Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@PasswordUsr", PasswordUsr));
                prmtrs.Add(new SqlParameter("@Observacion", Observacion));
                prmtrs.Add(new SqlParameter("@TipoUsuario", TipoUsuario));
                prmtrs.Add(new SqlParameter("@ClasificacionIdentidad_Id", ClasificacionIdentidad_Id));
                prmtrs.Add(new SqlParameter("@Identificacion", Identificacion));
                prmtrs.Add(new SqlParameter("@Estatus_Id", Estatus_Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuario_upd", prmtrs))
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

        //Login

        public bool Login(string username, string password)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", username));
                prmtrs.Add(new SqlParameter("@PasswordUsr", password));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.loginUsuario", prmtrs))
                { 
                    while (Resultado.Read())
                    {
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public int CrearModulo(string Nombre)
        {
            int idModulo = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.modulo_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idModulo = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idModulo;
        }
        public Modulo ConsultarModulo(int IdModulo)
        {
            Modulo modulo = null;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Modulo_Id", IdModulo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.modulo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        modulo = ConstruirModulo(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return modulo;
        }
        public List<Modulo> ConsultarModuloColeccion(bool Disponible)
        {
            List<Modulo> modulos = new List<Modulo>(); ;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.moduloColeccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var modulo = ConstruirModulo(Resultado);
                        modulos.Add(modulo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return modulos;
        }
        public List<Modulo> ConsultarModuloKardexColeccion(int IdKardex)
        {
            List<Modulo> modulos = new List<Modulo>(); ;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Kardex_Id", IdKardex));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.moduloKardexColeccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var modulo = ConstruirModulo(Resultado);
                        modulos.Add(modulo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return modulos;
        }
        public List<Modulo> ConsultarModuloUsuarioColeccion(int IdUsuario)
        {
            List<Modulo> modulos = new List<Modulo>(); ;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.moduloUsuarioColeccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var modulo = ConstruirModulo(Resultado);
                        modulos.Add(modulo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return modulos;
        }

        public bool EliminarModulo(int IdModulo)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Modulo_Id", IdModulo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.modulo_del", prmtrs))
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

        public bool EliminarModuloKardex(int IdModuloKardex)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@KardexModulo_Id", IdModuloKardex));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.moduloKardex_del", prmtrs))
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

        public int AgregarModuloUsuario(int IdUsuario, int IdModulo)
        {
            int respuesta = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Modulo_Id", IdModulo));
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.moduloUsuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }

        
        public bool ActualizarModulo(int IdModulo, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Modulo_Id", IdModulo));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.modulo_upd", prmtrs))
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
        private Modulo ConstruirModulo(DataTableReader Resultado)
        {
           var modulo = new Modulo()
            {
                Id = int.Parse(Resultado["Id"].ToString()),
                Nombre = Resultado["Nombre"].ToString(),
                Activo = Resultado["Activo"].ToString() == "1" ? true : false
            };
            try
            {
                modulo.IdKardexModulo = int.Parse(Resultado["km_Id"].ToString());
            }
            catch (Exception Error) { }
            return modulo;
        }
        public bool ActualizarFotoUsuario(long IdUsuario, string Foto)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Foto", Foto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFoto_upd", prmtrs))
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
        public UsuarioKardex SeguridadUsuario(string Nick, string Password)
        {
            UsuarioKardex respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", Password));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioSeguridad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuarioKardex(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public Usuario SeguridadPaciente(string Nick, string Password, string Token)
        {
            Usuario respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", Password));
                if (Token!= "")
                    prmtrs.Add(new SqlParameter("@Token", Token));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.pacienteSeguridad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public UsuarioKardex ConsultarUsuarioKardex(int Id)
        {
            UsuarioKardex respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioKardexId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuarioKardex(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }

        public UsuarioBase SeguridadUsuarioServicios(string Nick, string Password)
        {
            UsuarioBase respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", Password));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioSeguridadServicios_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuarioBase(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public void AsignarToken(int IdUsuario, string Token)
        {
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Token", Token));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioAsignarToken_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }
        public UsuarioKardex SeguridadUsuarioKardex(string Nick, string Password)
        {
            UsuarioKardex respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", Password));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioSeguridad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuarioKardex(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public string SaveImageUsuario(string ImgStr)
        {
            string imageName = "";
            Guid guid = Guid.NewGuid();
            try
            {
                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString() + @"\Users\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                imageName = guid + ".jpg";
                string imgPath = Path.Combine(path, imageName);
                byte[] imageBytes = Convert.FromBase64String(ImgStr);
                if (imageBytes.Length > 0)
                {
                    File.WriteAllBytes(imgPath, imageBytes);
                    try
                    {
                        CreateAndSaveThumbnail(path + @"\" + imageName, path + @"\" + "min_" + imageName, 80);

                    }
                    catch (Exception error) { }

                }
                else
                    imageName = "";


                return "min_" + imageName;
            }
            catch (Exception error)
            {
                throw error;
            }
        }
        public string SaveImageUsuarioIOS(string ImgStr)
        {
            string imageName = "";
            Guid guid = Guid.NewGuid();
            try
            {
                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString() + @"\Users\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                imageName = guid + ".jpeg";
                string imgPath = Path.Combine(path, imageName);
                byte[] imageBytes = Convert.FromBase64String(ImgStr);
                if (imageBytes.Length > 0)
                {
                    File.WriteAllBytes(imgPath, imageBytes);
                    try
                    {
                        CreateAndSaveThumbnail(path + @"\" + imageName, path + @"\" + "min_" + imageName, 80);
                    }
                    catch (Exception error) { }

                }
                else
                    imageName = "";


                return "min_" + imageName;
            }
            catch (Exception error)
            {
                throw error;
            }
        }
        public bool ActualizarMiFotoIOS(long IdUsuario, string Foto)
        {
            var imagen = SaveImageUsuarioIOS(Foto);
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Foto", imagen));
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFoto_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarMiFoto(long Identificador, string Foto)
        {
            var imagen = SaveImageUsuario(Foto);
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Foto", imagen));
                prmtrs.Add(new SqlParameter("@Usuario_Id", Identificador));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioFoto_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public static void CreateAndSaveThumbnail(string path, string output, int desiredSize)
        {
            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(path)))
            using (Bitmap src = new Bitmap(ms))
            {
                Rectangle crop = src.Width > src.Height ?
                    new Rectangle((src.Width - src.Height) / 2, 0, src.Height, src.Height) :
                    new Rectangle(0, 0, src.Width, src.Width);

                int size = Math.Min(desiredSize, crop.Width);

                using (Bitmap cropped = src.Clone(crop, src.PixelFormat))
                using (Bitmap resized = new Bitmap(size, size))
                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(cropped, 0, 0, size, size);
                    resized.Save(output, ImageFormat.Jpeg);
                }
            }
        }
        public bool ActualizarPassUsuario(int IdUsuario, string Pass)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Pass", MD5Hash(Pass)));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuarioPass_upd", prmtrs))
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
        private string MD5Hash(string text)
        {

            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}