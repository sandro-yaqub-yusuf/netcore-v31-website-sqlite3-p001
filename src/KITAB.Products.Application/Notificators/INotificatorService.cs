using System.Collections.Generic;
using KITAB.Products.Domain.Models;

namespace KITAB.Products.Application.Notificators
{
    public interface INotificatorService
    {
        void Handle(Notification p_notification);
        bool HaveNotification();
        List<Notification> GetAll();
    }
}
