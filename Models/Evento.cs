using CatalogoGalactico.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CatalogoGalactico.Models
{
    public class Evento
    {
        [Description("Identificador unico del evento.")]
        public int Id { get; init; }

        [Description("Nombre del evento registrado.")]
        public string Nombre { get; set; } = string.Empty;

        [Description("Fecha del evento expresada como numero de año en la cronologia del catalogo galactico.")]
        public int fecha { get; set; }

        [Description("Ubicacion donde ocurre el evento.")]
        public string Ubicacion { get; set; } = string.Empty;

        [Description("Descripcion del hecho o acontecimiento registrado.")]
        public string Descripcion { get; set; } = string.Empty;

        [Description("Lista de identificadores de los personajes que participan en el evento.")]
        public List<int> Participantes { get; set; } = new List<int>();

        [Description("Resultado registrado para el evento. Puede permanecer vacio mientras el evento no haya sido simulado.")]
        public string Resultado { get; set; } = string.Empty;

        public record CrearEventoDTO(
        [property: Description("Nombre del evento.")]
        string Nombre,
        [property: Description("Fecha del evento expresada como numero de año en la cronologia del catalogo galactico.")]
        int fecha,
        [property: Description("Ubicacion donde ocurre el evento.")]
        string Ubicacion,
        [property: Description("Descripcion del hecho o acontecimiento registrado.")]
        string Descripcion,
        [property: Description("Lista de identificadores de los personajes que participan en el evento.")]
        List<int> Participantes,
        [property: Description("Resultado del evento. Puede permanecer vacio antes de ejecutar una simulacion.")]
        string Resultado);

    }
}
