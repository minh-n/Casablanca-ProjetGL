using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Controllers
{
    public class JustificatoryUploadController1 : Controller
    {
        // GET: JustificatoryUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadJustificatory(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (file != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/APP_Data/UploadedFiles"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                    }
                    ViewBag.FileStatus = "Justificatory uploaded successfully.";
                }
                catch (Exception)
                {

                    ViewBag.FileStatus = "Error while Justificatory uploading.";
                }

            }
            //return View("Index");
            return View();
        }
    }
}