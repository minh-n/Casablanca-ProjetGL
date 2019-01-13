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

		public ExpenseReportController() : this(new Dal())
		{

		}

		private ExpenseReportController(IDal dal)
		{
			this.dal = dal;
		}

		public ActionResult Index() // TODO : get collID //TODO : is that done? I'd say yes.
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

		public ActionResult UpdateExpenseReport(int id)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			// Who are we 
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			ExpenseReport er = dal.GetExpenseReport(id);

			// Check if ER exists
			if (er == null)
				return Redirect("/ExpenseReport/Index");

			// Check if ER belongs to the right coll
			if (er.Collaborator.Id != coll.Id)
				return Redirect("/ExpenseReport/Index");

			// IMPORTANT : do not remove this line v
			er.ExpenseLines.Add(new ExpenseLine());
			AddExpenseLineVM model = new AddExpenseLineVM { ExpenseReport = er, CollaboratorMissions = GetMissionsList(coll) };

			return View(model);
		}



		/*
		 * ------------------------------------------------------------
		 * Display the ER a Chief needs to process---------------------
		 * ------------------------------------------------------------
		 */
		public ActionResult ProcessCDS(int ERId = 5)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			if (!HelperModel.CheckCDS(coll))
				return Redirect("/Home/Index");

			ExpenseReport er = dal.GetExpenseReport(ERId);

			// Check if ER exists
			if (er == null)
				return Redirect("/ExpenseReport/Index");

			// Check if ER status is "pending approval 1" 
			if (er.Status != ExpenseReportStatus.PENDING_APPROVAL_1)
				return Redirect("/ExpenseReport/Index");

			// Get the EL that require validation from this user
			List<ExpenseLine> ELList = new List<ExpenseLine>();
			foreach (ExpenseLine el in er.ExpenseLines)
			{
				if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id)
				{
					ELList.Add(el);
				}
			}

			// Check if ER contains EL for this cds
			if (ELList.Count == 0)
				return Redirect("/ExpenseReport/Index");

			ProcessExpenseLineVM model = new ProcessExpenseLineVM(er, ELList);

			return View(model);
		}

		[HttpPost]
		public ActionResult ProcessCDS(ProcessExpenseLineVM model)
		{
			// TODO : traiter les lignes
			// if not all checked, get model.ExpenseReportId et reset le status

			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			if (!HelperModel.CheckCDS(coll))
				return Redirect("/Home/Index");

			ExpenseReport er = dal.GetExpenseReport(model.ExpenseReport.Id);

			// Check if ER exists
			if (er == null)
				return Redirect("/ExpenseReport/ProcessList");

			// Check if ER status is "pending approval 1" 
			if (er.Status != ExpenseReportStatus.PENDING_APPROVAL_1)
				return Redirect("/ExpenseReport/Index");


			//dddd
			bool allValidatedInProcessed = true;
			foreach (ExpenseLine el in er.ExpenseLines)
			{
				foreach (ExpenseLine processedLine in model.ExpenseLines)
				{
					allValidatedInProcessed &= processedLine.Validated;
					if (el.Id == processedLine.Id)
						el.Validated = processedLine.Validated;
				}
			}


			if (allValidatedInProcessed)
			{
				bool allValidated = true;
				// Check if all EL are validated TODO
				foreach (ExpenseLine el in er.ExpenseLines)
					allValidated &= el.Validated;
				
				if(allValidated)
					er.Status = ExpenseReportStatus.PENDING_APPROVAL_2;
		
			}
			else
			{
				er.Status = ExpenseReportStatus.REFUSED; //we refused one or several lines
			}

			




			// If so, change er.status to "pending approval 2"
			// If not, change er.status to "refused"
			// If not but all in model are validated, don't do anything
			er.Status = er.Status; // TODO
			dal.SaveChanges();

			return Redirect("/ExpenseReport/ProcessList");
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

		[HttpPost]
		public ActionResult ProcessCompta(ExpenseReport model)
		{
			// TODO
			return View();
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

			List<ExpenseReport> AllERList = dal.GetExpenseReports();
			List<ExpenseReport> ERListToBeReturnedAsModel = new List<ExpenseReport>();


			// for each Expense Report, check if they meet the following criterias
			// if yes, add them to the list returned to the View
			foreach (ExpenseReport e in AllERList)
			{
				if (e.Collaborator != coll)
				{

					if (HelperModel.CheckCompta(coll))
					{
						if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_2)
							ERListToBeReturnedAsModel.Add(e);
					}
					else if (HelperModel.CheckCDS(coll))
					{
						if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_1)
						{
							// in order to know if the Chief needs to see the ER
							List<Mission> currentERMissionsList = new List<Mission>();
							foreach (ExpenseLine el in e.ExpenseLines)
							{
								if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id)
								{
									ERListToBeReturnedAsModel.Add(e);
									break;
								}
							}
						}
					}
				}
			}

			return View(ERListToBeReturnedAsModel);
		}



		//post part of er creation
		[HttpPost]
		public ActionResult CreateExpenseReport()
		{
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
		//public ActionResult UpdateExpenseReport()
		//{
		//	if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
		//		return Redirect("/Home/Index");
		//	Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
		//	// admin cannot have ER
		//	if (HelperModel.CheckAdmin(coll))
		//		return Redirect("/Home/Index");

		//	return View("AddExpenseReport", new AddExpenseLineVM { CollaboratorMissions = GetMissionsList(coll)});
		//}


		//Post
		[HttpPost]
		public ActionResult UpdateExpenseReport(AddExpenseLineVM model)
		{

			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			// admin cannot have ER
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if (HelperModel.CheckAdmin(coll))
				return Redirect("/Home/Index");

			if (!ModelState.IsValid)
			{
				model.CollaboratorMissions = GetMissionsList(coll);
				// IMPORTANT : do not remove this line v
				model.ExpenseReport.AddLine(new ExpenseLine());

				return View(model);
			}

			// Get current ER and clear its ELs
			ExpenseReport current = dal.GetExpenseReport(model.ExpenseReport.Id);
			dal.ClearExpenseLines(current);

			// If we received ELs from the view
			if (model.ExpenseReport.ExpenseLines != null)
			{
				// For each EL in the view
				foreach (ExpenseLine el in model.ExpenseReport.ExpenseLines)
				{
					// Create a new line from view fields and add it to the current ER
					ExpenseLine newEL = new ExpenseLine(dal.GetMission(el.Mission.Id), el.Type, el.Description, el.Cost, el.Date, el.Justificatory);

					// TODO : compute validator (in ctor or here and set it)
					current.AddLine(newEL);
				}
				dal.SaveChanges();
			}

			return Redirect("/ExpenseReport/Index");
		}

		public ActionResult SendExpenseReport(int id)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			// Who are we 
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			ExpenseReport er = dal.GetExpenseReport(id);

			// Check if ER exists
			if (er == null)
				return Redirect("/ExpenseReport/Index");

			// Check if ER belongs to the right coll
			if (er.Collaborator.Id != coll.Id)
				return Redirect("/ExpenseReport/Index");

			// Change the status of the ER
			er.Status = ExpenseReportStatus.PENDING_APPROVAL_1;
			dal.SaveChanges();

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
				var miss = new SelectListItem { Value = s.Id.ToString(), Text = s.Name };
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
	}
}