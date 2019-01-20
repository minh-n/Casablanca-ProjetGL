using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Casablanca.Models.ExpenseReports;
using Casablanca.Models.Leaves;

namespace Casablanca.Models
{
	/*
	 * ------------------------------------------------------------
	 * Helper methods: check if a Coll is in a certain category----
	 * depending on his service------------------------------------
	 * ------------------------------------------------------------
	 */

	public class HelperModel
	{
		public static string ToString(LeaveType type)
		{
			switch(type)
			{
				case LeaveType.OTHER: return "Autre";
				case LeaveType.PAID: return "Congé payé";
				case LeaveType.RTT: return "RTT";
			}
			return "Debug: TypeCongé";
		}

		public static string ToString(LeaveStatus status)
		{
			switch (status)
			{
				case LeaveStatus.APPROVED: return "Approuvée";
				case LeaveStatus.PENDING_APPROVAL_1: return "Traitement CDS";
				case LeaveStatus.PENDING_APPROVAL_2: return "Traitement RH";
				case LeaveStatus.REFUSED: return "Refusée";
			}
			return "Debug: StatutCongé";
		}

		public static string ToString(MissionStatus status)
		{
			switch (status)
			{
				case MissionStatus.COMPLETED: return "Terminée";
				case MissionStatus.IN_PROGRESS: return "En cours";
				case MissionStatus.CANCELED: return "Annulée";
				case MissionStatus.PLANNED: return "Planifiée";
			}
			return "Debug: StatutCongé";
		}

		public static string ToString(ExpenseReportStatus status)
		{
			switch (status)
			{
				case ExpenseReportStatus.UNSENT: return "Non envoyée";
				case ExpenseReportStatus.REFUSED: return "Refusée";
				case ExpenseReportStatus.APPROVED: return "Approuvée";
				case ExpenseReportStatus.PENDING_APPROVAL_1: return "Validation chef de service en cours ";
				case ExpenseReportStatus.PENDING_APPROVAL_2: return "Validation compta en cours ";
			}
			return "Debug: StatutER";
		}


		//public enum Treatment
		//{
		//	NOT_TREATED,
		//	CDS,
		//	COMPTA
		//}
		//true if the coll is in a management role (RH, Compta or Chief)
		public static bool CheckManagement(Collaborator coll)
		{
			return (CheckCompta (coll) || CheckRH(coll) || CheckDirection(coll)) || CheckCDS(coll) ? true : false;
		}

		public static bool CheckCDSCompta(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF && coll.Service.Name.Contains("Compta") ? true : false;
		}

		public static bool CheckCompta(Collaborator coll)
		{
            if(coll.Service != null)
			    return coll.Service.Name.Contains("Compta") ? true : false;
            return false;
		}

		public static bool CheckCDSRH(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF && coll.Service.Name.Contains("RH") ? true : false;
		}

		public static bool CheckRH(Collaborator coll)
		{
            if (coll.Service != null)
                return coll.Service.Name.Contains("RH") ? true : false;
            return false;
        }

		public static bool CheckPDG(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF && coll.Service.Name.Contains("Direction") ? true : false;
		}

		public static bool CheckDirection(Collaborator coll)
		{
            if (coll.Service != null)
                return coll.Service.Name.Contains("Direction") ? true : false;
            return false;
		}

		public static bool CheckCDS(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF ? true : false;
		}

		public static bool CheckAdmin(Collaborator coll)
		{
			return coll.Role == Casablanca.Models.Roles.ADMIN ? true : false;
		}

		public static string FirstCharToUpper(string input)
		{
			switch (input)
			{
				case null: throw new ArgumentNullException(nameof(input));
				case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
				default: return input.First().ToString().ToUpper() + input.Substring(1);
			}
		}

	}
}