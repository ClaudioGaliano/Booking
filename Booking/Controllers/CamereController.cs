using Booking;
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
                Response.Cookies["Destinazione"].Value = model.Citta.ToString();

                return RedirectToAction("RisultatiRicerca");
            }
            return View(model);
        }


        //public ActionResult RisultatiRicerca()
        //{

        //    var struttureDisponibili = TempData["RisultatiRicerca"] as List<Struttura>;

        //    return View(struttureDisponibili);
        //}

        public ActionResult RisultatiRicerca()
        {
            var struttureDisponibili = TempData["RisultatiRicerca"] as List<Struttura>;
            var strutturePunteggi = new List<StrutturaPunteggi>();

            // Calcola il punteggio medio per ogni struttura
            foreach (var struttura in struttureDisponibili)
            {
                var recensioni = db.Recensione.Where(r => r.IdStruttura == struttura.IdStruttura);
                double? mediaPunteggio = recensioni.Any() ? recensioni.Average(r => r.Punteggio) : 0;
                strutturePunteggi.Add(new StrutturaPunteggi
                {
                    Struttura = struttura,
                    MediaPunteggio = mediaPunteggio.HasValue ? Math.Round(mediaPunteggio.Value, 1) : (double?)null
                });
            }

            return View(strutturePunteggi);
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