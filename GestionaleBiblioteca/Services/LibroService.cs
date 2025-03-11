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

        //public async Task<LibriListaViewModel> GetAllLibriAsync()
        //{
        //    var LibriLista = new LibriListaViewModel();

        //    try
        //    {

        //        LibriLista.Libri = await _context.Libri.ToListAsync();

        //        return LibriLista;
        //    }
        //    catch
        //    {
        //        return new LibriListaViewModel() { Libri = new List<Libro>() };
        //    }
        //}

        private async Task<bool> SaveAsync()
        {
            try
            {
                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<LibriListaViewModel> GetAllLibriAsync()
        {
            var libriLista = new LibriListaViewModel();

            try
            {
                libriLista.Libri = await _context.Libri.ToListAsync();
            }
            catch
            {
                libriLista.Libri = null;
            }

            return libriLista;
        }

        public async Task<bool> AddLibroAsync(AddLibroViewModel addLibroViewModel)
        {
            var libro = new Libro()
            {
                Id = Guid.NewGuid(),
                Name = addLibroViewModel.Name,
                Author = addLibroViewModel.Author,
                Description = addLibroViewModel.Description,
                Price = addLibroViewModel.Price,
                Category = addLibroViewModel.Category
            };

            _context.Libri.Add(libro);

            return await SaveAsync();
        }

        public async Task<Libro?> GetLibroByIdAsync(Guid id)
        {
            var libro = await _context.Libri.FindAsync(id);

            if (libro == null)
            {
                return null;
            }

            return libro;
        }

        public async Task<bool> DeleteLibroByIdAsync(Guid id)
        {
            var libro = await _context.Libri.FindAsync(id);

            if (libro == null)
            {
                return false;
            }

            _context.Libri.Remove(libro);

            return await SaveAsync();
        }

        public async Task<bool> UpdateLibroAsync(EditLibroViewModel editLibroViewModel)
        {
            var libro = await _context.Libri.FindAsync(editLibroViewModel.Id);

            if (libro == null)
            {
                return false;
            }

            libro.Name = editLibroViewModel.Name;
            libro.Author = editLibroViewModel.Author;
            libro.Description = editLibroViewModel.Description;
            libro.Price = editLibroViewModel.Price;
            libro.Category = editLibroViewModel.Category;

            return await SaveAsync();
        }
    }
}
