using CatalogoGalactico.Data;
using CatalogoGalactico.Models;
using static CatalogoGalactico.Models.Personaje;

namespace CatalogoGalactico.Services
{
    public class PersonajeService
    {
        public List<Personaje> MostrarTodos() // get
        {
            return PersonajesStore.Personajes;
        }

        public Personaje? obtenerPorID(int Id) // get
        {
            var personaje = PersonajesStore.Personajes.FirstOrDefault(p => p.Id == Id);
            return personaje;
        }


        //GET /personajes/{id}/con-card

        public Personaje? ObtenerPersonajeConcard(int id)
        {
            var personajes = PersonajesStore.Personajes;

            var personaje = personajes
                .Join(
                    CardsPersonajesStore.cardsPersonajes,
                    personaje => personaje.Id,
                    card => card.PersonajeId,
                    (personaje, card) => personaje
                )
                .FirstOrDefault(personaje => personaje.Id == id);

            return personaje;
        }


        public Personaje Crear(CrearPersonajeDTO DTO) //post
        {
            var Nuevo = new Personaje
            {
                Id = PersonajesStore.SiguienteID,
                Nombre = DTO.Nombre,
                Especie = DTO.Especie,
                Faccion = DTO.Faccion,
                Afiliacion = DTO.Afiliacion,
                Estado = DTO.Estado,
                FuerzaSensitiva = DTO.FuerzaSensitiva
            };
            PersonajesStore.Personajes.Add(Nuevo);
            return Nuevo;
        
        }

        public Personaje? Modificar(int id, CrearPersonajeDTO DTO) // put
        {
            var personaje = obtenerPorID(id);
            if (personaje is null)
            {
                return personaje;
            }

            if (personaje.Estado == EstadoPersonaje.Muerto && DTO.Estado != EstadoPersonaje.Muerto)
            {
                throw new ArgumentException(
                    "Un personaje muerto no puede cambiar su estado.");
            }

            personaje.Nombre = DTO.Nombre;
            personaje.Especie = DTO.Especie;
            personaje.Faccion = DTO.Faccion;
            personaje.Afiliacion = DTO.Afiliacion;
            personaje.Estado = DTO.Estado;
            personaje.FuerzaSensitiva = DTO.FuerzaSensitiva;

            return personaje;

        }


        public bool Eliminar(int id) // delete
        {
            var personaje = obtenerPorID(id);

            if (personaje is null)
            {
                return false;
            }

            // Verificar si el personaje participa en algún evento
            var estaEnEvento = EventosStore.Eventos
                .Any(e => e.Participantes.Contains(id));

            if (estaEnEvento)
            {
                throw new ArgumentException(
                    $"El personaje con Id {id} no puede eliminarse porque está registrado en un evento.");
            }

            // Verificar si el personaje tiene una Card asociada
            var tieneCard = CardsPersonajesStore.cardsPersonajes
                .Any(c => c.PersonajeId == id);

            if (tieneCard)
            {
                throw new ArgumentException(
                    $"El personaje con Id {id} no puede eliminarse porque tiene una Card asociada.");
            }

            // Si no participa en eventos y no tiene Card, se elimina
            PersonajesStore.Personajes.Remove(personaje);

            return true;
        }

        public List<Personaje> RankingPorPoder()
        {
            return PersonajesStore.Personajes.Join(
                CardsPersonajesStore.cardsPersonajes,
                personaje => personaje.Id,
                card => card.PersonajeId,
                (personaje, card) => new
                {
                    Personaje = personaje,
                    Poder = card.poder
                }
                ).OrderByDescending(x => x.Poder).Select(x => x.Personaje).ToList();
        }


        public List<Personaje> Filtrar(Faccion? faccion, bool? fuerzaSensitivo)
        {
            var personajes = PersonajesStore.Personajes.AsEnumerable();

            if (faccion.HasValue)
            {
                personajes = personajes.Where(p => p.Faccion == faccion.Value);
            }

            if (fuerzaSensitivo.HasValue)
            {
                personajes = personajes.Where(p => p.FuerzaSensitiva == fuerzaSensitivo.Value);
            }

            return personajes.ToList();
        }

    }
}
