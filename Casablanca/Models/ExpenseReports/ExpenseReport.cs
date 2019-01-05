using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ExpenseReports {
    public enum ExpenseReportStatus {
        UNSENT,
        APPROVED,
        PENDING_APPROVAL_1,
        PENDING_APPROVAL_2,
        REFUSED
    }

    public enum Month {
        JANUARY, FEBRUARY, MARCH, APRIL, MAY, JUNE, JULY, AUGUST, SEPTEMBER, OCTOBER, NOVEMBER, DECEMBER 
    }

    public class ExpenseReport {
       
        [Key]
        public int Id { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public float TotalCost { get; set; }
        public int NbLines { get; set; }
        public ExpenseReportStatus Status { get; set; } // TODO : Enum statut ?

        public virtual Collaborator Collaborator { get; set; }
        public virtual List<ExpenseLine> ExpenseLines { get; set; }

        public ExpenseReport() {

        }

        public ExpenseReport(Collaborator coll, Month month, int year) {
            this.Month = month;
            this.Year = year;
            this.TotalCost = 0;
            this.NbLines = 0;
            this.Status = ExpenseReportStatus.UNSENT;
            this.ExpenseLines = new List<ExpenseLine>();
            this.Collaborator = coll;

            coll.ExpenseReports.Add(this);
        }

        public void AddLine(ExpenseLine el) {
            this.ExpenseLines.Add(el);
            this.NbLines++;
            this.TotalCost += el.Cost;
        }

        public void RemoveLine(ExpenseLine el) {
            if(this.ExpenseLines.Remove(el)) {
                this.NbLines--;
                this.TotalCost -= el.Cost;
            }
        }
    }
}