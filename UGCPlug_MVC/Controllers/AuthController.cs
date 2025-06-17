using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UGCPlug_MVC.Models;

namespace UGCPlug_MVC.Controllers
{
    public class AuthController : Controller
    {
        private UGCPlug_MVC_DB_ConString db = new UGCPlug_MVC_DB_ConString();

        // GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        public ActionResult Login(BusinessLogin login)
        {
            if (ModelState.IsValid)
            {
                var user = db.BusinessLogins
                             .FirstOrDefault(b => b.BusinessEmail == login.BusinessEmail && b.PasswordHash == login.PasswordHash);

                if (user != null)
                {
                    Session["BusinessId"] = user.BusinessId;
                    Session["BusinessName"] = user.BusinessName;

                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewBag.Error = "Invalid email or password.";
            return View(login);
        }

        // GET: Auth/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login");
        }


        // GET: Auth/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(BusinessLogin newBusiness)
        {
            if (ModelState.IsValid)
            {
                // Prevent duplicate emails
                if (!db.BusinessLogins.Any(b => b.BusinessEmail == newBusiness.BusinessEmail))
                {
                    // Generate a clean, unique BusinessId
                    var proposedId = newBusiness.BusinessName.ToLower().Replace(" ", "-");
                    string uniqueId = proposedId;
                    int counter = 1;

                    while (db.BusinessLogins.Any(b => b.BusinessId == uniqueId))
                    {
                        uniqueId = $"{proposedId}-{counter}";
                        counter++;
                    }

                    newBusiness.BusinessId = uniqueId;

                    // Save to DB
                    db.BusinessLogins.Add(newBusiness);
                    db.SaveChanges();

                    // Set login session
                    Session["BusinessId"] = newBusiness.BusinessId;
                    Session["BusinessName"] = newBusiness.BusinessName;

                    return RedirectToAction("Index", "Dashboard");
                }

                ViewBag.Error = "Email already registered.";
            }

            return View(newBusiness);
        }


    }
}
