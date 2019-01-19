using Casablanca.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ExpenseReports {
    public enum LineType {
        RESTAURANT,
        HOTEL,
        TAXI,
        OTHER
    }

    public enum Treatment {
        NOT_TREATED,
        CDS,
        COMPTA
    }

    public class ExpenseLine {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "mission")]
        public virtual Mission Mission { get; set; }
        [Display(Name = "validateur")]
        public virtual string ChiefValidator { get; set; }
        [Required]
        public LineType Type { get; set; }
        [Required]
        [Display(Name = "description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "montant du frais")]
        public float Cost { get; set; }
        [Required]
        [Display(Name = "date")]
        public DateTime Date { get; set; }
        [Display(Name = "justificatif")]
        public string Justificatory { get; set; }

        // Validations 
		public bool Validated { get; set; }         // Is validated by the CDS
		public Treatment Treated { get; set; }      // Has been treated by the CDS or the compta
		public bool FinalValidation { get; set; }   // Is validated by the compta

        public ExpenseLine() {
            Mission = null;
            ChiefValidator = "";
            Type = LineType.HOTEL;
            Description = "";
            Cost = 0;
            Date = new DateTime();
            Justificatory = "";
        }

		public ExpenseLine(Mission mission, LineType type, string chiefValidator, string description, float cost, DateTime date, string justificatory)
		{
			Mission = mission;
			ChiefValidator = chiefValidator;
			Type = type;
			Description = description;
			Cost = cost;
			Date = date;
			Justificatory = justificatory;
        }

		public ExpenseLine(Mission mission, LineType type, string description, float cost, DateTime date, string justificatory) {
            this.Mission = mission;
            this.Type = type;
            this.Description = description;
            this.Cost = cost;
            this.Date = date;
            this.Justificatory = justificatory;
        }

        public bool Equals(ExpenseLine other) {

            return (this.Mission == other.Mission &&
                    this.Type == other.Type &&
                    this.Description == other.Description &&
                    this.Cost == other.Cost &&
                    this.Date == other.Date &&
                    this.Justificatory == other.Justificatory);
        }

        public void ComputeValidator(Processing parent) {
            Dal dal = new Dal();

            switch (parent) {
                case Processing.CLASSIC:
                    ChiefValidator = dal.GetCollaborator(Mission.ChiefId).FirstName + " " + dal.GetCollaborator(Mission.ChiefId).LastName;
                    break;
                case Processing.COMPTA:
                    ChiefValidator = "Service Comptabilité";
                    break;
                case Processing.FINANCIAL_DIRECTOR:
                    ChiefValidator = "Directeur financier";
                    break;
                case Processing.CEO:
                    ChiefValidator = "PDG";
                    break;
            }

            //dal.SaveChanges();
        }
    }
}