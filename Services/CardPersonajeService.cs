using CatalogoGalactico.Data;
using CatalogoGalactico.Models;
using static CatalogoGalactico.Models.CardsPersonaje;

namespace CatalogoGalactico.Services
{
    public class CardPersonajeService
    {
        // get
        public List<CardsPersonaje> MostrarTodo()
        {
            return CardsPersonajesStore.cardsPersonajes; 
        }

        // get {id}
        public CardsPersonaje? ObtenerPorId(int id)
        {
            var card = CardsPersonajesStore.cardsPersonajes.FirstOrDefault(c => c.Id == id);
            return card;
        }

        // post 
        public CardsPersonaje Crear(CrearCardsPersonajeDTO DTO)
        {
            var Nuevo = new CardsPersonaje
            {
                Id = CardsPersonajesStore.SiguienteIDCards,
                PersonajeId = DTO.PersonajeId,
                poder = DTO.poder,
                HabilidadEspecial = DTO.HabilidadEspecial,
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
                return Card;
            }

            Card.PersonajeId = DTO.PersonajeId;
            Card.poder = DTO.poder;
            Card.HabilidadEspecial = DTO.HabilidadEspecial;
            Card.NivelDePeligrosidad = DTO.NivelDePeligrosidad;
            Card.ImegenURL = DTO.ImegenURL;

            return Card;

        }



    }
}
