using System.ComponentModel.DataAnnotations;

namespace GestionaleBiblioteca.Models
{
    public class Libro
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [StringLength(10000)]
        public string Description { get; set; }

        [Required]
        [Range(1, 5000)]
        public double Price { get; set; }

        [Required]
        public required string Category { get; set; }
    }
}
