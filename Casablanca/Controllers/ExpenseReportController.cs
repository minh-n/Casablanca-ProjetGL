using Casablanca.Models.Database;
using Casablanca.Models.ExpenseReports;
using Casablanca.Models.ViewModel;
using Casablanca.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
	}
}