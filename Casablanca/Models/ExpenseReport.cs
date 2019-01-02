using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models {
    public class ExpenseReport {
        public int id { get; set; }
        public string month { get; set; }
        public float totalCost { get; set; }
        public string nbLines { get; set; }
        public string status { get; set; } // TODO : Enum statut ?

        public Collaborator collaborator { get; set; }
    }
}