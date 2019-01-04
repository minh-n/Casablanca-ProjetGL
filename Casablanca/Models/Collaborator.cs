using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models {

	public enum Roles
	{
		USER,
		CHIEF,
		ADMIN
	}


	public class Collaborator {
        [Key]
        public int Id { get; set;  }

		[Required(ErrorMessage = "Le champ nom de compte doit être rempli.")]
		[Display(Name ="Nom de compte")]
        public virtual string Login { get; set; }

		[Required(ErrorMessage = "Le champ mot de passe doit être rempli.")]
		[Display(Name = "Mot de passe")]
		public virtual string Password { get; set; }
	
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Service Service { get; set; }
        public virtual List<Mission> Missions { get; set; }
		public Roles Role { get; set; }
		
        // TODO : Autres attributs ? Jour de congés restants par exemple

        public Collaborator(string firstName, string lastName, Mission mission) {
			this.Login = "default";
			this.Password = "default";
			this.FirstName = firstName;
            this.LastName = lastName;
            this.Service = null;
            this.Missions = new List<Mission>();
            this.Missions.Add(mission);
        }

        public Collaborator(string firstName, string lastName) {
			this.Login = "default";
			this.Password = "default";
			this.FirstName = firstName;
            this.LastName = lastName;
			this.Service = null;
			this.Missions = new List<Mission>();
        }


		public Collaborator(string firstName, string lastName, string log, string pass)
		{
			this.Login = log;
			this.Password = pass;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.Service = null;
			this.Missions = new List<Mission>();
		}

		public Collaborator() {
			this.Login = "";
			this.Password = "";
			this.FirstName = "";
			this.LastName = "";
			this.Service = null;
			this.Missions = null;
		}
    }
}