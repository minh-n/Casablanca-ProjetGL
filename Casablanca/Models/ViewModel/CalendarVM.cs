﻿using System.Collections.Generic;
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

		public CalendarVM(string title, string desc, string start_Date, string end_Date)
		{
			Title = title;
			Desc = desc;
			Start_Date = start_Date;
			End_Date = end_Date;
		}

		public CalendarVM(Leave leave)
		{
			Title = "salut leave " +leave.Type.ToString(); //ou RTT etc whatever
			Desc = leave.EventName;
			Start_Date = leave.StartDate.ToString("yyyy-MM-dd");
			End_Date = leave.EndDate.ToString("yyyy-MM-dd");
			Color = leave.Color;
		}
	}
}
