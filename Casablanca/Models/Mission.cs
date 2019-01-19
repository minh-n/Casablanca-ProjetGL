using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models {

    public enum MissionStatus {
        COMPLETED,
        IN_PROGRESS,
        PLANNED,
        CANCELED
    }

    public class Mission {
        [Key]
        [Display(Name = "mission")]
        public int Id { get; set; }
        public string Name { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
		public DateTime StartDate { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
		public DateTime EndDate { get; set; }

        public MissionStatus Status { get; set; }

        public virtual List<Collaborator> CollList { get; set; }

		public virtual int ChiefId { get; set; }

		//public virtual Service Service { get; set; }
		//public virtual Collaborator ChiefValidator { get; set; }



		public Mission() {

        }

		//     public Mission(string name, DateTime startDate, DateTime endDate, MissionStatus status, List<Collaborator> collList, Collaborator chief) {
		//         Name = name;
		//         StartDate = startDate;
		//         EndDate = endDate;
		//         Status = status;
		//         CollList = collList;
		//ChiefValidator = chief;
		//     }

		//     public Mission(string name, DateTime startDate, DateTime endDate, MissionStatus status, Collaborator chief) : this(name) {
		//         StartDate = startDate;
		//         EndDate = endDate;
		//         Status = status;
		//ChiefValidator = chief;
		//     }
		public Mission(string name, DateTime startDate, DateTime endDate, MissionStatus status, int chief) : this(name)
		{
			StartDate = startDate;
			EndDate = endDate;
			Status = status;
			ChiefId = chief;
		}
		public Mission(string name, DateTime startDate, DateTime endDate, MissionStatus status) : this(name) {
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }

		public Mission(string name, DateTime startDate, MissionStatus status) : this(name)
		{
			StartDate = startDate;
			EndDate = new DateTime(1970,1,1);
			Status = status;
		}

		public Mission(string name) {
            Name = name;
            StartDate = DateTime.Now;
            Status = MissionStatus.IN_PROGRESS;
        }
    }
}