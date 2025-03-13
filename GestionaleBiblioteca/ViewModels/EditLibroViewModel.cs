namespace GestionaleBiblioteca.ViewModels
{
    public class EditLibroViewModel
    {
        public required Guid Id { get; set; }

        public required string Titolo { get; set; }

        public required string Autore { get; set; }

        public required string Genere { get; set; }

        public required string Descrizione { get; set; }

        public required double Prezzo { get; set; }

        public required bool Disponibile  { get; set; }

        public string? PercorsoImmagineCopertina { get; set; }

    }
}
