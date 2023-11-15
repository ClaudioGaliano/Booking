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

                        db.Prenotazione.Add(prenotazione);
                        db.SaveChanges();

                        try
                        {
                            Mail();
                            TempData["Successo"] = "Richiesta inviata con successo. Entro 48h verrai ricontattato per approfondimenti sul tuo progetto";
                        }
                        catch (Exception ex)
                        {
                            TempData["Errore"] = "Si è verificato un errore durante l'invio dell'email: " + ex.Message;
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        public void Mail()
        {
            //var utente = db.Utente.FirstOrDefault(x => x.Username == User.Identity.Name);
            string senderEmail = ConfigurationManager.AppSettings["SmtpSenderEmail"];
            string senderPassword = ConfigurationManager.AppSettings["SmtpSenderPassword"];

            //setx NOME_VARIABILE "valore" promt dei comandi
            //string miaVariabile = Environment.GetEnvironmentVariable("NOME_VARIABILE");

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(senderEmail, "Nome azienda"),
                Subject = "Test",
                Body = @"
                <section class=""max-w-2xl px-6 py-8 mx-auto bg-white dark:bg-gray-900"">
                    <main class=""mt-8"">
                        <h2 class=""text-gray-700 dark:text-gray-200"">Pacchetto pro</h2>

                        <p class=""mt-2 leading-loose text-gray-600 dark:text-gray-300"">
                            Hai appena ricevuto una richiesta per il pacchetto pro con il seguente testo:
                        </p>

                        <p class=""mt-2 leading-loose text-gray-600 dark:text-gray-300 italic"">
                            " + @"
                        </p>
                    </main>
                    <footer class=""mt-8"">
                        <p class=""text-gray-500 dark:text-gray-400"">
                            Questa email è stata inviata da " + @"
                        </p>
                        <p class=""mt-1 leading-loose text-gray-600 dark:text-gray-300"">
                            Email: <a href="" + utente.Email + "" class=""text-blue-600 hover:underline dark:text-blue-400"" target=""_blank"">" + @"</a>.
                        </p>
                        <p class=""mt-1 leading-loose text-gray-600 dark:text-gray-300"">
                            Telefono: " + @"
                        </p>
                        <p class=""mt-3 text-gray-500 dark:text-gray-400"">" + DateTime.Now + @"</p>
                    </footer>
                </section>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add("jude.braun83@ethereal.email");

            smtpClient.Send(mailMessage);
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