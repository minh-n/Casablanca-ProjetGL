﻿using Casablanca.Models;
using Casablanca.Models.Database;
using Casablanca.Models.ExpenseReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Casablanca.Controllers {
    public class LoginController : Controller {
        private IDal dal;

        public LoginController() : this(new Dal()) {
        }

        private LoginController(IDal dal) {
            this.dal = dal;
        }

        // GET: Login
        public ActionResult Index() {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");


            Collaborator model = new Collaborator();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Collaborator model, string returnUrl) {
            if (ModelState.IsValid) {
                Collaborator utilisateur = dal.Login(model.Login, model.Password);
                if (utilisateur != null) {
                    FormsAuthentication.SetAuthCookie(utilisateur.Id.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/Home/Index");
                }
                ModelState.AddModelError("Utilisateur.Prenom", "Nom de compte et/ou mot de passe incorrect(s)");
            }
            return View(model);
        }

        public ActionResult Disconnect() {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Index");
        }

    }
}
