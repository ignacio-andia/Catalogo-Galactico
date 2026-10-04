using CatalogoGalactico.Models;

namespace CatalogoGalactico.Data
{
    public static class PersonajesStore
    {
        public static List<Personaje> Personajes { get; } = new List<Personaje>()
        {
            new Personaje(){Id = 1,Nombre = "Luke Skywalker",Especie = "Humano",Faccion = Faccion.Rebelde,Afiliacion = "Jedi",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = true},
            new Personaje(){Id = 2,Nombre = "Darth Vader",Especie = "Humano",Faccion = Faccion.Imperio,Afiliacion = "Sith",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = true},
            new Personaje(){Id = 3,Nombre = "Leia Organa",Especie = "Humano",Faccion = Faccion.Rebelde,Afiliacion = "Rebelión",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = true},
            new Personaje(){Id = 4,Nombre = "Yoda",Especie = "Nomo",Faccion = Faccion.Rebelde,Afiliacion = "Jedi",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = true},
            new Personaje(){Id = 5,Nombre = "Miki",Especie = "Humano",Faccion = Faccion.Rebelde,Afiliacion = "Jedi",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = false},
            new Personaje(){Id = 6,Nombre = "Koyun",Especie = "Humano",Faccion = Faccion.Imperio,Afiliacion = "Sith",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = false},
            new Personaje(){Id = 7,Nombre = "Boba Fett",Especie = "Humano",Faccion = Faccion.Imperio,Afiliacion = "Cazarrecompensas",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = false},
            new Personaje(){Id = 8,Nombre = "Hondo Ohnaka",Especie = "Weequay",Faccion = Faccion.Neutral,Afiliacion = "Piratas",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = false},
            new Personaje(){Id = 9,Nombre = "Ahsoka Tano",Especie = "Togruta",Faccion = Faccion.Rebelde,Afiliacion = "Jedi",Estado = EstadoPersonaje.Desconocido,FuerzaSensitiva = true},
            new Personaje(){Id = 10,Nombre = "Mon Mothma",Especie = "Humano",Faccion = Faccion.Rebelde,Afiliacion = "Rebelión",Estado = EstadoPersonaje.Vivo,FuerzaSensitiva = false}
        };

        public static int SiguienteID =>Personajes.Count > 0? Personajes.Max(p => p.Id) + 1: 1;
    }
}