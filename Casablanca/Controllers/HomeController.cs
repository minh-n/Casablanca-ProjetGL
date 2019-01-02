using Casablanca.Models;
using Casablanca.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Controllers
{
    public class HomeController : Controller
    {
        private IDal dal;

        public HomeController() : this(new Dal()) {

        }

        private HomeController(IDal dal) {
            this.dal = dal;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contactez l'administration";

            return View();
        }

        public ActionResult ListeCollaborateurs()
        {
            // Récupérer les collaborateurs dans la BD
            List<Collaborator> model = dal.GetCollaborators();

            return View(model);
        }

        public ActionResult CollaboratorView(int id)
        {

			//creer un modele qui contient un collabo et le filer a la vue
			//le collabo qui a cet id, ca nous fera gagner du temps jpense.

			Dal dal = new Dal();
			//dal.CreateCollaborator("Minh", "Nguyen");
			//dal.CreateCollaborator("Ming", "Ngoun");

			Collaborator model = dal.GetCollaborators()[id];
			
            return View(model);
        }
    }
}