using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models {

    public class Service {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Collaborator> CollList { get; set; }
        public virtual Collaborator Chief { get; set; }

        public Service(string name) {
            Name = name;
            CollList = new List<Collaborator>();
            Chief = null;
        }


        public Service() {
        }


		public int GetChiefFromService()
		{

			foreach(Collaborator coll in CollList)
			{
				if (coll.Role == Roles.CHIEF)
				{
					return coll.Id;
				}
			}

			return -1;
			
		}



	}
}