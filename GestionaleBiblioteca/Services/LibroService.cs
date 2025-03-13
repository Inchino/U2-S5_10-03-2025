using GestionaleBiblioteca.Data;
using GestionaleBiblioteca.ViewModels;
using GestionaleBiblioteca.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

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
                //libriLista.Libri = await _context.Libri.ToListAsync();
                libriLista.Libri = await _context.Libri.Include(b => b.Prestiti).ToListAsync();
            }
            catch
            {
                libriLista.Libri = null;
            }

            return libriLista;
        }

        public async Task<bool> AddLibroAsync(AddLibroViewModel addLibroViewModel)
        {
            var libro = new Libro
            {
                Id = Guid.NewGuid(),
                Titolo = addLibroViewModel.Titolo,
                Autore = addLibroViewModel.Autore,
                Genere = addLibroViewModel.Genere,
                Descrizione = addLibroViewModel.Descrizione,
                Prezzo = addLibroViewModel.Prezzo,
                Disponibile = addLibroViewModel.Disponibile,
                PercorsoImmagineCopertina = addLibroViewModel.PercorsoImmagineCopertina
            };

            _context.Libri.Add(libro);
            return await SaveAsync();
        }

        //public async Task<bool> AddLibroAsync(AddLibroViewModel addLibroViewModel)
        //{
        //    var libro = new Libro()
        //    {
        //        Id = Guid.NewGuid(),
        //        Titolo = addLibroViewModel.Titolo,
        //        Autore = addLibroViewModel.Autore,
        //        Descrizione = addLibroViewModel.Descrizione,
        //        Prezzo = addLibroViewModel.Prezzo,
        //        Genere = addLibroViewModel.Genere,
        //        Disponibile = addLibroViewModel.Disponibile
        //    };

        //    if (addLibroViewModel.ImmagineCopertina != null)
        //    {
        //        string uploadsFolder = Path.Combine("wwwroot", "images");
        //        Directory.CreateDirectory(uploadsFolder);

        //        string fileName = $"{Guid.NewGuid()}_{addLibroViewModel.ImmagineCopertina.FileName}";
        //        string filePath = Path.Combine(uploadsFolder, fileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await addLibroViewModel.ImmagineCopertina.CopyToAsync(fileStream);
        //        }

        //        libro.PercorsoImmagineCopertina = $"/images/{fileName}";
        //    }

        //    _context.Libri.Add(libro);
        //    return await SaveAsync();
        //}

        public async Task<Libro?> GetLibroByIdAsync(Guid id)
        {
            //var libro = await _context.Libri.FindAsync(id);
            var libro = await _context.Libri.Include(l => l.Prestiti).FirstOrDefaultAsync(l => l.Id == id);

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

        //public async Task<bool> UpdateLibroAsync(EditLibroViewModel editLibroViewModel)
        //{
        //    var libro = await _context.Libri.FindAsync(editLibroViewModel.Id);
        //    if (libro == null)
        //    {
        //        return false;
        //    }

        //    libro.Titolo = editLibroViewModel.Titolo;
        //    libro.Autore = editLibroViewModel.Autore;
        //    libro.Genere = editLibroViewModel.Genere;
        //    libro.Descrizione = editLibroViewModel.Descrizione;
        //    libro.Prezzo = editLibroViewModel.Prezzo;
        //    libro.Disponibile = editLibroViewModel.Disponibile;
        //    libro.PercorsoImmagineCopertina = editLibroViewModel.PercorsoImmagineCopertina;

        //    return await SaveAsync();
        //}
        //public async Task<bool> UpdateLibroAsync(EditLibroViewModel editLibroViewModel)
        //{
        //    var libro = await _context.Libri.FindAsync(editLibroViewModel.Id);
        //    if (libro == null)
        //    {
        //        return false;
        //    }

        //    libro.Titolo = editLibroViewModel.Titolo;
        //    libro.Autore = editLibroViewModel.Autore;
        //    libro.Genere = editLibroViewModel.Genere;
        //    libro.Descrizione = editLibroViewModel.Descrizione;
        //    libro.Prezzo = editLibroViewModel.Prezzo;
        //    libro.Disponibile = editLibroViewModel.Disponibile;
        //    libro.PercorsoImmagineCopertina = editLibroViewModel.PercorsoImmagineCopertina;

        //    _context.Entry(libro).State = EntityState.Modified;

        //    return await _context.SaveChangesAsync() > 0;
        //}

        public async Task<bool> UpdateLibroAsync(EditLibroViewModel editLibroViewModel)
        {
            var libro = await _context.Libri.FindAsync(editLibroViewModel.Id);
            if (libro == null)
            {
                return false;
            }

            libro.Titolo = editLibroViewModel.Titolo;
            libro.Autore = editLibroViewModel.Autore;
            libro.Genere = editLibroViewModel.Genere;
            libro.Descrizione = editLibroViewModel.Descrizione;
            libro.Prezzo = editLibroViewModel.Prezzo;
            libro.Disponibile = editLibroViewModel.Disponibile;

            if (editLibroViewModel.ImmagineCopertinaFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var uniqueFileName = $"{Guid.NewGuid()}_{editLibroViewModel.ImmagineCopertinaFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await editLibroViewModel.ImmagineCopertinaFile.CopyToAsync(fileStream);
                }

                libro.PercorsoImmagineCopertina = $"/images/{uniqueFileName}"; // Salva il nuovo percorso
            }

            _context.Entry(libro).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Libro>> SearchLibriAsync(string parolachiave)
        {
            if (string.IsNullOrEmpty(parolachiave))
            {
                return await _context.Libri.ToListAsync();
            }

            return await _context.Libri
                .Where(b => b.Titolo.Contains(parolachiave) ||
                            b.Autore.Contains(parolachiave) ||
                            b.Genere.Contains(parolachiave))
                .ToListAsync();
        }
    }
}
