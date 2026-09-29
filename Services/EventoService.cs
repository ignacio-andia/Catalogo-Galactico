using CatalogoGalactico.Data;
using CatalogoGalactico.Models;
using static CatalogoGalactico.Models.Evento;

namespace CatalogoGalactico.Services
{
    public class EventoService
    {
        // get
        public List<Evento> ObetnerTodo()
        {
            return EventosStore.Eventos; 
        }


        // get{id}
        public Evento? ObtenerPordId(int id)
        {
            var evento = EventosStore.Eventos.FirstOrDefault(e => e.Id == id);
            return evento;
        }


        //post 
        public Evento Crear(CrearEventoDTO DTO)
        {
            foreach (var participanteId in DTO.Participantes)
            {
                var existe = PersonajesStore.Personajes.Any(p => p.Id == participanteId);
                if (!existe)
                {
                    throw new ArgumentException($" no Exite el participante con Id {participanteId}");
                }
            }

            var Nuevo = new Evento
            {
                Id = EventosStore.SiguienteIDEvento,
                Nombre = DTO.Nombre,
                fecha = DTO.fecha,
                Ubicacion = DTO.Ubicacion,
                Descripcion = DTO.Descripcion,
                Participantes = DTO.Participantes,
                Resultado = DTO.Resultado,
            };
            EventosStore.Eventos.Add(Nuevo);
            return Nuevo;
        }


        //put
        public Evento? Modificar(int id, CrearEventoDTO DTO)
        {
            var evento = ObtenerPordId(id);
            if (evento is null)
            {
                return evento;
            }

            foreach (var participanteId in DTO.Participantes)
            {
                var existe = PersonajesStore.Personajes.Any(p => p.Id == participanteId);
                if (!existe)
                {
                    throw new ArgumentException($" no Exite el participante con Id {participanteId}");
                }
            }

            evento.Nombre = DTO.Nombre;
            evento.fecha = DTO.fecha;
            evento.Ubicacion = DTO.Ubicacion;
            evento.Descripcion = DTO.Descripcion;
            evento.Participantes = DTO.Participantes;
            evento.Resultado = DTO.Resultado;

            return evento;
        }

        public List<Evento> ObtenerEventosPorPersonaje(int idPersonaje)
        {
            var personajeExiste = PersonajesStore.Personajes.Any(p => p.Id == idPersonaje);
            if (!personajeExiste)
            {
                throw new KeyNotFoundException($"No existe Personaje con ID {idPersonaje}");
            }
            return EventosStore.Eventos.Where(e => e.Participantes.Contains(idPersonaje)).ToList();
        }

        public Personaje? ObtenerMVP(int idEvento)
        {
            var evento = ObtenerPordId(idEvento);
            if (evento is null)
            { 
                return null;
            }

            foreach (var participanteId in evento.Participantes) // revisar que todos los personajes tengan card
            {
                var tieneCard = CardsPersonajesStore.cardsPersonajes
                    .Any(c => c.PersonajeId == participanteId);

                if (!tieneCard)
                {
                    throw new ArgumentException(
                        $"El personaje con Id {participanteId} no tiene Card.");
                }
            }

            var mvp = evento.Participantes
            .Join(
            CardsPersonajesStore.cardsPersonajes,
            participanteId => participanteId,
            card => card.PersonajeId,
            (participanteId, card) => new
            {
                PersonajeId = participanteId,
                Poder = card.poder
            }
            ).OrderByDescending(x => x.Poder).FirstOrDefault();

            if (mvp is null)
            {
                return null;
            }

            return PersonajesStore.Personajes.First(p => p.Id == mvp.PersonajeId);
        }


    }
}
