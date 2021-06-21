using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Laboratorio.Administracion;

namespace Laboratorio.Web.Controllers
{
    public class UsuariosController : BaseController
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Usuarios = movil.ConsultarUsuariosColeccion();
                    var puestos = movil.ConsultarPuestoColeccion();
                    List<Administracion.Puesto> _puestos = new List<Puesto>();
                    _puestos.Add(new Puesto() { Nombre = "SELECCIONE UNA OPCION", Id = 0 });
                    _puestos.AddRange(puestos);
                    ViewBag.Puestos = new SelectList(_puestos, "Id", "Nombre");
                    List<Administracion.Laboratorio> _Laboratorios = new List<Administracion.Laboratorio>();
                    var labs = movil.ConsultarLaboratorioColeccion();
                    Administracion.Laboratorio _lab = new Administracion.Laboratorio() {  Id=0, Nombre="SELECCIONE UNA OPCION" };
                    _Laboratorios.Add(_lab);
                    _Laboratorios.AddRange(labs);
                    ViewBag.Laboratorios = new SelectList(_Laboratorios, "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
           
        }
        public ActionResult Detalle(long Id)
        {

            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario.Puesto.Id < 4)
            {
                return View("Solicitudes", "Estudios");
            }
            else
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Puestos = new SelectList(movil.ConsultarPuestoColeccion(), "Id", "Nombre");
                    ViewBag.Laboratorios = new SelectList(movil.ConsultarLaboratorioColeccion(), "Id", "Nombre");
                }
                return View();
            }
        }
        public ActionResult Nuevo() {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario.Puesto.Id > 3)
            {
                return View("Solicitudes", "Estudios");
            }
            else
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Puestos = new SelectList(movil.ConsultarPuestoColeccion(), "Id", "Nombre");
                    ViewBag.Laboratorios = new SelectList(movil.ConsultarLaboratorioColeccion(), "Id", "Nombre");
                }
                return View();
            }
            
        }

        public JsonResult Crear(string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular,string Email, int Laboratorio, int Puesto, int Genero)
        {
            bool res = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.CrearUsuario(Nombre, Paterno, Materno, Direccion, Telefono, Celular, Email, Laboratorio, Puesto, Genero);

            }
            return Json(new { Resultado = res });
        }
        public JsonResult ActualizarPass(string Pass)
        {
            bool res = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ActualizarPassUsuario(_Usuario.Id, Pass);

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
        public JsonResult CambiarLaboratorio(int Laboratorio, int Usuario, int Rol)
        {
            long resultado = 0;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CambiarUsuario(Laboratorio,Usuario,Rol);
            }
            return Json(new { Resultado = resultado });
        }
    }
}