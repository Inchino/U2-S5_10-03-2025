using Microsoft.AspNetCore.Mvc;

namespace GestionaleBiblioteca.Controllers
{
    public class LibroController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
