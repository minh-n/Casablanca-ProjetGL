using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ViewModel
{
    public class SelectNotificationEditorVM
    {
        public bool Selected { get; set; }
        public int Id { get; set; }
        public NotificationType NotifType { get; set; }
        public string NotifStatus { get; set; }
        public string NotifResult { get; set; }
        public string NotifContent { get; set; }
    }
}