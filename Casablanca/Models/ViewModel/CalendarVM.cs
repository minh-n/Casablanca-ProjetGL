using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Casablanca.Models.ViewModel
{
    public class PublicHoliday
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }

		public PublicHoliday(string title, string desc, string start_Date, string end_Date)
		{
			Title = title;
			Desc = desc;
			Start_Date = start_Date;
			End_Date = end_Date;
		}
	}



}
