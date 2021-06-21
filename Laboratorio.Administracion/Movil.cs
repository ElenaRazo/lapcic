using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Laboratorio.Administracion;
using Laboratorio.Libreria.BaseDatos;

namespace Laboratorio.Administracion.Movil
{
    public class Movil : Libreria.BaseClass.BaseObject
    {
        public Movil(string ConnectionString) : base(ConnectionString)
        {

        }
        public Movil(IBaseDatos Conexion) : base(Conexion)
        {

        }
        public MovilRespuesta PostAutenticarUsuarioToken(string Usuario, string Password, string Token)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.SeguridadPaciente(Usuario, Password, Token);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }

        public MovilRespuesta PostConsultarEstudiosPaciente(int Id)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarSolicitudPaciente(Id);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostActualizarMiPass(int Id, string Pass)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ActualizarPassUsuario(Id, Pass);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarEstudiosPacientePeriodo(int Id, int Anio)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarSolicitudPacientePeriodo(Id, Anio);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarEstudiosPacienteBusqueda(int Id, string Nombre)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarSolicitudPacienteBusqueda(Id, Nombre);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarEstudiosGabinetePaciente(int Id)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarEstudioGabinetePaciente(Id);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarDetalleGabinete(int Id)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarEstudioGabineteId(Id);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }

        public MovilRespuesta PostConsultarDetalleSolicitud(int Id)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarSolicitudPaciente(Id);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarEstudiosSolicitud(string ClaveSolicitud)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarEstudioSolicitud(ClaveSolicitud);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarResultadoSolicitud(string ClaveSolicitud)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarResutladoSolicitud(ClaveSolicitud);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarEstudiosNombre(string Nombre)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarEstudioNombre(Nombre);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostCrearCotizacion(string Nombre, string Paterno, string Materno, string Email, List<string> Estudios)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.CrearCotizacion(Nombre, Paterno, Materno, 1, Estudios, 1);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        /*public MovilRespuesta PostActualizarSesion(long Identificador)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Laboratorio.Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ActualizarSesion(Identificador);
                    if (respuesta.Respuesta != null)
                        respuesta.ResponseFlag = ResponseFlag.OK;
                    else
                    {
                        respuesta.ResponseFlag = ResponseFlag.FAIL;
                        respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }
        public MovilRespuesta PostConsultarReportesEmpleado(DateTime Inicio, DateTime Fin, long IdEmpleado)
        {
            var respuesta = new MovilRespuesta();
            using (var manage = new Laboratorio.Administracion.Metodos(base.conexion))
            {
                try
                {
                    respuesta.Respuesta = manage.ConsultarMensajeTipoColeccionEmpleado(Inicio, Fin, 2, IdEmpleado); respuesta.ResponseFlag = ResponseFlag.OK;
                }
                catch (Exception ex)
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Traza = ex.StackTrace.ToString();
                }
            }
            return respuesta;
        }*/
    }
}
