using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Laboratorio.Web.Controllers
{
    public class OrganizacionController : BaseController
    {
        public ActionResult Nuevo()
        {
            return View();
        }
        public JsonResult ActualizarPass(string Pass)
        {
            bool res = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioOrganizacion"];
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ActualizarPassOrganizacion(_Usuario.Id, Pass);

            }
            return Json(new { Resultado = res });
        }
        public ActionResult Perfil()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioOrganizacion"];
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Organizacion = _Usuario;
                    ViewBag.Estado = new SelectList(movil.ConsultarEstadoPais(151), "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Organizacion", "Seguridad");
            }
        }
        public ActionResult Expediente(int Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitudes = movil.ConsultarSolicitudOrganizacion(Id);
            }
            return View();
        }
        public ActionResult ListadoEstudiosGabinete(long Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Gabinete = movil.ConsultarEstudioGabineteOrganizacion(Id);
            }
            return View();
        }
        public JsonResult Actualizar(int Id, string Nombre, string Direccion, string Colonia, string Ciudad, string Telefono, string Telefono2, int ImprimirRecibo, int PagoDefault)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            bool res = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ActualizarEmpresa(Id, Nombre, Direccion, Colonia, Ciudad, Telefono, Telefono2, ImprimirRecibo, PagoDefault);
            }
            return Json(new { Resultado = res });
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