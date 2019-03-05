using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ExpenseReports {

	// See the requirements specification doc 
	//(Cahier des charges) for more info
	public enum ExpenseReportStatus {
        UNSENT,
        APPROVED,
        PENDING_APPROVAL_1,
        PENDING_APPROVAL_2,
        REFUSED
    }

    public enum Month {
        JANUARY, FEBRUARY, MARCH, APRIL, MAY, JUNE, JULY, AUGUST, SEPTEMBER, OCTOBER, NOVEMBER, DECEMBER, NONE 
    }

    public enum Processing {
        CLASSIC,
        COMPTA,
        FINANCIAL_DIRECTOR,
        CEO
    }

    public class ExpenseReport {

        [Key]
        public int Id { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public float TotalCost { get; set; }
        public int NbLines { get; set; }
        public ExpenseReportStatus Status { get; set; }

        public virtual Collaborator Collaborator { get; set; }
        public virtual List<ExpenseLine> ExpenseLines { get; set; }

        //Advance
        public bool IsAdvance { get; set; }

        public Processing Treatment { get; set; }

        public ExpenseReport() {
        }

        public ExpenseReport(Collaborator coll, Month month, int year, bool isAdvance) {
            this.Month = month;
            this.Year = year;
            this.TotalCost = 0;
            this.NbLines = 0;
            this.Status = ExpenseReportStatus.UNSENT;
            this.ExpenseLines = new List<ExpenseLine>();
            this.IsAdvance = isAdvance;
            this.Collaborator = coll;
            ComputeTreatment();

            coll.ExpenseReports.Add(this);
        }

		public ExpenseReport(Collaborator coll, Month month, int year, ExpenseReportStatus stat, bool isAdvance)
		{
			this.Month = month;
			this.Year = year;
			this.TotalCost = 0;
			this.NbLines = 0;
			this.Status = stat;
			this.ExpenseLines = new List<ExpenseLine>();
            this.IsAdvance = isAdvance;
			this.Collaborator = coll;
            ComputeTreatment();

            coll.ExpenseReports.Add(this);
		}

        //Case for advances
        /*public ExpenseReport(Collaborator coll, Month month, int year)
        {
            this.Month = month;
            this.Year = year;
            this.TotalCost = 0;
            this.NbLines = 0;
            this.Status = ExpenseReportStatus.UNSENT;
            this.ExpenseLines = new List<ExpenseLine>();
            this.IsAdvance = true;
            this.Collaborator = coll;
            ComputeTreatment();

            coll.ExpenseReports.Add(this);

        }*/
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
            
            if (this.ExpenseLines.Count == 0)
                this.TotalCost = 0.0f;
        }

        public void ComputeTreatment() {
            Service s = Collaborator.Service;
            Roles role = Collaborator.Role;

            switch(role) {
                case Roles.USER:
                    if(s.Name.Contains("Compta")) {
                        // Coll compta => double val DF
                        Treatment = Processing.FINANCIAL_DIRECTOR;
                    }
                    else {
                        // Cas classique
                        Treatment = Processing.CLASSIC;
                    }
                    break;
                case Roles.CHIEF:
                    if(s.Name.Contains("Compta")) {
                        // CDS compta => double val PDG
                        Treatment = Processing.CEO;
                    }
                    else if (s.Name.Contains("RH")) {
                        // CDS RH => double val compta
                        Treatment = Processing.COMPTA;
                    }
                    else if (s.Name.Contains("Direction")) {
                        // PDG => double val DF
                        Treatment = Processing.FINANCIAL_DIRECTOR;
                    }
                    else {
                        Treatment = Processing.COMPTA;
                    }
                    break;
            }
        }
    }
}