using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.IO;
using Laboratorio.Administracion.Reportes;
using System.Configuration;
using Newtonsoft.Json.Serialization;
using Rotativa;
using System.Net;
using System.Net.Mail;
//using bpac;

namespace Laboratorio.Web.Controllers
{
    public class EstudiosController : BaseController
    {
        // GET: Estudios
        public ActionResult ReporteSolicitudes()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    var listado = new List<Administracion.Laboratorio>();
                    listado.Add(new Administracion.Laboratorio() { Id = 0, Nombre = "Todos" });
                    listado.AddRange(conexion.ConsultarLaboratorioColeccion());
                    ViewBag.Laboratorios = new SelectList(listado, "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult ReporteEstudios()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    var listado = new List<Administracion.Estudio>();
                    listado.AddRange(conexion.ConsultarEstudios());
                    ViewBag.Estudios = new SelectList(listado, "ClaveEstudio", "Nombre");
                    var listado1 = new List<Administracion.Estudio>();
                    listado1.AddRange(conexion.ConsultarPerfilesColeccion());
                    ViewBag.Perfiles = new SelectList(listado1, "ClaveEstudio", "Nombre");
                    var listado2 = new List<Administracion.Laboratorio>();
                    listado2.Add(new Administracion.Laboratorio() { Id = 0, Nombre = "Todos" });
                    listado2.AddRange(conexion.ConsultarLaboratorioColeccion());
                    ViewBag.Laboratorios = new SelectList(listado2, "Id", "Nombre");
                    ViewBag.Estado = new SelectList(conexion.ConsultarEstadoPais(151), "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Se agregan las columnas haciendo un ciclo para cada fila

            // El encabezado de la primera columna es el mismo. 
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // El encabezado para las demas columnas
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[1].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Se agregan las columnas por cada renglón        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            
            }

            return outputTable;
        }
        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
        public ActionResult ReporteEstadistica()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    var listado = new List<Administracion.Laboratorio>();
                    listado.Add(new Administracion.Laboratorio() { Id = 0, Nombre = "Todos" });
                    listado.AddRange(conexion.ConsultarLaboratorioColeccion());
                    ViewBag.Laboratorios = new SelectList(listado, "Id", "Nombre");
                    ViewBag.Estado = new SelectList(conexion.ConsultarEstadoPais(151), "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult ReportePagos()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Laboratorios = new SelectList(conexion.ConsultarLaboratorioColeccion(), "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult ReporteDepositos()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    List<Laboratorio.Administracion.Laboratorio> _Laboratorios = new List<Administracion.Laboratorio>();
                    _Laboratorios.Add(new Administracion.Laboratorio() { Id = 0, Nombre = "Todos" });
                    _Laboratorios.AddRange(conexion.ConsultarLaboratorioColeccion());
                    ViewBag.Laboratorios = new SelectList(_Laboratorios, "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult ResultadoBusqueda(int Laboratorio, int Mes, int Anio)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Resultados = conexion.ConsultarSolicitudPeriodo(Laboratorio, Mes, Anio);
            }
            return View();
        }
        public ActionResult ResultadoEstadistica(int Laboratorio, int Mes, int Anio, int Ciudad, int Colonia, int Estado)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Resultados = conexion.ConsultarSolicitudEstadistica(Laboratorio, Mes, Anio, Colonia,Ciudad,Estado);
            }
            return View();
        }
        public string ResultadoEstudios(string Estudio, int Laboratorio, int Mes, int Anio, int Ciudad, int Colonia, int Estado)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var datos = conexion.ConsultarSolicitudEstudios(Estudio, Laboratorio, Mes, Anio, Colonia, Ciudad, Estado);
              //  datos = GenerateTransposedTable(datos);
                return ConvertDataTableToHTML(datos);
            }
            
        }
        public ActionResult PagosBusqueda(int Laboratorio, int Mes, int Anio)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Resultados = conexion.ConsultarPagosPeriodo(Laboratorio, Mes, Anio);
            }
            return View();
        }
        public ActionResult DepositosBusqueda(int Laboratorio, int Mes, int Anio)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Resultados = conexion.ConsultarDepositosPeriodo(Laboratorio, Mes, Anio);
            }
            return View();
        }
        public ActionResult Detalle(int Id)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Estudio = conexion.ConsultarEstudio(Id);
                    ViewBag.Departamentos = new SelectList(conexion.ConsultarDepartamentoColeccion(), "Id", "Nombre");
                    ViewBag.Depositos = new SelectList(conexion.ConsultarTipoDepositoColeccion(), "Id", "Nombre");
                    ViewBag.Muestras = new SelectList(conexion.ConsultarTipoMuestraColeccion(), "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult DetallePerfil(int Id)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Departamentos = new SelectList(conexion.ConsultarDepartamentoColeccion(), "Id", "Nombre");
                    ViewBag.Perfil = conexion.ConsultarEstudio(Id);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult DetalleComponente(int Id)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Componente = conexion.ConsultarComponente(Id);
                ViewBag.Elementos = conexion.ConsultarElementosComponente(Id);
                ViewBag.TipoCaptura = new SelectList(conexion.ConsultarTipoCapturaColeccion(), "Id", "Descripcion");
            }
            return View();
        }
        public ActionResult Resultado(string Estudio, int Edad, string Genero, bool Perfil, string Clave, string Nombre)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Edad = Edad;
                ViewBag.ClaveSolicitud = Clave;
                ViewBag.ClaveEstudio = Estudio;
                ViewBag.Perfil = Perfil;
                ViewBag.Nombre = Nombre;
                ViewBag.Genero = Genero;
                if (Perfil)
                    ViewBag.Componentes = conexion.ConsultarComponentesEstudioResultadoPerfil(Estudio, Clave);
                else
                    ViewBag.Componentes = conexion.ConsultarComponentesEstudioResultado(Estudio,Clave);
                var res = conexion.ConsultarResutladoSolicitud(Clave);
                ViewBag.Crear = res.Count == 0 ? true : false;
            }
            return View();
        }
        public ActionResult ResultadoImpresion(string Estudio, int Edad, string Genero, bool Perfil, string Clave, string Nombre)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Edad = Edad;
                ViewBag.ClaveSolicitud = Clave;
                ViewBag.ClaveEstudio = Estudio;
                ViewBag.Perfil = Perfil;
                ViewBag.Nombre = Nombre;
                ViewBag.Genero = Genero;
                if (Perfil)
                    ViewBag.Componentes = conexion.ConsultarComponentesEstudioResultadoPerfil(Estudio, Clave);
                else
                    ViewBag.Componentes = conexion.ConsultarComponentesEstudioResultado(Estudio, Clave);
                // ViewBag.Resultados = conexion.ConsultarResutladoSolicitud(Clave);
            }
            return View();
        }
        public ActionResult HojaTrabajo(long Solicitud)
        {
            Administracion.Solicitud solicitud = new Administracion.Solicitud();           
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                solicitud = conexion.ConsultarSolicitud(Solicitud);
            }
            string footer = "--footer-center \"" + "\n\n\n\n\"" + solicitud.ClaveSolicitud + "\nEl laboratorio Lapcic, dará informe sobre las prácticas de análisis a realizar conforme a lo que indica el médico o en \n su caso a solicitud del mismo paciente, para realizar los estudios de su interés, informándole al paciente sobre los \n procedimientos que se le van a practicar de la toma de muestra de los diferentes estudios que el interesado solicita. \n (Norma 007 - SSA3 - 2011)\n Firma el Paciente 1er Testigo 2do Testigo \n Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("ImpresionHoja", new { name = "Giorgio", Solicitud =Solicitud }) { FileName = "Test.pdf", PageSize = Rotativa.Options.Size.Letter, PageMargins = new Rotativa.Options.Margins(20, 20, 60, 20), CustomSwitches = footer };
            
        }
        public ActionResult ImpresionHoja(long Solicitud)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                ViewBag.Solicitud = soli;
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud3(soli.ClaveSolicitud);
            }
            return View();
        }
        public ActionResult Impresion(long Solicitud, string Clave, int Firma)
        {
            
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                ViewBag.Solicitud = soli;
                ViewBag.Firma = Firma;
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud(soli.ClaveSolicitud);
                ViewBag.Resultado = conexion.ConsultarResutladoSolicitud(Clave);
            }
            return View();
        }
        public ActionResult Impresion2(long Solicitud, string Clave, int Firma, int Id)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                if (Id == soli.Paciente.Id)
                {
                    ViewBag.Solicitud = soli;
                    ViewBag.Firma = Firma;
                    ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud(soli.ClaveSolicitud);
                    ViewBag.Resultado = conexion.ConsultarResutladoSolicitud(Clave);
                    return View();
                }
                else {
                    return View("Error");
                }
                
            }
            
        }
        public ActionResult Error() {
            return View();
        }
        public ActionResult ImpresionIndividual(long Solicitud, string Clave, int Firma, string estudios)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                ViewBag.Solicitud = soli;
                ViewBag.Firma = Firma;
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud(soli.ClaveSolicitud);
                var res = conexion.ConsultarResutladoSolicitud(Clave);
                Administracion.ResultadoSolicitud titulo = null;
                List<Administracion.ResultadoSolicitud> _resultado = new List<Administracion.ResultadoSolicitud>();
                var est = estudios.Split(',');
                foreach (var i in est) {
                    var desp = i.Split('-');
                    foreach (var e in res)
                    {
                        if (desp.Count() == 1)
                        {
                            if (e.ClaveEstudio == desp.First())
                            {
                                _resultado.Add(e);
                            }
                        }
                        else {
                            if (e.ClaveEstudioMain == e.ClaveEstudio && e.ClaveEstudioMain == desp.First() && e.EstudioComponente == int.Parse(desp.Last()))
                            {
                                _resultado.Add(e);
                            }
                            else if (e.ClaveEstudioMain != e.ClaveEstudio && e.ClaveEstudio == desp.First() && e.EstudioComponente == int.Parse(desp.Last()))
                            {
                                _resultado.Add(e);
                            }
                        }
                    }
                }
               
                ViewBag.Resultado = _resultado;
            }

            return View("Impresion");
        }

        public string Individual(long Solicitud, string Clave, int Firma, string estudios) {
            string _headerUrl = Url.Action("Header", "Base", new { Solicitud = Solicitud, Clave = Clave }, "https");
            if (Firma == 1)
            {
                string _footerUrl = Url.Action("Footer", "Base", null, "https");

                //string footer = "--footer-center \"" + "\n\n\"" + "\nQFB. Yazmin Sanchez Castillo \n RPF 9048926 SSG2996 \n (Norma 007 - SSA3 - 2011)\n Universidad de Guanajuato" + "--footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                //string footer = "--footer-center \"" + "\n\n\n\n\"" + "\nQFB. Yazmin Sanchez Castillo \n RPF 9048926 SSG2996 \n Universidad de Guanajuato \n Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                var elementos = new ActionAsPdf("ImpresionIndividual", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = Firma, estudios = estudios }) 
                {
                    FileName = "Resultados_" + Clave + ".pdf",
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(90, 15, 50, 15),
                    CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 13 " + "--footer-html " + _footerUrl + " --footer-spacing 0"
                };
                byte[] applicationPDFData = elementos.BuildFile(ControllerContext);
                var fileStream = new FileStream(ConfigurationManager.ConnectionStrings["Directorio"].ToString() + @"\Temp\" + Clave + ".pdf", FileMode.Create, FileAccess.Write);
                fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                fileStream.Close();
                return "Directorio/Temp/" + Clave + ".pdf";
            }
            else
            {
                var elementos = new ActionAsPdf("ImpresionIndividual", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = Firma, estudios = estudios })
                {
                    FileName = "Resultados_" + Clave + ".pdf",
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(90, 15, 35, 15),
                    CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 13 "
                };
                byte[] applicationPDFData = elementos.BuildFile(ControllerContext);
                var fileStream = new FileStream(ConfigurationManager.ConnectionStrings["Directorio"].ToString() + @"\Temp\" + Clave + ".pdf", FileMode.Create, FileAccess.Write);
                fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                fileStream.Close();
                return "Directorio/Temp/" + Clave + ".pdf";
            }
           
        }
        public string EnviarIndividual(long Solicitud, string Clave, string Correo, string estudios)
        {
            string _headerUrl = Url.Action("Header", "Base", new { Solicitud = Solicitud, Clave = Clave }, "https");
            var elementos = new ActionAsPdf("ImpresionIndividual", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = 0, estudios = estudios })
            {
                FileName = "Resultados_" + Clave + ".pdf",
                PageSize = Rotativa.Options.Size.Letter,
                PageMargins = new Rotativa.Options.Margins(90, 15, 35, 15),
                CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 13 "
            };
            byte[] applicationPDFData = elementos.BuildFile(ControllerContext);
            var fileStream = new FileStream(ConfigurationManager.ConnectionStrings["Directorio"].ToString() + @"\Temp\" + Clave + ".pdf", FileMode.Create, FileAccess.Write);
            fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
            fileStream.Close();
            EnvioCorreoArchivo(Correo, Solicitud, "Directorio/Temp/" + Clave + ".pdf", Clave, estudios);
            return "Directorio/Temp/" + Clave + ".pdf";
        }
        public ActionResult ImpresionInterpretacion(long Detalle, int Id)
        {

            try
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    var res = conexion.ConsultarEstudioGabineteId(Id);
                    ViewBag.Estudio = res;
                    ViewBag.Detalle = res.Estudios.Where(x => x.IdDetalle == Detalle).First();
                }
                return View();
            }
            catch (Exception er) {
                return View("Error");
            }
        }
        public ActionResult ImpresionPaciente(long Solicitud, string Clave)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var soli = conexion.ConsultarSolicitud(Solicitud);
                ViewBag.Solicitud = soli;
                ViewBag.Resultado = conexion.ConsultarResutladoSolicitud(Clave);
            }
            return View();
        }
          public ActionResult Interpretacion(long Detalle, long Id)
        {

            return new ActionAsPdf("ImpresionInterpretacion", new { name = "Giorgio", Detalle = Detalle, Id = Id }) { FileName = "Test.pdf", PageSize = Rotativa.Options.Size.Letter, PageMargins = new Rotativa.Options.Margins(10,25,10,25) };
        }
        public ActionResult DetalleS(long Solicitud, string Clave, bool Pagado, int EstatusSolicitud)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Pagado = Pagado;
                ViewBag.EstatusSolicitud = EstatusSolicitud;
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud(Clave);
                if(Pagado)
                    ViewBag.Resultado = conexion.ConsultarResutladoSolicitud(Clave);
            }
            return View();
        }
        public ActionResult ResultadosSolicitud(string Clave)
        {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Resultados = conexion.ConsultarResutladoSolicitud(Clave);
            }
            return View();
        }
        [Authorize]
        public ActionResult ResultadoPaciente(long Solicitud, string Clave)
        {

            return new ActionAsPdf("Impresion", new { name = "Giorgio", Solicitud = Solicitud, Clave=Clave }) { FileName = "Test.pdf", PageSize = Rotativa.Options.Size.Letter, PageMargins = new Rotativa.Options.Margins(80,25,60,25) };
        }
        public ActionResult Impresion1()
        {
            var report = new ViewAsPdf("Impresion")
            {
                PageMargins = { Left = 20, Bottom = 60, Right = 20, Top = 80 },
            };
            return report;
        }
        public ActionResult Resultados(long Solicitud, string Clave, int fs)
        {
            string _headerUrl = Url.Action("Header", "Base", new { Solicitud = Solicitud, Clave = Clave }, "https");
            if (fs == 0)
            {
                string _footerUrl = Url.Action("Footer2", "Base", null, "https");
                return new ActionAsPdf("Impresion", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = fs })
                {
                    FileName = "Resultados_" + Clave + ".pdf",
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(82, 13, 35, 13),
                    CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 6 " + "--footer-html " + _footerUrl + " --footer-spacing 1"
                };
                
            }
            else {
                string _footerUrl = Url.Action("Footer", "Base", null, "http");
                return new ActionAsPdf("Impresion", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = fs })
                {
                    FileName = "Resultados_" + Clave + ".pdf",
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(82, 13, 40, 13),
                    CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 6 " + "--footer-html " + _footerUrl + " --footer-spacing 0"
                };
            }
        }
        public ActionResult VerResultado(long Solicitud, string Clave, int fs)
        {
            return new ViewAsPdf("Impresion", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = fs });
        }
        public ActionResult ResultadosPaciente1(long Solicitud, string Clave)
        {
            string _headerUrl = Url.Action("HeaderPaciente", "Base", new { Solicitud = Solicitud, Clave = Clave }, "https");                    
            string _footerUrl = Url.Action("Footer", "Base", null, "http");
            return new ActionAsPdf("Impresion", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = 1 })
            {
                FileName = "Resultados_" + Clave + ".pdf",
                PageSize = Rotativa.Options.Size.Letter,
                PageMargins = new Rotativa.Options.Margins(70, 13, 40, 13),
                CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 6 " + "--footer-html " + _footerUrl + " --footer-spacing 0"
            };
        }
        public ActionResult ResultadosPacientePortal(long Solicitud, string Clave, long Id)
        {
            string _headerUrl = Url.Action("HeaderPaciente2", "Base", new { Solicitud = Solicitud, Clave = Clave, Id = Id }, "https");
            string _footerUrl = Url.Action("Footer", "Base", null, "http");
            return new ActionAsPdf("Impresion2", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = 1, Id = Id })
            {
                FileName = "Resultados_" + Clave + ".pdf",
                PageSize = Rotativa.Options.Size.Letter,
                PageMargins = new Rotativa.Options.Margins(70, 13, 40, 13),
                CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 6 " + "--footer-html " + _footerUrl + " --footer-spacing 0"
            };
        }
        [Authorize]
        public ActionResult PacienteResultado(long Solicitud, string Clave)
        {

            return new ActionAsPdf("ImpresionPaciente", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave }) { FileName = Clave + ".pdf" };
        }
        public ActionResult Footer()
        {

            return View();
        }
        public ActionResult FooterPaciente()
        {

            return View();
        }
        public ActionResult Envio(string receiver, long Solicitud, string Clave)
        {
            try
            {
                if (receiver != "")
                {
                    string _footerUrl = Url.Action("FooterPaciente", "Base", null, "https");

                    //string footer = "--footer-center \"" + "\n\n\"" + "\nQFB. Yazmin Sanchez Castillo \n RPF 9048926 SSG2996 \n (Norma 007 - SSA3 - 2011)\n Universidad de Guanajuato" + "--footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                    //string footer = "--footer-center \"" + "\n\n\n\n\"" + "\nQFB. Yazmin Sanchez Castillo \n RPF 9048926 SSG2996 \n Universidad de Guanajuato \n Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                    var elementos = new ActionAsPdf("ImpresionPaciente", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave })
                    {
                        FileName = "Resultados_" + Clave + ".pdf",
                        PageSize = Rotativa.Options.Size.Letter,
                        PageMargins = new Rotativa.Options.Margins(90, 15, 50, 15),
                        CustomSwitches =  _footerUrl + " --footer-spacing 0"
                    };
                    Byte[] PdfData = elementos.BuildFile(ControllerContext);
                    MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                    MailAddress to = new MailAddress(receiver);
                    MailMessage mm = new MailMessage(from, to);
                    mm.Subject = "Resultados de la solicitud " + Clave;
                    mm.Body = "El resultado de los análisis se encuentra disponible desde el portal web, se adjunta una copia del mismo.";
                    mm.Attachments.Add(new Attachment(new MemoryStream(PdfData), "Resultados_"+ Clave +".pdf"));
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
                else {
                    ViewBag.Mensaje = "Ha ocurrido un error al enviar el correo";
                }
                return View();
                
            }
            catch (Exception error)
            {
                ViewBag.Mensaje = "Ha ocurrido un error al enviar el correo";
                return View();
            }
        }
        public void EnvioCorreo(string receiver, long Solicitud, string Clave)
        {
            try
            {
                if (receiver != "")
                {
                    //string cusomtSwitches = string.Format("--print-media-type --allow {0} --footer-html {0} --footer-spacing -10",
                    //   Url.Action("Footer", "Estudios", new { area = "" }, "https"));

                    string _headerUrl = Url.Action("HeaderPaciente", "Base", new { Solicitud = Solicitud, Clave = Clave }, "https");                    
                    string _footerUrl = Url.Action("Footer", "Base", null, "http");
                    var pdf =  new ActionAsPdf("Impresion", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = 1 })
                    {
                        FileName = "Resultados_" + Clave + ".pdf",
                        PageSize = Rotativa.Options.Size.Letter,
                        PageMargins = new Rotativa.Options.Margins(70, 13, 40, 13),
                        CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 6 " + "--footer-html " + _footerUrl + " --footer-spacing 0"
                    };
                    Byte[] PdfData = pdf.BuildFile(ControllerContext);
                    MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                    MailAddress to = new MailAddress(receiver);
                    MailMessage mm = new MailMessage(from, to);
                    mm.Subject = "Resultados de la solicitud " + Clave;
                    mm.Body = "El resultado de los análisis se encuentra disponible desde el portal web, se adjunta una copia del mismo.";
                    mm.Attachments.Add(new Attachment(new MemoryStream(PdfData), "Resultados_"+Clave+".pdf"));
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
        public void EnvioCorreoArchivo(string receiver, long Solicitud, string archivo, string Clave, string estudios)
        {
            try
            {
                string _headerUrl = Url.Action("Header", "Base", new { Solicitud = Solicitud, Clave = Clave }, "https");

                if (receiver != "")
                {
                    var elementos = new ActionAsPdf("ImpresionIndividual", new { name = "Giorgio", Solicitud = Solicitud, Clave = Clave, Firma = 0, estudios = estudios })
                    {
                        FileName = "Resultados_" + Clave + ".pdf",
                        PageSize = Rotativa.Options.Size.Letter,
                        PageMargins = new Rotativa.Options.Margins(90, 15, 35, 15),
                        CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 13 "
                    };
                    byte[] applicationPDFData = elementos.BuildFile(ControllerContext);

                    MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                    MailAddress to = new MailAddress(receiver);
                    MailMessage mm = new MailMessage(from, to);
                    mm.Subject = "Resultados parciales de la solicitud";
                    mm.Body = "El resultado parcial de los análisis se encuentra disponible desde el portal web, se adjunta una copia del mismo.";
                    String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                    var archivo1 = path + archivo;
                    mm.Attachments.Add(new Attachment(new MemoryStream(applicationPDFData), "Resultados_" + Clave + ".pdf"));
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
        public void EnvioCorreoInterpretacion(string receiver, long Detalle, long Id)
        {
            try
            {
                if (receiver != "")
                {
                    var pdf = new ActionAsPdf("ImpresionInterpretacion", new { name = "Giorgio", Detalle = Detalle, Id = Id }) { FileName = "Interpretacion.pdf", PageSize = Rotativa.Options.Size.Letter, PageMargins = new Rotativa.Options.Margins(10, 25, 10, 25) };
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
        public ActionResult EnvioCotizacion(string receiver, string Clave)
        {
            try
            {
                if (receiver != "")
                {
                    var pdf = new ActionAsPdf("Cotizacion", new { name = "Giorgio", ClaveCotizacion = Clave }) { FileName = "Test.pdf" };

                    Byte[] PdfData = pdf.BuildFile(ControllerContext);
                    MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                    MailAddress to = new MailAddress(receiver);
                    MailMessage mm = new MailMessage(from, to);
                    mm.Subject = "Cotización de estudios " + Clave;
                    mm.Body = "Ha recibido la cotización solicitada, gracias por su preferencia.";
                    mm.Attachments.Add(new Attachment(new MemoryStream(PdfData), "Cotizacion_"+Clave+".pdf"));
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
                return View();

            }
            catch (Exception error)
            {
                ViewBag.Mensaje = "Ha ocurrido un error al enviar el correo";
                return View();
            }
        }
        public void EnvioCotizacion2(string receiver, string Clave)
        {
            try
            {
                if (receiver != "")
                {
                    var pdf = new ActionAsPdf("Cotizacion", new { name = "Giorgio", ClaveCotizacion = Clave }) { FileName = "Test.pdf" };

                    Byte[] PdfData = pdf.BuildFile(ControllerContext);
                    MailAddress from = new MailAddress(ConfigurationManager.ConnectionStrings["mail"].ToString(), "LAPCIC: ANÁLISIS CLÍNICOS");
                    MailAddress to = new MailAddress(receiver);
                    MailMessage mm = new MailMessage(from, to);
                    mm.Subject = "Cotización de estudios " + Clave;
                    mm.Body = "Ha recibido la cotización solicitada, gracias por su preferencia.";
                    mm.Attachments.Add(new Attachment(new MemoryStream(PdfData), "Cotizacion.pdf"));
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
        public ActionResult ResultadoSolicitudEstudio(string Estudio)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
               //ViewBag.Restulado = conexion.ConsultarColonia//
            }
            return View();
        }
        public ActionResult Gabinete()
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
        public ActionResult RadiologiaMedico()
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
        public ActionResult UltrasonidoMedico()
        {
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult ListadoGabinete()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = conexion.ConsultarEstudioGabineteColeccion();
            }
            return View();
        }
        public ActionResult ListadoEstudiosGabinete(long Id)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = movil.ConsultarEstudioGabinetePaciente(Id);
            }
            return View("ListadoGabinete");
        }
        public ActionResult ListadoEstudiosPerfil(string ClaveEstudio)
        {
            using (var movil = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.ClavePerfil = ClaveEstudio;
                ViewBag.Estudios = movil.ConsultarEstudioPerfil(ClaveEstudio);
            }
            return View();
        }
        public ActionResult ListadoUltrasonidoMedico()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = conexion.ConsultarEstudioUltrasonidoMedicoColeccion();
            }
            return View("ListadoGabinete");
        }
        public ActionResult ListadoRadiologiaMedico()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = conexion.ConsultarEstudioRadiologiaMedicoColeccion();
            }
            return View("ListadoGabinete");
        }
        public ActionResult ListadoRadiologiaTecnico()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = conexion.ConsultarEstudioRadiologiaTecnicoColeccion();
            }
            return View("ListadoGabinete");
        }
        public ActionResult ListadoEstudios()
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = conexion.ConsultarEstudioColeccion();
            }
            return View();
        }
        public ActionResult EstudiosGabinete()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                List<Administracion.Estudio> _estudios = new List<Administracion.Estudio>();
                _estudios.AddRange(conexion.ConsultarEstudioDepartamentoGabinete());
                ViewBag.Estudios = _estudios;
            }
            return View();
        }
        public ActionResult ListadoPerfiles()
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Perfiles = conexion.ConsultarPerfilesColeccion();
            }
            return View();
        }
        public JsonResult GetPacientes(string Nombre) {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                return Json(conexion.ConsultarPacientesNombre(Nombre));
            }
        }
        
        public JsonResult GetMedicos(string Nombre)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                return Json(conexion.ConsultarMedicosNombre(Nombre));
            }
        }
        public JsonResult GetEstudios(string Nombre)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                return Json(conexion.ConsultarEstudioNombre(Nombre));
            }
        }
        public JsonResult GetEstudiosTipo(int Tipo)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var estudios = conexion.ConsultarEstudioDepartamento(Tipo);
                return Json(estudios);
            }
        }
        public ActionResult Componentes(string Clave)
        {

            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"];
            ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.ClaveEstudio = Clave;
                    ViewBag.Componentes = conexion.ConsultarComponentesEstudio(Clave);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public JsonResult CapturaResultado(int Id, string Observaciones, string correo, long idestudio)
        {
            bool res = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; 
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
               res =  conexion.CrearResultadoEstudio(Id, Observaciones, _Usuario.Id);
                EnvioCorreoInterpretacion(correo, Id, idestudio);
            }
            return Json(new { Resultado = res });
        }
        public class AllowHtmlBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var request = controllerContext.HttpContext.Request;
                var name = bindingContext.ModelName;
                return request.Unvalidated[name]; //magic happens here
            }
        }
        public JsonResult CapturaResultado2(int Id, [ModelBinder(typeof(AllowHtmlBinder))]  string Observaciones, string correo, long idestudio, string adjuntos, int Enviar)
        {
            bool res = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.CrearResultadoEstudio2(Id, Observaciones, _Usuario.Id, adjuntos);
                if (Enviar == 1)
                    EnvioCorreoInterpretacion(correo, Id, idestudio);
            }
            return Json(new { Resultado = res });
        }
        public JsonResult CambiarEstatus(int Id, string Clave, bool Pagado,int Estatus)
        {
            bool res = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
                var email = conexion.CambiarEstatus(Clave, Pagado, Estatus, _Usuario.Id);
                try
                {
                    if (email != "" && Pagado && (Estatus == 4 || Estatus == 5))
                        EnvioCorreo(email, Id, Clave);
                }
                catch (Exception error) { };
                res = email != "" ? true : false;
            }
            return Json(new { Resultado = res });
        }
        [ValidateInput(false)]
        public JsonResult CapturaResultadosEstudios(List<Laboratorio.Administracion.ResultadoSolicitud> Resultados)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearResultadosEstudioSolicitud(Resultados);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EnvioEstudio(int Id, string Observaciones, string adjuntos)
        {
            bool res = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                res = conexion.EnviarEstudioGabinete(Id, Observaciones, adjuntos, _Usuario.Id);
            }
            return Json(new { Resultado = res });
        }
        public ActionResult Nuevo()
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Departamentos = new SelectList(conexion.ConsultarDepartamentoColeccion(), "Id", "Nombre");
                ViewBag.Depositos = new SelectList(conexion.ConsultarTipoDepositoColeccion(), "Id", "Nombre");
                ViewBag.Muestras = new SelectList(conexion.ConsultarTipoMuestraColeccion(), "Id", "Nombre");
            }
            return View();
        }
        public ActionResult NuevoGabinete()
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {

            }
            return View();
        }
        public ActionResult NuevoPerfil()
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Departamentos = new SelectList(conexion.ConsultarDepartamentoColeccion(), "Id", "Nombre");
            }
            return View();
        }
        public ActionResult NuevoComponente(string Clave) {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.ClaveEstudio = Clave;
                ViewBag.TipoCaptura = new SelectList(conexion.ConsultarTipoCapturaColeccion(), "Id", "Descripcion");

            }
            return View();
        }
        public JsonResult CrearComponente(string Nombre, string Abreviatura, string Unidad, int Orden, int TipoCaptura, string ClaveEstudio)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearComponenteEstudio(TipoCaptura,Nombre,Abreviatura,Unidad,Orden,ClaveEstudio);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult AplicarPago(string ClaveSolicitud, string Tipo, double Monto, double Descuento, double CostoUrgencias, string Adicional)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearPago(ClaveSolicitud, Tipo, Monto, Descuento, CostoUrgencias, _Usuario.Id, Adicional);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult AplicarPagoGabinete(long Id, string Tipo, double Monto)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearPagoGabinete(Id, Tipo, Monto, _Usuario.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult ActualizarComponente(Laboratorio.Administracion.Componente Componente, int tipo, [ModelBinder(typeof(AllowHtmlBinder))]  string Texto)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                if (Componente.TextoNormalGeneral == null)
                {
                    Componente.TextoNormalGeneral = Texto;
                }
                resultado = conexion.ActualizarComponente(Componente, tipo);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EliminarEstudioSolicitud(long Id)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarEstudioSolicitud(Id, _Usuario.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EliminarEstudioPerfil(long Id)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarEstudioPerfil(Id, _Usuario.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EliminarPago(long Id, string Observaciones)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
                ViewBag.Usuario = _Usuario;
                resultado = conexion.EliminarPago(Id,_Usuario.Id, _Usuario.NombreCompleto + ": " + Observaciones);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EliminarSolicitud(long Id)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarSolicitud(Id, _Usuario.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EliminarSolicitudGabinete(long Id)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarSolicitudGabinete(Id, _Usuario.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult Eliminar(long Id)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EliminarEstudio(Id, _Usuario.Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearElementoComponente(string Descripcion, int Orden, int Componente)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearElementoComponente(Descripcion,Componente,Orden);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult AsignarEstudioSolicitud(string ClaveSolicitud, string ClaveEstudio, int Orden)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.AsignarEstudioSolicitud(ClaveSolicitud, ClaveEstudio, _Usuario.Id, Orden);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult AsignarEstudioSolicitudGabinete(long Id, int ClaveEstudio, int Orden)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.AsignarEstudioGabinete(ClaveEstudio, _Usuario.Id, Id);
            }
            return Json(new { Resultado = resultado });
        }
        public ActionResult NuevoEstudioGabinete()
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Empresas = new SelectList(conexion.ConsultarOrganizacionesColeccion(), "Id", "Nombre");
                ViewBag.Medicos = new SelectList(conexion.ConsultarMedicosColeccion(), "Id", "NombreCompleto");
                ViewBag.TipoSolicitud = new SelectList(conexion.ConsultarEstudioGabinete(), "Id", "Nombre");
            }
            return View();
        }
        public ActionResult NuevaSolicitud()
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Empresas = new SelectList(conexion.ConsultarOrganizacionesColeccion(), "Id", "Nombre");
                ViewBag.Medicos = new SelectList(conexion.ConsultarMedicosColeccion(), "Id", "NombreCompleto");

            }
            return View();
        }
        public ActionResult DetalleSolicitud(long Id)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Solicitud = conexion.ConsultarSolicitudPagos(Id);
                    ViewBag.Empresas = new SelectList(conexion.ConsultarOrganizacionesColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");
                    ViewBag.Medicos = new SelectList(conexion.ConsultarMedicosColeccion(), "Id", "NombreCompleto");
                    //ViewBag.Muestras = conexion.ConsultarMuestrasSolicitud(ViewBag.Solicitud.ClaveSolicitud);
                }
                return View();
            }
            else {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult DetalleCotizacion(string Clave)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Cotizacion = conexion.ConsultarCotizacion(Clave);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult Solicitud(long Id)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitud = conexion.ConsultarSolicitud(Id);
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud(ViewBag.Solicitud.ClaveSolicitud);
                ViewBag.Muestras = conexion.ConsultarMuestrasSolicitud(ViewBag.Solicitud.ClaveSolicitud);
            }
            return View();
        }
        public ActionResult SolicitudGabinete(int Id)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitud = conexion.ConsultarEstudioGabineteId(Id);
            }
            return View();
        }
        public ActionResult Cotizacion(string ClaveCotizacion)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Cotizacion = conexion.ConsultarCotizacion(ClaveCotizacion);
            }
            return View();
        }
        public ActionResult DetalleEstudioGabinete(int Id)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Estudio = conexion.ConsultarEstudioGabineteId(Id);
                    ViewBag.Empresas = new SelectList(conexion.ConsultarOrganizacionesColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");
                    ViewBag.Medicos = new SelectList(conexion.ConsultarMedicosColeccion(), "Id", "NombreCompleto");
                    ViewBag.TipoSolicitud = new SelectList(conexion.ConsultarEstudioGabinete(), "Id", "Nombre");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult DetalleEstudioPacienteGabinete(int Id)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioPaciente"];
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Estudio = conexion.ConsultarEstudioGabineteId(Id);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Paciente","Seguridad");
            }
        }
        public ActionResult DetalleEstudioMedicoGabinete(int Id)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioMedico"];
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Estudio = conexion.ConsultarEstudioGabineteId(Id);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Medico", "Seguridad");
            }
        }
        public ActionResult DetalleEstudioOrganizacionGabinete(int Id)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["UsuarioOrganizacion"];
            ViewBag.Usuario = _Usuario;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Estudio = conexion.ConsultarEstudioGabineteId(Id);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Organizacion", "Seguridad");
            }
        }
        public ActionResult DetalleSolicitudPaciente(long Id)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitud = conexion.ConsultarSolicitud(Id);
                ViewBag.Empresas = new SelectList(conexion.ConsultarOrganizacionesColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");
                ViewBag.Medicos = new SelectList(conexion.ConsultarMedicosColeccion(), "Id", "NombreCompleto");
            }
            return View();
        }
        public ActionResult EstudiosSolicitud(string Solicitud)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitud = Solicitud;
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud2(Solicitud);


                //ViewBag.Estudios = new SelectList(conexion.ConsultarEstudioColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");

            }
            return View();
        }
        public ActionResult EstudiosImpresionSolicitud(string Solicitud, int IdSolicitud)
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"];
            List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"];
            ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitud = Solicitud;
                ViewBag.IdSolicitud = IdSolicitud;
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud2(Solicitud);

            }
            return View();
        }
        public ActionResult Cotizar()
        { Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Cotizaciones = conexion.ConsultarCotizacionLaboratorio(_Laboratorio.Id);
                }
                return View();
            } 
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult NuevaCotizacion()
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = new SelectList(conexion.ConsultarEstudioColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");
            }
            return View();
        }
        public ActionResult PagosSolicitud(string ClaveSolicitud, string Descuento, string CostoUrgencia)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.ClaveSolicitud = ClaveSolicitud;
                ViewBag.Descuento = Descuento;
                ViewBag.CostoUrgencia = CostoUrgencia;
                ViewBag.TipoPago = new SelectList(conexion.ConsultarTipoPagoColeccion().OrderBy(x => x.Nombre).ToList(), "Clave", "Nombre");

            }
            return View();
        }
        public ActionResult PagosGabinete(long Id)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitud = Id;
                ViewBag.TipoPago = new SelectList(conexion.ConsultarTipoPagoColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");

            }
            return View();
        }
        public ActionResult MovimientosSolicitud(string ClaveSolicitud)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Movimientos = conexion.ConsultarMovimientosSolicitud(ClaveSolicitud);

            }
            return View();
        }
        public ActionResult EstudiosSolicitudPaciente(string Solicitud)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud(Solicitud);
                ViewBag.Estudios = new SelectList(conexion.ConsultarEstudioColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");

            }
            return View();
        }
        public ActionResult EstudiosSolicitudListado(string Solicitud)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.EstudiosSolicitud = conexion.ConsultarEstudioSolicitud(Solicitud);
                ViewBag.Estudios = new SelectList(conexion.ConsultarEstudioColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");

            }
            return View();
        }
        public ActionResult PagosSolicitudPaciente(long Id)
        {
            //AccesoTotal.Manager.Atributos.Usuario _Usuario = (AccesoTotal.Manager.Atributos.Usuario)this.SharedSession["Usuario"];
            //ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {

                ViewBag.TipoPago = new SelectList(conexion.ConsultarTipoPagoColeccion().OrderBy(x => x.Nombre).ToList(), "Id", "Nombre");

            }
            return View();
        }
        public ActionResult Solicitudes()
        {
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            if (_Usuario != null)
            {
                using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                {
                    ViewBag.Id = _Laboratorio.Id;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Seguridad");
            }
        }
        public ActionResult ListadoSolicitudes(int Id, int Puesto)
        {
            ViewBag.Puesto = Puesto;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitudes = conexion.ConsultarSolicitudLaboratorio(Id);
            }
            return View();
        }
        public ActionResult ListadoSolicitudesBusqueda(string texto, int Puesto)
        {
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Puesto = Puesto;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Solicitudes = conexion.ConsultarSolicitudLaboratorioBsuqueda(_Laboratorio.Id,texto);
            }
            return View("ListadoSolicitudes");
        }
        public ActionResult ListadoGabineteBusqueda(string texto)
        {
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Estudios = conexion.ConsultarGabineteBusqueda(texto);
               
            }
            return View("ListadoGabinete");
        }
        public ActionResult ListadoMuestras(string Clave)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Clave = Clave;
                ViewBag.Listado = conexion.ConsultarMuestrasEstudio(Clave);
            }
            return View();
        }
        public JsonResult Crear(int IdMuestra, int IdDeposito, int IdDepartamento, string Nombre, string Clave, string Abreviatura, string Indicaciones, string Precio, string Unidad, string Urgencia, string Volumen, string Numero, int PermiteDescuento)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearEstudio(IdMuestra, IdDeposito, IdDepartamento, Nombre, Clave, Abreviatura, Indicaciones, Precio, Unidad, Urgencia, Volumen, Numero, PermiteDescuento,0);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearPerfil(int Departamento, string Nombre, string Clave, string Abreviatura, string Indicaciones, string Precio, int PermiteDescuento)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearEstudio(0, 0, Departamento, Nombre, Clave, Abreviatura, Indicaciones, Precio, null, null, null, null, PermiteDescuento, 1);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearEstudioGabinete(int Medico, int TipoEstudio, int Paciente, int Organizacion, int TipoSolicitud, string Observaciones, string PaseMedico, List<int> Estudios, bool Interpretacion, int Factura, string DatosFactura)
        {
            bool resultado = false;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearEstudioGabinete(Medico, TipoEstudio, Paciente, Organizacion, _Usuario.Id, TipoSolicitud, Observaciones, PaseMedico, Estudios, Interpretacion, Factura,DatosFactura);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearSolicitudEstudio(int Medico, int Paciente, int Empresa, string PaseMedico, string Observaciones, List<string> Estudios, int Urgencia, string ObservacionesUrgencia, int Factura, string DatosFactura)
        {
            long resultado = 0;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearSolicitud(Medico, Paciente,Empresa, _Usuario.Id, PaseMedico, Observaciones, Estudios, _Laboratorio.Id, Urgencia, ObservacionesUrgencia, Factura, DatosFactura);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult ActualizarSolicitud(long Solicitud, int Medico, int Paciente, int Empresa, string PaseMedico, string Observaciones, int Urgencia, string ObservacionesUrgencia, int Factura, string DatosFactura)
        {
            long resultado = 0;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.ActualizarSolicitud(Solicitud, Medico, Paciente, Empresa, _Usuario.Id, PaseMedico, Observaciones, _Laboratorio.Id, Urgencia, ObservacionesUrgencia, Factura, DatosFactura);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult ActualizarSolicitudGabinete(long Solicitud, int Medico, int Paciente, int Empresa, string PaseMedico, string Observaciones, int Factura, string DatosFactura)
        {
            long resultado = 0;
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.ActualizarSolicitudGabinete(Solicitud, Medico, Paciente, Empresa, _Usuario.Id, PaseMedico, Observaciones, Factura, DatosFactura);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearCotizacionEstudio(string Nombre, string Paterno, string Materno, string Email, List<string> Estudios)
        {
            string resultado = "";
            Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
            ViewBag.Laboratorio = _Laboratorio;
            Laboratorio.Administracion.Usuario _Usuario = (Laboratorio.Administracion.Usuario)this.SharedSession["Usuario"]; List<Laboratorio.Administracion.Laboratorio> _Labs = (List<Laboratorio.Administracion.Laboratorio>)this.SharedSession["Laboratorios"]; ViewBag.Labs = _Labs;
            ViewBag.Usuario = _Usuario;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearCotizacion(Nombre,Paterno,Materno, _Usuario.Id, Estudios, _Laboratorio.Id);
                try
                {
                    if (Email != "")
                    {
                        EnvioCotizacion(Email, resultado);
                    }
                }
                catch (Exception error) { }
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult CrearCotizacionEstudioGeneral(string Nombre, string Paterno, string Materno, string Email, List<string> Estudios)
        {
            string resultado = "";
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                Laboratorio.Administracion.Laboratorio _Laboratorio = (Laboratorio.Administracion.Laboratorio)this.SharedSession["Laboratorio"];
                resultado = conexion.CrearCotizacion(Nombre, Paterno, Materno, 1, Estudios, _Laboratorio.Id);
                try
                {
                    if (Email != "")
                    {
                        EnvioCotizacion(Email, resultado);
                    }
                }
                catch (Exception error) { }
            }
            return Json(new { Resultado = resultado });
        }
        public ActionResult ImprimirSolicitud(long Solicitud, string Clave)
        {
            return new ActionAsPdf("Solicitud", new { name = "Giorgio", Id = Solicitud }) { FileName = Clave +".pdf" };
        }
        public ActionResult ImprimirEtiquetas(string Clave)
        {
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                ViewBag.Muestras = conexion.ConsultarMuestrasSolicitud(Clave);
            }
            return View("Etiquetas");
        }
        public ActionResult ImprimirSolicitudGabinete(long Solicitud)
        {
            return new ActionAsPdf("SolicitudGabinete", new { name = "Giorgio", Id = Solicitud }) { FileName = "Solicitud_"+ Solicitud + ".pdf" };
        }
        public ActionResult ImprimirCotizacion(string ClaveCotizacion)
        {
            return new ActionAsPdf("Cotizacion", new { name = "Giorgio", ClaveCotizacion = ClaveCotizacion }) { FileName = ClaveCotizacion+".pdf" };
        }
        public JsonResult Actualizar(int IdEstudio, int IdMuestra, int IdDeposito, int IdDepartamento, string Nombre, string Clave, string Abreviatura, string Indicaciones, string Precio, string Unidad, string Urgencia, string Volumen, string Numero, int PermiteDescuento)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.ActualizarEstudio(IdEstudio, IdMuestra, IdDeposito, IdDepartamento, Nombre, Clave, Abreviatura, Indicaciones, Precio, Unidad, Urgencia, Volumen, Numero, PermiteDescuento,0);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult ActualizarPerfil(int IdEstudio, int IdDepartamento, string Nombre, string Clave, string Abreviatura, string Indicaciones, string Precio, int PermiteDescuento)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.ActualizarEstudio(IdEstudio, 0, 0, IdDepartamento, Nombre, Clave, Abreviatura, Indicaciones, Precio, "", "", "0", "0", PermiteDescuento, 1);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult GuardarEstudioPerfil(string ClavePerfil, string ClaveEstudio)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.AgregarEstudioPerfil(ClavePerfil,ClaveEstudio);
            }
            return Json(new { Resultado = resultado });
        }

        public JsonResult GuardarEstudioMuestra(string Clave, string Nombre)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.CrearEstudioMuestra(Nombre, Clave);
            }
            return Json(new { Resultado = resultado });
        }
        private string Recibo(Usuarios.Atributos.UsuarioFisica Usuario, string plantilla)
        {
            var Ruta = "Recibos";
            TipoReporte reporte = new TipoReporte();
            ReportesGeneral reporteControl = new ReportesGeneral(ConfigurationManager.ConnectionStrings["Administracion"].ToString());
            using (var report = new ReportesGeneral(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                reporte = report.ConsultarReporteClave(6);
            };
            reporte.Plantilla = plantilla;
            var elemento = new Parametro() { Nombre = "Nombre", Valor = Usuario.Nombre + " " + Usuario.Paterno + " " + Usuario.Materno };
            var items = new List<Parametro>();
            items.Add(elemento);
            elemento = new Parametro() { Nombre = "emailUsuario", Valor = Usuario.Usuario.Email };
            items.Add(elemento);
            elemento = new Parametro() { Nombre = "IdentificacionUsuario", Valor = Usuario.Usuario.Identificacion };
            items.Add(elemento);
            elemento = new Parametro() { Nombre = "nombreClasificacionIdentidad", Valor = Usuario.Usuario.ClasificacionIdentidad.Nombre };
            items.Add(elemento);
            elemento = new Parametro() { Nombre = "fechaUsuario", Valor = Usuario.Usuario.Fecha.ToLongDateString() };
            items.Add(elemento);
            reporte.Parametros = items;
            reporte.Nombre = "Recibo alta de usuario";
            reporte.TipoAmbitoReporte = TipoAmbito.ADMINISTRADOR;
            reporte.TipoRegresa = TipoRegresaReporte.PDFB;
            reporte.Procedimiento = reporte.Procedimiento + Usuario.Usuario.Id;
            var resultadoReporte = reporteControl.ReporteControlParametros(reporte);
            return Ruta;
        }
        public JsonResult ActualizarGeneralComponente(int Id, string General)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.ActualizarGeneralComponente(Id, General);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult ActualizarComponenteLista(int Id, string Nombre, string Orden)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.ActualizarComponenteLista(Id, Nombre, Orden);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EiminarComponenteLista(int Id)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EiminarComponenteLista(Id);
            }
            return Json(new { Resultado = resultado });
        }
        public JsonResult EiminarComponente(int Id)
        {
            bool resultado = false;
            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                resultado = conexion.EiminarComponente(Id);
            }
            return Json(new { Resultado = resultado });
        }
        public ActionResult ImprimirImagenes(int Id, long Detalle, string Imagenes) {

            using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
            {
                var res = conexion.ConsultarEstudioGabineteId(Id);
                ViewBag.Estudio = res;
                ViewBag.Detalle = res.Estudios.Where(x => x.IdDetalle == Detalle).First();
            }
            var _Imagenes = Imagenes.Split(',').ToList();
            ViewBag.Imagenes = _Imagenes;
            return View();
        }
    }
}