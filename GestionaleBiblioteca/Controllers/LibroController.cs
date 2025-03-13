using System.Threading.Tasks;
using GestionaleBiblioteca.Services;
using GestionaleBiblioteca.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GestionaleBiblioteca.Controllers
{
    public class LibroController : Controller
    {
        private readonly LibroService _libroService;

        public LibroController(LibroService libroService)
        {
            _libroService = libroService;
        }

        public async Task<IActionResult> Index()
        {
            var LibriLista = await _libroService.GetAllLibriAsync();

            return View(LibriLista);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddLibroViewModel addLibroViewModel)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Errore durante il salvataggio dei dati nel database";
                return RedirectToAction("Index");
            }

            var result = await _libroService.AddLibroAsync(addLibroViewModel);

            if(!result)
            {
                TempData["Error"] = "Errore durante il salvataggio dei dati nel database";
            }

            return RedirectToAction("Index");
        }

        [Route("libro/dettagli/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var libro = await _libroService.GetLibroByIdAsync(id);

            if (libro == null)
            {
                TempData["Error"] = "Errore mentre ottenevo informazino dal database";
                return RedirectToAction("Index");
            }

            var libroDettagliViewModel = new LibroDettagliViewModel()
            {
                Id = libro.Id,
                Titolo = libro.Titolo,
                Autore = libro.Autore,
                Descrizione = libro.Descrizione,
                Prezzo = libro.Prezzo,
                Genere = libro.Genere,
                Disponibile = libro.Disponibile,
                PercorsoImmagineCopertina = libro.PercorsoImmagineCopertina
            };

            return View(libroDettagliViewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _libroService.DeleteLibroByIdAsync(id);

            if (!result)
            {
                TempData["Error"] = "Errore mentre eliminavo il libro dal database";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var libro = await _libroService.GetLibroByIdAsync(id);

            if (libro == null)
            {
                return RedirectToAction("Index");
            }
            ;

            var editLibroViewModel = new EditLibroViewModel()
            {
                Id = libro.Id,
                Titolo = libro.Titolo,
                Autore = libro.Autore,
                Descrizione = libro.Descrizione,
                Prezzo = libro.Prezzo,
                Genere = libro.Genere,
                Disponibile = libro.Disponibile,
                PercorsoImmagineCopertina = libro.PercorsoImmagineCopertina
            };

            return View(editLibroViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditLibroViewModel editLibroViewModel)
        {
            var result = await _libroService.UpdateLibroAsync(editLibroViewModel);

            if (!result)
            {
                TempData["Error"] = "Errore mentre aggiornavo il libro nel database";
            }

            return RedirectToAction("Index");
        }
    }
}
