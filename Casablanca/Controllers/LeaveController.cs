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




namespace Casablanca.Controllers
{
	public class LeaveController : Controller
	{

		#region Index method

		/// <summary>
		/// GET: Home/Index method.
		/// </summary>
		/// <returns>Returns - index view page</returns> 
		public ActionResult Index()
		{
			// Info.
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
				List<PublicHoliday> data = new List<PublicHoliday>();
				data.Add(new PublicHoliday("salut", "description", "2019-01-03", "2019-01-08"));
				data.Add(new PublicHoliday("salut2", "descriddddption", "2019-01-03", "2019-01-08"));

				data.Add(new PublicHoliday("salut4", "descripfdgdtion", "2019-01-11", "2019-01-18"));

				data.Add(new PublicHoliday("salut6", "descriptgion", "2019-01-08", "2019-01-09"));

				data.Add(new PublicHoliday("salut5", "descrigption", "2019-02-03", "2019-06-08"));


				// Processing.
				result = this.Json(data, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);
			}

			// Return info.
			return result;
		}

		#endregion

	}
}