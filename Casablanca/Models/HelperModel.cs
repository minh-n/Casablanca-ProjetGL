using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
	/*
	 * ------------------------------------------------------------
	 * Helper methods: check if a Coll is in a certain category----
	 * depending on his service------------------------------------
	 * ------------------------------------------------------------
	 */

	public class HelperModel
	{
		//true if the coll is in a management role (RH, Compta or Chief)
		public static bool CheckManagement(Collaborator coll)
		{
			return (CheckCompta (coll) || CheckRH(coll) || CheckDirection(coll)) || CheckCDS(coll) ? true : false;
		}

		public static bool CheckCDSCompta(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF && coll.Service.ServiceName.Contains("compta") ? true : false;
		}

		public static bool CheckCompta(Collaborator coll)
		{
            if(coll.Service != null)
			    return coll.Service.ServiceName.Contains("Compta") ? true : false;
            return false;
		}

		public static bool CheckCDSRH(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF && coll.Service.ServiceName.Contains("RH") ? true : false;
		}

		public static bool CheckRH(Collaborator coll)
		{
            if (coll.Service != null)
                return coll.Service.ServiceName.Contains("RH") ? true : false;
            return false;
        }

		public static bool CheckPDG(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF && coll.Service.ServiceName.Contains("Direction") ? true : false;
		}

		public static bool CheckDirection(Collaborator coll)
		{
            if (coll.Service != null)
                return coll.Service.ServiceName.Contains("Direction") ? true : false;
            return false;
		}

		public static bool CheckCDS(Collaborator coll)
		{
			return coll.Role == Roles.CHIEF ? true : false;
		}

		public static bool CheckAdmin(Collaborator coll)
		{
			return coll.Role == Casablanca.Models.Roles.ADMIN ? true : false;
		}





	}
}