using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Laboratorio.Web.Controllers
{
    public class CotizacionesController : Controller
    {
        // GET: Cotizaciones
        public ActionResult Index()
        {
            return View();
        }
    }
}