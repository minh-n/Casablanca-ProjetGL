﻿using Casablanca.Models.Database;
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

        public ActionResult Index()
        {
            List<Collaborator> model = dal.GetCollaborators();

            return View(model);
        }

		public ActionResult AddExpenseReport()
		{
			return View();
		}
	}
}