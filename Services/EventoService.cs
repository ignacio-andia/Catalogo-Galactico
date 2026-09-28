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

            evento.Nombre = DTO.Nombre;
            evento.fecha = DTO.fecha;
            evento.Ubicacion = DTO.Ubicacion;
            evento.Descripcion = DTO.Descripcion;
            evento.Participantes = DTO.Participantes;
            evento.Resultado = DTO.Resultado;

            return evento;
        }
    }
}
