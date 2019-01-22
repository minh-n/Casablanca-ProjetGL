using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Casablanca.Models.ExpenseReports;

namespace Casablanca.Models.Advances {

	// See the requirements specification doc 
	//(Cahier des charges) for more info
	

    public class AdvanceReport {

        [Key]
        public int Id { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public float TotalCost { get; set; }
        public int NbLines { get; set; }
        public ExpenseReportStatus Status { get; set; }

        public virtual Collaborator Collaborator { get; set; }
        public virtual List<AdvanceLine> AdvanceLines { get; set; }

        public Processing Treatment { get; set; }

        public AdvanceReport() {
        }

        public AdvanceReport(Collaborator coll, Month month, int year) {
            this.Month = month;
            this.Year = year;
            this.TotalCost = 0;
            this.NbLines = 0;
            this.Status = ExpenseReportStatus.UNSENT;
            this.AdvanceLines = new List<AdvanceLine>();
            this.Collaborator = coll;
            ComputeTreatment();

            coll.AdvanceReports.Add(this);
        }

		public AdvanceReport(Collaborator coll, Month month, int year, ExpenseReportStatus stat)
		{
			this.Month = month;
			this.Year = year;
			this.TotalCost = 0;
			this.NbLines = 0;
			this.Status = stat;
			this.AdvanceLines = new List<AdvanceLine>();
			this.Collaborator = coll;
            ComputeTreatment();

            coll.AdvanceReports.Add(this);
		}

		public void AddLine(AdvanceLine el) {
            this.AdvanceLines.Add(el);
            this.NbLines++;
            this.TotalCost += el.Cost;
        }

        public void RemoveLine(AdvanceLine el) {
            if(this.AdvanceLines.Remove(el)) {
                this.NbLines--;
                this.TotalCost -= el.Cost;
            }
            
            if (this.AdvanceLines.Count == 0)
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
                        Debug.WriteLine("OMG");
                        Treatment = Processing.CLASSIC;
                    }
                    break;
                case Roles.CHIEF:
                    if(s.Name.Contains("Compta")) {
                        // CDS compta => double val PDG
                        Debug.WriteLine("OOOOOOOOOOH");
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