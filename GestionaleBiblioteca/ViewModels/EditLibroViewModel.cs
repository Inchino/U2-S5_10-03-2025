using System.ComponentModel.DataAnnotations.Schema;

namespace GestionaleBiblioteca.ViewModels
{
    public class EditLibroViewModel
    {
        //public required Guid Id { get; set; }
        //public required string Titolo { get; set; }
        //public required string Autore { get; set; }
        //public required string Genere { get; set; }
        //public required string Descrizione { get; set; }
        //public required double Prezzo { get; set; }
        //public required bool Disponibile { get; set; }
        //public string? PercorsoImmagineCopertina { get; set; }
            public Guid Id { get; set; }
            public string Titolo { get; set; } = string.Empty;
            public string Autore { get; set; } = string.Empty;
            public string Genere { get; set; } = string.Empty;
            public string Descrizione { get; set; } = string.Empty;
            public double Prezzo { get; set; }
            public bool Disponibile { get; set; }

            public string? PercorsoImmagineCopertina { get; set; } // Percorso del file salvato

            [NotMapped]
            public IFormFile? ImmagineCopertinaFile { get; set; } // File caricato dall'utente

    }
}
