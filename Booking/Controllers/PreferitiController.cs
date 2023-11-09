using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class PreferitiController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        public ActionResult Preferiti()
        {
            short idUtente;
            bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out idUtente);

            List<Preferiti> preferiti = new List<Preferiti>();

            if (isParsed)
            {
                preferiti = db.Preferiti.Include(s => s.Struttura).Where(a => a.IdUtente == idUtente).ToList();
            }
            return View(preferiti);
        }

        [HttpPost]
        public JsonResult AggiungiPreferito(int id)
        {
            if (Session["IdUtente"] != null)
            {
                short idUtente;
                bool isParsed = Int16.TryParse(Session["IdUtente"].ToString(), out idUtente);

                if (isParsed)
                {
                    // Cerca il preferito esistente
                    var preferito = db.Preferiti.FirstOrDefault(p => p.IdUtente == idUtente && p.IdStruttura == id);

                    if (preferito == null)
                    {
                        // Se il preferito non esiste, aggiungilo
                        preferito = new Preferiti();
                        preferito.IdUtente = idUtente;
                        preferito.IdStruttura = id;

                        db.Preferiti.Add(preferito);
                        db.SaveChanges();

                        return Json(new { success = true, action = "added" });
                    }
                    else
                    {
                        // Se il preferito esiste già, rimuovilo
                        db.Preferiti.Remove(preferito);
                        db.SaveChanges();

                        return Json(new { success = true, action = "removed" });
                    }
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false, action = "login" });
            }
        }
    }
}