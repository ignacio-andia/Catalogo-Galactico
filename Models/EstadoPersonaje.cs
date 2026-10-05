using System.ComponentModel;

namespace CatalogoGalactico.Models
{
    [Description("Estado actual del personaje. Valores: 0 = Vivo, 1 = Muerto, 2 = Desconocido.")]
    public enum EstadoPersonaje
    {
        Vivo,
        Muerto,
        Desconocido
    }
}
