﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class Collaborator
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string service { get; set; }
        public string mission { get; set; } // TODO : Plusieurs missions par collaborateur ?

        // TODO : Autres attributs ? Jour de congés restants par exemple

        public Collaborator(string firstName, string lastName, string service, string mission)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.service = service;
            this.mission = mission;
        }


        public Collaborator(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.service = "";
            this.mission = "";
        }

        public Collaborator()
        {

        }
    }
}