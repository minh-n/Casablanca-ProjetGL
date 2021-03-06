﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Casablanca.Models.ExpenseReports;
using Casablanca.Models.Leaves;
using Casablanca.Models.Database;
using System.Web.Mvc;

namespace Casablanca.Models
{
	/*
	 * ------------------------------------------------------------
	 * Helper methods: check if a Coll is in a certain category----
	 * depending on his service------------------------------------
	 * ------------------------------------------------------------
	 */


	public enum HelperProcessing
	{
		CLASSIC,
		COMPTA,
		FINANCIAL_DIRECTOR,
		CEO,
		DHR,
		HR
	}


	public class HelperModel
	{


		public static HelperProcessing ComputeTreatmentER(Collaborator coll)
		{
			Service s = coll.Service;
			Roles role = coll.Role;

			switch (role)
			{
				case Roles.USER:
					if (s.Name.Contains("Compta"))
					{
						// Coll compta => double val DF
						return HelperProcessing.FINANCIAL_DIRECTOR;
					}
					else
					{
						// Cas classique
						return HelperProcessing.CLASSIC;
					}
				case Roles.CHIEF:
					if (s.Name.Contains("Compta"))
					{
						// CDS compta => double val PDG
						return HelperProcessing.CEO;
					}
					else if (s.Name.Contains("RH"))
					{
						// CDS RH => double val compta
						return HelperProcessing.COMPTA;
					}
					else if (s.Name.Contains("Direction"))
					{
						// PDG => double val DF
						return HelperProcessing.FINANCIAL_DIRECTOR;
					}
					else
					{
						return HelperProcessing.COMPTA;
					}
				default:
					return HelperProcessing.CLASSIC;
			}
		}

		public static HelperProcessing ComputeTreatmentLeave(Collaborator coll)
		{
			Service s = coll.Service;
			Roles role = coll.Role;


			switch (role)
			{
				case Roles.USER:
					if(CheckRH(coll))
					{
						// Coll RH => double val DHR
						return HelperProcessing.DHR;
					}
					else
					{
						// Cas classique
						return HelperProcessing.CLASSIC;
					}
				case Roles.CHIEF:
					if (s.Name.Contains("Compta"))
					{
						// CDS compta => double val RH
						return HelperProcessing.HR;
					}
					else if (s.Name.Contains("RH"))
					{
						// CDS RH => double val PDG
						return HelperProcessing.CEO;
					}
					else if (s.Name.Contains("Direction"))
					{
						// PDG => double val DRH
						return HelperProcessing.DHR;
					}
					else
					{
						return HelperProcessing.HR;
					}
				default:
					return HelperProcessing.CLASSIC;
			}
		}

		public static int GetNumberLeave(Collaborator coll, LeaveStatus status, LeaveType type)
		{
			Dal dal = new Dal();
			int number = 0;

			foreach(Leave l in dal.GetLeaves())
			{
				if((l.Collaborator.Id == coll.Id)  && CheckIfDateIsCurrentYear(l.StartDate))
				{
					if((l.Status == status) && l.Type == type)
					{
						number += l.ComputeLengthLeave();
					}
				}
			}
			return number;
		}

		public static int GetNumberLeave(Collaborator coll, LeaveStatus status1, LeaveStatus status2, LeaveType type)
		{
			Dal dal = new Dal();
			int number = 0;

			foreach (Leave l in dal.GetLeaves())
			{
				if ((l.Collaborator.Id == coll.Id) && CheckIfDateIsCurrentYear(l.StartDate))
				{
					if ((l.Status == status1 | l.Status == status2) && l.Type == type)
					{
						number += l.ComputeLengthLeave();
					}
				}
			}
			return number;
		}

		public static bool CheckIfDateIsCurrentYear(DateTime date)
		{

			if((date.Year == DateTime.Now.Year) && (date.Month < 4) && (DateTime.Now.Month < 4))
			{
				return true;
			}
			else if ((date.Year == DateTime.Now.Year - 1 ) && (date.Month >= 4) && (DateTime.Now.Month < 4))
			{
				return true;
			}
			else if ((date.Year == DateTime.Now.Year) && (date.Month > 4) && (DateTime.Now.Month > 4))
			{
				return true;
			}
			return false;

		}

		public static bool AllowCancelLeave(Leave l)
		{
			if (l.Status == LeaveStatus.APPROVED)
				return (l.StartDate - DateTime.Now).Days <= 7 ? false : true;
			else if (l.Status == LeaveStatus.CANCELED)
				return false;
			else
				return true;
		}

