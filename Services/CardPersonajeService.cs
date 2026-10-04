using CatalogoGalactico.Data;
using CatalogoGalactico.Models;
using System.Data;
using static CatalogoGalactico.Models.CardsPersonaje;

namespace CatalogoGalactico.Services
{
    public class CardPersonajeService
    {
        // GET
        public List<CardsPersonaje> MostrarTodo()
        {
            return CardsPersonajesStore.cardsPersonajes;
        }

        // GET {id}
        public CardsPersonaje? ObtenerPorId(int id)
        {
            var card = CardsPersonajesStore.cardsPersonajes
                .FirstOrDefault(c => c.Id == id);

            return card;
        }

        // POST
        public CardsPersonaje Crear(CrearCardsPersonajeDTO DTO)
        {
            var personajeExiste = PersonajesStore.Personajes
                .Any(p => p.Id == DTO.PersonajeId);

            if (!personajeExiste)
            {
                throw new ArgumentException(
                    "El personaje indicado no existe");
            }

            var yaTieneCard = CardsPersonajesStore.cardsPersonajes
                .Any(c => c.PersonajeId == DTO.PersonajeId);

            if (yaTieneCard)
            {
                throw new ArgumentException(
                    "El personaje ya tiene una Card asociada");
            }

            var Nuevo = new CardsPersonaje
            {
                Id = CardsPersonajesStore.SiguienteIDCards,
                PersonajeId = DTO.PersonajeId,
                poder = DTO.poder,
                HabilidadEspecial = DTO.HabilidadEspecial,
                Arma = DTO.Arma,
                NivelDePeligrosidad = DTO.NivelDePeligrosidad,
                ImegenURL = DTO.ImegenURL
            };

            CardsPersonajesStore.cardsPersonajes.Add(Nuevo);

            return Nuevo;
        }


        // put
        public CardsPersonaje? Modificar(int id, CrearCardsPersonajeDTO DTO)
        {
            var Card = ObtenerPorId(id);

            if (Card is null)
            {
                return null;
            }

            // Verificar que el personaje exista
            var personajeExiste = PersonajesStore.Personajes
                .Any(p => p.Id == DTO.PersonajeId);

            if (!personajeExiste)
            {
                throw new ArgumentException(
                    "El personaje indicado no existe");
            }

            // Verificar que otro personaje no tenga ya esa Card
            var yaTieneOtraCard = CardsPersonajesStore.cardsPersonajes
                .Any(c =>
                    c.PersonajeId == DTO.PersonajeId &&
                    c.Id != id);

            if (yaTieneOtraCard)
            {
                throw new ArgumentException(
                    "El personaje ya tiene otra Card asociada");
            }

            Card.PersonajeId = DTO.PersonajeId;
            Card.poder = DTO.poder;
            Card.HabilidadEspecial = DTO.HabilidadEspecial;
            Card.Arma = DTO.Arma;
            Card.NivelDePeligrosidad = DTO.NivelDePeligrosidad;
            Card.ImegenURL = DTO.ImegenURL;

            return Card;

        }



    }
}
