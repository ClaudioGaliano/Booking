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
            double mediaPunteggio = db.Recensione.Where(c => c.IdStruttura == id).Average(c => c.Punteggio);
            mediaPunteggio = Math.Round(mediaPunteggio, 1);
            ViewBag.MediaPunteggio = mediaPunteggio;

            double numeroRecensioni = db.Recensione.Where(c => c.IdStruttura == id).Count();;
            ViewBag.NumeroRecensioni = numeroRecensioni;

            var struttura = db.Struttura
                .Include(s => s.Camera.Select(c => c.Servizi))
                .Include(s => s.Recensioni)
                .FirstOrDefault(s => s.IdStruttura == id);

            if (Session["IdUtente"] != null)
            {
                short idUtente;
                bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out idUtente);

                if (isParsed)
                {
                    var preferito = db.Preferiti.FirstOrDefault(p => p.IdUtente == idUtente && p.IdStruttura == id);
                    ViewBag.IsPreferito = preferito != null;
                }
            }
            return View(struttura);
        }
    }
}