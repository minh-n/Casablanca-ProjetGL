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
		REFUSED,
		CANCELED
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

		[Display(Name = "Commencer le congé le matin ou l'après-midi ?")]
		public string StartMorningOrAfternoon { get; set; }

		[Display(Name = "Terminer le congé le matin ou l'après-midi ?")]
		public string EndMorningOrAfternoon { get; set; }

		#region Helper

		public int ComputeLengthLeave()
		{


			int weekendDays = 0;

			for (DateTime date = StartDate; date.Date <= EndDate.Date; date = date.AddDays(1))
			{
				if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
				{
					weekendDays++;
				}
			}

			weekendDays *= 2;

			int totalHalfDays = (this.EndDate - this.StartDate).Days * 2;

			if(StartMorningOrAfternoon.Contains("Après-midi"))
			{
				--totalHalfDays;
			}
			if(EndMorningOrAfternoon.Contains("Après-midi"))
			{
				++totalHalfDays;
			}
			return totalHalfDays - weekendDays;

		}
		#endregion


		#region Constructor

		public Leave(LeaveStatus status, LeaveType type, Collaborator collaborator, DateTime startDate, DateTime endDate, string Start, string End)
		{

			Description = collaborator.FirstName + " " + collaborator.LastName + " (" + collaborator.Service.Name + ")"; //generer un nom du type "NomPrenom (Service) - nbDemiJournées"
			Status = status;
			Type = type;
			Collaborator = collaborator;
			StartDate = startDate;
			EndDate = endDate;
			StartMorningOrAfternoon = Start;
			EndMorningOrAfternoon = End;

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

		#endregion

	}
}