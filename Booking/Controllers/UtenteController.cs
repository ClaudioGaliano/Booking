using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class UtenteController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        public ActionResult DatiPersonali()
        {
            short id;
            bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out id);

            if (isParsed)
            {
                var utente = db.Utente.Find(id);
                return View(utente);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ModificaProfilo(Utente utente)
        {
            short id;
            string ruolo = "";
            bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out id);

            if (Session["Ruolo"] != null)
            {
                ruolo = Session["Ruolo"].ToString();
            }

            if (isParsed)
            {
                utente.Ruolo = ruolo;

                if (ModelState.IsValid)
                {
                    Session["Utente"] = utente.Nome + " " + utente.Cognome;
                    db.Entry(utente).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                    // Fai qualcosa con gli errori, come registrare o visualizzare gli errori
                }
            }
            return RedirectToAction("DatiPersonali");
        }
    }
}
