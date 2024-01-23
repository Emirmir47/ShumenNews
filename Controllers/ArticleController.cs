using Microsoft.AspNetCore.Mvc;

namespace ShumenNews.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
