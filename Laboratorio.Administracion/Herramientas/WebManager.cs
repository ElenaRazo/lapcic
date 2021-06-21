using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Laboratorio.Administracion.Herramientas
{
    public class WebManager
    {
        internal WebManagerResponse PostHttp(string Uri, byte[] Data)
        {
            string response = "";
            bool exito = false;

            HttpWebRequest requestHttp = HttpWebRequest.CreateHttp(Uri);
            requestHttp.Method = "POST";
            requestHttp.Headers.Add("Authorization", "Key=AIzaSyBEudqnwqNE_V0i3a71v8Bung0nqK3r-lQ");
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
        internal WebManagerResponse DeleteHttp(string Uri, byte[] Data)
        {
            string response = "";
            bool exito = false;

            HttpWebRequest requestHttp = HttpWebRequest.CreateHttp(Uri);
            requestHttp.Method = "DELETE";
            requestHttp.Headers.Add("Authorization", "Key=AIzaSyBEudqnwqNE_V0i3a71v8Bung0nqK3r-lQ");
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
        internal WebManagerResponse DeleteHttp(string Uri, string Params)
        {
            byte[] Data = Encoding.UTF8.GetBytes(Params);
            return DeleteHttp(Uri, Data);
        }
        internal WebManagerResponse PostTokenPayPal(string Uri)
        {

            string response = "";
            bool exito = false;

            HttpWebRequest requestHttp = HttpWebRequest.CreateHttp(Uri);
            requestHttp.Method = "POST";
            string authInfo = "";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            var body = Encoding.ASCII.GetBytes(authInfo);
            requestHttp.Credentials = new NetworkCredential("AR-NV2xr2PrszM3oHjIJhItyIQY0PIlC4I6VuXmWnhDl6KWu53WxXla7VzZdY3OvUEL484m2ybtXnfsY", "EHNnO79EfoR_xTWTXKaxWOH_Li3Er2oDcCXlFRfxi2B3rhB8LGySDktI6ZYc1k29pt30kc__WtDc_7Ux");
            requestHttp.Headers["grant-type"] = "client_credentials";
            requestHttp.ContentType = "application/x-wwww-form-urlencoded";
            requestHttp.ContentLength = authInfo.Length;


            //Esta peticion puede fallar
            try
            {
                using (Stream stream = requestHttp.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
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
    }
   
    public class AccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
    }
    public class WebManagerResponse
    {
        public bool Exito { get; set; }

        public object Response { get; set; }

        public string Mensaje { get; set; }
    }
}
