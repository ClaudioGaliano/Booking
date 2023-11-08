using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Camere()
        {
            return PartialView("_Camere");
        }

        #region Login/Logout
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Username,Password")] Utente utente)
        {
            var user = db.Utente.FirstOrDefault(u => u.Username == utente.Username && u.Password == utente.Password);
            if (user != null)
            {
                Session["IdUtente"] = user.IdUtente;
                Session["Ruolo"] = user.Ruolo;
                Session["Utente"] = user.Nome + " " + user.Cognome;
                FormsAuthentication.SetAuthCookie(user.Username, true);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
