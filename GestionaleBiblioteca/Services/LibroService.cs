using GestionaleBiblioteca.Data;
using GestionaleBiblioteca.ViewModels;
using GestionaleBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionaleBiblioteca.Services
{
    public class LibroService
    {
        private readonly GestionaleBibliotecaDbContext _context;

        public LibroService(GestionaleBibliotecaDbContext context)
        {
            _context = context;
        }

        public async Task<LibriListaViewModel> GetAllLibriAsync()
        {
            try
            {
                var LibriLista = new LibriListaViewModel();

                LibriLista.Libri = await _context.Libri.ToListAsync();

                return LibriLista;
            }
            catch
            {
                return new LibriListaViewModel() { Libri = new List<Libro>() };
            }
        }
    }
}
