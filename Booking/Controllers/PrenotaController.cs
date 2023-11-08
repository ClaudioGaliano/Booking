using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class PrenotaController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Riepilogo(int id)
        {
            var camera = db.Camera.Find(id);
            return View(camera);
        }

        public List<Prenotazione> getStorico()
        {
            List<Prenotazione> storico = new List<Prenotazione>();

            if (Session["IdUtente"] != null && Int16.TryParse(Session["IdUtente"].ToString(), out short id))
            {
                storico = db.Prenotazione.Include(p => p.Camera.Struttura).Where(p => p.IdUtenteFk == id).OrderByDescending(p => p.DataPrenotazione).ToList();
            }

            return storico;
        }

        public ActionResult Storico()
        {
            var storico = getStorico();
            return View(storico);
        }

        [HttpPost]
        public ActionResult Prenota(int IdCamera)
        {
            if(IdCamera != 0)
            {
                DateTime dataInizio = DateTime.Parse(Request.Cookies["DataInizio"].Value);
                DateTime dataFine = DateTime.Parse(Request.Cookies["DataFine"].Value);

                short id;
                bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out id);

                if (isParsed)
                {
                    Prenotazione prenotazione = new Prenotazione();
                    prenotazione.DataPrenotazione = DateTime.Today;
                    prenotazione.DataInizio = dataInizio;
                    prenotazione.DataFine = dataFine;
                    prenotazione.IdUtenteFk = id;
                    prenotazione.IdCameraFk = IdCamera;

                    db.Prenotazione.Add(prenotazione);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult EliminaPrenotazione(int id)
        {
            var prenotazione = db.Prenotazione.Find(id);
            if (prenotazione != null)
            {
                db.Prenotazione.Remove(prenotazione);
                db.SaveChanges();
            }
            return RedirectToAction("Storico");
        }
    }
}