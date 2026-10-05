using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CatalogoGalactico.Models
{
    public class CardsPersonaje
    {
        [Description("Identificador unico de la Card.")]
        public int Id { get; init; }

        [Description("Identificador del personaje al que pertenece la Card.")]
        public int PersonajeId { get; set; }

        [Description("Nivel de poder del personaje representado por la Card.")]
        public int poder { get; set; }

        [Description("Habilidad especial del personaje.")]
        public string HabilidadEspecial { get; set; } = string.Empty;

        [Description("Arma utilizada por el personaje.")]
        public string Arma { get; set; } = string.Empty;

        [Description("Nivel de peligrosidad asociado al personaje.")]
        public int NivelDePeligrosidad { get; set; }

        [Description("URL de la imagen asociada al personaje.")]
        public string ImegenURL { get; set; } = string.Empty;



        public record CrearCardsPersonajeDTO(
        [property: Description("Identificador del personaje al que se asociara la Card.")]
        int PersonajeId,
        [property: Description("Nivel de poder del personaje.")]
        int poder,
        [property: Description("Habilidad especial del personaje.")]
        string HabilidadEspecial,
        [property: Description("Arma utilizada por el personaje.")]
        string Arma,
        [property: Description("Nivel de peligrosidad asociado al personaje.")]
        int NivelDePeligrosidad,
        [property: Description("URL de la imagen asociada al personaje.")]
        string ImegenURL);
    }
}
