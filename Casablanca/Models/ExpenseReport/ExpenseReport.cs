using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ExpenseReport {
    public enum ExpenseReportStatus {
        UNSENT,
        APPROVED,
        PENDING_APPROVAL_1,
        PENDING_APPROVAL_2,
        REFUSED
    }

    public class ExpenseReport {
       
        public int Id { get; set; }
        public string Month { get; set; }
        public float TotalCost { get; set; }
        public int NbLines { get; set; }
        public ExpenseReportStatus Status { get; set; } // TODO : Enum statut ?

        public virtual Collaborator Collaborator { get; set; }
        public virtual List<ExpenseLine> ExpenseLines { get; set; }

        public ExpenseReport() {

        }

        public ExpenseReport(string month) {
            this.Month = month;
            this.TotalCost = 0;
            this.NbLines = 0;
            this.Status = ExpenseReportStatus.UNSENT;
        }

        public void addLine(ExpenseLine el) {
            this.ExpenseLines.Add(el);
        }

        public void removeLine(ExpenseLine el) {
            this.ExpenseLines.Remove(el);
        }
    }
}