using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestionaleBiblioteca.ViewModels;
using GestionaleBiblioteca.Services;
using GestionaleBiblioteca.Data;

namespace GestionaleBiblioteca.Controllers;

public class HomeController : Controller
{
    

    private readonly ILogger<HomeController> _logger;
    private readonly LibroService _libroService;
    private readonly PrestitoService _prestitoService;

    public HomeController(
        ILogger<HomeController> logger,
        LibroService libroService,
        PrestitoService prestitoService)
    {
        _logger = logger;
        _libroService = libroService;
        _prestitoService = prestitoService;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        var booksViewModel = await _libroService.GetAllLibriAsync();
        var books = booksViewModel.Libri;

        ViewBag.TotalBooks = books.Count;
        ViewBag.AvailableBooks = books.Count(b => b.Disponibile);
        return View(books);

    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
