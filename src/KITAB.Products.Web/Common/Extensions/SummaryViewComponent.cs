using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KITAB.Products.Application.Notificators;
using KITAB.Products.Domain.Models;

namespace KITAB.Products.Web.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificatorService _notificatorService;

        public SummaryViewComponent(INotificatorService notificatorService)
        {
            _notificatorService = notificatorService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Notification> notifications = await Task.FromResult(_notificatorService.GetAll()).ConfigureAwait(false);

            notifications.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));

            return View();
        }
    }
}
