using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Laboratorio.Web.Controllers
{
    public class MedicosController : BaseController
    {
        // GET: Medicos
        public ActionResult Index()
        { Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Medicos = conexion.ConsultarMedicosColeccion();
                }
                return View();
            }
            else {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult Nuevo()
        {
            return View();
        }
        public JsonResult ActualizarPass(string Pass)
        {
            bool res = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioMedico"];
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ActualizarPassMedico(_Usuario.Id, Pass);

            }
            return Json(new { Resultado = res });
        }
        public ActionResult Perfil()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioMedico"];
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Medico = _Usuario;
                    ViewBag.Estado = new SelectList(movil.ConsultarEstadoPais(151), "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Medico", "Seguridad");
            }
        }
        public ActionResult Expediente(int Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitudes = movil.ConsultarSolicitudMedico(Id);
            }
            return View();
        }
        public ActionResult ListadoEstudiosGabinete(long Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Gabinete = movil.ConsultarEstudioGabineteMedico(Id);
            }
            return View();
        }
        public JsonResult Crear(string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string Email, int Genero)
        {
            bool res = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.CrearMedico(Nombre, Paterno, Materno, Direccion, Telefono, Celular, Email, Genero);

            }
            return Json(new { Resultado = res });
        }
        public JsonResult Actualizar(long Id, string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string Email, int Genero)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            bool res = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ActualizarMedico(Id, Nombre, Paterno, Materno, Direccion, Telefono, Celular, Email, Genero);
            }
            return Json(new { Resultado = res });
        }
        public ActionResult Detalle(long Id)
        {

            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Medico = movil.ConsultarMedico(Id);
            }
            return View();
        }
        public ActionResult Listado()
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Medicos = conexion.ConsultarMedicosColeccion();
            }
            return View();
        }
        public JsonResult EliminarUsuario(int Usuario)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarUsuario(Usuario);
            }
            return Json(new { Resultado = resultado });
        }
    }
}