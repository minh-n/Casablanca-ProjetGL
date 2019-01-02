using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class Collaborator
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Service { get; set; }
        public List<Mission> MissionsList { get; set; }

        // TODO : Autres attributs ? Jour de congés restants par exemple

        public Collaborator(string firstName, string lastName, string service, Mission mission)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = service;
            this.MissionsList.Add(mission);
        }


        public Collaborator(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = "";
        }

        public Collaborator()
        {

        }
    }
}