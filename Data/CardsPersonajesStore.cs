using CatalogoGalactico.Models;

namespace CatalogoGalactico.Data
{
    public static class CardsPersonajesStore
    {
        public static List<CardsPersonaje> cardsPersonajes { get; } = new List<CardsPersonaje>()
        {
            new CardsPersonaje(){Id = 1,PersonajeId = 1,poder = 90,HabilidadEspecial = "Control de la Fuerza",Arma = "Espada laser",NivelDePeligrosidad = 8,ImegenURL = "https://upload.wikimedia.org/wikipedia/en/9/9b/Luke_Skywalker.png"},
            new CardsPersonaje(){Id = 2,PersonajeId = 2,poder = 95,HabilidadEspecial = "Control de la Fuerza",Arma = "Espada laser",NivelDePeligrosidad = 9,ImegenURL = "https://upload.wikimedia.org/wikipedia/en/7/7b/Darth_Vader.jpg"},
            new CardsPersonaje(){Id = 3,PersonajeId = 3,poder = 85,HabilidadEspecial = "Control de la Fuerza",Arma = "Espada laser",NivelDePeligrosidad = 7,ImegenURL = "https://upload.wikimedia.org/wikipedia/en/8/87/Leia_Organa.png"},
            new CardsPersonaje(){Id = 4,PersonajeId = 4,poder = 80,HabilidadEspecial = "Estocada Relampago",Arma = "Espada laser",NivelDePeligrosidad = 7,ImegenURL = "sin imagen"},
            new CardsPersonaje(){Id = 5,PersonajeId = 5,poder = 45,HabilidadEspecial = "Silencio Mortal",Arma = "Blaster",NivelDePeligrosidad = 6,ImegenURL = "sin imagen"},
            new CardsPersonaje(){Id = 6,PersonajeId = 6,poder = 150,HabilidadEspecial = "Mano del Gobernante",Arma = "Espada laser",NivelDePeligrosidad = 9,ImegenURL = "sin imagen"},
            new CardsPersonaje(){Id = 7,PersonajeId = 7,poder = 110,HabilidadEspecial = "Disparo de Precision",Arma = "Blaster",NivelDePeligrosidad = 8,ImegenURL = "sin imagen"},
            new CardsPersonaje(){Id = 8,PersonajeId = 8,poder = 70,HabilidadEspecial = "Negociacion Pirata",Arma = "Blaster",NivelDePeligrosidad = 5,ImegenURL = "sin imagen"},
            new CardsPersonaje(){Id = 9,PersonajeId = 9,poder = 100,HabilidadEspecial = "Dominio de la Fuerza",Arma = "Espada laser",NivelDePeligrosidad = 8,ImegenURL = "sin imagen"}
        };

        public static int SiguienteIDCards => cardsPersonajes.Count > 0 ? cardsPersonajes.Max(c => c.Id) + 1 : 1;
    }
}
