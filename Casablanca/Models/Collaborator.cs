using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Casablanca.Models.Database;
using Casablanca.Models.ExpenseReports;

namespace Casablanca.Models {

    public enum Roles {
        USER,
        CHIEF,
        ADMIN
    }

    public class Collaborator {

		#region Attributs

		[Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Le champ nom de compte doit être rempli.")]
        [Display(Name = "Nom de compte")]
        public virtual string Login { get; set; }

        [Required(ErrorMessage = "Le champ mot de passe doit être rempli.")]
        [Display(Name = "Mot de passe")]
        public virtual string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Service Service { get; set; }
        public virtual List<Mission> Missions { get; set; }

        public virtual List<ExpenseReport> ExpenseReports { get; set; }

        public float Balance { get; set; }

		public Roles Role { get; set; }

		// TODO : Autres attributs ? Jour de congés restants par exemple
		public int NbRTT { get; set; }
		public int NbPaid { get; set; }

        public HashSet<string> ConnectionIds { get; set; }

        #endregion

        #region Constructors 

        public Collaborator(string firstName, string lastName, Mission mission) {
            this.Login = "default";
            this.Password = "default";
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = null;
            this.Missions = new List<Mission>();
            this.Missions.Add(mission);
            this.ExpenseReports = new List<ExpenseReport>();
            this.Balance = 0;
			this.Role = Roles.USER;
			this.NbPaid = 0;
			this.NbRTT = 0;
		}

		public Collaborator(string firstName, string lastName, Mission mission, int paid, int rtt)
		{
			this.Login = "default";
			this.Password = "default";
			this.FirstName = firstName;
			this.LastName = lastName;
			this.Service = null;
			this.Missions = new List<Mission>();
			this.Missions.Add(mission);
			this.ExpenseReports = new List<ExpenseReport>();
            this.Balance = 0;
			this.Role = Roles.USER;
			this.NbPaid = paid;
			this.NbRTT = rtt;
		}

		public Collaborator(string firstName, string lastName) {
            this.Login = "default";
            this.Password = "default";
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = null;
            this.Missions = new List<Mission>();
            this.ExpenseReports = new List<ExpenseReport>();
            this.Balance = 0;
			this.Role = Roles.USER;
			this.NbPaid = 0;
			this.NbRTT = 0;
		}

        public Collaborator(string firstName, string lastName, string log, string pass) {
            this.Login = log;
            this.Password = pass;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = null;
            this.Missions = new List<Mission>();
            this.ExpenseReports = new List<ExpenseReport>();
            this.Balance = 0;
			this.Role = Roles.USER;
			this.NbPaid = 0;
			this.NbRTT = 0;
		}

        public Collaborator() {
            this.Login = "";
            this.Password = "";
            this.FirstName = "";
            this.LastName = "";
            this.Service = null;
            this.Missions = null;
            this.ExpenseReports = null;
            this.Balance = 0;
			this.Role = Roles.USER;
			this.NbPaid = 0;
			this.NbRTT = 0;
		}
		#endregion
	}
}