using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ViewModel
{
    public class NotificationSelectionVM
    {
        public List<SelectNotificationEditorVM> Notifications { get; set; }

        public NotificationSelectionVM()
        {
            this.Notifications = new List<SelectNotificationEditorVM>();
        }

        public IEnumerable<int> GetSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected notification:
            return (from n in this.Notifications where n.Selected select n.Id).ToList();
        }
    }
}