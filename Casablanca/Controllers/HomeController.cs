using Casablanca.Models;
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
            Collaborateurs model = new Collaborateurs();

            // %% TEMPORARY : 
            Collaborateur morgan = new Collaborateur("Morgan", "Feurte", "Informatique", "Carry");
            Collaborateur jafar = new Collaborateur("Jafar", "Goncalves", "Informatique", "Support");
            Collaborateur adrien = new Collaborateur("Adrien", "Lavillonnière", "Informatique", "Tank");
            Collaborateur minh = new Collaborateur("Minh", "Nguyen", "Informatique", "DPS");
            Collaborateur yao = new Collaborateur("Yao", "Shi", "Informatique", "Support");

            model.collaborateurs.Add(morgan);
            model.collaborateurs.Add(jafar);
            model.collaborateurs.Add(adrien);
            model.collaborateurs.Add(minh);
            model.collaborateurs.Add(yao);
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
    }
}