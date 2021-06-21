using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;
using Laboratorio.Administracion;

namespace Laboratorio.Web.Controllers
{
    public class SeguridadController : BaseController
    {
        // GET: Login
        public ActionResult Index(string error)
        {
            this.SharedSession["Usuario"] = null;
            if (error != "")
            {
                ViewBag.Error = error;
            }
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Laboratorios = conexion.ConsultarLaboratorioColeccion();

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Loguear(Usuario model, string returnUrl)
        {
            string respuesta = "";
            if (model.Nick == null || model.Password == null)
            {
                respuesta = "Verifique los datos de ingreso";
            }
            else
            {
                if (model.Laboratorio != 0)
                {
                    respuesta = EstablecerLogin(model);
                }
                else {
                    return RedirectToAction("Index", "Seguridad", new { error = "Seleccione un laboratorio" });
                }
            }
            if (respuesta == "")
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    this.SharedSession["Laboratorio"] = conexion.ConsultarLaboratorioId(model.Laboratorio);
                    var labs = conexion.ConsultarLaboratorioColeccion();
                    List<Administracion.Laboratorio> _labs = new List<Administracion.Laboratorio>();
                    _labs.Add(new Administracion.Laboratorio() { Nombre = "SELECCIONE UNA OPCIÓN", Id = 0 });
                    _labs.AddRange(labs);
                    this.SharedSession["Laboratorios"] = _labs;

                }
                Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
                if (_Usuario.Puesto.Id < 6)
                    return RedirectToAction("Solicitudes", "Estudios");
                else
                    return RedirectToAction("Gabinete", "Estudios");
            }
            else
                return RedirectToAction("Index", "Seguridad", new { error = respuesta });
        }
        public bool CambiarLaboratorio(int Laboratorio) {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                this.SharedSession["Laboratorio"] = conexion.ConsultarLaboratorioId(Laboratorio);

            }
            return true;
        }
        public string EstablecerLogin(Usuario Inicio)
        {
            string error = "";
            string password = Inicio.Password;
            Usuario Usuario = null;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var usuario = conexion.SeguridadUsuario(Inicio.Nick, MD5Hash(password), Inicio.Laboratorio);

                if (usuario == null || usuario.Id == 0)
                {
                    error = "No se ha encontrado dentro del sistema, o no tiene acceso al laboratorio solicitado, contacte al administrador del sistema";
                }
                else
                {
                   // var checkin = conexion.ChekinUsuario(usuario.Id, Inicio.Laboratorio, host, ip.ToString());
                    this.SharedSession["Usuario"] = usuario;
                }
            }
            return error;
        }
        public ActionResult Paciente(string error)
        {
            this.SharedSession["UsuarioPaciente"] = null;
            if (error != "")
            {
                ViewBag.Error = error;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoguearPaciente(Usuario model, string returnUrl)
        {
            string respuesta = "";
            if (model.Nick == null || model.Password == null)
            {
                respuesta = "Verifique los datos de ingreso";
            }
            else
            {
                respuesta = EstablecerLoginPaciente(model);
            }
            if (respuesta == "")
            {
                return RedirectToAction("Perfil", "Pacientes");
            }
            else
                return RedirectToAction("Paciente", "Seguridad", new { error = "" });
        }
        public string EstablecerLoginPaciente(Usuario Inicio)
        {
            string error = "";
            //string password = MD5Hash(Inicio.Password);
            string password = Inicio.Password;
            Usuario Usuario = null;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var usuario = conexion.SeguridadPaciente(Inicio.Nick, password,"");

                if (usuario == null || usuario.Id == 0)
                    error = "No se ha encontrado dentro del sistema, contacte al administrador del sistema";
                else
                {
                    this.SharedSession["UsuarioPaciente"] = usuario;
                }
            }
            return error;
        }
        public ActionResult Medico(string error)
        {
            this.SharedSession["UsuarioMedico"] = null;
            if (error != "")
            {
                ViewBag.Error = error;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoguearMedico(Usuario model, string returnUrl)
        {
            string respuesta = "";
            if (model.Nick == null || model.Password == null)
            {
                respuesta = "Verifique los datos de ingreso";
            }
            else
            {
                respuesta = EstablecerLoginMedico(model);
            }
            if (respuesta == "")
            {
                return RedirectToAction("Perfil", "Medicos");
            }
            else
                return RedirectToAction("Medico", "Seguridad", new { error = "" });
        }
        public string EstablecerLoginMedico(Usuario Inicio)
        {
            string error = "";
            //string password = MD5Hash(Inicio.Password);
            string password = Inicio.Password;
            Usuario Usuario = null;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var usuario = conexion.SeguridadMedico(Inicio.Nick, password, "");

                if (usuario == null || usuario.Id == 0)
                    error = "No se ha encontrado dentro del sistema, contacte al administrador del sistema";
                else
                {
                    this.SharedSession["UsuarioMedico"] = usuario;
                }
            }
            return error;
        }
        public ActionResult Organizacion(string error)
        {
            this.SharedSession["UsuarioOrganizacion"] = null;
            if (error != "")
            {
                ViewBag.Error = error;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoguearOrganizacion(Usuario model, string returnUrl)
        {
            string respuesta = "";
            if (model.Nick == null || model.Password == null)
            {
                respuesta = "Verifique los datos de ingreso";
            }
            else
            {
                respuesta = EstablecerLoginOrganizacion(model);
            }
            if (respuesta == "")
            {
                return RedirectToAction("Perfil", "Organizacion");
            }
            else
                return RedirectToAction("Organizacion", "Seguridad", new { error = "" });
        }
        public string EstablecerLoginOrganizacion(Usuario Inicio)
        {
            string error = "";
            //string password = MD5Hash(Inicio.Password);
            string password = Inicio.Password;
            Usuario Usuario = null;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var usuario = conexion.SeguridadOrganizacion(Inicio.Nick, password, "");

                if (usuario == null || usuario.Id == 0)
                    error = "No se ha encontrado dentro del sistema, contacte al administrador del sistema";
                else
                {
                    this.SharedSession["UsuarioOrganizacion"] = usuario;
                }
            }
            return error;
        }
        public JsonResult Chekin(Usuario Inicio)
        {
            string error = "";
            //string password = MD5Hash(Inicio.Password);
            string password = Inicio.Password;
            Usuario Usuario = null;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                //var ip = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).AddressList.First();
                //var host = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.ToString(); ;
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ip = context.Request.ServerVariables["remote_addr"].ToList().First().ToString();
                string host = context.Request.ServerVariables["remote_host"].ToList().First().ToString();
                var usuario = conexion.SeguridadUsuario(Inicio.Nick, MD5Hash(password), Inicio.Laboratorio);

                if (usuario == null || usuario.Id == 0)
                {                    
                    error = "No se ha encontrado dentro del sistema, o no tiene acceso al laboratorio solicitado, contacte al administrador del sistema";
                }
                else
                {
                    conexion.ChekinUsuario(usuario.Id, Inicio.Laboratorio, host, ip.ToString());
                }
            }
            return Json(new { error = error });
        }
    }
}