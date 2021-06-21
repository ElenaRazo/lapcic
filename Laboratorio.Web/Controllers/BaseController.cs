using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace Laboratorio.Web.Controllers
{
    public class BaseController : Controller
    {
        public System.Web.SessionState.HttpSessionState SharedSession
        {
            get
            {
                return System.Web.HttpContext.Current.Session;
            }
        }
        public ActionResult Footer() {
            return View();
        }
        public ActionResult Footer2()
        {
            return View();
        }
        public ActionResult Header(long Solicitud, string Clave)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                ViewBag.Solicitud = soli;
            }
            return View();
        }
        public ActionResult HeaderPaciente(long Solicitud, string Clave)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                ViewBag.Solicitud = soli;
            }
            return View();
        }
        public ActionResult HeaderPaciente2(long Solicitud, string Clave, long Id)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                if (soli.Paciente.Id == Id)
                {
                    ViewBag.Solicitud = soli;
                    return View("HeaderPaciente");
                }
                else {
                   return RedirectToAction("Error", "Estudios");
                }
            }
            
        }
        // GET: Base
        public string MD5Hash(string text)
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
        public ActionResult Contenido()
        {
            return View();
        }

    }
}