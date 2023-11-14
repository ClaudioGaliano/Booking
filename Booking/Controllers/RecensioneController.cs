using Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class RecensioneController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        [HttpGet]
        public ActionResult Recensione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recensione(int id, int punteggio, string commento)
        {
            short idUtente;
            bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out idUtente);

            if (isParsed)
            {
                Recensione recensione = new Recensione
                {
                    Punteggio = punteggio,
                    Commento = commento,
                    IdUtenteFk = idUtente,
                    IdStruttura = id

                };

                if (ModelState.IsValid)
                {
                    db.Recensione.Add(recensione);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Recensione");
        }

        public ActionResult Recensioni(int id)
        {
            List<Recensione> recensioni = db.Recensione.Where(a => a.IdStruttura == id).ToList();
            return View(recensioni);
        }
    }
}