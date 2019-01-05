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
            AddExpenseLineVM linesVM = new AddExpenseLineVM(er, dal.GetCollaboratorMissions(coll.Id));

            AddExpenseReportVM model = new AddExpenseReportVM(linesVM, reports);

            return View(model);
		}

		public ActionResult AddExpenseReport(int id = 2)
		{
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // Who are we 
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            ExpenseReport er = dal.GetExpenseReport(id);
			AddExpenseLineVM model = new AddExpenseLineVM(er, dal.GetCollaboratorMissions(coll.Id));

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
			//not in management OR is RH = cannot see
			if ((HelperModel.CheckManagement(coll) == false) || HelperModel.CheckRH(coll))
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
						ExpenseReport temp = new ExpenseReport(e.Collaborator, e.Month, e.Year);
						//todo foreach e.expenseline voir si le cds est bien notre coll
					}

					//TODO : get que les trucs en relation avec coll
				}
			}
			
			return View(model);
		}
		
	}
}