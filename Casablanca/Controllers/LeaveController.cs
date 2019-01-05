using Casablanca.Models;
using System;
using System.Collections.Generic;
using Casablanca.Models.Leaves;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Controllers
{
    public class LeaveController : Controller
    {

        public ActionResult Index()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // Récupérer les congés et demandes de congés dans la BD
            // var congés = GetCongés();

            // Créer un objet LeaveList
            //LeaveList model = new LeaveList();
            List<Leave> model = new List<Leave>();

            // Récupérer les données de congés dans la base de données
            // %% TEMPORARY : 
            Leave leave = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING_APPROVAL, new DateTime(2018, 10, 04));
            Leave leave2 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING_APPROVAL, new DateTime(2018, 10, 05));
            Leave leave3 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING_APPROVAL, new DateTime(2018, 10, 06));
            Leave pending = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 24));
            Leave pending2 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 25));
            Leave pending3 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 26));

            model.Add(leave);
            model.Add(leave2);
            model.Add(leave3);
            model.Add(pending);
            model.Add(pending2);
            model.Add(pending3);
            // %%

            return View(model);
        }









		//TODO
        public ActionResult Process()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // TODO : passer les demandes en attente

            // Récupérer les congés et demandes de congés dans la BD
            // var congés = GetCongés();

            // Créer un objet LeaveList
            //LeaveList model = new LeaveList();
            List<Leave> model = new List<Leave>();

            // Récupérer les données de congés dans la base de données
            // %% TEMPORARY : 
            Leave leave = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING_APPROVAL, new DateTime(2018, 10, 04));
            Leave leave2 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING_APPROVAL, new DateTime(2018, 10, 05));
            Leave leave3 = new Leave("Morgan 04/10 -> 06/10", LeaveStatus.PENDING_APPROVAL, new DateTime(2018, 10, 06));
            Leave pending = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 24));
            Leave pending2 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 25));
            Leave pending3 = new Leave("Minh 24/10 -> 26/10", LeaveStatus.APPROVED, new DateTime(2018, 10, 26));

            model.Add(leave);
            model.Add(leave2);
            model.Add(leave3);
            model.Add(pending);
            model.Add(pending2);
            model.Add(pending3);
            // %%


            return View(model);
        }












    }
}