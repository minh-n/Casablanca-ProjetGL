﻿using Casablanca.Models;
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
        public HomeController() : this(new Dal()) {}
        private HomeController(IDal dal) {this.dal = dal;}

		public ActionResult Index()
        {
			int collId;
			int.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out collId);
			return View(dal.GetCollaborator(collId));
        }

		//public ActionResult UserProfile()
		//{
		//	int collId;
		//	int.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out collId);
		//	return View(dal.GetCollaborator(collId));
		//}


		public ActionResult UserProfile(int id = 0)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			return View(dal.GetCollaborator(id));
		}

		public ActionResult About()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            ViewBag.Message = "Description.";

            return View();
        }

        public ActionResult Contact()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
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

    }
}