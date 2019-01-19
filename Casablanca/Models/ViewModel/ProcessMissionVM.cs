using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Models.ViewModel
{
    public class ProcessMissionVM 
    {
		Mission CurrentMission { get; set; }
		public List<Collaborator> Collaborators { get; set; }

		public ProcessMissionVM(Mission currentMission, List<Collaborator> collaborators)
		{
			CurrentMission = currentMission;
			Collaborators = collaborators;
		}
	}
}