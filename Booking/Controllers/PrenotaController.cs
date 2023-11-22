using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking.Models;
using Microsoft.AspNet.Identity;

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
            if (Session["IdUtente"] != null && Int16.TryParse(Session["IdUtente"].ToString(), out short idUtente))
            {
                var utente = db.Utente.Find(idUtente);
                ViewBag.nomeUtente = utente.Nome;
                ViewBag.cognomeUtente = utente.Cognome;
            }

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
            if (IdCamera != 0)
            {
                if (Request.Cookies["DataInizio"] != null && Request.Cookies["DataFine"] != null && Session["IdUtente"] != null)
                {
                    DateTime dataInizio;
                    DateTime dataFine;
                    short id;

                    bool isDataInizioParsed = DateTime.TryParse(Request.Cookies["DataInizio"].Value, out dataInizio);
                    bool isDataFineParsed = DateTime.TryParse(Request.Cookies["DataFine"].Value, out dataFine);
                    bool isIdParsed = Int16.TryParse(Session["IdUtente"].ToString(), out id);

                    if (isDataInizioParsed && isDataFineParsed && isIdParsed)
                    {
                        Prenotazione prenotazione = new Prenotazione();
                        prenotazione.DataPrenotazione = DateTime.Today;
                        prenotazione.DataInizio = dataInizio;
                        prenotazione.DataFine = dataFine;
                        prenotazione.IdUtenteFk = id;
                        prenotazione.IdCameraFk = IdCamera;
                        prenotazione.Totale = calcolaTotale(IdCamera, dataInizio, dataFine);

                        db.Prenotazione.Add(prenotazione);
                        db.SaveChanges();

                        Session["IdPrenotazione"] = prenotazione.IdPrenotazione;

                        //try
                        //{
                        //    EmailController.Mail(prenotazione.IdPrenotazione, db);
                        //    TempData["Successo"] = "";
                        //}
                        //catch (Exception ex)
                        //{
                        //    TempData["Errore"] = "Si è verificato un errore durante l'invio dell'email: " + ex.Message;
                        //}

                        return RedirectToAction("PaymentWithPaypal", "Paypal");
                        //return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Riepilogo");
        }

        public decimal calcolaTotale(int idCamera, DateTime dataInizio, DateTime dataFine)
        {
            Camera camera = db.Camera.Find(idCamera);
            int numeroNotti = (dataFine - dataInizio).Days;
            decimal totale = (numeroNotti * camera.Prezzo);
            return totale;
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