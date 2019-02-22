using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Casablanca.Models.Leaves;

namespace Casablanca.Models.ViewModel
{
	public class CalendarVM
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Desc { get; set; }
		public string Start_Date { get; set; }
		public string End_Date { get; set; }
		public string Color { get; set; }
		public char Group { get; set; }


		public CalendarVM(string title, string desc, string start_Date, string end_Date, char gr)
		{
			Title = title;
			Desc = desc;
			Start_Date = start_Date;
			End_Date = end_Date;
			Group = gr;
		}

		public CalendarVM(Leave leave, char gr)
		{
			Title =  leave.Collaborator.LastName; // + " ("+  leave.Collaborator.Service.Name +  ") - " + leave.Type.ToString() Name (Service) - LeaveType
			Desc = "Du " + leave.StartDate.ToString("dd/MM/yyyy") + " au " + leave.EndDate.ToString("dd/MM/yyyy") +
				"<br>Journées : " + leave.ComputeLengthLeave() +
				"<br>(" + leave.Description + ")";

			Start_Date = leave.StartDate.ToString("yyyy-MM-dd");

			if (leave.EndMorningOrAfternoon.Contains("Après-midi"))
			{		
				End_Date = leave.EndDate.AddDays(1).ToString("yyyy-MM-dd");
			}
			else
			{
				End_Date = leave.EndDate.ToString("yyyy-MM-dd");
			}

			Group = gr;
			Color = leave.Color;
		}
	}
}
