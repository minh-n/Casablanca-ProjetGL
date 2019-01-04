using Casablanca.Models;
using Casablanca.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Casablanca.Controllers
{
    public class HomeController : Controller
    {
        private IDal dal;

        public HomeController() : this(new Dal()) {

        }

        private HomeController(IDal dal) {
            this.dal = dal;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            ViewBag.Message = "Contactez l'administration";

            return View();
        }

        public ActionResult CollaboratorList()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // Récupérer les collaborateurs dans la BD
            List<Collaborator> model = dal.GetCollaborators();

			return View(model);
        }

        public ActionResult CollaboratorView(int id)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // TODO : check if it is the right user

            Collaborator model = dal.GetCollaborators()[id];
			
            return View(model);
        }
    }
}