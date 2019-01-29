using Casablanca.Models;
using Casablanca.Models.Database;
using Casablanca.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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

            // Check admin privilege
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            if (!HelperModel.CheckAdmin(coll))
                return Redirect("/Home/Index");

            return View();
		}

		//POST : account register
		[HttpPost]
		public ActionResult CreateAccount(Collaborator model /*, int collId*/)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

            // Check admin privilege
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            if (!HelperModel.CheckAdmin(coll))
                return Redirect("/Home/Index");

            // Validation
            if (ModelState.IsValid && ValidationLogin(model))
			{
                dal.CreateCollaborator(model.FirstName, model.LastName, model.Login, dal.EncodeMD5(model.Password));
				return Redirect("/Admin/Index"); 
			}
            else {
                ModelState.AddModelError("", "Le champ nom de compte doit être unique !");
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

		// Get: account creation
		public ActionResult DeleteUser(int id)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			// Check admin privilege
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if (!HelperModel.CheckAdmin(coll))
				return Redirect("/Home/Index");



			dal.RemoveCollaborator(id);


			return Redirect("/Home/Index");
		}


		// Coll List
		public ActionResult CollaboratorsList()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			List<Collaborator> model = dal.GetCollaborators();
	
			return View(model);
		}
		
        private bool ValidationLogin(Collaborator unique_coll) {
            string unique_field = unique_coll.Login;
            List<Collaborator> colls = dal.GetCollaborators();

            foreach (Collaborator c in colls) {
                if (c.Login == unique_field) {
                    return false;
                }
            }
            return true;
        }
	}
}