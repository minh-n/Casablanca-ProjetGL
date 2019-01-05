using Casablanca.Models;
using Casablanca.Models.Database;
using Casablanca.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Casablanca.Controllers
{
    public class AdminController : Controller
    {
		private IDal dal;
		public AdminController() : this(new Dal()){}
		private AdminController(IDal dal){this.dal = dal;}

		// GET: Admin
		public ActionResult Index()
        {
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			//TODO check if user roleis Admin
			//Collaborator model = new Collaborator();
			return View(/*model*/);
		}

		// Get: account creation
		public ActionResult CreateAccount()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			return View();
		}

		//POST : account register
		[HttpPost]
		public ActionResult CreateAccount(Collaborator model /*, int collId*/)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			//TODO bug
			int collId = 1;

			if (ModelState.IsValid)
			{
				dal.SetCollaboratorAccount(collId, model.Login, model.Password);
				//???FormsAuthentication.SetAuthCookie(collId.ToString(), false);
				return Redirect("/Admin/Index"); 
			}
			return View(model);
		}

		public ActionResult ServicesList()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			//TODO check if user roleis Admin
			List<Service> model = dal.GetServices();
			return View(model);
		}

		// Coll List
		public ActionResult CollaboratorsList()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			List<Collaborator> model = dal.GetCollaborators();
	
			return View(model);
		}
		

	}
}