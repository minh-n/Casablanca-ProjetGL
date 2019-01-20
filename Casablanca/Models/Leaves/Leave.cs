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

    public class Leave {
        [Key]
        public int Id { get; set; }

        public string EventName { get; set; }
		public LeaveStatus Status { get; set; }
		public string Color { get; set; }

		public virtual Collaborator Collaborator { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public string StartDateString { get; set; }
		public string EndDateString { get; set; }

		public Leave(LeaveStatus status, Collaborator collaborator, DateTime startDate, DateTime endDate)
		{
			
			EventName = collaborator.FirstName + " " + collaborator.LastName + "( " + collaborator.Service.Name + " )"; //generer un nom du type "NomPrenom (Service) - nbDemiJournées"
			Status = status;
		
			Collaborator = collaborator;
			StartDate = startDate;
			EndDate = endDate;

			StartDateString = StartDate.ToString("yyyy-MM-dd");
			EndDateString = EndDate.ToString("yyyy-MM-dd");

		

			if (status == LeaveStatus.APPROVED)
			{
				this.Color = "green";
			}
			else if (status == LeaveStatus.PENDING_APPROVAL)
			{
				this.Color = "orange";
			}
			else
			{
				this.Color = "red"; //TODO : mettre des couleurs pastel plus visibles
			}
		}

		public Leave()
		{
		}
	}
}