using Casablanca.Models.ExpenseReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ViewModel
{
	public class AddExpenseLineVM
	{
		public ExpenseReport ExpenseReport { get; set; }
		public List<Mission> CollaboratorMissions { get; set; }

		public AddExpenseLineVM(ExpenseReport expenseReport, List<Mission> collaboratorMissions)
		{
			ExpenseReport = expenseReport;
			CollaboratorMissions = collaboratorMissions;
		}
	}
}