using Microsoft.AspNetCore.Mvc;
using KITAB.Products.Web.ViewModels;

namespace KITAB.Products.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            ErrorViewModel modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Message = "Tente novamente mais tarde ou contate nosso suporte...";
                modelErro.Title = "Ocorreu um erro !!!";
                modelErro.ErrorCode = id;
            }
            else if (id == 404)
            {
                modelErro.Message = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte...";
                modelErro.Title = "Ops !!! Página não encontrada...";
                modelErro.ErrorCode = id;
            }
            else if (id == 403)
            {
                modelErro.Message = "Você não tem permissão para fazer isto...";
                modelErro.Title = "<<< Acesso Negado >>>";
                modelErro.ErrorCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
