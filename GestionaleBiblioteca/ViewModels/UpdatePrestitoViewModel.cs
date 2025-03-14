namespace GestionaleBiblioteca.ViewModels
{
    public class UpdatePrestitoViewModel
    {
        public Guid Id { get; set; }
        public string NomeUtente { get; set; }
        public string EmailUtente { get; set; }
        public DateTime? DataRestituzioneEffettiva { get; set; }
        public string? Note { get; set; }
    }
}
