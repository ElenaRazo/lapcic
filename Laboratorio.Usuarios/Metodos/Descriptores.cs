using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Laboratorio.Usuarios.Descriptores.Atributos;
using Laboratorio.Libreria.BaseDatos;

namespace Laboratorio.Usuarios.Descriptores.Metodos
{
    public class Descriptores : Libreria.BaseClass.BaseObject
    {
        public Descriptores(string ConnectionString) : base(ConnectionString)
        {

        }

        public Descriptores(IBaseDatos Conexion) : base(Conexion)
        {

        }

        #region object set
        public int CrearEmpresa(string Nombre, string RFC, string RazonSocial, string Direccion, string CP, string Colonia, int IdMunicipio)
        {
            int idEmpresa = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@RFC", RFC));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@CP", CP));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                prmtrs.Add(new SqlParameter("@Ciudad_Id", IdMunicipio));
                prmtrs.Add(new SqlParameter("@RazonSocial", RazonSocial));

                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empresa_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idEmpresa = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idEmpresa;
        }
        public int CrearDivision(int IdEmpresa, int Clasificacion, string Nombre, string Descripcion)
        {
            int idDepartamento = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Empresa_Id", IdEmpresa));
                prmtrs.Add(new SqlParameter("@ClasificacionDivision_Id", Clasificacion));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.division_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDepartamento = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDepartamento;
        }
        public int CrearSeccion(int IdDivision, string Nombre)
        {
            int idDivision = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Division_Id", IdDivision));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.seccion_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idDivision = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idDivision;
        }
        public int CrearOficina(int IdTipoOficina, int IdCiudad, int IdSeccion, string Nombre, string Descripcion, string Direccion, string Colonia, int Clave, string CP)
        {
            int idOficina = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@TipoOficina_Id", IdTipoOficina));
                prmtrs.Add(new SqlParameter("@Ciudad_Id", IdCiudad));
                prmtrs.Add(new SqlParameter("@Seccion_Id", IdSeccion));
                prmtrs.Add(new SqlParameter("@Cp", CP));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficina_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idOficina = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idOficina;
        }
        public int CrearTipoOficina(string Nombre, string Descripcion)
        {
            int idTipoOficina = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoOficina_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idTipoOficina = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idTipoOficina;
        }
        public int CrearClasificacionTelefono(string Nombre)
        {
            int idClasificacionTelefono = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionTelefono_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idClasificacionTelefono = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idClasificacionTelefono;
        }
        public int CrearClasificacionDivision(string Nombre)
        {
            int idClasificacionDivision = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionDivision_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idClasificacionDivision = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idClasificacionDivision;
        }
        public int CrearClasificacionIdentidad(string Nombre)
        {
            int idClasificacionIdentidad = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionIdentidad_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idClasificacionIdentidad = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idClasificacionIdentidad;
        }
        public int CrearClasificacionCuenta(string Nombre, string Descripcion)
        {
            int idClasificacionCuenta = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionCuenta_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idClasificacionCuenta = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idClasificacionCuenta;
        }
        public int CrearPuesto(string Nombre, int ClasificacionCuenta)
        {
            int idPuesto = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@ClasificacionCuenta_Id", ClasificacionCuenta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.puestos_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idPuesto = Resultado.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idPuesto;
        }

        public int CrearModulo(string Nombre)
        {
            int idModulo = 0;
            try
            {
                List<object> prmtrs = new List<object>();
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
        #endregion

        #region update
        public bool ActualizarClasificacionTelefono(int IdClasificacionTelefono, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@ClasificacionTelefono_Id", IdClasificacionTelefono));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionTelefono_upd", prmtrs))
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
        public bool ActualizarClasificacionDivision(int IdClasificacionDivision, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@ClasificacionDivision_Id", IdClasificacionDivision));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionDivision_upd", prmtrs))
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
        public bool ActualizarClasificacionIdentidad(int IdClasificacionIdentidad, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@ClasificacionIdentidad_Id", IdClasificacionIdentidad));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionIdentidad_upd", prmtrs))
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
                List<object> prmtrs = new List<object>();
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
        public bool ActualizarTipoOficina(int IdTipoOficina, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@TipoOficina_Id", IdTipoOficina));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoOficina_upd", prmtrs))
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

        public bool AsignarArchivo(int IdOficina, long IdArchivo)
        {
            bool resultado = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", IdArchivo));
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaArchivo_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        resultado = Resultado.GetInt32(0) == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return resultado;
        }
        public bool ActualizarPuesto(int IdPuesto, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Puestos_Id", IdPuesto));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.puestos_upd", prmtrs))
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
        public bool ActualizarSeccion(int IdSeccion, int IdDivision, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Seccion_Id", IdSeccion));
                prmtrs.Add(new SqlParameter("@Division_Id", IdDivision));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.seccion_upd", prmtrs))
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
        public bool ActualizarDivision(int IdDivision, int IdClasificacionDivision, int IdEmpresa, string Nombre, string Descripcion)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Division_Id", IdDivision));
                prmtrs.Add(new SqlParameter("@ClasificacionDivision_Id", IdClasificacionDivision));
                prmtrs.Add(new SqlParameter("@Empresa_Id", IdEmpresa));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.division_upd", prmtrs))
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
        public bool ActualizarEmpresa(int IdEmpresa, string Nombre, string Colonia, string RFC, string Direccion, int IdCiudad, string Codigo, string RazonSocial)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Empresa_Id", IdEmpresa));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                prmtrs.Add(new SqlParameter("@RFC", RFC));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Ciudad_Id", IdCiudad));
                prmtrs.Add(new SqlParameter("@CP", Codigo));
                prmtrs.Add(new SqlParameter("@RazonSocial", RazonSocial));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empresa_upd", prmtrs))
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
        public bool ActualizarOficina(int IdOficina, int IdTipoOficina, int IdCiudad, int IdSeccion, string Codigo, string Nombre, string Descripcion, string Direccion, string Clave, string Colonia)
        {
            bool respuesta = false;
            try
            {

                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                prmtrs.Add(new SqlParameter("@TipoOficina_Id", IdTipoOficina));
                prmtrs.Add(new SqlParameter("@Ciudad_Id", IdCiudad));
                prmtrs.Add(new SqlParameter("@Seccion_Id", IdSeccion));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                prmtrs.Add(new SqlParameter("@Cp", Codigo));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficina_upd", prmtrs))
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
        #endregion
        #region baja
        public bool EliminarClasificacionTelefono(int IdClasificacionTelefono)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@ClasificacionTelefono_Id", IdClasificacionTelefono));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionTelefono_del", prmtrs))
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
        public bool EliminarClasificacionDivision(int IdClasificacionDivision)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@ClasificacionDivision_Id", IdClasificacionDivision));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionDivision_del", prmtrs))
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
        public bool EliminarClasificacionIdentidad(int IdClasificacionIdentidad)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@ClasificacionIdentidad_Id", IdClasificacionIdentidad));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionIdentidad_del", prmtrs))
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
        public bool EliminarClasificacionCuenta(int IdClasificacionCuenta)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
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
        public bool EliminarTipoOficina(int IdTipoOficina)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@TipoOficina_Id", IdTipoOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoOficina_del", prmtrs))
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
        public bool EliminarSeccion(int IdSeccion)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Seccion_Id", IdSeccion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.seccion_del", prmtrs))
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
        public bool EliminarDivision(int IdDivision)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Division_Id", IdDivision));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.division_del", prmtrs))
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
        public bool EliminarOficina(int IdOficina)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficina_del", prmtrs))
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
        public bool EliminarPuesto(int IdPuesto)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Puestos_Id", IdPuesto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.puestos_del", prmtrs))
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
        public bool EliminarEmpresa(int IdEmpresa)
        {
            bool respuesta = false;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Empresa_Id", IdEmpresa));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empresa_del", prmtrs))
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
        #endregion
        #region object get

        //Empresa
        public Empresa ConsultarEmpresa(int IdEmpresa, bool Disponible)
        {
            Empresa empresa = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Empresa_Id", IdEmpresa));
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empresa_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        empresa = new Empresa()
                        {
                            Id = int.Parse(Resultado["idEmpresa"].ToString()),
                            Nombre = Resultado["nombreEmpresa"].ToString(),
                            RFC = Resultado["rfcEmpresa"].ToString(),
                            Direccion = Resultado["direccionEmpresa"].ToString(),
                            Colonia = Resultado["coloniaEmpresa"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return empresa;
        }
        public List<Empresa> ConsultarEmpresaColeccion(bool Disponible)
        {
            List<Empresa> empresas = new List<Empresa>();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empresaColeccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Empresa registro = new Empresa()
                        {
                            Id = int.Parse(Resultado["idEmpresa"].ToString()),
                            Nombre = Resultado["nombreEmpresa"].ToString(),
                            RFC = Resultado["rfcEmpresa"].ToString(),
                            RazonSocial = Resultado["razonsocialEmpresa"].ToString(),
                            Codigo = Resultado["cpEmpresa"].ToString(),
                            Colonia = Resultado["coloniaEmpresa"].ToString(),
                            Direccion = Resultado["direccionEmpresa"].ToString(),
                            Municipio = new Municipio() {
                                Id = int.Parse(Resultado["idCiudad"].ToString()),
                                Nombre = Resultado["nombreCiudad"].ToString()
                            }
                        };
                        empresas.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return empresas;
        }

        //Division
        public Division ConsultarDivision(int IdDivision)
        {
            Division division = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Division_Id", IdDivision));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.division_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        division = ConstruirDivision(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return division;
        }
        public List<Division> ConsultarDivisionEmpresa(int IdEmpresa, bool Disponible)
        {
            List<Division> divisiones = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Empresa_Id", IdEmpresa));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.divisionEmpresa_get", prmtrs))
                {
                    divisiones = new List<Division>();

                    while (Resultado.Read())
                    {
                        var Division = ConstruirDivision(Resultado);
                        divisiones.Add(Division);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return divisiones;
        }
        public List<Division> ConsultarDivisionColeccion(bool Disponible)
        {
            List<Division> divisiones = null;
            try
            {
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.divisionColeccion_get", null))
                {
                    divisiones = new List<Division>();

                    while (Resultado.Read())
                    {
                        var Division = ConstruirDivision(Resultado);
                        divisiones.Add(Division);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return divisiones;
        }
        public List<ClasificacionDivision> ConsultarClasificacionDivisionColeccion(bool Disponible)
        {
            List<ClasificacionDivision> divisiones = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionDivisionColeccion_get", prmtrs))
                {
                    divisiones = new List<ClasificacionDivision>();

                    while (Resultado.Read())
                    {
                        var Division = ConstruirClasificacionDivision(Resultado);
                        divisiones.Add(Division);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return divisiones;
        }
        private Division ConstruirDivisionRow(DataRow Resultado)
        {
            return new Division()
            {
                Id = int.Parse(Resultado["idDivision"].ToString()),
                Nombre = Resultado["nombreDivision"].ToString(),
                Empresa = new EmpresaBase()
                {
                    Id = int.Parse(Resultado["idEmpresa"].ToString()),
                    Nombre = Resultado["nombreEmpresa"].ToString(),
                    RFC = Resultado["rfcEmpresa"].ToString()
                },
                ClasificacionDivision = new ClasificacionDivision()
                {
                    Id = int.Parse(Resultado["idClasificacionDivision"].ToString()),
                    Nombre = Resultado["nombreClasificacionDivision"].ToString()
                }
            };
        }
        private Division ConstruirDivision(DataTableReader Resultado)
        {
            return new Division()
            {
                Id = int.Parse(Resultado["idDivision"].ToString()),
                Nombre = Resultado["nombreDivision"].ToString(),
                Empresa = new EmpresaBase()
                {
                    Id = int.Parse(Resultado["idEmpresa"].ToString()),
                    Nombre = Resultado["nombreEmpresa"].ToString(),
                    RFC = Resultado["rfcEmpresa"].ToString()
                },
                ClasificacionDivision = new ClasificacionDivision() {
                    Id = int.Parse(Resultado["idClasificacionDivision"].ToString()),
                    Nombre = Resultado["nombreClasificacionDivision"].ToString()
                }
            };
        }

        private ClasificacionDivision ConstruirClasificacionDivision(DataTableReader Resultado)
        {
            return new ClasificacionDivision()
            {
                Id = int.Parse(Resultado["idClasificacionDivision"].ToString()),
                Nombre = Resultado["nombreClasificacionDivision"].ToString(),
                Disponible = Resultado["disponibleClasificacionDivision"].ToString() == "1",
            };
        }
        //Seccion
        public Seccion ConsultarSeccion(int IdSeccion)
        {
            Seccion Seccion = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Seccion_Id", IdSeccion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.seccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Seccion = ConstruirSeccion(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return Seccion;
        }
        public List<Seccion> ConsultarSeccionDivision(int IdDivision, bool Disponible)
        {
            List<Seccion> Secciones = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Division_Id", IdDivision));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.seccionDivision_get", prmtrs))
                {
                    Secciones = new List<Seccion>();
                    while (Resultado.Read())
                    {
                        var Seccion = ConstruirSeccion(Resultado);
                        Secciones.Add(Seccion);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return Secciones;
        }
        public List<Seccion> ConsultarSeccionColeccion(bool Disponible)
        {
            List<Seccion> Secciones = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Estatus_Id", 1));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.seccionColeccion_get", prmtrs))
                {
                    Secciones = new List<Seccion>();
                    while (Resultado.Read())
                    {
                        var Seccion = ConstruirSeccion(Resultado);
                        Secciones.Add(Seccion);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return Secciones;
        }
        private Seccion ConstruirSeccion(DataTableReader Resultado)
        {
            return new Seccion()
            {
                Id = int.Parse(Resultado["idSeccion"].ToString()),
                Division = (Resultado["idDivision"].ToString() != "" && Resultado["idDivision"] != null) ? new DivisionBase()
                {
                    Id = int.Parse(Resultado["idDivision"].ToString()),
                    Nombre = Resultado["nombreDivision"].ToString(),
                    Disponible = Resultado["nombreDivision"].ToString() == "1"
                } : null,
                Nombre = Resultado["nombreSeccion"].ToString()
            };
        }


        //Oficina
        public Oficina ConsultarOficina(int IdOficina)
        {
            Oficina oficina = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficina_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        oficina = ConstruirOficina(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficina;
        }
        public OficinaBase ConsultarOficinaBase(int IdOficina)
        {
            OficinaBase oficina = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaBase_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        oficina = ConstruirOficinaBase(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficina;
        }
        public OficinaBasica ConsultarOficinaBasica(int IdOficina)
        {
            OficinaBasica oficina = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaBase_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        oficina = ConstruirOficinaBasica(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficina;
        }
        public long ConsultarArchivoOficina(int IdOficina)
        {
            long archivo = 0;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Oficina_Id", IdOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaArchivo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        archivo = long.Parse(Resultado["Archivo"].ToString());
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
        public List<Oficina> ConsultarOficinaMunicipio(int IdMunicipio)
        {
            List<Oficina> oficinas = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Ciudad_Id", IdMunicipio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaCiudad_get", prmtrs))
                {
                    oficinas = new List<Oficina>();
                    while (Resultado.Read())
                    {
                        var oficina = ConstruirOficina(Resultado);
                        oficinas.Add(oficina);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficinas;
        }
        public List<Oficina> ConsultarOficinaSeccion(int IdSeccion)
        {
            List<Oficina> oficinas = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Seccion_Id", IdSeccion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaSeccion_get", prmtrs))
                {
                    oficinas = new List<Oficina>();
                    while (Resultado.Read())
                    {
                        var oficina = ConstruirOficina(Resultado);
                        oficinas.Add(oficina);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficinas;
        }
        public List<Oficina> ConsultarOficinaTipo(int IdTipoOficina)
        {
            List<Oficina> oficinas = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@TipoOficina_Id", IdTipoOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaTipoOficina_get", prmtrs))
                {
                    oficinas = new List<Oficina>();
                    while (Resultado.Read())
                    {
                        var oficina = ConstruirOficina(Resultado);
                        oficinas.Add(oficina);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficinas;
        }
        public List<OficinaAutoridad> ConsultarOficinaTipoAutoridad(int IdTipoOficina)
        {
            List<OficinaAutoridad> oficinas = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@TipoOficina_Id", IdTipoOficina));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaTipoOficinaAutoridad_get", prmtrs))
                {
                    oficinas = new List<OficinaAutoridad>();
                    while (Resultado.Read())
                    {
                        var oficina = ConstruirOficinaAutoridad(Resultado);
                        oficinas.Add(oficina);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficinas;
        }
        public List<Oficina> ConsultarOficinaColeccion(bool Disponible)
        {
            List<Oficina> oficinas = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.oficinaColeccion_get", prmtrs))
                {
                    oficinas = new List<Oficina>();
                    while (Resultado.Read())
                    {
                        var oficina = ConstruirOficina(Resultado);
                        oficinas.Add(oficina);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return oficinas;
        }
        private Oficina ConstruirOficina(DataTableReader Resultado)
        {
            return new Oficina()
            {
                Id = int.Parse(Resultado["idOficina"].ToString()),
               // IdArchivo = Resultado["idArchivoOficina"].ToString() != "" ? long.Parse(Resultado["idArchivoOficina"].ToString()) : 0,
                Clave =Resultado["claveOficina"].ToString(),
                Nombre = Resultado["nombreOficina"].ToString(),
                Descripcion = Resultado["descripcionOficina"].ToString(),
                Direccion = Resultado["direccionOficina"].ToString(),
                Codigo = Resultado["cpOficina"].ToString(),
                Colonia = Resultado["coloniaOficina"].ToString(),
                TipoOficina = new TipoOficina()
                {
                    Id = int.Parse(Resultado["idTipoOficina"].ToString()),
                    Nombre = Resultado["nombreTipoOficina"].ToString(),
                    //Descripcion = Resultado["descripcionTipoOficina"].ToString(),
                    Disponible = Resultado["disponibleTipoOficina"].ToString() == "1"
                },
                Municipio = new Municipio()
                {
                    Id = int.Parse(Resultado["idCiudad"].ToString()),
                    Nombre = Resultado["nombreCiudad"].ToString()
                },
                Seccion = new Seccion()
                {
                    Id = int.Parse(Resultado["idSeccion"].ToString()),
                    Nombre = Resultado["nombreSeccion"].ToString(),
                    Division = Resultado["idDivision"] != null && Resultado["idDivision"].ToString() != "" ? new DivisionBase()
                    {
                        Id = int.Parse(Resultado["idDivision"].ToString()),
                        Nombre = Resultado["nombreDivision"].ToString()

                    } : null
                }
                
            };
        }

        private OficinaBase ConstruirOficinaBase(DataTableReader Resultado)
        {
            return new OficinaBase()
            {
                Id = int.Parse(Resultado["idOficina"].ToString()),
                Nombre = Resultado["nombreOficina"].ToString(),
                Descripcion = Resultado["descripcionOficina"].ToString(),
                Municipio = new Municipio()
                {
                    Id = int.Parse(Resultado["idCiudad"].ToString()),
                    Nombre = Resultado["nombreCiudad"].ToString()
                }
            };
        }
        private OficinaBasica ConstruirOficinaBasica(DataTableReader Resultado)
        {
            return new OficinaBasica()
            {
                Id = int.Parse(Resultado["idOficina"].ToString()),
                Nombre = Resultado["nombreOficina"].ToString(),
                Descripcion = Resultado["descripcionOficina"].ToString(),
            };
        }
        private OficinaAutoridad ConstruirOficinaAutoridad(DataTableReader Resultado)
        {
           var oficina = new OficinaAutoridad()
            {
                Id = int.Parse(Resultado["idOficina"].ToString()),
                Nombre = Resultado["nombreOficina"].ToString(),
                Descripcion = Resultado["descripcionOficina"].ToString(),
                Municipio = new Municipio()
                {
                    Id = int.Parse(Resultado["idCiudad"].ToString()),
                    Nombre = Resultado["nombreCiudad"].ToString()
                }
            };
            try
            {
                if (Resultado["idMagistrado"].ToString() != "")
                {
                    using (var usuarios = new Usuarios.Metodos.UsuarioSeguridad(base.conexion))
                    {
                        oficina.Magistrado = usuarios.ConsultarUsuarioBasico(int.Parse(Resultado["idMagistrado"].ToString()));
                    }
                    //oficina.Magistrado = new Usuarios.Atributos.UsuarioBasico()
                    //{
                    //    Id = int.Parse(Resultado["idMagistrado"].ToString()),
                    //    Nick = Resultado["nickMagistrado"].ToString(),
                    //    Foto = Resultado["fotoMagistrado"].ToString() != "" ? @"\Directorio\Users\" + Resultado["fotoMagistrado"].ToString() : "",
                    //    IdUsuarioFisica = int.Parse(Resultado["idUsuarioMagistrado"].ToString()),
                    //    Paterno = Resultado["paternoMagistrado"].ToString(),
                    //    Materno = Resultado["maternoMagistrado"].ToString(),
                    //    Nombre = Resultado["nombreMagistrado"].ToString(),
                    //    Genero = int.Parse(Resultado["generoMagistrado"].ToString())
                    //};
                }
            }
            catch (Exception error) { }
            try { 
                if (Resultado["idSecretario"].ToString() != "")
                {
                    using (var usuarios = new Usuarios.Metodos.UsuarioSeguridad(base.conexion))
                    {
                        oficina.Secretario = usuarios.ConsultarUsuarioBasico(int.Parse(Resultado["idSecretario"].ToString()));
                    }
                    //oficina.Secretario = new Usuarios.Atributos.UsuarioBasico()
                    //{
                    //    Id = int.Parse(Resultado["idSecretario"].ToString()),
                    //    Nick = Resultado["nickSecretario"].ToString(),
                    //    Foto = Resultado["fotoSecretario"].ToString() != "" ? @"\Directorio\Users\" + Resultado["fotoSecretario"].ToString() : "",
                    //    IdUsuarioFisica = int.Parse(Resultado["idUsuarioSecretario"].ToString()),
                    //    Paterno = Resultado["paternoSecretario"].ToString(),
                    //    Materno = Resultado["maternoSecretario"].ToString(),
                    //    Nombre = Resultado["nombreSecretario"].ToString(),
                    //    Genero = int.Parse(Resultado["generoSecretario"].ToString())
                    //};
                }
            }
            catch (Exception error) { }
            return oficina;
        }

        //Estado
        public Estado ConsultarEstado(int IdEstado)
        {
            Estado estado = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Estado_Id", IdEstado));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estado_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        estado = ConstruirEstado(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estado;
        }
        public List<Estado> ConsultarEstadoPais(int IdPais)
        {
            List<Estado> estado = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Pais_Id", IdPais));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estadoPais_get", prmtrs))
                {
                    estado = new List<Estado>();
                    while (Resultado.Read())
                    {
                        estado.Add(ConstruirEstado(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estado;
        }
        public List<Estado> ConsultarEstadoColeccion()
        {
            List<Estado> estados = null;
            try
            {
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estadoColeccion_get", null))
                {
                    estados = new List<Estado>();
                    while (Resultado.Read())
                    {
                        var estado = ConstruirEstado(Resultado);
                        estados.Add(estado);
                    }

                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estados;
        }
        private Estado ConstruirEstado(DataTableReader Resultado)
        {
            return new Estado()
            {
                Id = int.Parse(Resultado["idEstado"].ToString()),
                Nombre = Resultado["nombreEstado"].ToString(),
                Pais = new Pais() {
                    Id = int.Parse(Resultado["idPais"].ToString()),
                    Nombre = Resultado["nombrePais"].ToString(),
                    Abreviatura = Resultado["abreviaturaPais"].ToString(),
                }
            };
        }

        private Estado ConstruirEstadoBase(DataTableReader Resultado)
        {
            return new Estado()
            {
                Id = int.Parse(Resultado["idEstado"].ToString()),
                Nombre = Resultado["nombreEstado"].ToString()
            };
        }

        //Municipio

        public Colonia ConsultarColonia(int IdColonia)
        {
            Colonia Colonia = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Colonia_Id", IdColonia));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.colonia_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Colonia = ConstruirColonia(Resultado);

                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return Colonia;
        }
        public Municipio ConsultarMunicipio(int IdMunicipio)
        {
            Municipio municipio = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@IdCiudad", IdMunicipio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.municipio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        municipio = ConstruirMunicipio(Resultado);

                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipio;
        }
        public Municipio ConsultarMunicipioBase(int IdMunicipio)
        {
            Municipio municipio = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@IdCiudad", IdMunicipio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.municipio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        municipio = ConstruirMunicipioBase(Resultado);

                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipio;
        }
        public List<Municipio> ConsultarMunicipioEstado(int IdEstado)
        {
            List<Municipio> municipios = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Estado_Id", IdEstado));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ciudadEstado_get", prmtrs))
                {
                    municipios = new List<Municipio>();
                    while (Resultado.Read())
                    {
                        var municipio = ConstruirMunicipio(Resultado);
                        municipios.Add(municipio);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipios;
        }
        public List<Municipio> ConsultarMunicipioColeccion()
        {
            List<Municipio> municipios = null;
            try
            {
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ciudadColeccion_get", null))
                {
                    municipios = new List<Municipio>();
                    while (Resultado.Read())
                    {
                        var municipio = ConstruirMunicipio(Resultado);
                        municipios.Add(municipio);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipios;
        }
        private Municipio ConstruirMunicipio(DataTableReader Resultado)
        {
            return new Municipio()
            {
                Id = int.Parse(Resultado["idCiudad"].ToString()),
                Nombre = Resultado["nombreCiudad"].ToString(),
                Clave = int.Parse(Resultado["claveCiudad"].ToString()),
                Estado = ConstruirEstado(Resultado)
            };
        }

        private Municipio ConstruirMunicipioBase(DataTableReader Resultado)
        {
            return new Municipio()
            {
                Id = int.Parse(Resultado["idCiudad"].ToString()),
                Nombre = Resultado["nombreCiudad"].ToString(),
                Clave = int.Parse(Resultado["claveCiudad"].ToString()),
                Estado = ConstruirEstadoBase(Resultado)
            };
        }


        private Colonia ConstruirColonia(DataTableReader Resultado)
        {
            return new Colonia()
            {
                Id = int.Parse(Resultado["idColonia"].ToString()),
                Nombre = Resultado["nombreColonia"].ToString(),
                CodigoPostal =Resultado["codigoColonia"].ToString()
            };
        }
        public List<TipoOficina> ConsultarTipoOficinaColeccion(bool Disponible)
        {
            List<TipoOficina> tiposOficina = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoOficinaColeccion_get", prmtrs))
                {
                    tiposOficina = new List<TipoOficina>();
                    while (Resultado.Read())
                    {
                        var tipoOficina = ConstruirTipoOficina(Resultado);
                        tiposOficina.Add(tipoOficina);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tiposOficina;
        }
        private TipoOficina ConstruirTipoOficina(DataTableReader Resultado)
        {
            return new TipoOficina()
            {
                Id = int.Parse(Resultado["idTipoOficina"].ToString()),
                Nombre = Resultado["nombreTipoOficina"].ToString(),
                Disponible = Resultado["disponibleTipoOficina"].ToString() == "1"
            };
        }

        //Tipo clasificacion telefono 
        public ClasificacionTelefono ConsultarClasificacionTelefono(int IdClasificacionTelefono)
        {
            ClasificacionTelefono clasificacion = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@TipoMateria_Id", IdClasificacionTelefono));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionTelefono_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        clasificacion = ConstruirClasificacionTelefono(Resultado);

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
        public List<ClasificacionTelefono> ConsultarClasificacionTelefonoColeccion(bool Disponible)
        {
            List<ClasificacionTelefono> coleccion = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", Disponible ? 1 : 0));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.clasificacionTelefonoColeccion_get", prmtrs))
                {
                    coleccion = new List<ClasificacionTelefono>();
                    while (Resultado.Read())
                    {
                        var clasificacion = ConstruirClasificacionTelefono(Resultado);
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
        private ClasificacionTelefono ConstruirClasificacionTelefono(DataTableReader Resultado)
        {
            return new ClasificacionTelefono()
            {
                IdClasificacionTelefono = int.Parse(Resultado["Id"].ToString()),
                Nombre = Resultado["Nombre"].ToString(),
                Disponible = Resultado["Disponible"].ToString() == "1"
            };
        }
        public List<Colonia> ConsultarColoniaColeccion()
        {
            List<Colonia> colonias = new List<Colonia>();
            try
            {
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.coloniaColeccion_get", null))
                {
                    while (Resultado.Read())
                    {
                        Colonia registro = new Colonia()
                        {
                            Id = int.Parse(Resultado["idColonia"].ToString()),
                            Nombre = Resultado["nombreColonia"].ToString(),
                            CodigoPostal = Resultado["codigoPostalColonia"].ToString(),
                           
                        };
                        colonias.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return colonias;
        }
        public List<Colonia> ConsultarColoniaEstadoColeccion(int Id)
        {
            List<Colonia> colonias = new List<Colonia>();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Estado_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.coloniaColeccionPorEstado_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Colonia registro = new Colonia()
                        {
                            Id = int.Parse(Resultado["idColonia"].ToString()),
                            Nombre = Resultado["nombreColonia"].ToString(),
                            CodigoPostal = Resultado["codigoPostalColonia"].ToString(),

                        };
                        colonias.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return colonias;
        }
        public List<Colonia> ConsultarColoniaMunicipioColeccion(int Id)
        {
            List<Colonia> colonias = new List<Colonia>();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Municipio_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.coloniaColeccionPorMunicipio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Colonia registro = new Colonia()
                        {
                            Id = int.Parse(Resultado["idColonia"].ToString()),
                            Nombre = Resultado["nombreColonia"].ToString(),
                            CodigoPostal = Resultado["codigoPostalColonia"].ToString(),

                        };
                        colonias.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return colonias;
        }
        #endregion
    }
}
