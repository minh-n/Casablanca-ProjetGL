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
            // var collaborateurs = GetCollaborateurs();

            // Créer un objet Collaborateurs
            List<Collaborator> model = dal.GetCollaborators();

			//// Missions

			//Mission mission1 = new Mission("Voyage vers Tipperary", 0, DateTime.Today, new DateTime(2019, 5, 1), MissionStatus.IN_PROGRESS);
			//Mission mission2 = new Mission("Voyage vers Agartha", 1, new DateTime(2019, 2, 9), new DateTime(2019, 3, 1), MissionStatus.PLANNED);
			//Mission mission3 = new Mission("Voyage vers l'au-delà", 2, new DateTime(2018, 12, 25), new DateTime(2018, 12, 26), MissionStatus.CANCELED);
			//Mission mission4 = new Mission("Voyage voyage", 3, new DateTime(2019, 2, 25), new DateTime(2019, 2, 26), MissionStatus.COMPLETED);
			//Mission mission5 = new Mission("Voyage vers Mars", 4, new DateTime(2019, 2, 6), new DateTime(2019, 3, 1), MissionStatus.PLANNED);
			//Mission mission6 = new Mission("Voyage vers le Mur", 5, new DateTime(2019, 1, 9), new DateTime(2019, 11, 1), MissionStatus.PLANNED);
			//Mission mission7 = new Mission("Voyage à Fuji-san", 6, new DateTime(2019, 1, 2), new DateTime(2019, 1, 4), MissionStatus.IN_PROGRESS);

			//// %% TEMPORARY : 
			//Collaborator morgan = new Collaborator("Morgan", "Feurte", "Informatique", mission7);
   //         Collaborator jafar = new Collaborator("Jafar", "Goncalves", "Informatique", mission2);
   //         Collaborator adrien = new Collaborator("Adrien", "Lavillonnière", "Informatique", mission1);
   //         Collaborator minh = new Collaborator("Minh", "Nguyen", "Informatique", mission1);
   //         Collaborator yao = new Collaborator("Yao", "Shi", "Informatique", mission1);

			//yao.Missions.Add(mission3);
			//yao.Missions.Add(mission4);

			//adrien.Missions.Add(mission2);
			//adrien.Missions.Add(mission3);
			//adrien.Missions.Add(mission4);
			//adrien.Missions.Add(mission5);
			//adrien.Missions.Add(mission6);

			//model.Add(morgan);
   //         model.Add(jafar);
   //         model.Add(adrien);
   //         model.Add(minh);
   //         model.Add(yao);
            // %%

            // Exemple de projections des résultats BD dans un Modèle
            /*
            IList<MenuItems> menuItems = null;
            if (menuItemsFromDb != null)
            {
                model.MenuItems = (from menuItem in menuItemsFromDb
                                   select new MenuItem()
                                   {
                                       Name = menuItem.Name,
                                       Price = menuItem.Price,
                                       IsVegetarian = menuItem.IsVegetarian
                                   }).ToList();
            }
            */

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