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
        public Mission Mission { get; set; }
        public LineType Type { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public DateTime Date { get; set; }
        public string Justificatory { get; set; }

        public ExpenseLine() {

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