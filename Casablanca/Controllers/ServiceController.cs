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
	public class Servicecontroller : Controller
	{
		private IDal dal;
		public Servicecontroller() : this(new Dal()) { }
		private Servicecontroller(IDal dal) { this.dal = dal; }

		public ActionResult Index()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			//not in cds = cannot see
			if ((HelperModel.CheckCDS(coll) == false))
				return Redirect("/Home/Index");

			//--------------------------------------------------//


			return View(coll.Service);
		}
	}
		
}