using Booking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class CamereController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public List<Struttura> GetStruttureConCamereDisponibili(string citta, DateTime dataInizio, DateTime dataFine)
        {
            var struttureConCamereDisponibili = db.Struttura
                    .Include(s => s.Camera.Select(c => c.Prenotazione)) /*sequenza che contiene solo le prenotazioni per ogni camera.*/
                    .Where(s => s.Citta == citta)
                    .Where(s => s.Camera.Any(c => !c.Prenotazione.Any(p => (dataInizio <= p.DataFine && dataFine >= p.DataInizio))))
                    .ToList();

            return struttureConCamereDisponibili;
        }

        [HttpPost]
        public ActionResult RicercaCamere(RicercaCamere model)
        {
            if (ModelState.IsValid)
            {
                var struttureDisponibili = GetStruttureConCamereDisponibili(model.Citta, model.DataInizio, model.DataFine);

                TempData["RisultatiRicerca"] = struttureDisponibili;
                Response.Cookies["DataInizio"].Value = model.DataInizio.ToString();
                Response.Cookies["DataFine"].Value = model.DataFine.ToString();

                return RedirectToAction("RisultatiRicerca");
            }
            return View(model);
        }


        public ActionResult RisultatiRicerca()
        {
            var struttureDisponibili = TempData["RisultatiRicerca"] as List<Struttura>;

            return View(struttureDisponibili);
        }

        //public ActionResult DettaglioStruttura(int id)
        //{
        //    var struttura = db.Struttura
        //        .Include(s => s.Camera.Select(c => c.Servizi))
        //        .FirstOrDefault(s => s.IdStruttura == id);

        //    return View(struttura);
        //}
    }
}