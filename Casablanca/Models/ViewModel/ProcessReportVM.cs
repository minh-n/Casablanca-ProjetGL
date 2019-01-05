using Casablanca.Models.ExpenseReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Models.ViewModel
{
    public class ProcessReportVM 
    {
		public List<ExpenseReport> ExpenseReports { get; set; }
		public List<Mission> Missions { get; set; }
		public List<Service> Services{ get; set; }

		public ProcessReportVM(List<ExpenseReport> expenseReports, List<Mission> missions, List<Service> services)
		{
			ExpenseReports = expenseReports;
			Missions = missions;
			Services = services;
		}
	}
}