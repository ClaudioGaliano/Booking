using Booking.Models;
using PagedList;
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
        public ActionResult Recensione(int id, int punteggio, string titolo, string commento)
        {
            short idUtente;
            bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out idUtente);

            if (isParsed)
            {
                Recensione recensione = new Recensione
                {
                    Punteggio = punteggio,
                    Titolo = titolo,
                    Commento = commento,
                    IdUtenteFk = idUtente,
                    Data = DateTime.Today,
                    IdStruttura = id

                };

                if (ModelState.IsValid)
                {
                    db.Recensione.Add(recensione);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Storico", "Prenota");
        }

        public ActionResult Recensioni(int id, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IPagedList<Recensione> recensioni = db.Recensione.Where(a => a.IdStruttura == id).OrderByDescending(r => r.Data).ToPagedList(pageNumber, pageSize);
            return View(recensioni);
        }
    }
}