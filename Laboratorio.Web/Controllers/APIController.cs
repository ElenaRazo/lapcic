using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Laboratorio.Administracion;
using Laboratorio.Administracion.Movil;
using Laboratorio.Web.Controllers;

using System.Configuration;
using System.Web.Routing;
using System.Web;

namespace Laboratorio.Web.Controllers
{
    public class APIController : ApiController
    {
        string connectionString = String.Empty;
        public APIController()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Administracion"].ToString();
        }
        [Route("api/Control/PostAutenticarUsuarioToken")]
        public MovilRespuesta PostAutenticarUsuarioToken([FromBody]MovilLoginToken Val)
        {
            var respuesta = new MovilRespuesta();
            using (var access = new Laboratorio.Administracion.Metodos(connectionString))
            {
                var resp = access.SeguridadPaciente(Val.Usuario, Val.Password, Val.Token);
                if (resp != null)
                {
                    resp.Token = TokenGenerator.GenerateTokenJwt(Val.Usuario);
                    respuesta.Respuesta = resp;
                    respuesta.ResponseFlag = ResponseFlag.OK;
                }
                else
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                    respuesta.Respuesta = "USUARIO NO ENCONTRADO";
                }
            }
            return respuesta;
        }
        [Authorize]
        [Route("api/Control/PostConsultarEstudiosPaciente")]
        public MovilRespuesta PostConsultarEstudiosPaciente([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarEstudiosPaciente(Val.Id);
            }
        }
        [Authorize]
        [Route("api/Control/PostActualizarMiPass")]
        public MovilRespuesta PostActualizarMiPass([FromBody]MovilContrasena Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostActualizarMiPass(Val.Identificador, Val.Pass);
            }
        }
        [Authorize]
        [Route("api/Control/PostConsultarEstudiosPacientePeriodo")]
        public MovilRespuesta PostConsultarEstudiosPacientePeriodo([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarEstudiosPacientePeriodo(Val.Id, Val.Anio);
            }
        }
        [Authorize]
        [Route("api/Control/PostConsultarEstudiosPacienteBusqueda")]
        public MovilRespuesta PostConsultarEstudiosPacienteBusqueda([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarEstudiosPacienteBusqueda(Val.Id, Val.Nombre);
            }
        }
        [Authorize]
        [Route("api/Control/PostConsultarEstudiosGabinetePaciente")]
        public MovilRespuesta PostConsultarEstudiosGabinetePaciente([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarEstudiosGabinetePaciente(Val.Id);
            }
        }
        [Authorize]
        [Route("api/Control/PostConsultarDetalleGabinete")]
        public MovilRespuesta PostConsultarDetalleGabinete([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarDetalleGabinete(Val.Id);
            }
        }
        [Authorize]
        [Route("api/Control/PostConsultarDetalleSolicitud")]
        public MovilRespuesta PostConsultarDetalleSolicitud([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarDetalleSolicitud(Val.Id);
            }
        }
        [Authorize]
        [Route("api/Control/PostConsultarEstudiosSolicitud")]
        public MovilRespuesta PostConsultarEstudiosSolicitud([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarEstudiosSolicitud(Val.ClaveSolicitud);
            }
        }
        [Authorize]
        [Route("api/Control/PostConsultarResultadoSolicitud")]
        public MovilRespuesta PostConsultarResultadoEstudioSolicitud([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarResultadoSolicitud(Val.ClaveSolicitud);
            }
        }
        [Route("api/Control/PostConsultarEstudiosNombre")]
        public MovilRespuesta PostConsultarEstudiosNombre([FromBody]MovilDetalle Val)
        {
            using (var access = new Movil(connectionString))
            {
                return access.PostConsultarEstudiosNombre(Val.Nombre);
            }
        }
        [Route("api/Control/PostCrearCotizacion")]
        public MovilRespuesta PostCrearCotizacion([FromBody]MovilCotizacion Val)
        {
            using (var access = new Movil(connectionString))
            {
                List<string> imagenes = new List<string>();
                {
                    var image = Val.Estudios.Replace("[", "");
                    image = image.Replace("]", "");
                    imagenes = image.ToString().Trim(' ').Split(',').ToList();
                }
                var res = access.PostCrearCotizacion(Val.Nombre, Val.Paterno, Val.Materno, Val.Email, imagenes);
                RouteData route = new RouteData();
                route.Values.Add("action", "EnvioCotizacion2"); // ActionName
                route.Values.Add("controller", "Estudios"); // Controller Name
                EstudiosController estudios = new EstudiosController();
                System.Web.Mvc.ControllerContext newContext = new System.Web.Mvc.ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), route, estudios);
                estudios.ControllerContext = newContext;
                estudios.EnvioCotizacion2(Val.Email, res.Respuesta.ToString());
                    return res;
            }

        }

        [Route("api/Control/PostEtiquetas")]
        public MovilRespuesta PostEtiquetas([FromBody]Etiqueta Val)
        {
            var respuesta = new MovilRespuesta();
            using (var access = new Laboratorio.Administracion.Metodos(connectionString))
            {
                var resp = access.ConsultarMuestrasSolicitud(Val.clave);
                if (resp != null)
                {
                    respuesta.Respuesta = resp;
                    respuesta.ResponseFlag = ResponseFlag.OK;
                }
                else
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                }
            }
            return respuesta;
        }
        [Route("api/Control/PostCumpleanos")]
        public MovilRespuesta PostCumpleanos()
        {
            var respuesta = new MovilRespuesta();
            using (var access = new Laboratorio.Administracion.Metodos(connectionString))
            {
                var resp = access.ConsultarUsuariosCumpleanos();
                if (resp != null)
                {
                    respuesta.Respuesta = resp;
                    respuesta.ResponseFlag = ResponseFlag.OK;
                }
                else
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                }
            }
            return respuesta;
        }
        [Route("api/Control/PostCheckin")]
        public MovilRespuesta PostCheckin([FromBody]Chekin Val)
        {
            var respuesta = new MovilRespuesta();
            using (var access = new Laboratorio.Administracion.Metodos(connectionString))
            {
                var resp = access.ChekinUsuario(Val.idusuario,Val.idlaboratorio,Val.maquina,Val.ip);
                if (resp)
                {
                    respuesta.Respuesta = resp;
                    respuesta.ResponseFlag = ResponseFlag.OK;
                }
                else
                {
                    respuesta.ResponseFlag = ResponseFlag.FAIL;
                }
            }
            return respuesta;
        }
    }
   
}