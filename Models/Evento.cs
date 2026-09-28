using CatalogoGalactico.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Specialized;

namespace CatalogoGalactico.Models
{
    public class Evento
    {
        public int Id { get; init; }
        public string Nombre { get; set; } = string.Empty;
        public int fecha { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<int> Participantes { get; set; } = new List<int>();
        public string Resultado { get; set; } = string.Empty;

    }
}
