using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Casablanca.Models.ExpenseReports;

namespace Casablanca.Models.ViewModel {
    public class AddExpenseReportVM {
        public AddExpenseLineVM modalVM;
        public List<ExpenseReport> ExpenseReports;

        public AddExpenseReportVM(AddExpenseLineVM modalVM, List<ExpenseReport> expenseReports) {
            this.modalVM = modalVM;
            ExpenseReports = expenseReports;
        }
    }
}