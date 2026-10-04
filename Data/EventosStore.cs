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
                Resultado = ""
            },
            new Evento
            {
                Id = 2,
                Nombre = "Batalla de Hoth",
                fecha = 3,
                Ubicacion = "Hoth",
                Descripcion = "La Batalla de Hoth fue un enfrentamiento militar entre el Imperio Galáctico y la Alianza Rebelde durante la Guerra Civil Galáctica. La batalla tuvo lugar en el planeta helado de Hoth, donde la Alianza Rebelde había establecido una base secreta.",
                Participantes = new List<int> { 4, 5, 6 },
                Resultado = ""
            },
            new Evento
            {
                Id = 3,
                Nombre = "Enfrentamiento en Tatooine",
                fecha = 5,
                Ubicacion = "Tatooine",
                Descripcion = "Un enfrentamiento entre fuerzas rebeldes e imperiales en el planeta Tatooine, con la participación de un personaje neutral.",
                Participantes = new List<int> { 7, 8, 9 },
                Resultado = ""
            },
            new Evento
            {
                Id = 4,
                Nombre = "Incidente en Endor",
                fecha = 7,
                Ubicacion = "Endor",
                Descripcion = "Un nuevo enfrentamiento entre la Alianza Rebelde y el Imperio Galáctico en las cercanías de Endor.",
                Participantes = new List<int> { 1, 6, 8 },
                Resultado = ""
            },
            new Evento
            {
                Id = 5,
                Nombre = "Operación de la Rebelión",
                fecha = 9,
                Ubicacion = "Naboo",
                Descripcion = "Una operación de la Alianza Rebelde que reúne a varios de sus miembros para enfrentarse a fuerzas imperiales.",
                Participantes = new List<int> { 3, 5, 7 },
                Resultado = ""
            }
        };

        public static int SiguienteIDEvento => Eventos.Count > 0 ? Eventos.Max(e => e.Id) + 1 : 1;
    }
}
