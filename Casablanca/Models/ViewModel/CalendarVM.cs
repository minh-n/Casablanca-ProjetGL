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
		public string URL { get; set; }


		public CalendarVM(string title, string desc, string start_Date, string end_Date)
		{
			Title = title;
			Desc = desc;
			Start_Date = start_Date;
			End_Date = end_Date;
		}

		public CalendarVM(Leave leave)
		{
			Title =  leave.Collaborator.LastName + " ("+  leave.Collaborator.Service.Name +  ") - " + leave.Type.ToString(); //Name (Service) - LeaveType
			Desc = leave.Description;
			Start_Date = leave.StartDate.ToString("yyyy-MM-dd");
			End_Date = leave.EndDate.ToString("yyyy-MM-dd");
			Color = leave.Color;
		}
	}
}
