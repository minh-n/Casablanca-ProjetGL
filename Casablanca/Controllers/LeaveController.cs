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
using Casablanca.Models;
using System.Globalization;

namespace Casablanca.Controllers
{
	public class LeaveController : Controller
	{
        private IDal dal;
        public LeaveController() : this(new Dal()) { }
        private LeaveController(IDal dal) { this.dal = dal; }
		
        public ActionResult Index()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//
			
			// fill the leave table
			List<Leave> model = new List<Leave>();

			foreach (Leave l in dal.GetLeaves())
			{
				if (l.Collaborator.Id == coll.Id)
					model.Add(l);
			}
			return View(model);
		}

		public ActionResult ViewLeave(int id = 1)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//
			
			return View(dal.GetLeave(id));
		}


		#region Calendar Helpers

		public ActionResult CalendarViewFull()
		{
			return this.View();
		}

		public ActionResult GetCalendarData()
		{
			// Initialization
			JsonResult result = new JsonResult();

			try
			{
				// Loading
				List<CalendarVM> data = ConvertLeavesIntoCalendarVM();
				
				// Processing
				result = this.Json(data, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex){Console.Write(ex);}
			return result;
		}

		public List<CalendarVM> ConvertLeavesIntoCalendarVM()
		{
			List<CalendarVM> leaves = new List<CalendarVM>();

			foreach (Leave l in dal.GetLeaves())
			{				
				leaves.Add(new CalendarVM(l));
			}
			return leaves;
		}
		#endregion


		#region Create and Edit Leave

		//[HttpPost] // Backend call of create page
		public ActionResult CreateLeave()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//

			// Create the ER
			Leave tempLeave = new Leave(LeaveStatus.PENDING_APPROVAL_1, LeaveType.RTT, coll, DateTime.Now, DateTime.Now, "Matin", "Matin");

			//string redirectString = "/Leave/UpdateLeave/?id=" + tempLeave.Id;

			return View("UpdateLeave", tempLeave);
		}




		[HttpPost] // Backend call of UpdateLeave page
		public ActionResult UpdateLeave(Leave model)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//


			// Create the leave
			if(!ModelState.IsValid)
			{
				return View(model);
			}

			// Create the temp leave
			Leave tempToDb = new Leave
			{
				Collaborator = coll,
				Type = model.Type,
				Description = model.Description,
				EndDate = model.EndDate,
				StartDate = model.StartDate,
				Status = LeaveStatus.PENDING_APPROVAL_1,
				StartMorningOrAfternoon = model.StartMorningOrAfternoon,
				EndMorningOrAfternoon = model.EndMorningOrAfternoon,
				Treatment = HelperModel.ComputeTreatmentLeave(coll)
			};

			int leaveLength = tempToDb.ComputeLengthLeave();
			//if leave length inferior to available leave (for RTT and for PAID), allow the creation
			if ((leaveLength <= coll.NbPaid) && tempToDb.Type == LeaveType.PAID)
			{
				coll.NbPaid -= leaveLength;
				dal.CreateLeave(tempToDb);
				dal.SaveChanges();
			}
			else if((leaveLength <= coll.NbRTT) && tempToDb.Type == LeaveType.RTT)
			{
				coll.NbRTT -= leaveLength;
				dal.CreateLeave(tempToDb);
				dal.SaveChanges();
			}
			else
			{
				// Error alert to the coll. The leave is not saved into the database. 
				TempData["alertMessage"] = "Vous n'avez pas assez de jour de congé. \nPar conséquent, le congé n'a pas pu être créé.";
				return View(model);
			}

			return Redirect("/Leave/Index");

		}


		#endregion


		#region Process Leaves

		public ActionResult ProcessList()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

			//not in management = cannot see
			if ((HelperModel.CheckManagement(coll) == false))
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			List<Leave> LeaveListToBeReturnedAsModel = new List<Leave>();

			List<Leave> allLeaves = dal.GetLeaves();

			// for each Leave, check if they meet the following criterias
			// if yes, add them to the list returned to the View
			foreach (Leave e in allLeaves)
			{
				if (e.Collaborator != coll) // a coll cannot validate his own ER
				{
					// If the ER needs to be treated the classic way
					if (e.Treatment == Processing.CLASSIC)
					{
						if (HelperModel.CheckCDSRH(coll)) // CDS RH
						{
							if (e.Status == LeaveStatus.PENDING_APPROVAL_2)
							{
								LeaveListToBeReturnedAsModel.Add(e);
							}
							if (e.Status == LeaveStatus.PENDING_APPROVAL_1)
							{								
								if (e.Collaborator.Service.GetChiefFromService() == coll.Id)
								{
									LeaveListToBeReturnedAsModel.Add(e);
								}
							}
						}
						else if (HelperModel.CheckRH(coll)) // RH
						{
							if (e.Status == LeaveStatus.PENDING_APPROVAL_2)
								LeaveListToBeReturnedAsModel.Add(e);
						}
						else if (HelperModel.CheckCDS(coll)) // CDS
						{
							if (e.Status == LeaveStatus.PENDING_APPROVAL_1)
							{
								// in order to know if the Chief needs to see the leave
								
								if (e.Collaborator.Service.GetChiefFromService() == coll.Id)
								{
									LeaveListToBeReturnedAsModel.Add(e);
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
								case Processing.DHR:
									if (HelperModel.CheckCDSRH(coll)) //si le coll traiteur est un CDSRH
									{
										LeaveListToBeReturnedAsModel.Add(e);
									}
									break;
								case Processing.HR:
									if (HelperModel.CheckRH(coll))
									{
										LeaveListToBeReturnedAsModel.Add(e);
									}
									break;
								case Processing.CEO:
									if (HelperModel.CheckPDG(coll))
									{
										LeaveListToBeReturnedAsModel.Add(e);
									}
									break;
							}
						}
					}
				}
			}

			return View(LeaveListToBeReturnedAsModel);
			
		}

		#endregion

		#region Accept or refuse leave

		public ActionResult AcceptLeave(int id = 1)
		{
			Leave l = dal.GetLeave(id);
			if (l.Status == LeaveStatus.PENDING_APPROVAL_1)
			{
				l.Status = LeaveStatus.PENDING_APPROVAL_2;
			}
			else
			{
				l.Status = LeaveStatus.APPROVED;
				//switch(l.Type)
				//{
				//	case LeaveType.PAID:
				//		l.Collaborator.NbPaid -= l.ComputeLengthLeave();
				//		break;
				//	case LeaveType.RTT:
				//		l.Collaborator.NbRTT -= l.ComputeLengthLeave();
				//		break;
				//}
				
			}
			dal.SaveChanges();
			return Redirect("/Leave/ProcessList");
		}

		public ActionResult RefuseLeave(int id = 1)
		{
			Leave l = dal.GetLeave(id);
			l.Status = LeaveStatus.REFUSED;
			dal.SaveChanges();
			return Redirect("/Leave/ProcessList");
		}

		public ActionResult AcceptLeaveDHR(int id = 1)
		{
			Leave l = dal.GetLeave(id);
			l.Status = LeaveStatus.APPROVED;
			//switch (l.Type)
			//{
			//	case LeaveType.PAID:
			//		l.Collaborator.NbPaid -= l.ComputeLengthLeave();
			//		break;
			//	case LeaveType.RTT:
			//		l.Collaborator.NbRTT -= l.ComputeLengthLeave();
			//		break;
			//}
			dal.SaveChanges();
			return Redirect("/Leave/ProcessList");
		}

		public ActionResult RefuseLeaveDHR(int id = 1)
		{
			Leave l = dal.GetLeave(id);
			l.Status = LeaveStatus.REFUSED;
			dal.SaveChanges();
			return Redirect("/Leave/ProcessList");
		}



		public ActionResult RemoveLeave(int id)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			//--------------------------------------------------//

			Leave currentLeave = dal.GetLeave(id);

			// Transfer the leave days into the collab's available days
			if (currentLeave.Type == LeaveType.RTT)
			{
				coll.NbRTT = currentLeave.ComputeLengthLeave();
			}
			else if(currentLeave.Type == LeaveType.PAID)
			{
				coll.NbPaid = currentLeave.ComputeLengthLeave();
			}


			// Remove the ER
			dal.GetLeave(id).Status = LeaveStatus.CANCELED;

			dal.SaveChanges();

			return Redirect("/Leave/Index");
		}



		#endregion
	}
}