using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
	public enum NotificationStatus
	{
		READ,
		UNREAD
	}

	public enum NotificationType
	{
		EXPENSE,
		LEAVE,
		ADVANCE,
		INFORMATION
	}

    public enum NotificationResult
    {
        VALIDATION,
        REFUSAL,
        RECALL,
        TREATMENT
    }

    public enum NotificationManage
    {
        MANAGE,
        RESULT
    }

    public class Notification
	{
        [Key]
        public int Id { get; set; }
		public NotificationType NotifType { get; set; }
		public NotificationStatus NotifStatus { get; set; }
        public NotificationResult NotifResult { get; set; }
        public NotificationManage NotifManage { get; set; }
        public string NotifContent { get; set; }
        public Collaborator Receiver { get; set; }
        public Collaborator Transmitter { get; set; }

        public Notification(NotificationType notifType, NotificationStatus notifStatus, string notifContent)
		{
			NotifType = notifType;
			NotifStatus = notifStatus;
			NotifContent = notifContent;
		}

        public Notification(Collaborator transmitter, Collaborator receiver, NotificationType notifType, NotificationResult notifResult)
        {
            Transmitter = transmitter;
            Receiver = receiver;
            NotifStatus = NotificationStatus.UNREAD;
            NotifType = notifType;
            NotifResult = notifResult;
            NotifManage = NotificationManage.RESULT;
            Content();
        }

        public Notification(Collaborator transmitter, Collaborator receiver, NotificationType notifType)
        {
            Transmitter = transmitter;
            Receiver = receiver;
            NotifStatus = NotificationStatus.UNREAD;
            NotifType = notifType;
            NotifManage = NotificationManage.MANAGE;
            NotifResult = NotificationResult.TREATMENT;
            Content();
        }

        public Notification(Collaborator transmitter, Collaborator receiver, NotificationType notifType, NotificationResult notifResult, string notifContent)
        {
            Transmitter = transmitter;
            Receiver = receiver;
            NotifStatus = NotificationStatus.UNREAD;
            NotifType = notifType;
            NotifManage = NotificationManage.MANAGE;
            NotifResult = notifResult;
            NotifContent = notifContent;
        }

        public Notification()
		{
		}

        public void Content()
        {
            if(NotifManage != NotificationManage.MANAGE)
            {
                if (NotifResult != NotificationResult.RECALL)
                {
                    if(NotifType == NotificationType.EXPENSE)
                    {
                        if(NotifResult == NotificationResult.VALIDATION)
                            NotifContent = "Votre note de frais est validée";
                        else if(NotifResult == NotificationResult.REFUSAL)
                            NotifContent = "Votre note de frais est refusée";
                    }
                    else
                        NotifContent = Transmitter.FirstName.ToString() + " " + Transmitter.LastName.ToString() + " a " + ToString(NotifResult) + " votre " + ToString(NotifType);
                }
                else
                {
                    //TODO
                    NotifContent = "rappel";
                }
            }
            else
            {
                NotifContent = "Une " + ToString(NotifType) + " de " + Transmitter.FirstName.ToString() + " " + Transmitter.LastName.ToString() + " est en attente de validation";
            }
            
        }

        public string ToString(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.EXPENSE: return "note de frais";
                case NotificationType.LEAVE: return "demande de congé";
                case NotificationType.ADVANCE: return "demande d'avance";
                case NotificationType.INFORMATION: return "Information";
            }
            return "Debug: TypeNotification";
        }

        public static string ToString(NotificationResult type)
        {
            switch (type)
            {
                case NotificationResult.VALIDATION: return "validé";
                case NotificationResult.REFUSAL: return "refusé";
                case NotificationResult.RECALL: return "Rappel";
            }
            return "Debug: ResultNotification";
        }
    }
}