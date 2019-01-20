using Casablanca.Models.ViewModel;
using System;
using System.Collections.Generic;
using Casablanca.Models.Leaves;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Diagnostics;
using System.Reflection;
using Casablanca.Models.Database;

namespace Casablanca.Controllers
{
	public class LeaveController : Controller
	{
        private IDal dal;
        public LeaveController() : this(new Dal()) { }
        private LeaveController(IDal dal) { this.dal = dal; }


        #region Index method

        /// <summary>
        /// GET: Home/Index method.
        /// </summary>
        /// <returns>Returns - index view page</returns> 
        public ActionResult Index()
		{
			return this.View();
		}

		#endregion

		#region Get Calendar data method.

		/// <summary>
		/// GET: /Home/GetCalendarData
		/// </summary>
		/// <returns>Return data</returns>
		public ActionResult GetCalendarData()
		{
			// Initialization.
			JsonResult result = new JsonResult();

			try
			{
				// Loading.
				//List<PublicHoliday> data = this.LoadData();
				List<CalendarVM> data = ConvertLeavesIntoCalendarVM();
				
				// Processing.
				result = this.Json(data, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex){Console.Write(ex);}
			return result;
		}
		#endregion

		public List<CalendarVM> ConvertLeavesIntoCalendarVM()
		{
			List<CalendarVM> leaves = new List<CalendarVM>();

			foreach(Leave l in dal.GetLeaves())
			{
				//CalendarVM cal = new CalendarVM("titre vm", l.EventName, l.StartDateString, l.EndDateString);
				CalendarVM cal = new CalendarVM(l);

				leaves.Add(cal);
				Debug.WriteLine("Salut conversion. Cal = " + cal.Desc + " " + cal.Start_Date);
			}
			return leaves;
		}


	}
}