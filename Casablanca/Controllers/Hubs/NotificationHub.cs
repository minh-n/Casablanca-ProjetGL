using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casablanca.Models;
using Casablanca.Models.Database;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Casablanca.Controllers.Hubs
{
    [HubName("notificationTickerMini")]
    public class NotificationHub : Hub
    {
        /*private static readonly ConcurrentDictionary<string, Collaborator> Users =
            new ConcurrentDictionary<string, Collaborator>(StringComparer.InvariantCultureIgnoreCase);*/

        private readonly NotificationsTicker _notificationsTicker;

        public NotificationHub() : this(NotificationsTicker.Instance) { }

        public NotificationHub(NotificationsTicker notificationsTicker)
        {
            _notificationsTicker = notificationsTicker;
        }

        public Task<IEnumerable<Notification>> GetNotifications()
        {
            int.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out int collId);
            Collaborator loggedUser = _notificationsTicker.GetCollaborator(collId);

            return Task.Run(() => _notificationsTicker.GetNotifications(loggedUser));
        }

        /*
        public IEnumerable<Notification> GetNotifications()
        {
            int.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out int collId);
            Collaborator loggedUser = _notificationsTicker.GetCollaborator(collId);

            return _notificationsTicker.GetNotifications(loggedUser));
        }
        */

        /*
        public void GetNotification()
        {
            try
            {
                int.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out int collId);
                var loggedUser = _dal.GetCollaborator(collId);

                //Get TotalNotification  
                string totalNotif = TotalNotification(loggedUser);

                //Send To                  
                var cid = loggedUser.ConnectionIds.FirstOrDefault();
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.Client(cid).displayNotification(totalNotif);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //Specific User Call  
        public void SendNotification(Collaborator receiver, NotificationType notifType, string notifContent)
        {
            try
            {
                int.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out int collId);
                var loggedUser = _dal.GetCollaborator(collId);

                Notification send = new Notification(loggedUser, receiver, notifType, notifContent);
                _dal.AddNotification(send);
                
                var cid = receiver.ConnectionIds.FirstOrDefault();
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.Client(cid).broadcastNotif(notifContent);                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private string TotalNotification(Collaborator col)
        {
            List<Notification> tmp = _dal.GetNotifications(col);           
            return tmp.Count().ToString();
        }

        public override Task OnConnected()
        {
            string connectionId = Context.ConnectionId;

            int.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out int collId);    
            var user = _dal.GetCollaborator(collId);

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
                if (user.ConnectionIds.Count == 1)
                {
                    Clients.Others.userConnected(user.LastName);
                }
            }

            return base.OnConnected();
        }
        */
    }    
}