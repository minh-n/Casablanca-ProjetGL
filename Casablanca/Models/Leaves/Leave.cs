using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Casablanca.Models;

namespace Casablanca.Models.Leaves {
    public enum LeaveStatus {
        APPROVED,
        PENDING_APPROVAL_1,
		PENDING_APPROVAL_2,
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

        public string Description { get; set; }
		public LeaveStatus Status { get; set; }
		public string Color { get; set; }

		public LeaveType Type { get; set; }
		public Processing Treatment { get; set; }

		public virtual Collaborator Collaborator { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public bool StartMorningOrAfternoon { get; set; } //TODO: true for leave starting on Morning, false for on Afternoon
		public bool EndMorningOrAfternoon { get; set; } //TODO: true for leave ending on Morning, false for on Afternoon

		public Leave(LeaveStatus status, LeaveType type, Collaborator collaborator, DateTime startDate, DateTime endDate)
		{

			Description = collaborator.FirstName + " " + collaborator.LastName + " (" + collaborator.Service.Name + ")"; //generer un nom du type "NomPrenom (Service) - nbDemiJournées"
			Status = status;
			Type = type;
			Collaborator = collaborator;
			StartDate = startDate;
			EndDate = endDate;

			Treatment = HelperModel.ComputeTreatmentLeave(Collaborator);

			if (status == LeaveStatus.APPROVED)
			{
				this.Color = "#256cbf";
			}
			else if (status == LeaveStatus.PENDING_APPROVAL_1)
			{
				this.Color = "#c69b00";
			}
			else if (status == LeaveStatus.PENDING_APPROVAL_2)
			{
				this.Color = "#c68b90";
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