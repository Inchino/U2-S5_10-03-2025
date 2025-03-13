using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionaleBiblioteca.Services;
using GestionaleBiblioteca.ViewModels;

namespace GestionaleBiblioteca.Controllers
{
    public class PrestitiController : Controller
    {
        private readonly PrestitoService _prestitoService;
        private readonly LibroService _libroService;

        public PrestitiController(
            PrestitoService prestitoService,
            LibroService libroService)
        {
            _prestitoService = prestitoService;
            _libroService = libroService;
        }

        public async Task<IActionResult> Index()
        {
            var prestiti = await _prestitoService.GetOnGoingLoansAsync();
            return View(prestiti);
        }

        [HttpGet("/prestito/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var prestito = await _prestitoService.GetLoanByIdAsync(id);
            if (prestito == null)
            {
                return NotFound();
            }
            return View(prestito);
        }

        public async Task<IActionResult> Create()
        {
            var libriViewModel = await _libroService.GetAllLibriAsync();
            var libriDisponibili = libriViewModel.Libri
                .Where(b => b.Disponibile)
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Titolo })
                .ToList();

            var viewModel = new CreatePrestitoViewModel
            {
                LibroId = Guid.Empty
            };

            ViewBag.LibriDisponibili = libriDisponibili;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePrestitoViewModel prestitoViewModel)
        {
            if (!ModelState.IsValid)
            {
                var libriViewModel = await _libroService.GetAllLibriAsync();
                var libriDisponibili = libriViewModel.Libri
                    .Where(b => b.Disponibile)
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Titolo })
                    .ToList();

                ViewBag.LibriDisponibili = libriDisponibili;
                return View(prestitoViewModel);
            }

            var result = await _prestitoService.AddLoanAsync(prestitoViewModel);
            if (!result)
            {
                TempData["Error"] = "Errore durante l'aggiunta del prestito.";
                return RedirectToAction(nameof(Create));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("/prestito/return/{id:guid}")]
        public async Task<IActionResult> Return(Guid id)
        {
            try
            {
                var result = await _prestitoService.ReturnBookAsync(new UpdatePrestitoViewModel { Id = id });
                if (!result)
                {
                    TempData["Error"] = "Errore durante la restituzione del libro.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}