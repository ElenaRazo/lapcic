using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Laboratorio.Usuarios.Herramientas
{
    public class WebManager
    {
        internal WebManagerResponse PostHttp(string Uri, byte[] Data)
        {
            string response = "";
            bool exito = false;

            HttpWebRequest requestHttp = HttpWebRequest.CreateHttp(Uri);
            requestHttp.Method = "POST";
            requestHttp.Headers.Add("Authorization", "Key=AIzaSyCgx08uwBJ4jNL5rwHfNu7snMcp8YQ2ApU");
            requestHttp.ContentType = "application/json";
            requestHttp.ContentLength = Data.Length;

            //Esta peticion puede fallar
            try
            {
                using (Stream stream = requestHttp.GetRequestStream())
                {
                    stream.Write(Data, 0, Data.Length);
                }

                using (Stream stream = requestHttp.GetResponse().GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        response = reader.ReadToEnd();
                    }
                }
                exito = true;
            }
            catch (Exception ex)
            {
                response = ex.StackTrace.ToString();
            }
            return new WebManagerResponse() { Exito = exito, Response = response, Mensaje = response };
        }

        internal WebManagerResponse PostHttp(string Uri, string Params)
        {
            byte[] Data = Encoding.UTF8.GetBytes(Params);
            return PostHttp(Uri, Data);
        }
    }

    public class WebManagerResponse
    {
        public bool Exito { get; set; }

        public object Response { get; set; }

        public string Mensaje { get; set; }
    }
}
