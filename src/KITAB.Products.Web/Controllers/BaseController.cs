using Microsoft.AspNetCore.Mvc;
using KITAB.Products.Application.Notificators;

namespace KITAB.Products.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificatorService _notificatorService;

        protected BaseController(INotificatorService notificatorService)
        {
            _notificatorService = notificatorService;
        }

        protected bool ValidOperation()
        {
            return !_notificatorService.HaveNotification();
        }
    }
}
