using Casablanca.Models.ViewModel;
using System;
using System.Collections.Generic;
using Casablanca.Models.Leaves;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Diagnostics;
using System.Reflection;
using Casablanca.Models.Database;
using Casablanca.Models;
using System.Globalization;

namespace Casablanca.Controllers
{
	public class LeaveController : Controller
	{
        private IDal dal;
        public LeaveController() : this(new Dal()) { }
        private LeaveController(IDal dal) { this.dal = dal; }
		
        public ActionResult Index()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//
			
			// fill the leave table
			List<Leave> model = new List<Leave>();

			foreach (Leave l in dal.GetLeaves())
			{
				if (l.Collaborator.Id == coll.Id)
					model.Add(l);
			}
			return View(model);
		}

		public ActionResult ViewLeave(int id = 1)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//
			
			return View(dal.GetLeave(id));
		}


		#region Calendar Helpers

		public ActionResult CalendarViewFull()
		{
			return this.View();
		}

		public ActionResult GetCalendarData()
		{
			// Initialization
			JsonResult result = new JsonResult();

			try
			{
				// Loading
				List<CalendarVM> data = ConvertLeavesIntoCalendarVM();
				
				// Processing
				result = this.Json(data, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex){Console.Write(ex);}
			return result;
		}

		public List<CalendarVM> ConvertLeavesIntoCalendarVM()
		{
			List<CalendarVM> leaves = new List<CalendarVM>();

			foreach (Leave l in dal.GetLeaves())
			{
				//Debug.WriteLine("Salut  = " + l.EventName + "#  " +  l.Collaborator.LastName);
				
				leaves.Add(new CalendarVM(l));
			}
			return leaves;
		}
		#endregion


		#region Create and Edit Leave

		[HttpPost] // Backend call of create page
		public ActionResult CreateLeave()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//

			// Create the ER
			Leave tempLeave = new Leave(LeaveStatus.PENDING_APPROVAL_1, LeaveType.RTT, coll, DateTime.Now, DateTime.Now);

			//string redirectString = "/Leave/UpdateLeave/?id=" + tempLeave.Id;

			return View("UpdateLeave", tempLeave);
		}

		//public ActionResult UpdateLeave()
		//{
		//	//------------Background identity check-------------//
		//	if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
		//		return Redirect("/Home/Index");

		//	Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
		//	//--------------------------------------------------//


		//	return View();
		//}

		[HttpPost] // Backend call of UpdateLeave page
		public ActionResult UpdateLeave(Leave model)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//


			// Create the leave. TODO : check model state validity ?

			if(!ModelState.IsValid)
			{
				return View(model);
			}

			Leave tempToDb = new Leave
			{
				Collaborator = coll,
				Type = model.Type,
				EventName = model.EventName,
				EndDate = model.EndDate,
				StartDate = model.StartDate
			};


			dal.CreateLeave(tempToDb);
			dal.SaveChanges();
			

			return Redirect("/Leave/Index");
		}


		#endregion



		#region Process Leaves

		public ActionResult ProcessList()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			//not in management OR isCompta = cannot see
			if ((HelperModel.CheckManagement(coll) == false))
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			return View();
		}


		#endregion
	}
}