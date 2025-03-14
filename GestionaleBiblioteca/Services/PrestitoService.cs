using GestionaleBiblioteca.Data;
using GestionaleBiblioteca.Models;
using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using GestionaleBiblioteca.ViewModels;
using GestionaleBiblioteca.Services;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GestionaleBiblioteca.Services
{
    public class PrestitoService
    {
        private readonly GestionaleBibliotecaDbContext _context;
        private readonly IFluentEmail _fluentEmail;
        private readonly LibroService _libroService;

        public PrestitoService(GestionaleBibliotecaDbContext context, IFluentEmail fluentEmail, LibroService libroService)
        {
            _context = context;
            _fluentEmail = fluentEmail;
            _libroService = libroService;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Prestito>> GetReturnedLoansAsync()
        {

            List<Prestito>? prestitiTotali = await _context.Prestiti
                .Where(p => p.DataRestituzioneEffettiva != null)
                .Include(l => l.Libro)
                .ToListAsync();

            foreach (var prestito in prestitiTotali)
            {
                prestito.Scaduto = prestito.DataRestituzione < DateTime.Now &&
                                  prestito.DataRestituzioneEffettiva == null;
            }

            return prestitiTotali;
        }

        public async Task<Prestito>? GetLoanByIdAsync(Guid id)
        {
            return await _context.Prestiti
                .Include(l => l.Libro)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<PrestitoViewModel>> GetOnGoingLoansAsync()
        {
            var prestiti = await _context.Prestiti
                .Where(p => p.DataRestituzioneEffettiva == null)
                .Include(p => p.Libro)
                .Select(p => new PrestitoViewModel
                {
                    Id = p.Id.Value,
                    NomeUtente = p.NomeUtente,
                    EmailUtente = p.EmailUtente,
                    DataPrestito = p.DataPrestito,
                    DataRestituzione = p.DataRestituzione,
                    DataRestituzioneEffettiva = p.DataRestituzioneEffettiva,
                    Scaduto = p.DataRestituzione < DateTime.Now && p.DataRestituzioneEffettiva == null,
                    LibroId = p.LibroId,
                    TitoloLibro = p.Libro.Titolo
                })
                .ToListAsync();

            return prestiti;
        }

        //public async Task<bool> AddLoanAsync(CreatePrestitoViewModel model)
        //{
        //    var libro = await _libroService.GetLibroByIdAsync(model.LibroId);
        //    if (libro == null || !libro.Disponibile)
        //    {
        //        throw new InvalidOperationException("Il libro non è disponibile per il prestito");
        //    }

        //    var prestito = new Prestito
        //    {
        //        Id = Guid.NewGuid(),
        //        NomeUtente = model.NomeUtente,
        //        EmailUtente = model.EmailUtente,
        //        DataPrestito = DateTime.Now,
        //        DataRestituzione = DateTime.Now.AddDays(14),
        //        LibroId = model.LibroId,
        //        Libro = libro
        //    };

        //    libro.Disponibile = false;
        //    await _libroService.UpdateLibroAsync(libro);
        //    _context.Prestiti.Add(prestito);

        //    var success = await _context.SaveChangesAsync() > 0;
        //    if (!success) return false;

        //    await _fluentEmail
        //        .To(prestito.EmailUtente)
        //        .Subject($"Prestito Libro: {libro.Titolo}")
        //        .Body($"Gentile {prestito.NomeUtente},\n\nLe confermiamo il prestito del libro '{libro.Titolo}'.\nLa data di restituzione è {prestito.DataRestituzione:dd/MM/yyyy}.\n\nGrazie.")
        //        .SendAsync();

        //    return true;
        //}

        //public async Task<bool> AddLoanAsync(CreatePrestitoViewModel model)
        //{
        //    var libro = await _libroService.GetLibroByIdAsync(model.LibroId);
        //    if (libro == null || !libro.Disponibile)
        //    {
        //        throw new InvalidOperationException("Il libro non è disponibile per il prestito");
        //    }

        //    var prestito = new Prestito
        //    {
        //        Id = Guid.NewGuid(),
        //        NomeUtente = model.NomeUtente,
        //        EmailUtente = model.EmailUtente,
        //        DataPrestito = DateTime.Now,
        //        DataRestituzione = DateTime.Now.AddDays(14),
        //        LibroId = model.LibroId,
        //        Libro = libro
        //    };

        //    libro.Disponibile = false;

        //    var libroViewModel = new EditLibroViewModel
        //    {
        //        Id = libro.Id,
        //        Titolo = libro.Titolo,
        //        Autore = libro.Autore,
        //        Genere = libro.Genere,
        //        Descrizione = libro.Descrizione,
        //        Prezzo = libro.Prezzo,
        //        Disponibile = libro.Disponibile,
        //        PercorsoImmagineCopertina = libro.ImmagineCopertina
        //    };

        //    await _libroService.UpdateLibroAsync(libroViewModel);

        //    _context.Prestiti.Add(prestito);
        //    var success = await _context.SaveChangesAsync() > 0;
        //    if (!success) return false;

        //    await _fluentEmail
        //        .To(prestito.EmailUtente)
        //        .Subject($"Prestito Libro: {libro.Titolo}")
        //        .Body($"Gentile {prestito.NomeUtente},\n\nLe confermiamo il prestito del libro '{libro.Titolo}'.\nLa data di restituzione è {prestito.DataRestituzione:dd/MM/yyyy}.\n\nGrazie.")
        //        .SendAsync();

        //    return true;
        //}

        public async Task<bool> AddLoanAsync(AddPrestitoViewModel model)
        {
            var libro = await _libroService.GetLibroByIdAsync(model.LibroId);
            if (libro == null || !libro.Disponibile)
            {
                throw new InvalidOperationException("Il libro non è disponibile per il prestito");
            }

            var prestito = new Prestito
            {
                Id = Guid.NewGuid(),
                NomeUtente = model.NomeUtente,
                EmailUtente = model.EmailUtente,
                DataPrestito = DateTime.Now,
                DataRestituzione = DateTime.Now.AddDays(14),
                LibroId = model.LibroId,
                Libro = libro
            };

            libro.Disponibile = false;

            if (model.ImmagineCopertinaFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var uniqueFileName = $"{Guid.NewGuid()}_{model.ImmagineCopertinaFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImmagineCopertinaFile.CopyToAsync(fileStream);
                }

                libro.PercorsoImmagineCopertina = $"/images/{uniqueFileName}";
            }

            var libroViewModel = new EditLibroViewModel
            {
                Id = libro.Id,
                Titolo = libro.Titolo,
                Autore = libro.Autore,
                Genere = libro.Genere,
                Descrizione = libro.Descrizione,
                Prezzo = libro.Prezzo,
                Disponibile = libro.Disponibile,
                PercorsoImmagineCopertina = libro.PercorsoImmagineCopertina
            };

            await _libroService.UpdateLibroAsync(libroViewModel);
            _context.Prestiti.Add(prestito);
            return await _context.SaveChangesAsync() > 0;
        }


        //public async Task<bool> ReturnBookAsync(UpdatePrestitoViewModel model)
        //{
        //    var prestito = await _context.Prestiti.FindAsync(model.Id);
        //    if (prestito == null)
        //        throw new InvalidOperationException("Prestito non trovato");

        //    prestito.DataRestituzioneEffettiva = model.DataRestituzioneEffettiva ?? DateTime.Now;

        //    var libro = await _libroService.GetLibroByIdAsync(prestito.LibroId);
        //    if (libro == null)
        //        throw new InvalidOperationException("Libro non trovato");

        //    libro.Disponibile = true;
        //    await _libroService.UpdateLibroAsync(libro);
        //    _context.Entry(prestito).State = EntityState.Modified;

        //    return await _context.SaveChangesAsync() > 0;
        //}

        //public async Task<bool> ReturnBookAsync(UpdatePrestitoViewModel model)
        //{
        //    var prestito = await _context.Prestiti.FindAsync(model.Id);
        //    if (prestito == null)
        //        throw new InvalidOperationException("Prestito non trovato");

        //    prestito.DataRestituzioneEffettiva = model.DataRestituzioneEffettiva ?? DateTime.Now;

        //    var libro = await _libroService.GetLibroByIdAsync(prestito.LibroId);
        //    if (libro == null)
        //        throw new InvalidOperationException("Libro non trovato");

        //    libro.Disponibile = true;

        //    var libroViewModel = new EditLibroViewModel
        //    {
        //        Id = libro.Id,
        //        Titolo = libro.Titolo,
        //        Autore = libro.Autore,
        //        Genere = libro.Genere,
        //        Descrizione = libro.Descrizione,
        //        Prezzo = libro.Prezzo,
        //        Disponibile = libro.Disponibile,
        //        PercorsoImmagineCopertina = libro.ImmagineCopertina
        //    };

        //    await _libroService.UpdateLibroAsync(libroViewModel);

        //    _context.Entry(prestito).State = EntityState.Modified;

        //    return await _context.SaveChangesAsync() > 0;
        //}

        //public async Task SendReminderEmailAsync(Guid prestitoId)
        //{
        //    var prestito = await GetLoanByIdAsync(prestitoId);
        //    if (prestito == null)
        //    {
        //        throw new InvalidOperationException("Prestito non trovato");
        //    }

        //    await _fluentEmail
        //        .To(prestito.EmailUtente)
        //        .Subject($"Promemoria restituzione: {prestito.Libro.Titolo}")
        //        .Body($"Gentile {prestito.NomeUtente},\n\nLe ricordiamo che il prestito del libro '{prestito.Libro.Titolo}' scadrà il {prestito.DataRestituzione:dd/MM/yyyy}.\nLa preghiamo di procedere alla restituzione entro tale data.\n\nGrazie per la collaborazione.\n\nBiblioteca App")
        //        .SendAsync();
        //}

        public async Task<bool> UpdateLoanAsync(UpdatePrestitoViewModel model)
        {
            var prestito = await _context.Prestiti.FindAsync(model.Id);
            if (prestito == null)
            {
                throw new InvalidOperationException("Prestito non trovato");
            }

            prestito.NomeUtente = model.NomeUtente;
            prestito.EmailUtente = model.EmailUtente;
            prestito.DataRestituzioneEffettiva = model.DataRestituzioneEffettiva;
            prestito.Note = model.Note;

            _context.Prestiti.Update(prestito);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> ReturnBookAsync(UpdatePrestitoViewModel model)
        {
            var prestito = await _context.Prestiti.FindAsync(model.Id);
            if (prestito == null)
                throw new InvalidOperationException("Prestito non trovato");

            prestito.DataRestituzioneEffettiva = model.DataRestituzioneEffettiva ?? DateTime.Now;

            var libro = await _libroService.GetLibroByIdAsync(prestito.LibroId);
            if (libro == null)
                throw new InvalidOperationException("Libro non trovato");

            libro.Disponibile = true;

            var libroViewModel = new EditLibroViewModel
            {
                Id = libro.Id,
                Titolo = libro.Titolo,
                Autore = libro.Autore,
                Genere = libro.Genere,
                Descrizione = libro.Descrizione,
                Prezzo = libro.Prezzo,
                Disponibile = libro.Disponibile,
                PercorsoImmagineCopertina = libro.PercorsoImmagineCopertina
            };

            await _libroService.UpdateLibroAsync(libroViewModel);
            _context.Entry(prestito).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Prestito>> GetOverdueLoansAsync()
        {
            var today = DateTime.Today;
            return await _context.Prestiti
                .Include(l => l.Libro)
                .Where(l => l.DataRestituzione < today && l.DataRestituzioneEffettiva == null)
                .ToListAsync();
        }

        public async Task<List<Prestito>> GetLoansByBookIdAsync(Guid libroId)
        {
            return await _context.Prestiti
                .Include(l => l.Libro)
                .Where(l => l.LibroId == libroId)
                .ToListAsync();
        }
    }
}