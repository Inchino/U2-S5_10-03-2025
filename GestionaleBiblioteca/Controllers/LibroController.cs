using System.Threading.Tasks;
using GestionaleBiblioteca.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionaleBiblioteca.Controllers
{
    public class LibroController : Controller
    {
        private readonly LibroService _productService;

        public LibroController(LibroService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var LibriLista = await _productService.GetAllLibriAsync();

            return View(LibriLista);
        }
    }
}
