using CatalogoGalactico.Data;
using CatalogoGalactico.Models;
using static CatalogoGalactico.Models.Personaje;

namespace CatalogoGalactico.Services
{
    public class PersonajeSerevice
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
            personaje.Nombre = DTO.Nombre;
            personaje.Especie = DTO.Especie;
            personaje.Faccion = DTO.Faccion;
            personaje.Afiliacion = DTO.Afiliacion;
            personaje.Estado = DTO.Estado;
            personaje.FuerzaSensitiva = DTO.FuerzaSensitiva;

            return personaje;

        }


        public bool Eliminar(int id) //Delete
        {
            var personaje = obtenerPorID(id);
            if (personaje is null)
            {
                return false;
            }
            PersonajesStore.Personajes.Remove(personaje);
            return true;
        }
    }
}
