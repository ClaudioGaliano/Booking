using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public static void Mail(int idPrenotazione, ModelDbContext db)
        {
            var prenotazione = db.Prenotazione.Include(a => a.Utente).Where(a => a.IdPrenotazione == idPrenotazione).FirstOrDefault();

            string senderEmail = ConfigurationManager.AppSettings["SmtpSenderEmail"];
            string senderPassword = ConfigurationManager.AppSettings["SmtpSenderPassword"];

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(senderEmail, "Travel"),
                Subject = "Conferma prenotazione",
                Body = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Conferma di prenotazione</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                        }
                        .container {
                            width: 80%;
                            margin: auto;
                        }
                        .header {
                            text-align: center;
                            padding: 20px;
                            background-color: #f8f8f8;
                            border-bottom: 1px solid #ddd;
                        }
                        .content {
                            padding: 20px;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Conferma di prenotazione</h1>
                        </div>
                        <div class='content'>
                            <p>Gentile " + prenotazione.Utente.Nome + " " + prenotazione.Utente.Cognome + @",</p>
                            <p>Grazie per aver prenotato con noi. Siamo lieti di confermare la tua prenotazione con i seguenti dettagli:</p>
                            <p>ID prenotazione: <strong>" + prenotazione.IdPrenotazione + @"</strong></p>
                            <p>Data di inizio: <strong>" + prenotazione.DataInizio.ToShortDateString() + @"</strong></p>
                            <p>Data di fine: <strong>" + prenotazione.DataFine.ToShortDateString() + @"</strong></p>
                            <p>Totale: <strong>" + prenotazione.Totale.ToString("C2") + @"</strong></p>
                            <p>Se hai domande sulla tua prenotazione, non esitare a contattarci.</p>
                            <p>Cordiali saluti,</p>
                            <p>Travel</p>
                        </div>
                    </div>
                </body>
                </html>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add("claudiogal92@gmail.com");

            smtpClient.Send(mailMessage);
        }
    }
}