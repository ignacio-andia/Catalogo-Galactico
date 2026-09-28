namespace CatalogoGalactico.Models
{
    public class CardsPersonaje
    {
        public int Id { get; init; }
        public int PersonajeId { get; set; }
        public int poder { get; set; }
        public string HabilidadEspecial { get; set; } = string.Empty;
        public int NivelDePeligrosidad { get; set; }
        public string ImegenURL { get; set; } = string.Empty;

        public record CrearCardsPersonajeDTO(int PersonajeId, int poder, string HabilidadEspecial, int NivelDePeligrosidad, string ImegenURL);
    }
}
