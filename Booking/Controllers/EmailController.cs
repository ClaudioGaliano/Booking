using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking.Models;

namespace Booking.Controllers
{
    public class EmailController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        //public void MailProPack(string description)
        //{
        //    var utente = db.Utente.FirstOrDefault(x => x.Username == User.Identity.Name);
        //    string senderEmail = ConfigurationManager.AppSettings["SmtpSenderEmail"];
        //    string senderPassword = ConfigurationManager.AppSettings["SmtpSenderPassword"];

        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential(senderEmail, senderPassword),
        //        EnableSsl = true,
        //    };

        //    var mailMessage = new MailMessage()
        //    {
        //        From = new MailAddress(senderEmail, "Paolo Manca Consulting"),
        //        Subject = "Richiesta Pacchetto Pro",
        //        Body = @"
        //        <section class=""max-w-2xl px-6 py-8 mx-auto bg-white dark:bg-gray-900"">
        //            <main class=""mt-8"">
        //                <h2 class=""text-gray-700 dark:text-gray-200"">Pacchetto pro</h2>

        //                <p class=""mt-2 leading-loose text-gray-600 dark:text-gray-300"">
        //                    Hai appena ricevuto una richiesta per il pacchetto pro con il seguente testo:
        //                </p>

        //                <p class=""mt-2 leading-loose text-gray-600 dark:text-gray-300 italic"">
        //                    " + description + @"
        //                </p>
        //            </main>
        //            <footer class=""mt-8"">
        //                <p class=""text-gray-500 dark:text-gray-400"">
        //                    Questa email è stata inviata da " + utente.Nome + " " + utente.Cognome + @"
        //                </p>
        //                <p class=""mt-1 leading-loose text-gray-600 dark:text-gray-300"">
        //                    Email: <a href="" + utente.Email + "" class=""text-blue-600 hover:underline dark:text-blue-400"" target=""_blank"">" + utente.Email + @"</a>.
        //                </p>
        //                <p class=""mt-1 leading-loose text-gray-600 dark:text-gray-300"">
        //                    Telefono: " + utente.Phone + @"
        //                </p>
        //                <p class=""mt-3 text-gray-500 dark:text-gray-400"">" + DateTime.Now + @"</p>
        //            </footer>
        //        </section>",
        //        IsBodyHtml = true,
        //    };
        //    mailMessage.To.Add("jude.braun83@ethereal.email");

        //    smtpClient.Send(mailMessage);
        //}

        //public ActionResult ProfessionalPack(string description)
        //{
        //    int id = Convert.ToInt16(TempData["IdProdotto"]);
        //    if (description != "")
        //    {
        //        MailProPack(description);
        //        TempData["Successo"] = "Richiesta inviata con successo. Entro 48h verrai ricontattato per approfondimenti sul tuo progetto";
        //        return RedirectToAction("Details", "Home", new { id = id });
        //    }
        //    TempData["Errore"] = "Il campo descrizione non può essere vuoto";
        //    return RedirectToAction("Details", "Home", new { id = id });
        //}
    }
}