﻿using System;
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

		public ActionResult ProcessMission(int id = 1)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
            //--------------------------------------------------//
            Mission model = dal.GetMission(id);

			//string[] collId = new string[model.CollList.Count];
			//int i = 0;
			//foreach (Collaborator c in dal.GetMission(id).CollList)
			//{
			//	collId[i++] = c.Id.ToString();
			//}

			//ViewBag.Collablist = GetMultiCollaborators(collId);

			return View(model);
		}

		[HttpPost] // Backend call of ProcessMission page
		public ActionResult ProcessMission(Mission model)
		{          
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
            //--------------------------------------------------//
            List<Collaborator> collList = new List<Collaborator>();
            Mission m = dal.GetMission(model.Id);

            string selectedValues = Request.Form["right"];
            if (selectedValues != null) {
                string[] selectedCollaboratorsId = selectedValues.Split(',');

                collList = new List<Collaborator>();

                foreach (string i in selectedCollaboratorsId) {
                    int.TryParse(i, out int collId);
                    Collaborator c = dal.GetCollaborator(collId);
                    collList.Add(c);

                    if(!c.Missions.Contains(m)) {
                        c.Missions.Add(m);
                    }
                }
            }

			m.Name = model.Name;
			m.StartDate = model.StartDate;
			m.EndDate = model.EndDate;

			if((m.EndDate < DateTime.Now) && (m.Status != MissionStatus.CANCELED))
			{
				m.Status = MissionStatus.COMPLETED;
			}
			else if ((m.StartDate < DateTime.Now) && (m.EndDate >= DateTime.Now) && (m.Status != MissionStatus.CANCELED))
			{
				m.Status = MissionStatus.IN_PROGRESS;
			}
			else if ((m.StartDate > DateTime.Now) && (m.EndDate > DateTime.Now) && (m.Status != MissionStatus.CANCELED))
			{
				m.Status = MissionStatus.PLANNED;
			}
			else
			{
				m.Status = model.Status;
			}

            m.CollList.Clear();
			m.CollList = collList;

			dal.SaveChanges();

			return Redirect("/Mission/Index");
		}

		public ActionResult ResetStatusMission(int id)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			Mission m = dal.GetMission(id);

			if ((m.EndDate < DateTime.Now))
			{
				m.Status = MissionStatus.COMPLETED;
			}
			else if ((m.StartDate < DateTime.Now) && (m.EndDate >= DateTime.Now))
			{
				m.Status = MissionStatus.IN_PROGRESS;
			}
			else if ((m.StartDate > DateTime.Now) && (m.EndDate > DateTime.Now))
			{
				m.Status = MissionStatus.PLANNED;
			}

			dal.SaveChanges();

			return Redirect("/Mission/Index");
		}

		public ActionResult CancelMission(int id)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//
			Mission m = dal.GetMission(id);

			m.Status = MissionStatus.CANCELED;

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