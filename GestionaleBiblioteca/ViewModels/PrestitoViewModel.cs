namespace GestionaleBiblioteca.ViewModels;
public class PrestitoViewModel
{
    public Guid Id { get; set; }
    public string NomeUtente { get; set; } = string.Empty;
    public string EmailUtente { get; set; } = string.Empty;
    public DateTime DataPrestito { get; set; }
    public DateTime? DataRestituzione { get; set; }
    public DateTime? DataRestituzioneEffettiva { get; set; }
    public bool Scaduto { get; set; }
    public Guid LibroId { get; set; }
    public string TitoloLibro { get; set; } = string.Empty;
}
