using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionaleBiblioteca.Models
{
    public class Libro
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Il titolo è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
        public required string Titolo { get; set; }

        [Required(ErrorMessage = "L'autore è obbligatorio")]
        [StringLength(100, ErrorMessage = "L'autore non può superare i 100 caratteri")]
        public required string Autore { get; set; }

        [Required(ErrorMessage = "Il genere è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il genere non può superare i 50 caratteri")]
        [Display(Name = "Genere")]
        public required string Genere { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatorio")]
        [StringLength(1000, ErrorMessage = "La descrizione non può superare i 1000 caratteri")]
        public required string Descrizione { get; set; }


        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Range(1, 5000, ErrorMessage = "Il prezzo non può superare i 5000 euro")]
        public double Prezzo { get; set; }

        public required bool Disponibile { get; set; }

        [NotMapped]
        [Display(Name = "Carica copertina")]
        public IFormFile? ImmagineCopertina { get; set; }

        public string? PercorsoImmagineCopertina { get; set; }

        public virtual ICollection<Prestito>? Prestiti { get; set; }

    }
}
