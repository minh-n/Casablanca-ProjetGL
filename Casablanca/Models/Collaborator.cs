using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models {

    public class Collaborator {
        [Key]
        public int Id { get; set;  }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Service { get; set; }
        public virtual List<Mission> Missions { get; set; }


        // TODO : Autres attributs ? Jour de congés restants par exemple

        public Collaborator(string firstName, string lastName, string service, Mission mission) {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = service;
            this.Missions = new List<Mission>();
            this.Missions.Add(mission);
        }

        public Collaborator(string firstName, string lastName, string service) {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = service;
            this.Missions = new List<Mission>();
        }

        public Collaborator() {

        }
    }
}