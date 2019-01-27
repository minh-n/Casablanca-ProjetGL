using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Casablanca.Controllers.Hubs;
using Casablanca.Models.Database;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Casablanca.Models
{
    public class NotificationsTicker
    {
        // Singleton instance
        private readonly static Lazy<NotificationsTicker> _instance = new Lazy<NotificationsTicker>(() => new NotificationsTicker(GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients, new Dal()));

        private IDal _dal;

        private readonly object _updateNotificationsLock = new object();
        private volatile bool _updatingNotifications = false;

        private NotificationsTicker(IHubConnectionContext<dynamic> clients, IDal dal)
        {
            Clients = clients;
            _dal = dal;
        }

        public static NotificationsTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public Collaborator GetCollaborator(int collId)
        {
            return _dal.GetCollaborator(collId);
        }

        public IEnumerable<Notification> GetNotifications(Collaborator coll)
        {
            return _dal.GetNotifications(coll);
        }

        private void UpdateNotifications(object state)
        {
            lock (_updateNotificationsLock)
            {
                if (!_updatingNotifications)
                {
                    _updatingNotifications = true;

                    foreach(Notification n in _dal.GetNotifications())
                    {
                        BroadcastNotification(n);
                    }

                    _updatingNotifications = false;
                }
            }
        }

        private void BroadcastNotification(Notification notif)
        {
            Clients.All.updateNotification(notif);
        }

    }
}