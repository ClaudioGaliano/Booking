﻿using Booking;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Booking.Controllers
{
    public class UtenteController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        [HttpGet]
        public ActionResult Registrati()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registrati(Registrazione model)
        {
            if (ModelState.IsValid)
            {
                var user = new Utente
                {
                    Cognome = model.Cognome,
                    Nome = model.Nome,
                    Email = model.Email,
                    Username = model.Username,
                    Ruolo = "User"
                };

                user.SetPassword(model.Password);

                db.Utente.Add(user);
                await db.SaveChangesAsync();

                // User created successfully, now log in the user
                Session["IdUtente"] = user.IdUtente;
                Session["Ruolo"] = user.Ruolo;
                Session["Utente"] = user.Nome + " " + user.Cognome;
                Session["Iniziali"] = user.Nome.Substring(0, 1) + user.Cognome.Substring(0, 1);
                FormsAuthentication.SetAuthCookie(user.Username, true);

                return RedirectToAction("Index", "Home");
            }

            // Something failed, redisplay form
            return RedirectToAction("Login", "Home");
        }

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
