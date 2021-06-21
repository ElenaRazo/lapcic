using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
namespace Laboratorio.Web.Controllers
{
    public class MultimediaController : Controller
    {
        [HttpPost]
        public ActionResult Subir()
        {
            List<string> nombres = new List<string>();
            var fileCount = Request.Files.Count;
            if (fileCount > 0)
            {
                HttpPostedFileBase uploadFile = null;
                for (int i = 0; i < (fileCount); i++)
                {
                    try
                    {
                        uploadFile = Request.Files[i] as HttpPostedFileBase;
                        var ruta = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
                        uploadFile.SaveAs(ruta + @"\Temp\" + uploadFile.FileName);
                        FileInfo info = new FileInfo(ruta + @"\Temp\" + uploadFile.FileName);
                        
                           nombres.Add(uploadFile.FileName);
                       
                    }
                    catch (Exception Error)
                    {

                    }
                }
            }
            return Json(new { Resultado = nombres });
        }
        
        [HttpPost]
        public ActionResult SubirCategoria(long Identificador)
        {
            List<string> nombres = new List<string>();
            var fileCount = Request.Files.Count;
            if (fileCount > 0)
            {
                HttpPostedFileBase uploadFile = null;
                for (int i = 0; i < (fileCount); i++)
                {
                    try
                    {
                        uploadFile = Request.Files[i] as HttpPostedFileBase;
                        string nombre = Guid.NewGuid().ToString();
                        var ruta = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
                        uploadFile.SaveAs(ruta + @"\Imagenes\" + uploadFile.FileName);
                        FileInfo info = new FileInfo(ruta + @"\Imagenes\" + uploadFile.FileName);
                        uploadFile.SaveAs(ruta + @"\Imagenes\" + nombre + info.Extension);
                        if (info.Extension == ".mp4")
                        {
                            var tmb = generateThumb(ruta + @"\Imagenes\" + nombre + info.Extension);
                            nombres.Add(nombre + info.Extension);
                        }
                        if (System.IO.File.Exists(ruta + @"\Imagenes\" + uploadFile.FileName))
                        {
                            System.IO.File.Delete(ruta + @"\Imagenes\" + uploadFile.FileName);
                        }
                        using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Conexion"].ToString()))
                        {
                            //var resultado = conexion.ActualizarFotoCategoria(Identificador, nombre + info.Extension);
                            nombres.Add(nombre + info.Extension);
                        }

                    }
                    catch (Exception Error)
                    {

                    }
                }
            }
            return Json(new { Resultado = nombres });
        }
        [HttpPost]
        public ActionResult SubirFoto(long Identificador)
        {
            List<string> nombres = new List<string>();
            var fileCount = Request.Files.Count;
            if (fileCount > 0)
            {
                HttpPostedFileBase uploadFile = null;
                for (int i = 0; i < (fileCount); i++)
                {
                    try
                    {
                        uploadFile = Request.Files[i] as HttpPostedFileBase;
                        string nombre = Guid.NewGuid().ToString();
                        var ruta = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
                        FileInfo info = new FileInfo(ruta + @"\Imagenes\" + uploadFile.FileName);
                        uploadFile.SaveAs(ruta + @"\Imagenes\" + nombre + info.Extension);
                        using (var conexion = new Laboratorio.Administracion.Metodos(ConfigurationManager.ConnectionStrings["Administracion"].ToString()))
                        {
                            nombres.Add(nombre + info.Extension);
                            var resultado = conexion.ActualizarFotoPaciente(Identificador, @"/Imagenes/" + nombre + info.Extension);
                        }

                    }
                    catch (Exception Error)
                    {

                    }
                }
            }
            return Json(new { Resultado = nombres });
        }
        private byte[] readFileContents(HttpPostedFileBase file)
        {
            Stream fileStream = file.InputStream;
            var mStreamer = new MemoryStream();
            mStreamer.SetLength(fileStream.Length);
            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
            mStreamer.Seek(0, SeekOrigin.Begin);
            byte[] fileBytes = mStreamer.GetBuffer();
            return fileBytes;
        }
        public static string generateThumb(string file)
        {
            string thumb = "";

            try
            {
                FileInfo fi = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(file));
                string filename = Path.GetFileNameWithoutExtension(fi.Name);
                Random random = new Random();
                int rand = random.Next(1, 9999999);
                string newfilename = "/video/" + filename + "___(" + rand.ToString() + ").jpg";
                var processInfo = new ProcessStartInfo();
                processInfo.FileName = "\"" + System.Web.HttpContext.Current.Server.MapPath("/video/ffmpeg.exe") + "\"";
                processInfo.Arguments = string.Format("-ss {0} -i {1} -f image2 -vframes 1 -y {2}", 5, "\"" + System.Web.HttpContext.Current.Server.MapPath(file) + "\"", "\"" + System.Web.HttpContext.Current.Server.MapPath(newfilename) + "\"");
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                using (var process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();
                    process.WaitForExit();
                    thumb = newfilename;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return thumb;
        }
    }
}