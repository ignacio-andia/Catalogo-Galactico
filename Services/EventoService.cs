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


        private double GenerarFactorAleatorio()
        {
            return Random.Shared.NextDouble() * (1.10 - 0.90) + 0.90;
        }


        //--------------------------------------------------------
        public object Simular(int idEvento)
        {
            // Buscar el evento
            var evento = ObtenerPordId(idEvento);

            if (evento is null)
            {
                throw new KeyNotFoundException(
                    $"No existe Evento con Id {idEvento}");
            }


            // Verificar que tenga participantes
            if (evento.Participantes.Count == 0)
            {
                throw new ArgumentException(
                    "El evento no tiene participantes.");
            }


            // Obtener los personajes participantes
            var personajes = PersonajesStore.Personajes
                .Where(p => evento.Participantes.Contains(p.Id))
                .ToList();


            // Verificar que exista al menos un Rebelde y un Imperial
            if (!personajes.Any(p => p.Faccion == Faccion.Rebelde) ||
                !personajes.Any(p => p.Faccion == Faccion.Imperio))
            {
                throw new ArgumentException(
                    "El evento debe tener al menos un participante Rebelde y uno Imperial.");
            }


            // Verificar que todos tengan Card
            foreach (var personaje in personajes)
            {
                var tieneCard = CardsPersonajesStore.cardsPersonajes
                    .Any(c => c.PersonajeId == personaje.Id);

                if (!tieneCard)
                {
                    throw new ArgumentException(
                        $"El personaje con Id {personaje.Id} no tiene Card.");
                }
            }


            // Obtener las Cards de los participantes
            var participantesConCard = personajes
                .Join(
                    CardsPersonajesStore.cardsPersonajes,
                    personaje => personaje.Id,
                    card => card.PersonajeId,
                    (personaje, card) => new
                    {
                        Personaje = personaje,
                        Card = card
                    }
                )
                .ToList();


            // Calcular poder base por facción
            // Los neutrales NO participan en la batalla
            double poderRebelde = participantesConCard
                .Where(x => x.Personaje.Faccion == Faccion.Rebelde)
                .Sum(x => x.Card.poder);

            double poderImperio = participantesConCard
                .Where(x => x.Personaje.Faccion == Faccion.Imperio)
                .Sum(x => x.Card.poder);


            // Generar factores aleatorios
            double factorRebelde = GenerarFactorAleatorio();
            double factorImperio = GenerarFactorAleatorio();


            // Aplicar los factores
            double poderFinalRebelde = poderRebelde * factorRebelde;
            double poderFinalImperio = poderImperio * factorImperio;


            // Determinar ganador
            var poderesFinales = new Dictionary<Faccion, double>
    {
        { Faccion.Rebelde, poderFinalRebelde },
        { Faccion.Imperio, poderFinalImperio }
    };

            var ganador = poderesFinales
                .OrderByDescending(x => x.Value)
                .First();


            // Marcar como muertos a los participantes
            // de la facción perdedora.
            // Los neutrales nunca mueren por la simulación.
            foreach (var personaje in personajes)
            {
                if (personaje.Faccion != ganador.Key &&
                    personaje.Faccion != Faccion.Neutral)
                {
                    personaje.Estado = EstadoPersonaje.Muerto;
                }
            }


            // Guardar resultado en el evento
            evento.Resultado = $"Victoria de {ganador.Key}";


            // Devolver información de la simulación
            return new
            {
                eventoId = evento.Id,

                poderBase = new
                {
                    rebelde = poderRebelde,
                    imperio = poderImperio
                },

                factores = new
                {
                    rebelde = factorRebelde,
                    imperio = factorImperio
                },

                poderFinal = new
                {
                    rebelde = poderFinalRebelde,
                    imperio = poderFinalImperio
                },

                ganador = ganador.Key.ToString(),

                criterio = "Gana la facción con mayor poder final después de aplicar el factor aleatorio. Los participantes de la facción perdedora mueren; los neutrales no participan en el combate."
            };
        }



    }
}
