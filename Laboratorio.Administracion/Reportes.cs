using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements.BarCoding;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.ReportWriter;
using ceTe.DynamicPDF.ReportWriter.Data;
using System.Configuration;
using System.Security.Cryptography;


namespace Laboratorio.Administracion.Reportes
{
    public enum TipoRegresaReporte
    {
        INDEFINIDO = 0,
        PDF = 1,
        PDFB = 2,
        CSV = 3,
        CSVB = 4
    }

    public enum TipoFiltro
    {
        INDEFINIDO = 0,
        SINFILTRO = 1,
        SOLOFECHAS = 2,
        OFICINA = 3,
        ID = 4,
    }

    public enum TipoAmbito
    {
        INDEFINIDO = 0,
        ADMINISTRADOR = 1,
        SISTEMA = 2,
        ADMINISTRADORMETODO = 3,
    }
    public class Parametro
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
    public class TipoReporte
    {
        public List<Parametro> Parametros { get; set; }
        public int IdReporte { get; set; }

        public string Nombre { get; set; }

        public int Item { get; set; }

        public TipoFiltro TipoFiltro { get; set; }

        public string Procedimiento { get; set; }

        public string Plantilla { get; set; }

        public bool Disponible { get; set; }

        public string FiltroFecha { get; set; }

        public string FiltroFechaAl { get; set; }

        public string FiltroOficina { get; set; }

        public string WhereInicial { get; set; }

        public string Ordenamiento { get; set; }

        public DateTime FechaDel { get; set; }

        public DateTime FechaAl { get; set; }

        public double Identificador { get; set; }

        public double Identificador_2 { get; set; }

        public string Reporte { get; set; }

        public byte[] Reportebyte { get; set; }

        public TipoRegresaReporte TipoRegresa { get; set; }

        public TipoAmbito TipoAmbitoReporte { get; set; }

        public DataTable TablaDatos { get; set; }
        public Stream ReporteStream { get; set; }

    }

    public class ReporteEstacion
    {
        public int IdReporte { get; set; }

        public bool Exito { get; set; }

        public string Cadena { get; set; }
    }
    public class ReportesGeneral : Libreria.BaseClass.BaseObject
    {
        string repositorio = "";
        string cadena = "";

        #region Constructor
        public ReportesGeneral(string ConnectionString) : base(ConnectionString)
        {
            ceTe.DynamicPDF.Document.AddLicense(ConfigurationManager.AppSettings.Get("ceTe.LicenseKey"));
            repositorio = ConfigurationManager.AppSettings.Get("Repositorio");
            cadena = ConnectionString;
        }
        #endregion

        public TipoReporte ReporteControl(TipoReporte Reporte)
        {
            TipoReporte ReporteConsulta = Reporte;

            ReporteConsulta = ConsultarReporteClave(Reporte.IdReporte);
            ReporteConsulta.FechaDel = Reporte.FechaDel;
            ReporteConsulta.FechaAl = Reporte.FechaAl;
            ReporteConsulta.TipoRegresa = Reporte.TipoRegresa != TipoRegresaReporte.INDEFINIDO ? Reporte.TipoRegresa : TipoRegresaReporte.PDFB;
            ReporteConsulta.Identificador = Reporte.Identificador;
            ReporteConsulta.Parametros = Reporte.Parametros;
            ReporteConsulta.ReporteStream = Reporte.ReporteStream;
            ReporteConsulta.Identificador_2 = Reporte.Identificador_2;
            ReporteConsulta = RegresaConsultaReporte(ReporteConsulta);

            if (ReporteConsulta.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR)
            {
                if (ReporteConsulta.TipoRegresa == TipoRegresaReporte.PDF || ReporteConsulta.TipoRegresa == TipoRegresaReporte.PDFB)
                    ReporteConsulta = EjecutarReporte(ReporteConsulta);
                else if (ReporteConsulta.TipoRegresa == TipoRegresaReporte.CSV || ReporteConsulta.TipoRegresa == TipoRegresaReporte.CSVB)
                    ReporteConsulta = EjecutarReporteCSV(ReporteConsulta);
            }
            return ReporteConsulta;
        }
        public TipoReporte ReporteControlParametros(TipoReporte Reporte)
        {
            if (Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR)
            {
                if (Reporte.TipoRegresa == TipoRegresaReporte.PDF || Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
                    Reporte = EjecutarReporte(Reporte);
                else if (Reporte.TipoRegresa == TipoRegresaReporte.CSV || Reporte.TipoRegresa == TipoRegresaReporte.CSVB)
                    Reporte = EjecutarReporteCSV(Reporte);
            }
            return Reporte;
        }
        public ReporteEstacion EjecutarReporteEstacion(int IdReporte, string Xml)
        {

            ReporteEstacion reporteTerminal = new ReporteEstacion();
            //Aqui va el byte
            var reporte = ConsultarReporteClave(IdReporte);
            try
            {
                var Resultado = EjecutarReporteEstacion(reporte, Xml);
                var metaArray = Convert.ToBase64String(Resultado.Reportebyte);

                reporteTerminal.Exito = true;
                reporteTerminal.Cadena = metaArray;
            }
            catch (Exception ex)
            {
                reporteTerminal.Exito = false;
                reporteTerminal.Cadena = ex.Message.ToString();
            }
            return reporteTerminal;
        }
        public TipoReporte EjecutarReporteEstacion(TipoReporte Reporte, string Xml)
        {

            DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
            Query query1 = reportDocument.GetQueryById("Query1");
            string consulta = Reporte.Procedimiento;

            query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
            {
                EventDrivenQuery query = (EventDrivenQuery)sender;
                DataSet dataset = new DataSet();
                SqlConnection connection = new SqlConnection(cadena);
                MemoryStream mem = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml));
                dataset.ReadXml(mem);
                e.RecordSet = new DataTableRecordSet(connection, dataset.Tables[0]);
            });

