using System;
using System.Collections.Generic;
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
        public int Id { get; set; }
        public string MissionName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MissionStatus Status { get; set; }

		public List<Collaborator> CollList { get; set; }

		public Service Service { get; set; }

        public Mission() {

        }

		public Mission(int id, string missionName, DateTime startDate, DateTime endDate, MissionStatus status, List<Collaborator> collList, Service service)
		{
			Id = id;
			MissionName = missionName;
			StartDate = startDate;
			EndDate = endDate;
			Status = status;
			CollList = collList;
			Service = service;
		}

		public Mission(string missionName, int id, DateTime startDate, DateTime endDate, MissionStatus status, Service service) : this(missionName) {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
			Service = service;
        }

		public Mission(string missionName, int id, DateTime startDate, DateTime endDate, MissionStatus status) : this(missionName)
		{
			Id = id;
			StartDate = startDate;
			EndDate = endDate;
			Status = status;
		}

		public Mission(string missionName, Service service) {
            MissionName = missionName;
            StartDate = DateTime.Now;
            Status = MissionStatus.IN_PROGRESS;
			Service = service;
        }

		public Mission(string missionName)
		{
			MissionName = missionName;
			StartDate = DateTime.Now;
			Status = MissionStatus.IN_PROGRESS;

		}


		
	}
}