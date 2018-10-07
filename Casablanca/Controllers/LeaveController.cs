using Casablanca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Controllers
{
    public class LeaveController : Controller
    {
        public ActionResult Index()
        {
            // Récupérer les congés et demandes de congés dans la BD
            // var congés = GetCongés();

            // Créer un objet LeaveList
            LeaveList model = new LeaveList();

            // Récupérer les données de congés dans la base de données
            // %% TEMPORARY : 
            Leave leave = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING, new DateTime(2018, 10, 04));
            Leave leave2 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING, new DateTime(2018, 10, 05));
            Leave leave3 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING, new DateTime(2018, 10, 06));
            Leave pending = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 24));
            Leave pending2 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 25));
            Leave pending3 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 26));

            model.leaves.Add(leave);
            model.leaves.Add(leave2);
            model.leaves.Add(leave3);
            model.leaves.Add(pending);
            model.leaves.Add(pending2);
            model.leaves.Add(pending3);
            // %%

            return View(model);
        }

        public ActionResult Traiter()
        {
            // Récupérer les congés et demandes de congés dans la BD
            // var congés = GetCongés();

            // Créer un objet LeaveList
            LeaveList model = new LeaveList();

            // Récupérer les données de congés dans la base de données
            // %% TEMPORARY : 
            Leave leave = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING, new DateTime(2018, 10, 04));
            Leave leave2 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING, new DateTime(2018, 10, 05));
            Leave leave3 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING, new DateTime(2018, 10, 06));
            Leave pending = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 24));
            Leave pending2 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 25));
            Leave pending3 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 26));

            model.leaves.Add(leave);
            model.leaves.Add(leave2);
            model.leaves.Add(leave3);
            model.leaves.Add(pending);
            model.leaves.Add(pending2);
            model.leaves.Add(pending3);
            // %%

            return View(model);
        }
    }
}