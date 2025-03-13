using GestionaleBiblioteca.Data;
using GestionaleBiblioteca.Models;
using GestionaleBiblioteca.ViewModels;
using GestionaleBiblioteca.Services;
using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    public class PrestitoService
    {
        private readonly GestionaleBibliotecaDbContext _context;
        private readonly IFluentEmail _fluentEmail;
        private readonly LibroService _libroService;

        public PrestitoService(
            GestionaleBibliotecaDbContext context,
            IFluentEmail fluentEmail,
            LibroService libroService)
        {
            _context = context;
            _fluentEmail = fluentEmail;
            _libroService = libroService;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<PrestitoViewModel>> GetOnGoingLoansAsync()
        {
            var prestiti = await _context.Prestiti
                .Where(p => p.DataRestituzioneEffettiva == null)
                .Include(l => l.Libro)
                .Select(p => new PrestitoViewModel
                {
                    Id = p.Id,
                    NomeUtente = p.NomeUtente,
                    EmailUtente = p.EmailUtente,
                    DataPrestito = p.DataPrestito,
                    DataRestituzione = p.DataRestituzione,
                    DataRestituzioneEffettiva = p.DataRestituzioneEffettiva,
                    Note = p.Note,
                    LibroId = p.LibroId,
                    TitoloLibro = p.Libro.Titolo,
                    Scaduto = p.DataRestituzione < DateTime.Now && p.DataRestituzioneEffettiva == null
                })
                .ToListAsync();

            return prestiti;
        }

        public async Task<PrestitoViewModel?> GetLibroByIdAsync(Guid id)
        {
            var prestito = await _context.Prestiti
                .Include(l => l.Libro)
                .Where(l => l.Id == id)
                .Select(p => new PrestitoViewModel
                {
                    Id = p.Id,
                    NomeUtente = p.NomeUtente,
                    EmailUtente = p.EmailUtente,
                    DataPrestito = p.DataPrestito,
                    DataRestituzione = p.DataRestituzione,
                    DataRestituzioneEffettiva = p.DataRestituzioneEffettiva,
                    Note = p.Note,
                    LibroId = p.LibroId,
                    TitoloLibro = p.Libro.Titolo,
                    Scaduto = p.DataRestituzione < DateTime.Now && p.DataRestituzioneEffettiva == null
                })
                .FirstOrDefaultAsync();

            return prestito;
        }

        public async Task<bool> AddLoanAsync(PrestitoViewModel prestitoViewModel)
        {
            var libro = await _libroService.GetLibroByIdAsync(prestitoViewModel.LibroId);
            if (libro == null || !libro.Disponibile)
            {
                throw new InvalidOperationException("Il libro non è disponibile per il prestito");
            }

            var prestito = new Prestito
            {
                Id = Guid.NewGuid(),
                NomeUtente = prestitoViewModel.NomeUtente,
                EmailUtente = prestitoViewModel.EmailUtente,
                DataPrestito = prestitoViewModel.DataPrestito,
                DataRestituzione = prestitoViewModel.DataRestituzione,
                Note = prestitoViewModel.Note,
                LibroId = prestitoViewModel.LibroId
            };

            libro.Disponibile = false;
            await _libroService.UpdateLibroAsync(libro);

            _context.Prestiti.Add(prestito);
            return await SaveAsync();
        }

        public async Task<bool> ReturnBookAsync(Guid prestitoId)
        {
            var prestito = await _context.Prestiti.FindAsync(prestitoId);
            if (prestito == null)
            {
                throw new InvalidOperationException("Prestito non trovato");
            }

            prestito.DataRestituzioneEffettiva = DateTime.Now;

            var libro = await _libroService.GetLibroByIdAsync(prestito.LibroId);
            if (libro == null)
            {
                throw new InvalidOperationException("Libro non trovato");
            }

            libro.Disponibile = true;
            await _libroService.UpdateLibroAsync(libro);
            await SaveAsync();

            return true;
        }
    }
}