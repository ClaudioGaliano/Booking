using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Booking.Controllers
{
    public class StrutturaController : Controller
    {
        ModelDbContext db = new ModelDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DettaglioStruttura(int id)
        {
            var struttura = db.Struttura
                .Include(s => s.Camera.Select(c => c.Servizi))
                .FirstOrDefault(s => s.IdStruttura == id);

            return View(struttura);
        }
    }
}