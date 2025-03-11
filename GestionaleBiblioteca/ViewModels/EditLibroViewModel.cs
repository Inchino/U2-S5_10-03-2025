namespace GestionaleBiblioteca.ViewModels
{
    public class EditLibroViewModel
    {
        public required Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Author { get; set; }

        public required string Description { get; set; }

        public required double Price { get; set; }

        public required string Category { get; set; }

    }
}
