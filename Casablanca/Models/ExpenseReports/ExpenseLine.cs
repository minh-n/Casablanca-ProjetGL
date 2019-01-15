using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ExpenseReports {
    public enum LineType {
        RESTAURANT,
        HOTEL,
        TAXI,
        OTHER
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

		public bool Validated { get; set; }

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
        }
    }
}