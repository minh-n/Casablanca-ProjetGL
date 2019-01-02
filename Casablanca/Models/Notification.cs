using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
	public enum NotificationStatus
	{
		READ,
		UNREAD,
		PROCESSED,
		UNPROCESSED
	}

	public enum NotificationType
	{
		EXPENSE,
		LEAVE,
		ADVANCE,
		INFORMATION
	}

	public class Notification
	{
		public NotificationType NotifType { get; set; }
		public NotificationStatus NotifStatus { get; set; }
		public string NotifContent { get; set; }

		public Notification(NotificationType notifType, NotificationStatus notifStatus, string notifContent)
		{
			NotifType = notifType;
			NotifStatus = notifStatus;
			NotifContent = notifContent;
		}

		public Notification()
		{
		}
	}
}