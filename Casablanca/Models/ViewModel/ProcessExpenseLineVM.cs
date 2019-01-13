using Casablanca.Models.ExpenseReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ViewModel
{
	public class ProcessExpenseLineVM
	{
		public List<ExpenseLine> ExpenseLines { get; set; }
		public ExpenseReport ExpenseReport { get; set; }

		public ProcessExpenseLineVM(ExpenseReport expenseReport, List<ExpenseLine> expenseLines)
		{
			ExpenseLines = expenseLines;
			ExpenseReport = expenseReport;
		}

		public ProcessExpenseLineVM()
		{
			ExpenseLines = new List<ExpenseLine>();
			ExpenseReport = new ExpenseReport();
		}
	}
}