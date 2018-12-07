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
            List<Collaborator> model = new List<Collaborator>();

            // %% TEMPORARY : 
            Collaborator morgan = new Collaborator("Morgan", "Feurte", "Informatique", "Carry");
            Collaborator jafar = new Collaborator("Jafar", "Goncalves", "Informatique", "Support");
            Collaborator adrien = new Collaborator("Adrien", "Lavillonnière", "Informatique", "Tank");
            Collaborator minh = new Collaborator("Minh", "Nguyen", "Informatique", "DPS");
            Collaborator yao = new Collaborator("Yao", "Shi", "Informatique", "Support");

            model.Add(morgan);
            model.Add(jafar);
            model.Add(adrien);
            model.Add(minh);
            model.Add(yao);
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