		public static int GetNumberERToProcess(Collaborator coll)
		{

			int number = 0;
			
			Dal dal = new Dal();
			List<ExpenseReport> AllERList = dal.GetExpenseReports();

			// for each Expense Report, check if they meet the following criterias
			// if yes, add them to the list returned to the View
			foreach (ExpenseReport e in AllERList)
			{
				if (e.Collaborator != coll) // a coll cannot validate his own ER
				{
					// If the ER needs to be treated the classic way
					if (e.Treatment == ExpenseReports.Processing.CLASSIC)
					{
						if (HelperModel.CheckCDSCompta(coll)) // CDS Compta
						{
							if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_2)
							{
								++number;
							}
							if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_1)
							{
								// in order to know if the Chief needs to see the ER, check if coll is the chief of a mission in ELs
								foreach (ExpenseLine el in e.ExpenseLines)
								{
									if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id)
									{
										++number;
										break;
									}
								}
							}
						}
						else if (HelperModel.CheckCompta(coll)) // Compta
						{
							if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_2)
								++number;
						}
						else if (HelperModel.CheckCDS(coll)) // CDS
						{
							if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_1)
							{
								// in order to know if the Chief needs to see the ER
								foreach (ExpenseLine el in e.ExpenseLines)
								{
									if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id)
									{
										++number;
										break;
									}
								}
							}
						}
					}
					else
					{ // The ER needs to be treated specifically
						if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_1)
						{
							switch (e.Treatment)
							{
								case ExpenseReports.Processing.COMPTA:
									if (HelperModel.CheckCompta(coll))
									{
										++number;
									}
									break;
                                case ExpenseReports.Processing.FINANCIAL_DIRECTOR:
									if (HelperModel.CheckCDSCompta(coll))
									{
										++number;
									}
									break;
                                case ExpenseReports.Processing.CEO:
									if (HelperModel.CheckPDG(coll))
									{
										++number;
									}
									break;
							}
						}
					}
				}
			}

			return number;
		}

		public static int GetNumberLeaveToProcess(Collaborator coll)
		{
			int number = 0;

			Dal dal = new Dal();

			List<Leave> LeaveListToBeReturnedAsModel = new List<Leave>();

			List<Leave> allLeaves = dal.GetLeaves();

			// for each Leave, check if they meet the following criterias
			// if yes, ++number;
			foreach (Leave e in allLeaves)
			{
				if (e.Collaborator != coll) // a coll cannot validate his own ER
				{
					// If the ER needs to be treated the classic way
					if (e.Treatment == HelperProcessing.CLASSIC)
					{
						if (HelperModel.CheckCDSRH(coll)) // CDS RH
						{
							if (e.Status == LeaveStatus.PENDING_APPROVAL_2)
							{
								++number;
							}
							if (e.Status == LeaveStatus.PENDING_APPROVAL_1)
							{
								if (e.Collaborator.Service.GetChiefFromService() == coll.Id)
								{
									++number;
								}
							}
						}
						else if (HelperModel.CheckRH(coll)) // RH
						{
							if (e.Status == LeaveStatus.PENDING_APPROVAL_2)
								++number;
						}
						else if (HelperModel.CheckCDS(coll)) // CDS
						{
							if (e.Status == LeaveStatus.PENDING_APPROVAL_1)
							{
								// in order to know if the Chief needs to see the leave

								if (e.Collaborator.Service.GetChiefFromService() == coll.Id)
								{
									++number;
								}

							}
						}
					}


					else
					{ // The ER needs to be treated specifically
						if (e.Status == LeaveStatus.PENDING_APPROVAL_1) // please do not put pending2 in DAL for those leaves
						{
							switch (e.Treatment)
							{
								case HelperProcessing.DHR:
									if (HelperModel.CheckCDSRH(coll)) //si le coll traiteur est un CDSRH
									{
										++number;
									}
									break;
								case HelperProcessing.HR:
									if (HelperModel.CheckRH(coll))
									{
										++number;
									}
									break;
								case HelperProcessing.CEO:
									if (HelperModel.CheckPDG(coll))
									{
										++number;
									}
									break;
							}
						}
					}
				}
			}

			return number;
		}

		public static int GetNumberNotifications(Dal dal) 
		{
            int notifUnread = 0;            
            foreach(Notification n in dal.GetNotifications(dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name)))
            {
                if (n.NotifStatus == NotificationStatus.UNREAD)
                    notifUnread++;
            }
            return notifUnread;
		}



		public static List<Collaborator> GetAllCollaboratorsFromAService(Service s)
		{

			List<Collaborator> list = new List<Collaborator>();

			Dal dal = new Dal();
			foreach(Collaborator coll in dal.GetCollaborators())
			{
				if(coll.Service.Id == s.Id)
				{
					list.Add(coll);
				}
			}

			return list;
		}


		public static List<Mission> GetAllMissionsFromAService(Collaborator coll)
		{

			List<Mission> list = new List<Mission>();

			Dal dal = new Dal();
			foreach (Mission miss in dal.GetMissions())
			{
				if (miss.ChiefId == coll.Id)
				{
					list.Add(miss);
				}
			}

			return list;
		}


		#region ToString

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

		public static string ToString(LeaveStatus status, HelperProcessing process)
		{
			if(process == HelperProcessing.CLASSIC)
			{
				switch (status)
				{
					case LeaveStatus.APPROVED: return "Approuvée";
					case LeaveStatus.PENDING_APPROVAL_1: return "Traitement CDS";
					case LeaveStatus.PENDING_APPROVAL_2: return "Traitement RH";
					case LeaveStatus.REFUSED: return "Refusée";
					case LeaveStatus.CANCELED: return "Annulée";

				}
			}
			else
			{
				if(status == LeaveStatus.APPROVED)
					return "Approuvée";
						
				else if(status == LeaveStatus.PENDING_APPROVAL_1 | status ==LeaveStatus.PENDING_APPROVAL_2)
				{
					if (process == HelperProcessing.CEO)
					{
						return "Traitement PDG";
					}
					else if (process == HelperProcessing.COMPTA)
					{
						return "Traitement Compta";
					}
					else if (process == HelperProcessing.DHR)
					{
						return "Traitement DRH";
					}
					else if (process == HelperProcessing.FINANCIAL_DIRECTOR)
					{
						return "Traitement D.Financier";
					}
					else if (process == HelperProcessing.HR)
					{
						return "Traitement RH";
					}
					else
					{
						return "Debug: Traitement spécial";
					}
				}
					
				else if(status == LeaveStatus.REFUSED)
					return "Refusée";
				else if (status == LeaveStatus.CANCELED)
					return "Annulée";
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

		public static string ToString(ExpenseReportStatus status, Processing process)
		{

			if (process == Processing.CLASSIC)
			{
				switch (status)
				{
					case ExpenseReportStatus.UNSENT: return "Non envoyée";
					case ExpenseReportStatus.REFUSED: return "Refusée";
					case ExpenseReportStatus.APPROVED: return "Approuvée";
					case ExpenseReportStatus.PENDING_APPROVAL_1: return "Validation chef de service en cours ";
					case ExpenseReportStatus.PENDING_APPROVAL_2: return "Validation compta en cours ";

				}
			}
			else
			{
				if (status == ExpenseReportStatus.APPROVED)
					return "Approuvée";

				else if (status == ExpenseReportStatus.PENDING_APPROVAL_1 | status == ExpenseReportStatus.PENDING_APPROVAL_2)
				{
					if (process == Processing.CEO)
					{
						return "Traitement PDG";
					}
					else if (process == Processing.COMPTA)
					{
						return "Traitement Compta";
					}
					else if (process == Processing.FINANCIAL_DIRECTOR)
					{
						return "Traitement D.Financier";
					}
					else
					{
						return "Debug: Traitement spécial";
					}
				}
				else
				{
					return "Non envoyée";
				}
			}

			return "Debug: StatutER";
			
			
		}


		public static string ToString(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.EXPENSE: return "Frais";
                case NotificationType.LEAVE: return "Congé";
                case NotificationType.ADVANCE: return "Avance";
                case NotificationType.INFORMATION: return "Information";
            }
            return "Debug: TypeNotification";
        }

        public static string ToString(NotificationResult type)
        {
            switch (type)
            {
                case NotificationResult.VALIDATION: return "Validation";
                case NotificationResult.REFUSAL: return "Refus";
                case NotificationResult.RECALL: return "Rappel";
                case NotificationResult.TREATMENT: return "Traitement";
            }
            return "Debug: ResultNotification";
        }

        public static string ToString(NotificationStatus type)
        {
            switch (type)
            {
                case NotificationStatus.READ: return "Lu";
                case NotificationStatus.UNREAD: return "Non lu";
            }
            return "Debug: StatusNotification";
        }

		#endregion

		#region CheckManagement..
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

		#endregion 


		public static string FirstCharToUpper(string input)
		{
			switch (input)
			{
				case null: throw new ArgumentNullException(nameof(input));
				case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
				default: return input.First().ToString().ToUpper() + input.Substring(1);
			}
		}



		public static IEnumerable<SelectListItem> GetCollaboratorsList(Mission currentMission) //most likely useless
		{
			var returnedColls = new List<SelectListItem>();

			Dal dal = new Dal();

			foreach (var coll in dal.GetCollaborators())
			{
				if (!coll.Missions.Contains(currentMission))
				{
					var collSelect = new SelectListItem { Value = coll.Id.ToString(), Text = coll.FirstName + " " + coll.LastName };
					returnedColls.Add(collSelect);
				}
			}
			return returnedColls;
		}

        public static bool IsERTreated(ExpenseReport er, Collaborator coll) {
            bool treated = true;
            if(er.Status == ExpenseReportStatus.PENDING_APPROVAL_1) {
                foreach (ExpenseLine el in er.ExpenseLines) {
                    if (el.Mission.ChiefId == coll.Id) {
                        treated = treated && (el.Treated == Treatment.CDS);
                    }
                }
            }
            else {
                treated = false;
            }

            return treated;
        }
    }
}