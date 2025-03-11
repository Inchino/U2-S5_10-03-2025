using System.ComponentModel.DataAnnotations;

namespace GestionaleBiblioteca.ViewModels
{
    public class LibroDettagliViewModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        public string? Author { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public string? Category { get; set; }
    }
}
