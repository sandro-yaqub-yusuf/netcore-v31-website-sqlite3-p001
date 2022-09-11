using System.Collections.Generic;
using System.Linq;
using KITAB.Products.Domain.Models;

namespace KITAB.Products.Application.Notificators
{
    public class NotificatorService : INotificatorService
    {
        private readonly List<Notification> _notifications;

        public NotificatorService()
        {
            _notifications = new List<Notification>();
        }

        public void Handle(Notification p_notification)
        {
            _notifications.Add(p_notification);
        }

        public bool HaveNotification()
        {
            return _notifications.Any();
        }

        public List<Notification> GetAll()
        {
            return _notifications;
        }
    }
}
