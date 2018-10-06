using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class Collaborateur
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string service { get; set; }
        public string mission { get; set; } // TODO : Plusieurs missions par collaborateur ?

        // TODO : Autres attributs ? Jour de congés restants par exemple

        public Collaborateur(string firstName, string lastName, string service, string mission)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.service = service;
            this.mission = mission;
        }
    }
}