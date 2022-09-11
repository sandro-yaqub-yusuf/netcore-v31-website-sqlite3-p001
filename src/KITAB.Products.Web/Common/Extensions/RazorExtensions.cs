using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace KITAB.Products.Web.Extensions
{
    public static class RazorExtensions
    {
        public static HtmlString FormatStatus(this RazorPage p_page, string p_status = "A")
        {
            if (p_page is null) return new HtmlString("<span class='badge badge-success'>INDEFINIDO</span>");

            return new HtmlString((p_status == "A" ? "<span class='badge badge-success'>ATIVADO</span>" : "<span class='badge badge-danger'>DESATIVADO</span>"));
        }
    }
}
