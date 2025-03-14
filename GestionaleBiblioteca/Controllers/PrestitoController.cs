using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionaleBiblioteca.Services;
using GestionaleBiblioteca.ViewModels;

namespace GestionaleBiblioteca.Controllers
{
    [Route("Prestito")]
    public class PrestitoController : Controller
    {
        private readonly PrestitoService _prestitoService;
        private readonly LibroService _libroService;

        public PrestitoController(
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

        [HttpGet("Details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var prestito = await _prestitoService.GetLoanByIdAsync(id);
            if (prestito == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(prestito);
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add()
        {
            var libriViewModel = await _libroService.GetAllLibriAsync();
            var libriDisponibili = libriViewModel.Libri
                .Where(b => b.Disponibile)
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Titolo })
                .ToList();

            var viewModel = new AddPrestitoViewModel
            {
                LibroId = Guid.Empty
            };

            ViewBag.LibriDisponibili = libriDisponibili;
            return View(viewModel);
        }

        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddPrestitoViewModel prestitoViewModel)
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
                return RedirectToAction(nameof(Add));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var prestito = await _prestitoService.GetLoanByIdAsync(id);
            if (prestito == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdatePrestitoViewModel
            {
                Id = prestito.Id.Value,
                NomeUtente = prestito.NomeUtente,
                EmailUtente = prestito.EmailUtente,
                DataRestituzioneEffettiva = prestito.DataRestituzioneEffettiva,
                Note = prestito.Note
            };

            return View(viewModel);
        }

        [HttpPost("Edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePrestitoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _prestitoService.UpdateLoanAsync(model);
            if (!result)
            {
                TempData["Error"] = "Errore durante la modifica del prestito.";
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Return/{id:guid}")]
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
