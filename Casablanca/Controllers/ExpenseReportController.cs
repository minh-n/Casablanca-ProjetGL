using Casablanca.Models.Database;
using Casablanca.Models.ExpenseReports;
using Casablanca.Models.ViewModel;
using Casablanca.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace Casablanca.Controllers
{
    public class ExpenseReportController : Controller
    {
        private IDal dal;

        public ExpenseReportController() : this(new Dal()) {

        }

        private ExpenseReportController(IDal dal) {
            this.dal = dal;
        }

        public ActionResult Index() // TODO : get collID
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // Who are we 
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            List<ExpenseReport> reports = coll.ExpenseReports;

            ExpenseReport er = dal.GetExpenseReport(1);
            AddExpenseLineVM linesVM = new AddExpenseLineVM { ExpenseReport = er, CollaboratorMissions = GetMissionsList(coll) };

            AddExpenseReportVM model = new AddExpenseReportVM(linesVM, reports);

            return View(model);
		}

		public ActionResult AddExpenseReport(int id = 3)
		{
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // Who are we 
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            ExpenseReport er = dal.GetExpenseReport(id);
			AddExpenseLineVM model = new AddExpenseLineVM {ExpenseReport = er, CollaboratorMissions = GetMissionsList(coll) };

			return View(model);
		}

		public ActionResult ProcessCDS()
		{
			return View();
		}

		public ActionResult ProcessCompta(int ERId)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			// in compta or cannot see
			if (!HelperModel.CheckCompta(coll))
				return Redirect("/Home/Index");

			ExpenseReport model = dal.GetExpenseReport(ERId);
			return View(model);
		}

		/*
		 * display the ER list management needs to process
		 */
		public ActionResult ProcessList()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			//not in management OR isRH = cannot see
			if ((HelperModel.CheckManagement(coll) == false) || HelperModel.CheckRH(coll))
				return Redirect("/Home/Index");
		
			List<ExpenseReport> reports = dal.GetExpenseReports();
			List<ExpenseReport> model = new List<ExpenseReport>();

			foreach (ExpenseReport e in reports)
			{
				if (HelperModel.CheckCompta(coll))
				{
					if(e.Status == ExpenseReportStatus.PENDING_APPROVAL_2)
						model.Add(e);
				}
				else if (HelperModel.CheckCDS(coll))
				{
					if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_1)
					{
						
						//todo foreach e.expenseline voir si le cds est bien notre coll
					}

					//TODO : get que les trucs en relation avec coll
				}
			}
			
			return View(model);
		}
		
        [HttpPost]
        public ActionResult CreateExpenseReport() {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            // admin cannot have ER
            if (HelperModel.CheckAdmin(coll))
                return Redirect("/Home/Index");

            string month = Request.Form["monthName"].ToString();

            // Compute year
            int year = DateTime.Now.Year;
            if (month != DateTime.Now.ToString("MMMM") && month == new CultureInfo("en-US").DateTimeFormat.GetMonthName(12))
                year = DateTime.Now.Year - 1;

            // Compute month
            Enum.TryParse(month, out Month m);
    
            // Create the ER
            dal.CreateExpenseReport(coll, m, year);

            return Redirect("/ExpenseReport/Index");
        }

		//---------------------------------------------------------------------------------
		//---------------------------------------------------------------------------------
		//----------------------Update Expense Report--------------------------------------
		//---------------------------------------------------------------------------------
		

		//Get
		public ActionResult UpdateExpenseReport()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			// admin cannot have ER
			if (HelperModel.CheckAdmin(coll))
				return Redirect("/Home/Index");

			return View(new AddExpenseLineVM { CollaboratorMissions = GetMissionsList(coll)});
		}


		//Post
		[HttpPost]
		public ActionResult UpdateExpenseReport(AddExpenseLineVM model) {

            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            // admin cannot have ER
            if (HelperModel.CheckAdmin(coll))
                return Redirect("/Home/Index");

			if (!ModelState.IsValid)
			{
				model.CollaboratorMissions = GetMissionsList(coll);
				return View(model);
			}

			//Debug.WriteLine("Id de la mission = " + model.SelectedMission);

			model.ExpenseReport.ExpenseLines[0].Mission = dal.GetMission(model.ExpenseReport.ExpenseLines[0].Mission.Id);
			Debug.WriteLine("ty de la mission = " + model.ExpenseReport.ExpenseLines[1].Type);


			return Redirect("/ExpenseReport/Index");
        }

		//Tuto magique qui m'a sauvé sur ce coup-ci
		//https://stackoverflow.com/questions/48170338/dropdownlist-selected-value-is-set-to-null-in-the-model-on-post-action
		//Merci dey.shin !
		private static IEnumerable<SelectListItem> GetMissionsList(Collaborator coll)
		{
			var missions = new List<SelectListItem>();
			foreach (var s in coll.Missions.ToList())
			{
				var miss = new SelectListItem { Value = s.Id.ToString(), Text = s.Name};
				missions.Add(miss);
			}
			return missions;
		}



		

		//public ActionResult UpdateExpenseReport(AddExpenseLineVM model)
		//{
		//	if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
		//		return Redirect("/Home/Index");

		//	Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

		//	// admin cannot have ER
		//	if (HelperModel.CheckAdmin(coll))
		//		return Redirect("/Home/Index");

		//	Debug.WriteLine(model.ExpenseReport.ExpenseLines[0].Description);
		//	Debug.WriteLine(model.ExpenseReport.ExpenseLines[0].Type);

		//	return Redirect("/ExpenseReport/Index");
		//}

		//public ActionResult VUUE() {
		//    dd d = new dd();
		//    return View(d);
		//}

		//public ActionResult TEST(dd d) {
		//    return View(d);
		//}
	}
}