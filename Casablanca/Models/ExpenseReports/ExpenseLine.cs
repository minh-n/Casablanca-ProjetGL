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
        public virtual Collaborator ChiefValidator { get; set; }
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

        // TODO : line validation status (has to be validated by the same person ? validation compta only by DF ? etc.)

        public ExpenseLine() {
            Mission = null;
            ChiefValidator = null;
            Type = LineType.HOTEL;
            Description = "";
            Cost = 0;
            Date = new DateTime();
            Justificatory = "";
        }

		public ExpenseLine(Mission mission, LineType type, Collaborator chiefValidator, string description, float cost, DateTime date, string justificatory)
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

            // TODO : compute who is the chiefvalidator depending on mission ?
        }

        public bool Equals(ExpenseLine other, int id) {
            //Debug.WriteLine("[" + this.Id + " ID " + other.Id + "]"); TODO : remove
            //Debug.WriteLine("[" + this.Mission + " Mission " + other.Mission + "]");
            //Debug.WriteLine("[" + this.Description + " Desc " + other.Description + "]");
            //Debug.WriteLine("[" + this.Cost + " Cost " + other.Cost + "]");
            //Debug.WriteLine("[" + this.Date + " Date " + other.Date + "]");
            //Debug.WriteLine("[" + this.Justificatory + " Justif " + other.Justificatory + "]");

            return (id == other.Id &&
                    this.Mission == other.Mission &&
                    this.Type == other.Type &&
                    this.Description == other.Description &&
                    this.Cost == other.Cost &&
                    this.Date == other.Date &&
                    this.Justificatory == other.Justificatory);
        }
    }
}