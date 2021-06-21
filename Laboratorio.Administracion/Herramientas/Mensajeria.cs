using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Laboratorio.Administracion;
using System.Net.Mail;
using System.Net;

namespace Laboratorio.Administracion.Herramientas
{
    public class Mensajeria : WebManager
    {
        string baseUri = "https://fcm.googleapis.com/fcm/send";
        string iidUri = "https://iid.googleapis.com/iid/v1/";
        string uriPayPal = "https://api.sandbox.paypal.com/v1/oauth2/token";
        public async Task<Tuple<WebManagerResponse, string>> PostMensajeAsync(MovilMensajeria RqMensaje)
        {
            var jsonReq = JsonConvert.SerializeObject(RqMensaje);
            var httpTask = Task<WebManagerResponse>.Factory.StartNew(() => PostHttp(baseUri, jsonReq));
            var resultado = "";
            if (httpTask.Result.Exito)
            {
               resultado = httpTask.Result.Response.ToString();
            }
            return new Tuple<WebManagerResponse, string>(httpTask.Result, resultado);
        }
        public async Task<Tuple<WebManagerResponse, string>> PostSuscribirAsync(string Token, string Tema)
        {
            var jsonReq = JsonConvert.SerializeObject("");
            var httpTask = Task<WebManagerResponse>.Factory.StartNew(() => PostHttp(iidUri + Token + "/rel/topics/" + Tema, jsonReq));
            var resultado = "";
            if (httpTask.Result.Exito)
            {
                resultado = httpTask.Result.Response.ToString();
            }
            return new Tuple<WebManagerResponse, string>(httpTask.Result, resultado);
        }
        public async Task<Tuple<WebManagerResponse, string>> PostDeSuscribirAsync(string Token, string Tema)
        {
            var jsonReq = JsonConvert.SerializeObject("");
            var httpTask = Task<WebManagerResponse>.Factory.StartNew(() => DeleteHttp(iidUri+ Token + "/rel/topics/" + Tema, jsonReq));
            var resultado = "";
            if (httpTask.Result.Exito)
            {
                resultado = httpTask.Result.Response.ToString();
            }
            return new Tuple<WebManagerResponse, string>(httpTask.Result, resultado);
        }
        public Tuple<WebManagerResponse, string> PostPayPal()
        {
            var httpTask = Task<WebManagerResponse>.Factory.StartNew(() => PostTokenPayPal(uriPayPal));
            var resultado = "";
            if (httpTask.Result.Exito)
            {
                resultado = httpTask.Result.Response.ToString();
            }
            return new Tuple<WebManagerResponse, string>(httpTask.Result, resultado);
        }
        public void SendMail(string Asunto, string Mensaje, string Email)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential("accesototal.rushtecnologias@gmail.com", "RushT3cn0");

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("accesototal.rushtecnologias@gmail.com"),
                    Subject = Asunto,
                    Body = Mensaje
                };

                mail.To.Add(new MailAddress(Email));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
              
            }


        }
    }
}
