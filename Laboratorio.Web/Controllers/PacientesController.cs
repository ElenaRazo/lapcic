using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Laboratorio.Administracion;

namespace Laboratorio.Web.Controllers
{
    public class PacientesController : BaseController
    {
        // GET: Pacientes
        public ActionResult Index()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }

        public ActionResult Listado()
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Pacientes = movil.ConsultarUsuariosColeccion();
            }
            return View();
        }
        public ActionResult ListadoPacientes(string Nombre)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Pacientes = conexion.ConsultarPacientesNombre(Nombre);
            }
            return View("Listado");
        }
        public ActionResult ListadoPacientesBusqueda(string Nombre)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Pacientes = conexion.ConsultarPacientesBusqueda(Nombre);
            }
            return View("Listado");
        }
        public ActionResult Detalle(long Id) {

            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estado = new SelectList(movil.ConsultarEstadoPais(151), "Id", "Nombre");
                ViewBag.Organizacion = new SelectList(movil.ConsultarOrganizacionesColeccion(), "Id", "Nombre");
                ViewBag.Paciente = movil.ConsultarUsuario(Id);
            }
            return View();
        }
        public ActionResult DetalleSimple(long Id)
        {

            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estado = new SelectList(movil.ConsultarEstadoPais(151), "Id", "Nombre");
                ViewBag.Organizacion = new SelectList(movil.ConsultarOrganizacionesColeccion(), "Id", "Nombre");
                ViewBag.Paciente = movil.ConsultarUsuario(Id);
            }
            return View();
        }

        public ActionResult ListadoEstudios(int Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitudes = movil.ConsultarSolicitudPaciente(Id);
            }
            return View();
        }
        public ActionResult ListadoEstudiosGabinete(long Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Gabinete = movil.ConsultarEstudioGabinetePaciente(Id);
            }
            return View();
        }
        public ActionResult Expediente(int Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitudes = movil.ConsultarSolicitudPaciente(Id);
            }
            return View();
        }
        public ActionResult Nuevo()
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estado = new SelectList(conexion.ConsultarEstadoPais(151), "Id", "Nombre");
                ViewBag.Organizacion = new SelectList(conexion.ConsultarOrganizacionesColeccion(), "Id", "Nombre");
            }
            return View();
        }
        public JsonResult ActualizarPass(string Pass)
        {
            bool res = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioPaciente"];
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ActualizarPassPaciente(_Usuario.Id, Pass);

            }
            return Json(new { Resultado = res });
        }
        public JsonResult Crear(string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string NSS, string RFC, string Email, int Ciudad, int Colonia, int Profesion, int EstadoCivil, int Edad, DateTime Fecha, string Genero, int Organizacion)
        {
            bool res = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.CrearPaciente(Nombre, Paterno, Materno, Direccion, Telefono, Celular, NSS, RFC, Email, Ciudad, Colonia, Profesion, EstadoCivil, Edad, Fecha, Genero, Organizacion);

            }
            return Json(new { Resultado = res });
        }
        public JsonResult Eliminar(long Id)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarPaciente(Id, _Usuario.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult Actualizar(long Id, string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string NSS, string RFC, string Email, int Ciudad, int Colonia, int Profesion, int EstadoCivil, int Edad, DateTime Fecha, string Genero, int Organizacion)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            bool res = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ActualizarPaciente(Id, Nombre, Paterno, Materno, Direccion, Telefono, Celular, NSS, RFC, Email, Ciudad, Colonia, Profesion,EstadoCivil,Edad,Fecha,Genero,Organizacion);
            }
            return Json(new { Resultado = res });
        }
        public ActionResult Perfil()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioPaciente"];
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Paciente = _Usuario;
                    ViewBag.Estado = new SelectList(movil.ConsultarEstadoPais(151), "Id", "Nombre");
                }
                return View();
            }
            else {
                return RedirectToAction("Paciente", "Seguridad");
            }
        }
    }
}