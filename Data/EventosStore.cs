using CatalogoGalactico.Models;
using System.Diagnostics;

namespace CatalogoGalactico.Data
{
    public static class EventosStore
    {
        public static List<Evento> Eventos { get; set; } = new List<Evento>
        {
            new Evento
            {
                Id = 1,
                Nombre = "Batalla de Yavin",
                fecha = 0,
                Ubicacion = "Yavin IV",
                Descripcion = "La Batalla de Yavin fue un enfrentamiento crucial entre la Alianza Rebelde y el Imperio Galáctico durante la Guerra Civil Galáctica. La batalla tuvo lugar en el sistema Yavin, donde la Alianza Rebelde lanzó un ataque sorpresa contra la estación espacial del Imperio, la Estrella de la Muerte.",
                Participantes = new List<int> { 1, 2, 3 },
                Resultado = "Victoria de la Alianza Rebelde"
            },
            new Evento
            {
                Id = 2,
                Nombre = "Batalla de Hoth",
                fecha = 3,
                Ubicacion = "Hoth",
                Descripcion = "La Batalla de Hoth fue un enfrentamiento militar entre el Imperio Galáctico y la Alianza Rebelde durante la Guerra Civil Galáctica. La batalla tuvo lugar en el planeta helado de Hoth, donde la Alianza Rebelde había establecido una base secreta.",
                Participantes = new List<int> { 4, 5, 6 },
                Resultado = "Victoria del Imperio Galáctico"
            }
        };

        public static int SiguienteIDEvento => Eventos.Count > 0 ? Eventos.Max(e => e.Id) + 1 : 1;
    }
}