            ParameterDictionary param = new ParameterDictionary();
            param.Add("Reporte", Reporte.Nombre.ToString());
            param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
            param.Add("Sistema", "");
            Document document = reportDocument.Run(param);
            Reporte.Reportebyte = new byte[0];
            using (var memory = new MemoryStream())
            {
                document.Draw(memory);
                Reporte.Reportebyte = memory.ToArray();
            }
            return Reporte;
        }
        public TipoReporte ReporteControlMetodo(TipoReporte Reporte)
        {
            TipoReporte ReporteConsulta = Reporte;

            ReporteConsulta.TipoRegresa = TipoRegresaReporte.PDFB;
            ReporteConsulta.Procedimiento = Reporte.Procedimiento;
            ReporteConsulta.Plantilla = Reporte.Plantilla;
            ReporteConsulta.ReporteStream = Reporte.ReporteStream;
            ReporteConsulta.Identificador_2 = Reporte.Identificador_2;

            if (ReporteConsulta.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR)
            {
                if (ReporteConsulta.TipoRegresa == TipoRegresaReporte.PDF || ReporteConsulta.TipoRegresa == TipoRegresaReporte.PDFB)
                    ReporteConsulta = EjecutarReporteMetodo(ReporteConsulta);
                else if (ReporteConsulta.TipoRegresa == TipoRegresaReporte.CSV || ReporteConsulta.TipoRegresa == TipoRegresaReporte.CSVB)
                    ReporteConsulta = EjecutarReporteCSV(ReporteConsulta);
            }
            return ReporteConsulta;
        }

        public TipoReporte ConsultarReporteClave(long IdReporte)
        {
            //Obtiene la configuración del reporte almacenada en la base de datos
            TipoReporte generales = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Reporte_Id", IdReporte));

                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.reporte_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        generales = new TipoReporte()
                        {
                            IdReporte = int.Parse(Resultado["Id"].ToString()),
                            Nombre = Resultado["Nombre"].ToString(),
                            Item = int.Parse(Resultado["Item"].ToString()),
                            TipoFiltro = (TipoFiltro)int.Parse(Resultado["TipoFiltro"].ToString()),
                            Procedimiento = Resultado["Procedimiento"].ToString(),
                            Plantilla = Resultado["Plantilla"].ToString(),
                            FiltroFecha = Resultado["FiltroFecha"].ToString(),
                            FiltroFechaAl = Resultado["FiltroFechaAl"].ToString(),
                            FiltroOficina = Resultado["FiltroOficina"].ToString(),
                            WhereInicial = Resultado["WhereInicial"].ToString(),
                            Ordenamiento = Resultado["Ordenamiento"].ToString(),
                            TipoAmbitoReporte = (TipoAmbito)int.Parse(Resultado["TipoAmbito"].ToString())
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return generales;

        }

        public List<TipoReporte> ConsultarReportesCuenta(int IdCuenta)
        {
            List<TipoReporte> Reportes = new List<TipoReporte>();
            TipoReporte generales = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Disponible", 1));
                prmtrs.Add(new SqlParameter("@Cuenta", IdCuenta));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.reporteColeccionCuenta_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        generales = new TipoReporte()
                        {
                            IdReporte = int.Parse(Resultado["Id"].ToString()),
                            Nombre = Resultado["Nombre"].ToString(),
                            Item = int.Parse(Resultado["Item"].ToString()),
                            TipoFiltro = (TipoFiltro)int.Parse(Resultado["TipoFiltro"].ToString()),
                            Procedimiento = Resultado["Procedimiento"].ToString(),
                            Plantilla = Resultado["Plantilla"].ToString(),
                            FiltroFecha = Resultado["FiltroFecha"].ToString(),
                            FiltroFechaAl = Resultado["FiltroFechaAl"].ToString(),
                            FiltroOficina = Resultado["FiltroOficina"].ToString(),
                            WhereInicial = Resultado["WhereInicial"].ToString(),
                            Ordenamiento = Resultado["Ordenamiento"].ToString(),
                            TipoAmbitoReporte = (TipoAmbito)int.Parse(Resultado["TipoAmbito"].ToString())
                        };
                        Reportes.Add(generales);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return Reportes;

        }
        private DataTable LlenarTablaMetodo(DataTable tablaOriginal, TipoReporte Reporte)
        {
            DataTable tablaNueva = new DataTable();
            DataView tablaDV = tablaNueva.DefaultView;
            return tablaDV.ToTable();
        }

        public TipoReporte EjecutarReporteMetodo(TipoReporte Reporte)
        {
            DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
            Query query1 = reportDocument.GetQueryById("Query1");
            string consulta = Reporte.Procedimiento;

            query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
            {
                EventDrivenQuery query = (EventDrivenQuery)sender;
                SqlConnection connection = new SqlConnection(cadena);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, connection);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                DataTable tablaNueva = new DataTable();
                tablaNueva = LlenarTablaMetodo(dataTable, Reporte);
                e.RecordSet = new DataTableRecordSet(connection, tablaNueva);
            });

            ParameterDictionary param = new ParameterDictionary();
            param.Add("Reporte", Reporte.Nombre.ToString());
            param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
            param.Add("Sistema", "Tribunal de Justicia Administrativa del Estado de Guanajuato");
            Document document = reportDocument.Run(param);

            if (Reporte.TipoRegresa == TipoRegresaReporte.PDF)
            {
                Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".pdf";
                document.Draw(repositorio + Reporte.Reporte);
            }
            else if (Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
            {
                Reporte.Reportebyte = new byte[0];
                using (var memory = new MemoryStream())
                {
                    document.Draw(memory);
                    Reporte.Reportebyte = memory.ToArray();
                }
            }
            return Reporte;
        }
        public TipoReporte RegresaConsultaReporte(TipoReporte Reporte)
        {

            string ConsultaGenerada = "";
            ConsultaGenerada = Reporte.Procedimiento;
            if (Reporte.WhereInicial != "")
            {
                ConsultaGenerada = ConsultaGenerada + " " + Reporte.WhereInicial;
            }
            //si Reporte.Identificador = 0, muestra todo
            if (Reporte.TipoFiltro == TipoFiltro.ID)
            {
                ConsultaGenerada = ConsultaGenerada + Reporte.Identificador;
            }
            if (Reporte.TipoFiltro == TipoFiltro.OFICINA)
            {
                if (Reporte.Identificador != 0)
                {
                    if (Reporte.WhereInicial != "")
                    {
                        ConsultaGenerada = ConsultaGenerada + " and " + Reporte.FiltroOficina + Reporte.Identificador;
                    }
                    else
                    {
                        ConsultaGenerada = ConsultaGenerada + " Where " + Reporte.FiltroOficina + Reporte.Identificador;
                    }
                    if (Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR || Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADORMETODO)
                        ConsultaGenerada = ConsultaGenerada + " and " + Reporte.FiltroFecha + "'" + Reporte.FechaDel.ToString("yyyy/MM/dd") + "' and " + Reporte.FiltroFechaAl + "'" + Reporte.FechaAl.ToString("yyyy/MM/dd") + "'";
                    else
                        ConsultaGenerada = ConsultaGenerada + " and " + Reporte.FiltroFecha + "" + Reporte.FechaDel.Ticks + " and " + Reporte.FiltroFechaAl + "" + Reporte.FechaAl.Ticks + "";
                }
                else
                {
                    if (Reporte.WhereInicial == "")
                    {
                        ConsultaGenerada = ConsultaGenerada + " Where ";
                        if (Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR || Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADORMETODO)
                            ConsultaGenerada = ConsultaGenerada + Reporte.FiltroFecha + "'" + Reporte.FechaDel.ToString("yyyy/MM/dd") + "' and " + Reporte.FiltroFechaAl + "'" + Reporte.FechaAl.ToString("yyyy/MM/dd") + "'";
                        else
                            ConsultaGenerada = ConsultaGenerada + Reporte.FiltroFecha + "" + Reporte.FechaDel.Ticks + " and " + Reporte.FiltroFechaAl + "" + Reporte.FechaAl.Ticks + "";
                    }
                    else
                    {
                        if (Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR || Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADORMETODO)
                            ConsultaGenerada = ConsultaGenerada + " and " + Reporte.FiltroFecha + "'" + Reporte.FechaDel.ToString("yyyy/MM/dd") + "' and " + Reporte.FiltroFechaAl + "'" + Reporte.FechaAl.ToString("yyyy/MM/dd") + "'";
                        else
                            ConsultaGenerada = ConsultaGenerada + " and " + Reporte.FiltroFecha + "" + Reporte.FechaDel.Ticks + " and " + Reporte.FiltroFechaAl + "" + Reporte.FechaAl.Ticks + "";
                    }
                }
            }

            if (Reporte.TipoFiltro == TipoFiltro.SOLOFECHAS)
            {
                if (Reporte.WhereInicial == "")
                {
                    ConsultaGenerada = ConsultaGenerada + " Where ";
                    if (Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR || Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADORMETODO)
                        ConsultaGenerada = ConsultaGenerada + Reporte.FiltroFecha + "'" + Reporte.FechaDel.ToString("yyyy/MM/dd") + "' and " + Reporte.FiltroFechaAl + "'" + Reporte.FechaAl.ToString("yyyy/MM/dd") + "'";
                    else
                        ConsultaGenerada = ConsultaGenerada + Reporte.FiltroFecha + "" + Reporte.FechaDel.Ticks + " and " + Reporte.FiltroFechaAl + "" + Reporte.FechaAl.Ticks + "";
                }
                else
                {
                    if (Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADOR || Reporte.TipoAmbitoReporte == TipoAmbito.ADMINISTRADORMETODO)
                        ConsultaGenerada = ConsultaGenerada + " and " + Reporte.FiltroFecha + "'" + Reporte.FechaDel.ToString("yyyy/MM/dd") + "' and " + Reporte.FiltroFechaAl + "'" + Reporte.FechaAl.ToString("yyyy/MM/dd") + "'";
                    else
                        ConsultaGenerada = ConsultaGenerada + " and " + Reporte.FiltroFecha + "" + Reporte.FechaDel.Ticks + " and " + Reporte.FiltroFechaAl + "" + Reporte.FechaAl.Ticks + "";
                }
            }

            if (Reporte.Ordenamiento != "")
            {
                ConsultaGenerada = ConsultaGenerada + " " + Reporte.Ordenamiento;
            }
            Reporte.Procedimiento = ConsultaGenerada;

            return Reporte;
        }
        public TipoReporte EjecutarReporte(TipoReporte Reporte)
        {
            DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
            Query query1 = reportDocument.GetQueryById("Query1");
            string consulta = Reporte.Procedimiento;

            query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
            {
                EventDrivenQuery query = (EventDrivenQuery)sender;
                SqlConnection connection = new SqlConnection(cadena);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, connection);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                e.RecordSet = new DataTableRecordSet(connection, dataTable);
            });

            ParameterDictionary param = new ParameterDictionary();
            param.Add("Reporte", Reporte.Nombre.ToString());
            param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
            param.Add("Sistema", "Tribunal de Justicia Administrativa del Estado de Guanajuato");
            if (Reporte.Parametros != null)
            {
                foreach (Parametro item in Reporte.Parametros)
                {
                    param.Add(item.Nombre, item.Valor);
                }
            }
            try
            {
                Document document = reportDocument.Run(param);
                if (Reporte.ReporteStream != null)
                {
                    Page page = new Page();
                    page = document.Pages[0];
                    Image image = new Image(Reporte.ReporteStream, 0, 600, 1);
                    page.Elements.Add(image);
                }

                if (Reporte.TipoRegresa == TipoRegresaReporte.PDF)
                {
                    Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".pdf";
                    document.Draw(repositorio + Reporte.Reporte);
                }
                else if (Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
                {
                    Reporte.Reportebyte = new byte[0];
                    using (var memory = new MemoryStream())
                    {
                        document.Draw(memory);
                        Reporte.Reportebyte = memory.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex;
            }


            return Reporte;
        }
        public TipoReporte EjecutarReporteCSV(TipoReporte Reporte)
        {
            DataTable Tabla = new DataTable();
            List<object> prmtrs = new List<object>();
            Tabla = base.conexion.EjecutarQuery(Reporte.Procedimiento, prmtrs);

            if (Reporte.TipoRegresa == TipoRegresaReporte.CSV)
            {
                Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".csv";
                using (StreamWriter writer = new StreamWriter(repositorio + Reporte.Reporte, true, Encoding.UTF8))
                {
                    WriteDataTable(Tabla, writer, true);
                }
            }
            else if (Reporte.TipoRegresa == TipoRegresaReporte.CSVB)
            {
                Reporte.Reportebyte = new byte[0];
                using (MemoryStream tempStream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(tempStream, Encoding.UTF8))
                    {
                        WriteDataTable(Tabla, writer, true);
                    }
                    Reporte.Reportebyte = tempStream.ToArray();
                }
            }
            return Reporte;
        }
        public TipoReporte EjecutarReporteDatos(TipoReporte Reporte)
        {
            DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
            Query query1 = reportDocument.GetQueryById("Query1");

            query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
            {
                EventDrivenQuery query = (EventDrivenQuery)sender;
                e.RecordSet = new DataTableRecordSet(Reporte.TablaDatos);
            });

            ParameterDictionary param = new ParameterDictionary();
            param.Add("Reporte", Reporte.Nombre.ToString());
            param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
            param.Add("Sistema", "LAPCIC");
            Document document = reportDocument.Run(param);

            if (Reporte.ReporteStream != null)
            {
                Page page = new Page();
                page = document.Pages[0];
                Image image = new Image(Reporte.ReporteStream, 450, 50, 1);
                page.Elements.Add(image);
            }

            if (Reporte.TipoRegresa == TipoRegresaReporte.PDF)
            {
                Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".pdf";
                document.Draw(repositorio + Reporte.Reporte);
            }
            else if (Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
            {
                Reporte.Reportebyte = new byte[0];
                using (var memory = new MemoryStream())
                {
                    document.Draw(memory);
                    Reporte.Reportebyte = memory.ToArray();
                }
            }
            return Reporte;
        }
        public static void WriteDataTable_R(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            if (includeHeaders)
            {
                IEnumerable<String> headerValues = sourceTable.Columns
                    .OfType<DataColumn>()
                    .Select(column => QuoteValue(column.ColumnName));

                writer.WriteLine(String.Join(",", headerValues));

            }

            IEnumerable<String> items = null;

            foreach (DataRow row in sourceTable.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o?.ToString() ?? String.Empty));
                writer.WriteLine(String.Join(",", items));
            }

            writer.Flush();
        }

        public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            string seperator = ",";
            StringBuilder sb = new StringBuilder();

            if (includeHeaders)
            {
                for (int i = 0; i < sourceTable.Columns.Count; i++)
                {
                    sb.Append(sourceTable.Columns[i]);
                    if (i < sourceTable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                writer.WriteLine(sb.ToString());
            }

            foreach (DataRow dr in sourceTable.Rows)
            {
                sb = new StringBuilder();
                for (int i = 0; i < sourceTable.Columns.Count; i++)
                {
                    sb.Append(dr[i].ToString());

                    if (i < sourceTable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                writer.WriteLine(sb.ToString());
            }

            writer.Flush();
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"",
            value.Replace("\"", "\"\""), "\"");
        }

        private string DataTableToCSV(DataTable datatable, char seperator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                sb.Append(datatable.Columns[i]);
                if (i < datatable.Columns.Count - 1)
                    sb.Append(seperator);
            }
            sb.AppendLine();
            foreach (DataRow dr in datatable.Rows)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(dr[i].ToString());

                    if (i < datatable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static void Query_OpeningRecordSet(object sender, OpeningRecordSetEventArgs e)
        {
            EventDrivenQuery query = (EventDrivenQuery)sender;
            query.ConnectionString = "";
            string consulta = "";
            SqlConnection connection = new SqlConnection(query.ConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, connection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            e.RecordSet = new DataTableRecordSet(connection, dataTable);
        }

        //public TipoReporte ReciboFirmaAcuerdo(TipoReporte Reporte, string Proceso, string Acuerdo, string Acciones)
        //{
        //    TipoReporte ReporteConsulta = Reporte;
        //    ReporteConsulta.IdReporte = 1;
        //    ReporteConsulta.Nombre = "RECEPCIÓN DE PROMOCIONES";
        //    ReporteConsulta.Item = 1;
        //    ReporteConsulta.TipoFiltro = TipoFiltro.INDEFINIDO;

        //    ReporteConsulta.FiltroFecha = "";
        //    ReporteConsulta.FiltroFechaAl = "";


        //    ReporteConsulta.FiltroOficina = "";
        //    ReporteConsulta.WhereInicial = "";
        //    ReporteConsulta.Ordenamiento = "";
        //    ReporteConsulta.TipoAmbitoReporte = TipoAmbito.ADMINISTRADOR;
        //    ReporteConsulta.TipoRegresa = TipoRegresaReporte.PDFB;
        //    ReporteConsulta.FechaDel = Reporte.FechaDel;
        //    ReporteConsulta.FechaAl = Reporte.FechaAl;
        //    ReporteConsulta.Identificador = Reporte.Identificador;
        //    ReporteConsulta.Identificador_2 = Reporte.Identificador_2;
        //    ReporteConsulta = EjecutarReciboFirmaAcuerdo(ReporteConsulta, Proceso, Acuerdo, Acciones);
        //    return ReporteConsulta;
        //}
        //public TipoReporte ReciboFirmaNotificacion(TipoReporte Reporte, string Proceso, string Actuario, string Cuenta, long Evidencia, DateTime Fecha, string Notificado, string Papel, string TipoAuto, DateTime FechaAuto, string Evidencias)
        //{
        //    TipoReporte ReporteConsulta = Reporte;
        //    ReporteConsulta.IdReporte = 1;
        //    ReporteConsulta.Nombre = "RECIBO DE NOTIFICACION ELECTRÓNICA";
        //    ReporteConsulta.Item = 1;
        //    ReporteConsulta.TipoFiltro = TipoFiltro.INDEFINIDO;

        //    ReporteConsulta.FiltroFecha = "";
        //    ReporteConsulta.FiltroFechaAl = "";


        //    ReporteConsulta.FiltroOficina = "";
        //    ReporteConsulta.WhereInicial = "";
        //    ReporteConsulta.Ordenamiento = "";
        //    ReporteConsulta.TipoAmbitoReporte = TipoAmbito.ADMINISTRADOR;
        //    ReporteConsulta.TipoRegresa = TipoRegresaReporte.PDFB;
        //    ReporteConsulta.FechaDel = Reporte.FechaDel;
        //    ReporteConsulta.FechaAl = Reporte.FechaAl;
        //    ReporteConsulta.Identificador = Reporte.Identificador;
        //    ReporteConsulta.Identificador_2 = Reporte.Identificador_2;
        //    ReporteConsulta = EjecutarReciboFirmaNotificacion(ReporteConsulta, Proceso, Actuario, Cuenta, Evidencia, Fecha, Notificado, Papel, TipoAuto, FechaAuto, Evidencias);
        //    return ReporteConsulta;
        //}
        public TipoReporte ReciboListaAcuerdo(TipoReporte Reporte, string Oficina, string Fecha)
        {
            TipoReporte ReporteConsulta = Reporte;
            ReporteConsulta.IdReporte = 1;
            ReporteConsulta.Nombre = "LISTA DE ACUERDOS";
            ReporteConsulta.Item = 1;
            ReporteConsulta.TipoFiltro = TipoFiltro.INDEFINIDO;

            ReporteConsulta.FiltroFecha = "";
            ReporteConsulta.FiltroFechaAl = "";


            ReporteConsulta.FiltroOficina = "";
            ReporteConsulta.WhereInicial = "";
            ReporteConsulta.Ordenamiento = "";
            ReporteConsulta.TipoAmbitoReporte = TipoAmbito.ADMINISTRADOR;
            ReporteConsulta.TipoRegresa = TipoRegresaReporte.PDFB;
            ReporteConsulta.FechaDel = Reporte.FechaDel;
            ReporteConsulta.FechaAl = Reporte.FechaAl;
            ReporteConsulta.Identificador = Reporte.Identificador;
            ReporteConsulta.Identificador_2 = Reporte.Identificador_2;
            ReporteConsulta = EjecutarListaAcuerdo(ReporteConsulta, Oficina, Fecha);
            return ReporteConsulta;
        }
        public TipoReporte ReciboActuaria(TipoReporte Reporte, string Oficina, DateTime Fecha, int FolioActuaria, int FolioSala, string Actuario, string Usuario)
        {
            TipoReporte ReporteConsulta = Reporte;
            ReporteConsulta.IdReporte = 1;
            ReporteConsulta.Nombre = "LISTA DE ACUERDOS";
            ReporteConsulta.Item = 1;
            ReporteConsulta.TipoFiltro = TipoFiltro.INDEFINIDO;
            ReporteConsulta.TipoAmbitoReporte = TipoAmbito.ADMINISTRADOR;
            ReporteConsulta.TipoRegresa = TipoRegresaReporte.PDFB;
            ReporteConsulta.FechaDel = Reporte.FechaDel;
            ReporteConsulta.FechaAl = Reporte.FechaAl;
            ReporteConsulta.Identificador = Reporte.Identificador;
            ReporteConsulta.Identificador_2 = Reporte.Identificador_2;
            ReporteConsulta = EjecutarReciboActuaria(ReporteConsulta, Oficina, Fecha, FolioActuaria, FolioSala, Actuario, Usuario);
            return ReporteConsulta;
        }
        //public TipoReporte EjecutarReciboFirmaAcuerdo(TipoReporte Reporte, string Proceso, string Acuerdo, string Acciones)
        //{
        //    DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
        //    Query query1 = reportDocument.GetQueryById("Query1");
        //    string consulta = Reporte.Procedimiento;

        //    query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
        //    {
        //        EventDrivenQuery query = (EventDrivenQuery)sender;
        //        SqlConnection connection = new SqlConnection(cadena);
        //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, connection);
        //        DataTable dataTable = new DataTable();
        //        sqlDataAdapter.Fill(dataTable);
        //        e.RecordSet = new DataTableRecordSet(connection, dataTable);
        //    });

        //    ParameterDictionary param = new ParameterDictionary();
        //    param.Add("Proceso", Proceso);
        //    param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
        //    param.Add("FechaAuto", Acuerdo);
        //    param.Add("Acciones", Acciones);
        //    param.Add("Sistema", "Tribunal de Justicia Administrativa del Estado de Guanajuato");
        //    try
        //    {
        //        Document document = reportDocument.Run(param);
        //        if (Reporte.ReporteStream != null)
        //        {
        //            Page page = new Page();
        //            page = document.Pages[0];
        //            Image image = new Image(Reporte.ReporteStream, 0, 600, 1);
        //            page.Elements.Add(image);
        //        }

        //        if (Reporte.TipoRegresa == TipoRegresaReporte.PDF)
        //        {
        //            Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".pdf";
        //            document.Draw(repositorio + Reporte.Reporte);
        //        }
        //        else if (Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
        //        {
        //            Reporte.Reportebyte = new byte[0];
        //            using (var memory = new MemoryStream())
        //            {
        //                document.Draw(memory);
        //                Reporte.Reportebyte = memory.ToArray();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = ex;
        //    }


        //    return Reporte;
        //}
        //public TipoReporte EjecutarReciboFirmaNotificacion(TipoReporte Reporte, string Proceso, string Actuario, string Cuenta, long Evidencia, DateTime Fecha, string Notificado, string Papel, string TipoAuto, DateTime FechaAuto, string Evidencias)
        //{
        //    DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
        //    Query query1 = reportDocument.GetQueryById("Query1");
        //    string consulta = Reporte.Procedimiento;

        //    query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
        //    {
        //        EventDrivenQuery query = (EventDrivenQuery)sender;
        //        SqlConnection connection = new SqlConnection(cadena);
        //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, connection);
        //        DataTable dataTable = new DataTable();
        //        sqlDataAdapter.Fill(dataTable);
        //        e.RecordSet = new DataTableRecordSet(connection, dataTable);
        //    });

        //    ParameterDictionary param = new ParameterDictionary();
        //    param.Add("Proceso", Proceso);
        //    param.Add("Evidencia", Evidencia);
        //    param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
        //    param.Add("Fecha", Fecha);
        //    param.Add("Hora", Fecha.Hour);
        //    param.Add("Minutos", Fecha.Minute);
        //    param.Add("Cuenta", Cuenta);
        //    param.Add("Papel", Papel);
        //    param.Add("Actuario", Actuario);
        //    param.Add("Evidencias", Evidencias);
        //    param.Add("Notificado", Notificado);
        //    param.Add("TipoAuto", TipoAuto);
        //    param.Add("FechaAuto", FechaAuto.ToLongDateString());
        //    param.Add("FechaLarga", Fecha.ToLongDateString());

        //    param.Add("Sistema", "Tribunal de Justicia Administrativa del Estado de Guanajuato");
        //    try
        //    {
        //        Document document = reportDocument.Run(param);
        //        if (Reporte.ReporteStream != null)
        //        {
        //            Page page = new Page();
        //            page = document.Pages[0];
        //            Image image = new Image(Reporte.ReporteStream, 30
        //                , 600, 1);
        //            page.Elements.Add(image);
        //        }

        //        if (Reporte.TipoRegresa == TipoRegresaReporte.PDF)
        //        {
        //            Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".pdf";
        //            document.Draw(repositorio + Reporte.Reporte);
        //        }
        //        else if (Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
        //        {
        //            Reporte.Reportebyte = new byte[0];
        //            using (var memory = new MemoryStream())
        //            {
        //                document.Draw(memory);
        //                Reporte.Reportebyte = memory.ToArray();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = ex;
        //    }


        //    return Reporte;
        //}
        public TipoReporte EjecutarListaAcuerdo(TipoReporte Reporte, string Oficina, string Fecha)
        {
            DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
            Query query1 = reportDocument.GetQueryById("Query1");
            string consulta = Reporte.Procedimiento;

            query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
            {
                EventDrivenQuery query = (EventDrivenQuery)sender;
                e.RecordSet = new DataTableRecordSet(Reporte.TablaDatos);
            });

            ParameterDictionary param = new ParameterDictionary();
            param.Add("Oficina", Oficina);
            param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
            param.Add("Fecha", Fecha);
            param.Add("Sistema", "Tribunal de Justicia Administrativa del Estado de Guanajuato");
            try
            {
                Document document = reportDocument.Run(param);
                if (Reporte.ReporteStream != null)
                {
                    Page page = new Page();
                    page = document.Pages[0];
                    Image image = new Image(Reporte.ReporteStream, 0, 600, 1);
                    page.Elements.Add(image);
                }

                if (Reporte.TipoRegresa == TipoRegresaReporte.PDF)
                {
                    Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".pdf";
                    document.Draw(repositorio + Reporte.Reporte);
                }
                else if (Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
                {
                    Reporte.Reportebyte = new byte[0];
                    using (var memory = new MemoryStream())
                    {
                        document.Draw(memory);
                        Reporte.Reportebyte = memory.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex;
            }


            return Reporte;
        }
        public TipoReporte EjecutarReciboActuaria(TipoReporte Reporte, string Oficina, DateTime Fecha, int FolioActuaria, int FolioSala, string Actuario, string Usuario)
        {
            DocumentLayout reportDocument = new DocumentLayout(repositorio + Reporte.Plantilla + ".dplx");
            Query query1 = reportDocument.GetQueryById("Query1");
            string consulta = Reporte.Procedimiento;

            query1.OpeningRecordSet += new OpeningRecordSetEventHandler((object sender, OpeningRecordSetEventArgs e) =>
            {
                EventDrivenQuery query = (EventDrivenQuery)sender;
                e.RecordSet = new DataTableRecordSet(Reporte.TablaDatos);
            });

            ParameterDictionary param = new ParameterDictionary();
            param.Add("Oficina", Oficina);
            param.Add("Actuario", Actuario);
            param.Add("Usuario", Usuario);
            param.Add("FolioActuaria", FolioActuaria);
            param.Add("FolioSala", FolioSala);
            param.Add("Periodo", "Impreso el " + DateTime.Now.ToShortDateString());
            param.Add("Fecha", Fecha.ToLongDateString());
            param.Add("Sistema", "LAPCIC");
            try
            {
                Document document = reportDocument.Run(param);
                if (Reporte.ReporteStream != null)
                {
                    Page page = new Page();
                    page = document.Pages[0];
                    Image image = new Image(Reporte.ReporteStream, 0, 600, 1);
                    page.Elements.Add(image);
                }

                if (Reporte.TipoRegresa == TipoRegresaReporte.PDF)
                {
                    Reporte.Reporte = Reporte.Plantilla + DateTime.Now.Ticks.ToString() + ".pdf";
                    document.Draw(repositorio + Reporte.Reporte);
                }
                else if (Reporte.TipoRegresa == TipoRegresaReporte.PDFB)
                {
                    Reporte.Reportebyte = new byte[0];
                    using (var memory = new MemoryStream())
                    {
                        document.Draw(memory);
                        Reporte.Reportebyte = memory.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex;
            }


            return Reporte;
        }



    }
}
