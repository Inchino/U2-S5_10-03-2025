namespace GestionaleBiblioteca.ViewModels;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PrestitoViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Il nome utente è obbligatorio")]
    public string NomeUtente { get; set; }

    [Required(ErrorMessage = "L'email è obbligatoria")]
    [EmailAddress(ErrorMessage = "Inserisci un'email valida")]
    public string EmailUtente { get; set; }

    [Required(ErrorMessage = "La data del prestito è obbligatoria")]
    [DataType(DataType.DateTime)]
    public DateTime DataPrestito { get; set; }

    [Required(ErrorMessage = "La data di restituzione è obbligatoria")]
    [DataType(DataType.DateTime)]
    public DateTime? DataRestituzione { get; set; }

    public DateTime? DataRestituzioneEffettiva { get; set; }
    public string? Note { get; set; }

    [Required(ErrorMessage = "Devi selezionare un libro")]
    public Guid LibroId { get; set; }

    public string TitoloLibro { get; set; } // Per mostrare il titolo del libro nella vista

    public bool Scaduto { get; set; }

    public List<SelectListItem>? LibriDisponibili { get; set; } // Per dropdown lista libri disponibili
}
