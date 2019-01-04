using Casablanca.Models.Database;
using Casablanca.Models.ExpenseReports;
using Casablanca.Models.ViewModel;


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

            ExpenseReport er = dal.GetExpenseReport(1);
            AddExpenseLineVM linesVM = new AddExpenseLineVM(er, dal.GetCollaboratorMissions(er.Collaborator.Id));
            List<ExpenseReport> reports = dal.GetExpenseReports();

            AddExpenseReportVM model = new AddExpenseReportVM(linesVM, reports);

            return View(model);
		}

		public ActionResult AddExpenseReport(int id)
		{
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            ExpenseReport er = dal.GetExpenseReport(id);

			AddExpenseLineVM model = new AddExpenseLineVM(er, dal.GetCollaboratorMissions(er.Collaborator.Id));

			return View(model);
		}
	}
}