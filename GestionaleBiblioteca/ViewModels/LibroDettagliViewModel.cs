using System.ComponentModel.DataAnnotations;

namespace GestionaleBiblioteca.ViewModels
{
    public class LibroDettagliViewModel
    {
        public Guid? Id { get; set; }
        public string? Titolo { get; set; }

        public string? Autore { get; set; }

        public string? Genere { get; set; }

        public string? Descrizione { get; set; }

        public double? Prezzo { get; set; }

        public required bool? Disponibile { get; set; }

        public string? PercorsoImmagineCopertina { get; set; }

    }
}
