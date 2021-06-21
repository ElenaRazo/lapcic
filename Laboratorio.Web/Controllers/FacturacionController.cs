using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio.Web.Controllers
{
    public class FacturacionController : Controller
    {
        // GET: Facturacion
        public ActionResult Emitir()
        {
            return View();
        }
    }
}