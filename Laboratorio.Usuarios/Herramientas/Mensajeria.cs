using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Laboratorio.Usuarios.Atributos;

namespace Laboratorio.Usuarios.Herramientas
{
    public class Mensajeria : WebManager
    {
        string baseUri = "https://fcm.googleapis.com/fcm/send";
        string iidUri = "https://iid.googleapis.com/iid/v1/";
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
    }
}
