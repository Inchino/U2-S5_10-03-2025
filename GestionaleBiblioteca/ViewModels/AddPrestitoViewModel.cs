using System.ComponentModel.DataAnnotations.Schema;

namespace GestionaleBiblioteca.ViewModels
{
    public class AddPrestitoViewModel
    {
        //public string NomeUtente { get; set; } = string.Empty;
        //public string EmailUtente { get; set; } = string.Empty;
        //public Guid LibroId { get; set; }
        public string NomeUtente { get; set; } = string.Empty;
        public string EmailUtente { get; set; } = string.Empty;
        public Guid LibroId { get; set; }

        [NotMapped]
        public IFormFile? ImmagineCopertinaFile { get; set; }
    }
}
