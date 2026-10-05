using System.ComponentModel;
namespace CatalogoGalactico.Models
{
    public class Personaje
    {
        [Description("Identificador unico del personaje.")]
        public int Id { get; init; }

        [Description("Nombre del personaje.")]
        public string Nombre { get; set; } = string.Empty;

        [Description("Especie a la que pertenece el personaje.")]
        public string Especie { get; set; } = string.Empty;

        [Description("Faccion a la que pertenece el personaje: Rebelde, Imperio o Neutral.")]
        public Faccion Faccion { get; set; }

        [Description("Afiliacion, grupo u organizacion asociada al personaje.")]
        public string Afiliacion { get; set; } = string.Empty;

        [Description("Estado actual del personaje: Vivo, Muerto o Desconocido.")]
        public EstadoPersonaje Estado { get; set; }

        [Description("Indica si el personaje es sensible a la Fuerza.")]
        public bool FuerzaSensitiva { get; set; }

        [Description("Datos necesarios para crear un nuevo personaje.")]



        public record CrearPersonajeDTO(
        [property: Description("Nombre del personaje.")]
        string Nombre,
        [property: Description("Especie a la que pertenece el personaje.")]
        string Especie,
        [property: Description("Faccion del personaje: Rebelde, Imperio o Neutral.")]
        Faccion Faccion,
        [property: Description("Afiliacion, grupo u organizacion asociada al personaje.")]
        string Afiliacion,
        [property: Description("Estado inicial o actual del personaje: Vivo, Muerto o Desconocido.")]
        EstadoPersonaje Estado,
        [property: Description("Indica si el personaje es sensible a la Fuerza.")]
        bool FuerzaSensitiva);
    }
}
