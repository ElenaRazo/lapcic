using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Laboratorio.Administracion.Reportes;
using System.Configuration;
using Newtonsoft.Json.Serialization;
using Rotativa;
using System.Net;
using System.Net.Mail;
using static Laboratorio.Web.Controllers.EstudiosController;
using Laboratorio.Administracion;

namespace Laboratorio.Web.Controllers
{
    public class CatalogosController : BaseController
    {
        // GET: Catalogos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Cumpleanos()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            return View();
        }
        public JsonResult GuardarCup(string Titulo, string adjuntos)
        {
            var ruta = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
            
                var adj = adjuntos.ToString().Trim(' ');
                var ext = adj.Trim().ToLower().Split('.');
                for (int a = 0; a < ext.Count(); a++)
                {
                    String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                    var archivo = ruta + @"\Temp\" + adjuntos;
                System.IO.File.Delete(ruta + @"\Imagenes\cumpleanos.png");
                System.IO.File.Copy(archivo, ruta + @"\Imagenes\cumpleanos.png");
                }
            return Json(new { Resultado = "" });

        }
        public ActionResult Promocion()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            return View();
        }
        public JsonResult EnviarPromocion(string Titulo, string adjuntos)
        {
            List<string> res = new List<string>();
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.ConsultarCorreos();
                var ruta = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
                var Adjuntos1 = adjuntos.ToString() != "" ? adjuntos.ToString().Trim(' ').Split(',').ToList() : null;
                if (adjuntos.Contains(","))
                {
                    if (res.Count > 0)
                    {
                        try
                        {
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = ConfigurationManager.ConnectionStrings["host"].ToString();
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential();
                            NetworkCred.UserName = ConfigurationManager.ConnectionStrings["mail"].ToString();
                            NetworkCred.Password = ConfigurationManager.ConnectionStrings["pass"].ToString();
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = int.Parse(ConfigurationManager.ConnectionStrings["port"].ToString());
                            var indice = 0;
                            var numero = 100;
                            var listas = (int.Parse((res.Count / numero).ToString()) + 1);
                            var correos = "";

                            for (var e = 0; e < res.Count; e++)
                            {
                                // for (var contador = indice; contador < (numero * e); contador++)
                                //{
                                //indice = indice + 1;
                                //if (correos == "")
                                //{
                                //    correos = res[contador].ToString();
                                //}
                                //else
                                //{
                                //    correos = correos + "; " + res[indice].ToString();
                                //}
                                //if (contador == (res.Count - 1) || contador == ((numero * e) - 1))
                                //{
                                try
                                {
                                    MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                                    MailAddress to = new MailAddress(res[e]);
                                    MailMessage mm = new MailMessage(from, to);
                                    Administracion.EstudioGabinete x = new Administracion.EstudioGabinete();
                                    try
                                    {
                                        for (int i = 0; i < Adjuntos1.Count(); i++)
                                        {
                                            var adj = Adjuntos1[i].ToString().Trim(' ');
                                            var ext = adj.Trim().ToLower().Split('.');
                                            for (int a = 0; a < ext.Count(); a++)
                                            {
                                                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                                                var archivo = ruta + @"\Temp\" + Adjuntos1[i];
                                                mm.Attachments.Add(new Attachment(archivo));
                                            }
                                        }
                                    }
                                    catch (Exception error) { };
                                    mm.Subject = Titulo;
                                    mm.Body = "";
                                    mm.IsBodyHtml = true;

                                    smtp.Send(mm);
                                    ViewBag.Mensaje = "El correo electrónico ha sido enviado";
                                    //    }
                                    //}
                                }
                                catch (Exception error) { };
                            }
                        }
                        catch (Exception error)
                        {
                            res = new List<string>();
                            res.Add("eror");
                        }
                    }
                    else
                    {
                        res = new List<string>();
                        ViewBag.Mensaje = "Ha ocurrido un error al enviar el correo";
                    }
                }
                else { 
                    if (res.Count > 0)
                    {
                        try
                        {
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = ConfigurationManager.ConnectionStrings["host"].ToString();
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential();
                            NetworkCred.UserName = ConfigurationManager.ConnectionStrings["mail"].ToString();
                            NetworkCred.Password = ConfigurationManager.ConnectionStrings["pass"].ToString();
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = int.Parse(ConfigurationManager.ConnectionStrings["port"].ToString());
                            var indice = 0;
                            var numero = 100;
                            var listas = (int.Parse((res.Count / numero).ToString()) + 1);
                            var correos = "";

                            for (var e = 0; e <= res.Count; e++)
                            {
                                //for (var contador = indice; contador < (numero * e); contador++)
                                //{
                                //    indice = indice + 1;
                                //    if (correos == "")
                                //    {
                                //        correos = res[contador].ToString();
                                //    }
                                //    else
                                //    {
                                //        correos = correos + "; " + res[indice].ToString();
                                //    }
                                //    if (contador == (res.Count - 1) || contador == ((numero * e) - 1))
                                //    {
                                try { 
                                        MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                                        MailAddress to = new MailAddress(res[e]);
                                        MailMessage mm = new MailMessage(from, to);
                                        Administracion.EstudioGabinete x = new Administracion.EstudioGabinete();
                                        try
                                        {
                                            var adj = adjuntos.Trim(' ');
                                            var ext = adj.Trim().ToLower().Split('.');
                                            for (int a = 0; a < ext.Count(); a++)
                                            {
                                                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                                                var archivo = ruta + @"\Temp\" + adjuntos;
                                                mm.Attachments.Add(new Attachment(archivo));
                                            }
                                        }
                                        catch (Exception error) { };
                                        mm.Subject = Titulo;
                                        mm.Body = "";
                                        mm.IsBodyHtml = true;
                                        smtp.Send(mm);
                                        ViewBag.Mensaje = "El correo electrónico ha sido enviado";
                                    //    }
                                    //}

                                }
                                catch (Exception error) { };
                            }
                        }
                        catch (Exception error)
                        {
                            res = new List<string>();
                            res.Add("eror");
                        }
                    }
                    else
                    {
                        res = new List<string>();
                        ViewBag.Mensaje = "Ha ocurrido un error al enviar el correo";
                    }
                }
            }
            return Json(new { Resultado = res });
        }
        public void EnvioCorreoPromocion(string receiver, long Detalle, long Id)
        {
            try
            {
                if (receiver != "")
                {
                    var pdf = new ActionAsPdf("ImpresionInterpretacion", new { name = "Giorgio", Detalle = Detalle, Id = Id }) { FileName = "Interpretacion.pdf", PageSize = Rotativa.Options.Size.Letter, PageMargins = new Rotativa.Options.Margins(50, 25, 60, 25) };
                    Byte[] PdfData = pdf.BuildFile(ControllerContext);
                    MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                    MailAddress to = new MailAddress(receiver);
                    MailMessage mm = new MailMessage(from, to);
                    Administracion.EstudioGabinete x = new Administracion.EstudioGabinete();
                    try
                    {
                        using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                        {
                            x = conexion.ConsultarEstudioGabineteId((int)Id);
                            foreach (var i in x.Adjunto)
                            {
                                var webClient = new WebClient();
                                byte[] imageBytes = webClient.DownloadData(@"https://servicioslapcic.com.mx/Directorio/" + i.Trim());
                                mm.Attachments.Add(new Attachment(new MemoryStream(imageBytes), i.Split('/').Last()));
                            }
                        }
                    }
                    catch (Exception error) { };
                    mm.Subject = "Interpretación de estudios de gabinete";
                    mm.Body = "El resultado de los análisis se encuentra disponible desde el portal web, se adjunta una copia del mismo.";
                    mm.Attachments.Add(new Attachment(new MemoryStream(PdfData), "Resultados.pdf"));
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.ConnectionStrings["host"].ToString();
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.ConnectionStrings["mail"].ToString();
                    NetworkCred.Password = ConfigurationManager.ConnectionStrings["pass"].ToString();
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.ConnectionStrings["port"].ToString());
                    smtp.Send(mm);
                    ViewBag.Mensaje = "El correo electrónico ha sido enviado";
                }
                else
                {
                    ViewBag.Mensaje = "Ha ocurrido un error al enviar el correo";
                }

            }
            catch (Exception error)
            {
                ViewBag.Mensaje = "Ha ocurrido un error al enviar el correo";
            }
        }
        public ActionResult Estudios()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    //ViewBag.Estudios = movil.ConsultarEstudioColeccion();
                }
                return View();
            }
            else {
                return RedirectToAction("Index", "Seguridad");
            }
                
        }
        public ActionResult EstudiosGabinete()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    //ViewBag.Estudios = movil.ConsultarEstudioColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }

        }
        public ActionResult TipoDeposito()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Tipos = movil.ConsultarTipoDepositoColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
            
        }
        public ActionResult TipoCaptura()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Tipos = movil.ConsultarTipoCapturaColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }

        }
        public ActionResult TipoMuestra()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Tipos = movil.ConsultarTipoMuestraColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }

        public ActionResult Departamento()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Tipos = movil.ConsultarDepartamentoColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }

        }
        public ActionResult TipoPago()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Tipos = movil.ConsultarTipoPagoColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }

        }
        public ActionResult Perfiles()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    //ViewBag.Estudios = movil.ConsultarPerfilesColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult Insumos()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                   // ViewBag.Insumos = movil.ConsultarInsumosLaboratorio(_Laboratorio.Id);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }           
        }
        public ActionResult ListadoInsumos()
        {
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Insumos = movil.ConsultarInsumosLaboratorio(_Laboratorio.Id);
                }
                return View();
        }
        public ActionResult InsumosBusqueda(DateTime Inicio, DateTime Fin)
        {
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Insumos = movil.ConsultarInsumosBusquedaLaboratorio(Inicio,Fin, _Laboratorio.Id);
            }
            return View("ListadoInsumos");
        }
        public ActionResult Empresas()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Empresas = movil.ConsultarOrganizacionesColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
            
        }
        public ActionResult NuevoDepartamento() {
            return View();
        }
        public ActionResult NuevoTipoPago()
        {
            return View();
        }

        public ActionResult NuevoProveedor()
        {
            return View();
        }
        public ActionResult NuevoInsumo()
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Proveedor = new SelectList(conexion.ConsultarProveedoresColeccion(), "Id", "Nombre");
                ViewBag.TipoDeposito = new SelectList(conexion.ConsultarTipoDepositoColeccion(), "Id", "Nombre");
            }
            return View();
        }
        public ActionResult EnvioInsumo()
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.ProveedorEnvio = new SelectList(conexion.ConsultarProveedoresColeccion(), "Id", "Nombre");
                ViewBag.TipoDepositoEnvio = new SelectList(conexion.ConsultarTipoDepositoColeccion(), "Id", "Nombre");
                Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
                var _Labs = conexion.ConsultarLaboratorioColeccion();
                ViewBag.Laboratorios = new SelectList(_Labs.Where(x=> x.Id != _Laboratorio.Id).ToList(), "Id", "Nombre");
            }
            return View();
        }
        public ActionResult NuevoTipoDeposito()
        {
            return View();
        }
        public ActionResult NuevoTipoMuestra()
        {
            return View();
        }
        public ActionResult NuevoLaboratorio()
        {
            return View();
        }
        public ActionResult NuevaEmpresa()
        {
            return View();
        }
        public JsonResult CrearDepartamento(string Nombre)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearDepartamento(Nombre);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearTipoPago(string Nombre, string Clave)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearTipoPago(Nombre, Clave);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearProveedor(string Nombre)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearProveedor(Nombre);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearTipoMuestra(string Nombre, string Indicaciones)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearMuestra(Nombre, Indicaciones);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearTipoDeposito(string Nombre,int Existencia, string Indicaciones)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearDeposito(Nombre, Existencia, Indicaciones);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearInsumo(int Cantidad, int Proveedor, int TipoDeposito, int TotalCompra, int TotalPiezas, string Tipo, DateTime Fecha)
        {
            bool resultado = false;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearInsumo(Cantidad, Proveedor, TipoDeposito, TotalCompra, TotalPiezas, Tipo,Fecha, _Laboratorio.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EnviarInsumo(int Cantidad, int Proveedor, int TipoDeposito, double TotalCompra, int TotalPiezas, string Tipo, DateTime Fecha, int Laboratorio)
        {
            bool resultado = false;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EnviarInsumo(Cantidad, Proveedor, TipoDeposito, TotalPiezas, Tipo, Fecha, _Laboratorio.Id, Laboratorio);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearLaboratorio(string Nombre, string Direccion, string Telefono, string Iniciales, string Responsable)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearLaboratorio(Nombre, Direccion, Telefono, Iniciales, Responsable);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearEmpresa(string Nombre, string Direccion,string Colonia, string Ciudad, string Telefono, string Telefono2, int ImprimirRecibo, int PagoDefault)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearEmpresa(Nombre, Direccion, Colonia, Ciudad, Telefono, Telefono2, ImprimirRecibo, PagoDefault);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EliminarEmpresa(int Empresa)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarEmpresa(Empresa);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EliminarMedico(int Medico)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarMedico(Medico);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult ActualizarDepartamento(int Id, string Nombre)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearDepartamento(Nombre);
            }
            return Json(new { Resultado = resultado });
        }
        public ActionResult Medicos()
        {
            return View();
        }
        public ActionResult Laboratorios()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Laboratorios = movil.ConsultarLaboratorioColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
            
        }

        public ActionResult Proveedores()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Proveedores = movil.ConsultarProveedoresColeccion();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }

        }

        public ActionResult DetalleLaboratorio(int Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Laboratorio = movil.ConsultarLaboratorioColeccion().First();
            }
            return View();
        }
        public ActionResult GetMunicipios()
        {
            using (var coneccion = new  Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var listado = coneccion.ConsultarMunicipioEstado(11);
                return Json(listado.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetMunicipiosPorEstado(int Id)
        {
            using (var conexion = new  Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var listado = conexion.ConsultarMunicipioEstado(Id);
                return Json(listado.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetColoniasPorEstado(int Id)
        {
            using (var conexion = new  Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var listado = conexion.ConsultarColoniaEstadoColeccion(Id);
                var colonias = new List<Colonia>();
                colonias.Add(new Colonia { Id = 0, Nombre = "OTRA" });
                colonias.AddRange(listado);
                return Json(colonias.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetColoniasPorMunicipio(int Id)
        {
            using (var conexion = new  Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var listado = conexion.ConsultarColoniaMunicipioColeccion(Id);
                var colonias = new List<Colonia>();
                colonias.Add(new Colonia { Id = 0, Nombre = "OTRA" });
                colonias.AddRange(listado);
                return Json(colonias.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Accesos()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Accesos = movil.ConsultarAccesos();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }

    }
}