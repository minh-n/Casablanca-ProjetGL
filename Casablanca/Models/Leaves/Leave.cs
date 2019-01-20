using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using System.Diagnostics;

namespace Casablanca.Models.Leaves {
    public enum LeaveStatus {
        APPROVED,
        PENDING_APPROVAL,
        REFUSED
    }

	public enum LeaveType
	{
		RTT,
		PAID,
		OTHER
	}

	public class Leave {
        [Key]
        public int Id { get; set; }

        public string EventName { get; set; }
		public LeaveStatus Status { get; set; }
		public string Color { get; set; }

		public LeaveType Type { get; set; }

		public virtual Collaborator Collaborator { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public string StartDateString { get; set; }
		public string EndDateString { get; set; }

		public Leave(LeaveStatus status, LeaveType type, Collaborator collaborator, DateTime startDate, DateTime endDate)
		{
			
			EventName = collaborator.FirstName + " " + collaborator.LastName + " (" + collaborator.Service.Name + ")"; //generer un nom du type "NomPrenom (Service) - nbDemiJournées"
			Status = status;
			Type = type;
			Collaborator = collaborator;
			StartDate = startDate;
			EndDate = endDate;

			
			if (status == LeaveStatus.APPROVED)
			{
				this.Color = "#256cbf";
			}
			else if (status == LeaveStatus.PENDING_APPROVAL)
			{
				this.Color = "#c69b00";
			}
			else
			{
				this.Color = "#bf4425"; 
			}
		}

		public Leave()
		{
		}
	}
}