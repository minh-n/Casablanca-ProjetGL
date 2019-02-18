using Casablanca.Models.Database;
using Casablanca.Models.ExpenseReports;
using Casablanca.Models.ViewModel;
using Casablanca.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Casablanca.Controllers {
    public class ExpenseReportController : Controller {
        private IDal dal;

        public ExpenseReportController() : this(new Dal()) {

        }

        private ExpenseReportController(IDal dal) {
            this.dal = dal;
        }

        /* ############################################################
		 * ------------------------------------------------------------
		 * * * * * * * * * * * * C O L L  V I E W * * * * * * * * * * * 
		 * ------------------------------------------------------------
		 ############################################################*/

        // Page where a coll can see its current and past ER
        public ActionResult Index() {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            List<ExpenseReport> reports = coll.ExpenseReports;

            return View(reports);
        }

        [HttpPost] // Backend call of Index page
        public ActionResult CreateExpenseReport() {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            // Compute year
            string month = Request.Form["monthName"].ToString();
            int year = DateTime.Now.Year;
            if (month != DateTime.Now.ToString("MMMM") && month == new CultureInfo("en-US").DateTimeFormat.GetMonthName(12).ToUpper())
                year = DateTime.Now.Year - 1;

            // Compute month
            Enum.TryParse(month, out Month m);

            // Create the ER
            int returnedId = dal.CreateExpenseReport(coll, m, year,false);
            dal.TransferFromAdvanceToEr(returnedId);
			string redirectString = "/ExpenseReport/UpdateExpenseReport/?id=" + returnedId;

			return Redirect(redirectString);
        }

        [HttpPost] // Backend call of Index page
        public ActionResult CreateAdvance()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            // Compute year
            string month = Request.Form["monthName"].ToString();
            int year = DateTime.Now.Year;
            if (month != DateTime.Now.ToString("MMMM") && month == new CultureInfo("en-US").DateTimeFormat.GetMonthName(12).ToUpper())
                year = DateTime.Now.Year - 1;

            // Compute month
            Enum.TryParse(month, out Month m);

            // Create the ER
            int returnedId = dal.CreateAdvance(coll, m, year, true);
            string redirectString = "/ExpenseReport/UpdateExpenseReport/?id=" + returnedId;

            return Redirect(redirectString);
        }
        // Changes the status of ER to PENDING_APPROVAL (from Index page)
        public ActionResult SendExpenseReport(int id = 1) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport er = dal.GetExpenseReport(id);

            // Check if ER exists
            if (er == null)
                return Redirect("/ExpenseReport/Index");

            // Check if ER belongs to the right coll
            if (er.Collaborator.Id != coll.Id)
                return Redirect("/ExpenseReport/Index");

            if(er.ExpenseLines.Count == 0) {
                return Redirect("/ExpenseReport/Index");
            }

            // Change the status of the ER
            er.Status = ExpenseReportStatus.PENDING_APPROVAL_1;

            //send a notification
            switch (er.Collaborator.Role)
            {
                case Roles.USER:
                    if (coll.Service.Name.Contains("Compta"))
                    {
                        dal.AddNotification(new Notification(coll, dal.GetCollaborator(coll.Service.GetChiefFromService()), NotificationType.EXPENSE));
                    }
                    else
                    {
                        List<Collaborator> tmp = new List<Collaborator>();

                        foreach(ExpenseLine expenseLine in er.ExpenseLines)
                        {
                            if(!tmp.Contains(dal.GetCollaborator(expenseLine.Mission.ChiefId)))
                                tmp.Add(dal.GetCollaborator(expenseLine.Mission.ChiefId));
                        }

                        foreach(Collaborator c in tmp)
                        {
                            dal.AddNotification(new Notification(coll, c, NotificationType.EXPENSE));
                        }                        
                    }                        
                    break;
                case Roles.CHIEF:
                    if (coll.Service.Name.Contains("Compta"))
                    {
                        foreach (Collaborator c in dal.GetCollaborators())
                        {
                            if (c.Role == Roles.CHIEF && c.Service.Name.Contains("Direction"))
                            {
                                dal.AddNotification(new Notification(coll, c, NotificationType.EXPENSE));
                            }
                        }
                    }
                    else if (coll.Service.Name.Contains("Direction"))
                    {
                        foreach (Collaborator c in dal.GetCollaborators())
                        {
                            if (c.Role == Roles.CHIEF && c.Service.Name.Contains("Compta"))
                            {
                                dal.AddNotification(new Notification(coll, c, NotificationType.EXPENSE));
                            }
                        }
                    }
                    else
                    {
                        foreach (Collaborator c in dal.GetCollaborators())
                        {
                            if (c.Role != Roles.CHIEF && c.Service.Name.Contains("Compta"))
                            {
                                dal.AddNotification(new Notification(coll, c, NotificationType.EXPENSE));
                            }
                        }
                    }
                    break;
            }

            dal.SaveChanges();

            return Redirect("/ExpenseReport/Index");
        }

        // Page where a coll can modify a given ER
        public ActionResult UpdateExpenseReport(int id = 1) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport er = dal.GetExpenseReport(id);

            // Check if ER exists
            if (er == null)
                return Redirect("/ExpenseReport/Index");

            // Check if ER belongs to the right coll
            if (er.Collaborator.Id != coll.Id)
                return Redirect("/ExpenseReport/Index");

            er.ExpenseLines.Add(new ExpenseLine()); // IMPORTANT : do not remove this line
            AddExpenseLineVM model = new AddExpenseLineVM { ExpenseReport = er, CollaboratorMissions = GetMissionsList(coll) };

            return View(model);
        }

        [HttpPost] // Backend call of UpdateER page
        public ActionResult UpdateExpenseReport(AddExpenseLineVM model) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            // If validation fails
            if (!ModelState.IsValid) {
                model.CollaboratorMissions = GetMissionsList(coll); // Put the missions list back into the model
                foreach (ExpenseLine el in model.ExpenseReport.ExpenseLines) { // All dates that are not set go to Now
                    if(el.Date == new DateTime(0001,01,01)) {
                        el.Date = DateTime.Now;
                    }
                }

                model.ExpenseReport.AddLine(new ExpenseLine()); // IMPORTANT : do not remove this line 

                return View(model);
            }

            // Get current ER and clear its ELs
            ExpenseReport current = dal.GetExpenseReport(model.ExpenseReport.Id);
            List<ExpenseLine> tempList = new List<ExpenseLine>();

            // If we received ELs from the view, create a new line from view fields and add it to the current ER
            if (model.ExpenseReport.ExpenseLines != null) {
                foreach (ExpenseLine el in model.ExpenseReport.ExpenseLines) {
                    // Create a new EL with the informations from the view
                    ExpenseLine newEL = new ExpenseLine(dal.GetMission(el.Mission.Id), el.Type, el.Description, el.Cost, el.Date, el.Justificatory) {
                        Validated = false,
                        Treated = Treatment.NOT_TREATED,
                        FinalValidation = false
                    };

                    if (current.IsAdvance)
                    {
                        newEL.IsAdvance = true;
                    }


                    newEL.ComputeValidator(current.Treatment);

                    // Check if an EL exists with the same values (which means we did not modify this line)
                    foreach (ExpenseLine old in current.ExpenseLines) {
                        if (newEL.Equals(old)) {
                            newEL.Id = old.Id;
                            newEL.Validated = old.Validated;
                            newEL.Treated = old.Treated;
                            newEL.FinalValidation = old.FinalValidation;
                            newEL.IsAdvance = old.IsAdvance;
                            break;
                        }
                    }

                    tempList.Add(newEL);
                }
            }

            // Clear all previous ELs
            dal.ClearExpenseLines(current);

            // Finally, add all new ELs to the current ER
            foreach (ExpenseLine el in tempList) {
                current.AddLine(el);
            }
            dal.SaveChanges();

            return Redirect("/ExpenseReport/Index");
        }

        // Visualize an already sent ER
        public ActionResult ViewExpenseReport(int ERId = 1) {

            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport model = dal.GetExpenseReport(ERId);

            // if it is not our own ER = cannot see 
            if((!HelperModel.CheckManagement(coll)) && (!coll.ExpenseReports.Contains(model)))
                return Redirect("/Home/Index");

            return View(model);
        }


        /* ############################################################
		 * ------------------------------------------------------------
		 * * * * * * * * * * * * P R O C E S S * * * * * * * * * * * *
		 * ------------------------------------------------------------
		 ############################################################*/

        // Displays the ER list management needs to process
        public ActionResult ProcessList() {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            //not in management OR CDS = cannot see
            if (!HelperModel.CheckManagement(coll) && !HelperModel.CheckCDS(coll))
                return Redirect("/Home/Index");

            List<ExpenseReport> AllERList = dal.GetExpenseReports();
            List<ExpenseReport> ERListToBeReturnedAsModel = new List<ExpenseReport>();


            // for each Expense Report, check if they meet the following criterias
            // if yes, add them to the list returned to the View
            foreach (ExpenseReport e in AllERList) {
                if (e.Collaborator != coll) // a coll cannot validate his own ER
                {
                    // If the ER needs to be treated the classic way
                    if(e.Treatment == Processing.CLASSIC) { 
                        if (HelperModel.CheckCDSCompta(coll)) // CDS Compta
                        {
                            if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_2) {
                                ERListToBeReturnedAsModel.Add(e);
                            }
                            if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_1) {
                                // in order to know if the Chief needs to see the ER, check if coll is the chief of a mission in ELs
                                foreach (ExpenseLine el in e.ExpenseLines) {
                                    if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id) {
                                        ERListToBeReturnedAsModel.Add(e);
                                        break;
                                    }
                                }
                            }
                        }
                        else if (HelperModel.CheckCompta(coll)) // Compta
                        {
                            if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_2)
                                ERListToBeReturnedAsModel.Add(e);
                        }
                        else if (HelperModel.CheckCDS(coll)) // CDS
                        {
                            if (e.Status == ExpenseReportStatus.PENDING_APPROVAL_1) {
                                // in order to know if the Chief needs to see the ER
                                foreach (ExpenseLine el in e.ExpenseLines) {
                                    if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id) {
                                        ERListToBeReturnedAsModel.Add(e);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else { // The ER needs to be treated specifically
                        if(e.Status == ExpenseReportStatus.PENDING_APPROVAL_1) {
                            switch(e.Treatment) {
                                case Processing.COMPTA:
                                    if (HelperModel.CheckCompta(coll)) {
                                        ERListToBeReturnedAsModel.Add(e);
                                    }
                                    break;
                                case Processing.FINANCIAL_DIRECTOR:
                                    if (HelperModel.CheckCDSCompta(coll)) {
                                        ERListToBeReturnedAsModel.Add(e);
                                    }
                                    break;
                                case Processing.CEO:
                                    if(HelperModel.CheckPDG(coll)) {
                                        ERListToBeReturnedAsModel.Add(e);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            return View(ERListToBeReturnedAsModel);
        }

        // Display the ER a Chief needs to process
        public ActionResult ProcessCDS(int ERId = 5) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport er = dal.GetExpenseReport(ERId);

            if (!HelperModel.CheckCDS(coll))
                return Redirect("/Home/Index");

            // Check if ER exists
            if (er == null)
                return Redirect("/ExpenseReport/Index");

            // Check if ER status is "pending approval 1" 
            if (er.Status != ExpenseReportStatus.PENDING_APPROVAL_1)
                return Redirect("/ExpenseReport/Index");

            // Get the EL that require validation from this user
            List<ExpenseLine> ELList = new List<ExpenseLine>();
            foreach (ExpenseLine el in er.ExpenseLines) {
                if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id) {
                    ELList.Add(el);
                }
            }

            // Check if ER contains EL for this cds
            if (ELList.Count == 0)
                return Redirect("/ExpenseReport/Index");

            ProcessExpenseLineVM model = new ProcessExpenseLineVM(er, ELList);

            return View(model);
        }

        [HttpPost] // Backend call from ProcessCDS page
        public ActionResult ProcessCDS(ProcessExpenseLineVM model) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            if (!HelperModel.CheckCDS(coll))
                return Redirect("/Home/Index");

            ExpenseReport er = dal.GetExpenseReport(model.ExpenseReport.Id);

            // Check if ER exists
            if (er == null)
                return Redirect("/ExpenseReport/ProcessList");

            // Check if ER status is "pending approval 1" 
            if (er.Status != ExpenseReportStatus.PENDING_APPROVAL_1)
                return Redirect("/ExpenseReport/Index");

            // Check if we validated everything in the current processing list
            bool allValidatedInProcessed = true;
            foreach (ExpenseLine el in er.ExpenseLines) {
                foreach (ExpenseLine processedLine in model.ExpenseLines) {
                    allValidatedInProcessed &= processedLine.Validated;

                    // Tick the Validated field of the EL and update the Treated field
                    if (el.Id == processedLine.Id) {
                        el.Validated = processedLine.Validated;
                        el.Treated = Treatment.CDS;
                    }
                }
            }

            if (allValidatedInProcessed) {
                // Check if all EL in ER are validated
                bool allValidated = true;
                foreach (ExpenseLine el in er.ExpenseLines)
                    allValidated &= el.Validated;

                // If all EL are validated, switch to next status
                if (allValidated)
                {
                    er.Status = ExpenseReportStatus.PENDING_APPROVAL_2;

                    //send notifications
                    dal.AddNotification(new Notification(coll, er.Collaborator, NotificationType.EXPENSE, NotificationResult.VALIDATION, "Votre note de frais est validée par le(s) chef(s) de service concerné(s)"));

                    foreach (Collaborator c in dal.GetCollaborators())
                    {
                        if (c.Role != Roles.CHIEF && c.Service.Name.Contains("Compta"))
                        {
                            dal.AddNotification(new Notification(er.Collaborator, c, NotificationType.EXPENSE));
                        }
                    }
                }                    
            }
            else {
                er.Status = ExpenseReportStatus.REFUSED; // We refused one or several lines     

                //send a notification
                dal.AddNotification(new Notification(coll, er.Collaborator, NotificationType.EXPENSE, NotificationResult.REFUSAL));
            }

            dal.SaveChanges();

            return Redirect("/ExpenseReport/ProcessList");
        }

        // Displays the ER a comptaboy needs to process
        public ActionResult ProcessCompta(int ERId = 1) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport model = dal.GetExpenseReport(ERId);

            // Check if ER exists
            if (model == null)
                return Redirect("/ExpenseReport/ProcessList");

            if (!HelperModel.CheckCompta(coll))
                return Redirect("/Home/Index");

            return View(model);
        }

        [HttpPost] // Backend call from ProcessCompta page
        public ActionResult ProcessCompta(ExpenseReport model) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport er = dal.GetExpenseReport(model.Id);

            if (!HelperModel.CheckCompta(coll))
                return Redirect("/Home/Index");

            // Check if ER exists
            if (model == null)
                return Redirect("/ExpenseReport/ProcessList");

            // Check if ER status is "pending approval 2" 
            if (er.Status != ExpenseReportStatus.PENDING_APPROVAL_2)
                return Redirect("/ExpenseReport/ProcessList");

            // Check if all EL are validated 
            bool allValidated = true;
            foreach (ExpenseLine processedLine in model.ExpenseLines) {
                allValidated &= processedLine.FinalValidation;
                foreach (ExpenseLine el in er.ExpenseLines) {

                    // Tick the Validated field of the EL and update the Treated field
                    if (el.Id == processedLine.Id) {
                        el.FinalValidation = processedLine.FinalValidation;
                        el.Treated = Treatment.COMPTA;
                    }
                }
            }

            if (allValidated)
            {
                er.Status = ExpenseReportStatus.APPROVED;
                //send a notification
                dal.AddNotification(new Notification(coll, er.Collaborator, NotificationType.EXPENSE, NotificationResult.VALIDATION));
            }
            else
            {
                er.Status = ExpenseReportStatus.REFUSED;    // We refused one or several lines 
                //send a notification
                dal.AddNotification(new Notification(coll, er.Collaborator, NotificationType.EXPENSE, NotificationResult.REFUSAL));
            }

            dal.SaveChanges();

            return Redirect("/ExpenseReport/ProcessList");
        }

        // Displays the ER a compta coll needs to process
        public ActionResult OneStepProcess(int ERId = 1) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport model = dal.GetExpenseReport(ERId);

            // Check if ER exists
            if (model == null)
                return Redirect("/ExpenseReport/ProcessList");

            // Check if coll has the right attributions
            switch (model.Treatment) {
                case Processing.COMPTA:
                    if (!HelperModel.CheckCompta(coll)) {
                        return Redirect("/ExpenseReport/ProcessList");
                    }
                    break;
                case Processing.FINANCIAL_DIRECTOR:
                    if (!HelperModel.CheckCDSCompta(coll)) {
                        return Redirect("/ExpenseReport/ProcessList");
                    }
                    break;
                case Processing.CEO:
                    if (!HelperModel.CheckPDG(coll)) {
                        return Redirect("/ExpenseReport/ProcessList");
                    }
                    break;
            }

            // Check if ER status is "pending approval 1" 
            if (model.Status != ExpenseReportStatus.PENDING_APPROVAL_1)
                return Redirect("/ExpenseReport/ProcessList");

            return View(model);
        }

        [HttpPost] // // Backend call from OneStepProcess page
        public ActionResult OneStepProcess(ExpenseReport model) {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            ExpenseReport er = dal.GetExpenseReport(model.Id);

            // Check if coll has the right attributions
            switch (model.Treatment) {
                case Processing.COMPTA:
                    if (!HelperModel.CheckCompta(coll)) {
                        return Redirect("/ExpenseReport/ProcessList");
                    }
                    break;
                case Processing.FINANCIAL_DIRECTOR:
                    if (!HelperModel.CheckCDSCompta(coll)) {
                        return Redirect("/ExpenseReport/ProcessList");
                    }
                    break;
                case Processing.CEO:
                    if (!HelperModel.CheckPDG(coll)) {
                        return Redirect("/ExpenseReport/ProcessList");
                    }
                    break;
            }

            // Check if ER exists
            if (model == null)
                return Redirect("/ExpenseReport/ProcessList");

            // Check if ER status is "pending approval 1" 
            if (er.Status != ExpenseReportStatus.PENDING_APPROVAL_1)
                return Redirect("/ExpenseReport/ProcessList");

            // Check if all EL are validated 
            bool allValidated = true;
            foreach (ExpenseLine processedLine in model.ExpenseLines) {
                allValidated &= processedLine.FinalValidation;
                foreach (ExpenseLine el in er.ExpenseLines) {

                    // Tick the Validated field of the EL and update the Treated field
                    if (el.Id == processedLine.Id) {
                        el.FinalValidation = processedLine.FinalValidation;
                        el.Treated = Treatment.COMPTA;
                    }
                }
            }

            if (allValidated)
            {
                er.Status = ExpenseReportStatus.APPROVED;
                //send a notification
                dal.AddNotification(new Notification(coll, er.Collaborator, NotificationType.EXPENSE, NotificationResult.VALIDATION));
            }                
            else
            {
                er.Status = ExpenseReportStatus.REFUSED;    // We refused one or several lines 
                //send a notification
                dal.AddNotification(new Notification(coll, er.Collaborator, NotificationType.EXPENSE, NotificationResult.REFUSAL));
            }                 

            dal.SaveChanges();

            return Redirect("/ExpenseReport/ProcessList");
        }

        /* ############################################################
		 * ------------------------------------------------------------
		 * * * * * * * * * * * * H E L P E R S * * * * * * * * * * * *
		 * ------------------------------------------------------------
		 ############################################################*/

        private static IEnumerable<SelectListItem> GetMissionsList(Collaborator coll) {
            var missions = new List<SelectListItem>();
            foreach (var collMission in coll.Missions.ToList()) {
				if(collMission.Status != MissionStatus.CANCELED)
				{
					var miss = new SelectListItem { Value = collMission.Id.ToString(), Text = collMission.Name };
					missions.Add(miss);
				}
            }
            return missions;
        }



		//HISTORY



		// Displays the ER list management already processed
		public ActionResult HistoryList()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);

            //not in management OR isRH = cannot see
            if (!HelperModel.CheckManagement(coll) && !HelperModel.CheckRH(coll))
				return Redirect("/Home/Index");

			List<ExpenseReport> AllERList = dal.GetExpenseReports();
			List<ExpenseReport> ERListToBeReturnedAsModel = new List<ExpenseReport>();

			// for each Expense Report, check if they meet the following criterias
			// if yes, add them to the list returned to the View
			foreach (ExpenseReport e in AllERList)
			{
				if (e.Collaborator != coll) // a coll cannot validate his own ER
				{
					// If the ER needs to be treated the classic way
					if (e.Treatment == Processing.CLASSIC)
					{
						if (HelperModel.CheckCompta(coll)) // CDS Compta et Compta
						{
							if ((e.Status == ExpenseReportStatus.APPROVED) | (e.Status == ExpenseReportStatus.REFUSED))
							{
								ERListToBeReturnedAsModel.Add(e);
							}
						}
						else if (HelperModel.CheckCDS(coll)) // CDS
						{
							if ((e.Status == ExpenseReportStatus.APPROVED) | (e.Status == ExpenseReportStatus.REFUSED))
							{
								// in order to know if the Chief needs to see the ER
								foreach (ExpenseLine el in e.ExpenseLines)
								{
									if (dal.GetCollaborator(el.Mission.ChiefId).Id == coll.Id)
									{
										ERListToBeReturnedAsModel.Add(e);
										break;
									}
								}
							}
						}
					}
					else
					{ // The ER needs to be treated specifically
						if ((e.Status == ExpenseReportStatus.APPROVED) | (e.Status == ExpenseReportStatus.REFUSED))
						{
							switch (e.Treatment)
							{
								case Processing.COMPTA:
									if (HelperModel.CheckCompta(coll))
									{
										ERListToBeReturnedAsModel.Add(e);
									}
									break;
								case Processing.FINANCIAL_DIRECTOR:
									if (HelperModel.CheckCDSCompta(coll))
									{
										ERListToBeReturnedAsModel.Add(e);
									}
									break;
								case Processing.CEO:
									if (HelperModel.CheckPDG(coll))
									{
										ERListToBeReturnedAsModel.Add(e);
									}
									break;
							}
						}
					}
				}
			}

			return View(ERListToBeReturnedAsModel);
		}






	}

    public class JustificatoryUploadController : Controller
    {
        // GET: JustificatoryUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadJustificatory(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (file != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/APP_Data/UploadedFiles"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                    }
                    ViewBag.FileStatus = "Justificatory uploaded successfully.";
                }
                catch (Exception)
                {

                    ViewBag.FileStatus = "Error while Justificatory uploading.";
                }

            }
            return View("Index");
        }
    }
}