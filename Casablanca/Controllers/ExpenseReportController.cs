using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Controllers
{
    public class ExpenseReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}