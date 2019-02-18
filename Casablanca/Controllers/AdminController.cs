using Casablanca.Models;
using Casablanca.Models.Database;
using Casablanca.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Casablanca.Controllers
{
    public class AdminController : Controller
    {
		private IDal dal;
		public AdminController() : this(new Dal()){}
		private AdminController(IDal dal){this.dal = dal;}

		// GET: Admin
		public ActionResult Index()
        {
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			//TODO check if user roleis Admin
			//Collaborator model = new Collaborator();
			return View(/*model*/);
		}

		// Get: account creation
		public ActionResult CreateAccount()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

            // Check admin privilege
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            if (!HelperModel.CheckAdmin(coll))
                return Redirect("/Home/Index");

            return View();
		}

		//POST : account register
		[HttpPost]
		public ActionResult CreateAccount(Collaborator model /*, int collId*/)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

            // Check admin privilege
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            if (!HelperModel.CheckAdmin(coll))
                return Redirect("/Home/Index");

            // Validation
            if (ModelState.IsValid && ValidationLogin(model))
			{
                dal.CreateCollaborator(model.FirstName, model.LastName, model.Login, dal.EncodeMD5(model.Password));
				return Redirect("/Admin/Index"); 
			}
            else {
                ModelState.AddModelError("", "Le champ nom de compte doit être unique !");
            }
			return View(model);
		}

		public ActionResult ServicesList()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");
			//TODO check if user roleis Admin
			List<Service> model = dal.GetServices();
			return View(model);
		}


		/*--------------------------------------------------------------------*/

		// Get: account deletion
		public ActionResult DeleteUser(int collId)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			// Check admin privilege
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if (!HelperModel.CheckAdmin(coll))
				return Redirect("/Home/Index");



			dal.RemoveCollaborator(collId);


			return Redirect("/Home/Index");
		}



		public ActionResult ChangeService(int collId)
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			// Check admin privilege
			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if (!HelperModel.CheckAdmin(coll))
				return Redirect("/Home/Index");



			//HelperModel.ChangeService(collId);

			//coll->coll

			//Demande de congés : mettre à jour le responsable de la validation de la demande 
			//(remplacer l’ancien CDS par le nouveau). Notifier le nouveau CDS qu’il possède 
			//une nouvelle demande à valider si celle-ci était en attente de validation par l’ancien CDS,
			//notifier le service RH du changement sinon.

			//il faut une fct pour faire ça. S'applique à quasiment tous les cas. Voir document "fonctionnalités décembre"



			//compta → coll simple

			//Notes de frais: les notes de frais des collaborateurs du service comptabilité étant validées
			//en seconde partie par le directeur financier, les validateurs doivent être mis à jour.
			//Les notes de frais sont désormais à valider par le service comptabilité. 



			//-------------------------------
			//coll simple → coll du service compta
			//pareil que en haut, mais dans le sens inverse

			//Notes de frais: les notes de frais des collaborateurs simple étant validées
			//en seconde partie par le service comptabilité, les validateurs doivent être mis à jour.
			//Les notes de frais sont désormais à valider par le directeur financier. 



			//----------------------
			/*
			service RH → coll simple

			Demande de congés: les notes de frais des collaborateurs du service RH étant validées
			en seconde partie par le chef du service RH, les validateurs doivent être mis à jour.
			Les notes de frais sont désormais à valider par le service RH. 

			*/






			/*
				* 
				* 
				* 
				* 
				* 
				*
			coll simple → coll du service RH
			Demande de congés : Dans le cas où c’est un collaborateur issu du services RH qui réalise une demande de congés,
			c’est le chef du service RH uniquement qui valide ou refuse cette demande
			(sa validation comptera comme une double validation).





			 coll du service RH → coll du service compta

			Notes de frais : le directeur financier devient le validateur des demandes du collaborateur.

			Demande de congés : le collaborateur revient à un cycle de double validation normal (CDS → RH).






			coll du service compta → coll du service RH

			Notes de frais : le collaborateur revient à un cycle de double validation normal (CDS → Compta).

			Demande de congés : le CDS RH devient le seul validateur de cette demande 
			(sa validation comptera comme une double validation).







			 coll → CDS d’un autre service

			Notes de frais : le nouveau collaborateur hérite des notes à traiter de l’ancien CDS. 
			Ses validateurs ne changent pas, car ils sont liés à une mission et non un service.

			Demande de congés : le nouveau collaborateur hérite des demandes de congés 
			à traiter de l’ancien CDS. Le validateur de ses propres demandes devient le service RH 
			qui compte comme une double validation.







			CDS → coll d’un autre service

			Notes de frais : le collaborateur perd les notes qu’il avait à traiter.  --> ok c'est fait
			Ses validateurs ne changent pas, car ils sont liés à une mission et non un service. 

			Demande de congés : le nouveau collaborateur perd les demandes de congés qu’il avait à traiter.
			Il revient à un cycle de validation classique (CDS → RH).






			CDS compta → CDS RH

			Notes de frais : le nouveau CDS hérite des notes à traiter de l’ancien occupant du poste. 
			Ses validateurs ne changent pas, car ils sont liés à une mission et non un service, excepté le cas où il réalise un mission dont il est le responsable. Le validateur de la note de frais qui en découle devient alors le Directeur financier (appartenant au service Direction). Il compte comme une double validation.

			Demande de congés : le nouveau collaborateur hérite des demandes de congés à traiter de
			l’ancien occupant du poste. C’est le PDG (chef du service Direction) qui validera
			ou non la demande du collaborateur (compte comme une double validation).





			CDS RH → CDS Compta

			Notes de frais : identique au CDS compta → CDS RH

			Demande de congés : identique au CDS compta → CDS RH





			PDG → CDS compta ou RH

			Notes de frais : le collaborateur hérite les notes à traiter de l’ancien occupant du poste.
			Ses premiers validateurs ne changent pas, car ils sont liés à une mission et non un service.
			S’il est CDS de la compta, le second validateur devient le nouveau PDG.

			Demande de congés : le collaborateur hérite les demandes de congés à traiter de
			l’ancien occupant du poste. S’il est CDS de la RH, la validation de ses propres 
			demandes de congés est effectuée par le PDG (compte comme double validation).





			CDS compta ou RH → PDG

			Notes de frais : le collaborateur hérite les notes à traiter de l’ancien occupant du poste.
			Ses premiers validateurs ne changent pas, car ils sont liés à une mission et non un service.
			Le second validateur devient le nouveau CDS Compta.

			Demande de congés : le collaborateur hérite les demandes de congés à traiter de
			l’ancien occupant du poste. La validation de ses propres demandes de congés est effectuée par
			le service RH (compte comme double validation).



			14. Changement de service entre coll → PDG

			Notes de frais : identique au 13.

			Demande de congés : identique au 13.





			PDG → coll

			Notes de frais : le collaborateur revient à un cycle de validation normal.
			Demande de congé : le collaborateur revient à un cycle de validation normal.



			14. Cas général d’un coll qui prend la place d’un CDS responsable de ses propres missions 

			Notes de frais : les demandes en cours de ce collaborateurs sont données
			au service comptabilité pour validation. Cela compte également comme une double validation.

			*/






			return Redirect("/Home/Index");
}





// Coll List
public ActionResult CollaboratorsList()
{
if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
	return Redirect("/Home/Index");

List<Collaborator> model = dal.GetCollaborators();

return View(model);
}

private bool ValidationLogin(Collaborator unique_coll) {
string unique_field = unique_coll.Login;
List<Collaborator> colls = dal.GetCollaborators();

foreach (Collaborator c in colls) {
	if (c.Login == unique_field) {
		return false;
	}
}
return true;
}
}
}
 