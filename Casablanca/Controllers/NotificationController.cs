using Casablanca.Models;
using Casablanca.Models.Database;
using Casablanca.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Casablanca.Controllers
{
    public enum NotificationCommand
    {
        MARK,
        DELETE
    }

    public class NotificationController : Controller
    {
        private IDal dal;

        public NotificationController() : this(new Dal())
        {

        }

        private NotificationController(IDal dal)
        {
            this.dal = dal;
        }

        // GET: Notification
        public ActionResult Index()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            NotificationSelectionVM model = new NotificationSelectionVM();

            foreach (Notification n in dal.GetNotifications(coll))
            {
                SelectNotificationEditorVM notifVM = new SelectNotificationEditorVM()
                {
                    Id = n.Id,
                    NotifType = n.NotifType,
                    NotifStatus = (n.NotifStatus == NotificationStatus.UNREAD ? "unread" : "read"),
                    NotifResult = n.NotifResult.ToString(),
                    NotifContent = n.NotifContent,
                    Selected = false
                };
                model.Notifications.Insert(0, notifVM);
            }

            return View(model);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "operation", MatchFormValue = "Marquer comme non lu")]
        public ActionResult MarkAsUnread(NotificationSelectionVM model)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // get the ids of the items selected:
            var selectedIds = model.GetSelectedIds();

            System.Diagnostics.Debug.WriteLine("taille list id: " + selectedIds.Count().ToString());

            // Use the ids to retrieve the records for the selected notification
            // from the database:
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            var selectedNotification = from x in dal.GetNotifications(coll)
                                 where selectedIds.Contains(x.Id)
                                 select x;

            System.Diagnostics.Debug.WriteLine("taille list: " + selectedNotification.Count().ToString());

            // Process
            foreach (Notification notif in selectedNotification)
            {
                notif.NotifStatus = NotificationStatus.UNREAD;
            }

            dal.SaveChanges();

            // Redirect the results:
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "operation", MatchFormValue = "Marquer comme lu")]
        public ActionResult MarkAsRead(NotificationSelectionVM model)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // get the ids of the items selected:
            var selectedIds = model.GetSelectedIds();

            System.Diagnostics.Debug.WriteLine("taille list id: " + selectedIds.Count().ToString());

            // Use the ids to retrieve the records for the selected notification
            // from the database:
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            var selectedNotification = from x in dal.GetNotifications(coll)
                                       where selectedIds.Contains(x.Id)
                                       select x;

            System.Diagnostics.Debug.WriteLine("taille list: " + selectedNotification.Count().ToString());

            // Process
            foreach (Notification notif in selectedNotification)
            {
                notif.NotifStatus = NotificationStatus.READ;
            }

            dal.SaveChanges();

            // Redirect the results:
            return RedirectToAction("Index");
        }

        public ActionResult Link(int id)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Notification notif = dal.GetNotifications(id);
            notif.NotifStatus = NotificationStatus.READ;

            dal.SaveChanges();

            if(notif.NotifManage == NotificationManage.MANAGE)
            {
                switch (notif.NotifType)
                {
                    case NotificationType.ADVANCE:
                        return Redirect("/ExpenseReport/AdvanceProcessList");
                    case NotificationType.EXPENSE:
                        return Redirect("/ExpenseReport/ProcessList");
                    case NotificationType.INFORMATION:
                        return Redirect("/Home/Contact");
                    case NotificationType.LEAVE:
                        return Redirect("/Leave/ProcessList");
                    default:
                        return Redirect("/Home/Index");
                }
            }
            else
            {
                switch (notif.NotifType)
                {
                    case NotificationType.ADVANCE:
                        return Redirect("/ExpenseReport/Index");
                    case NotificationType.EXPENSE:
                        return Redirect("/ExpenseReport/Index");
                    case NotificationType.INFORMATION:
                        return Redirect("/Home/Contact");
                    case NotificationType.LEAVE:
                        return Redirect("/Leave/Index");
                    default:
                        return Redirect("/Home/Index");
                }
            }            
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "operation", MatchFormValue = "Supprimer")]
        public ActionResult DeleteNotification(NotificationSelectionVM model)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            // get the ids of the items selected:
            var selectedIds = model.GetSelectedIds();

            // Use the ids to retrieve the records for the selected notification
            // from the database:
            Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
            var selectedNotification = from x in dal.GetNotifications(coll)
                                       where selectedIds.Contains(x.Id)
                                       select x;

            System.Diagnostics.Debug.WriteLine(selectedNotification.Count().ToString());

            // Process
            foreach (Notification notif in selectedNotification)
            {
                dal.DeleteNotification(notif);
            }

            dal.SaveChanges();

            // Redirect the results:
            return RedirectToAction("Index");
        }
    }
}