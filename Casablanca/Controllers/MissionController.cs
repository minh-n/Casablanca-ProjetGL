using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Casablanca.Models.Database;
using Casablanca.Models;

namespace Casablanca.Controllers
{
    public class MissionController : Controller
    {
		private IDal dal;

		public MissionController() : this(new Dal())
		{

		}

		private MissionController(IDal dal)
		{
			this.dal = dal;
		}


		private MultiSelectList GetMultiCollaborators(string[] selectedValues)
		{

			List<Collaborator> CollaboratorsList = dal.GetCollaborators();

			return new MultiSelectList(CollaboratorsList, "Id", "FirstName", selectedValues);

		}


		// GET: Mission
		public ActionResult Index()
        {
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			List<Mission> model = new List<Mission>();

			foreach (Mission mi in dal.GetMissions())
			{
				if (mi.ChiefId == coll.Id)
					model.Add(mi);
			}

			return View(model);
        }

		public ActionResult ProcessMission(int id)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			string[] collId = new string[dal.GetMission(id).CollList.Count];
			int i = 0;
			foreach (Collaborator c in dal.GetMission(id).CollList)
			{
				collId[i++] = c.Id.ToString();
			}

			ViewBag.Collablist = GetMultiCollaborators(collId);

			return View(dal.GetMission(id));
		}

		[HttpPost] // Backend call of ProcessMission page
		public ActionResult ProcessMission(Mission model, int id)
		{          
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			string selectedValues = Request.Form["Collabos"];
			string[] selectedCollaboratorsId = selectedValues.Split(',');

			List<Collaborator> collList = new List<Collaborator>();

			foreach (string i in selectedCollaboratorsId)
			{
				int.TryParse(i, out int collId);

				collList.Add(dal.GetCollaborator(collId));
			}

			//ViewBag.Countrieslist = GetMultiCollaborators(selectedValues.Split(','));



			//TODO : check model state validity ?
			dal.GetMission(id).Name = model.Name;
			dal.GetMission(id).StartDate = model.StartDate;
			dal.GetMission(id).EndDate = model.EndDate;
			dal.GetMission(id).CollList = collList;

			dal.SaveChanges();

			return Redirect("/Mission/Index");
		}


		[HttpPost] // Backend call of Index page
		public ActionResult CreateMission()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//


			// Create the Mission
			int returnedId = dal.CreateMission(coll.Id);
			string redirectString = "/Mission/ProcessMission/?id=" + returnedId;

			//redirect to the right page, displaying the newly created Mission
			return Redirect(redirectString);
		}

		public ActionResult ViewMission(int id)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			return View(dal.GetMission(id));
		}

	}
}