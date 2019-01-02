﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ExpenseReport {
    public enum LineType {
        APPROVED,
        PENDING_APPROVAL,
        REFUSED
    }

    public class ExpenseLine {
        public int Id { get; set; }
        public string Mission { get; set; } // TODO : Mission
        public LineType Type { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public DateTime Date { get; set; }
        public string Justificatory { get; set; }

        public ExpenseLine() {

        }

        public ExpenseLine(string mission, LineType type, string description, float cost, DateTime date, string justificatory) {
            this.Mission = mission;
            this.Type = type;
            this.Description = description;
            this.Cost = cost;
            this.Date = date;
            this.Justificatory = justificatory;
        }
    }
}