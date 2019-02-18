using Casablanca.Models.ExpenseReports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Models.ViewModel {
    public class AddExpenseLineVM {

        public ExpenseReport ExpenseReport { get; set; }

		public IEnumerable<SelectListItem> CollaboratorMissions { get; set; }

		public string SubmitValue { get; set; }

        //public string file { get; set; } //by Yao

        public AddExpenseLineVM(ExpenseReport expenseReport, int selectedMission, IEnumerable<SelectListItem> collaboratorMissions)
		{
			ExpenseReport = expenseReport;
			CollaboratorMissions = collaboratorMissions;
			SubmitValue = "";
		}

		public AddExpenseLineVM()
		{
		}


        //public List<Mission> CollaboratorMissions { get; set; }

        //public AddExpenseLineVM(ExpenseReport expenseReport, List<Mission> collaboratorMissions)
        //{
        //	ExpenseReport = expenseReport;
        //	SelectedMission = "";
        //	CollaboratorMissions = collaboratorMissions;
        //}

        //public AddExpenseLineVM()
        //{
        //	ExpenseReport = null;
        //	CollaboratorMissions = new List<Mission>();
        //}

        //by Yao
        /*
        public class JustificatoryUploadModel
        {
            [DataType(DataType.Upload)]
            [Display(Name = "Upload Justificatory")]
            [Required(ErrorMessage = "Please choose file to upload.")]
            public string file { get; set; }
        }
        */

    }


}