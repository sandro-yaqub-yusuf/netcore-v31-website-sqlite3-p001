using Microsoft.AspNetCore.Mvc;

namespace KITAB.Products.Web.Extensions
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
