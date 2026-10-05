using CatalogoGalactico.Models;
using CatalogoGalactico.Services;
using static CatalogoGalactico.Models.Evento;

namespace CatalogoGalactico.EndPoints
{
    public static class EventoEndpoints
    {

        public static void MapEventoEndpoints(this WebApplication app)
        {
            // GET /eventos
            app.MapGet("/eventos", (EventoService service) =>
            {
                var eventos = service.ObetnerTodo();
                return Results.Ok(eventos);
            })
            .WithTags("Eventos")
            .WithSummary("Consultar todos los eventos")
            .WithDescription("""
            Obtiene todos los eventos registrados en el catalogo galactico.

            Cada evento contiene informacion sobre el hecho registrado, su ubicacion,
            su descripcion, su fecha, sus participantes y su resultado.

            La consulta es de solo lectura y no modifica los datos almacenados.

            Si existen eventos registrados, la API devuelve la coleccion completa.
            Si no existen eventos, devuelve una coleccion vacia.
            """)
            .Produces<List<Evento>>(StatusCodes.Status200OK);





            // GET /eventos/{id}
            app.MapGet("/eventos/{id}", (int id, EventoService service) =>
            {
                var evento = service.ObtenerPordId(id);

                if (evento is null)
                {
                    return Results.NotFound(new
                    {
                        error = $"No existe Evento con Id {id}"
                    });
                }

                return Results.Ok(evento);
            })
            .WithTags("Eventos")
            .WithSummary("Obtener un evento por ID")
            .WithDescription("""
            Obtiene la informacion completa de un evento mediante su identificador unico.

            El identificador corresponde al campo Id del evento y debe ser un valor entero.

            Si existe un evento con el identificador proporcionado, la API devuelve sus datos con HTTP 200 OK.

            Si no existe un evento con ese identificador, la API devuelve HTTP 404 Not Found.
            """)
            .Produces<Evento>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);





            // POST /eventos
            app.MapPost("/eventos", (CrearEventoDTO DTO, EventoService service) =>
            {
                try
                {
                    var evento = service.Crear(DTO);

                    return Results.Created(
                        $"/eventos/{evento.Id}",
                        evento
                    );
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            })
            .WithTags("Eventos")
            .WithSummary("Crear un nuevo evento")
            .WithDescription("""
            Registra un nuevo evento en el catalogo galactico.

            El identificador del evento es generado automaticamente por la aplicacion y no debe enviarse en la solicitud.

            El campo Participantes contiene los identificadores de los personajes relacionados con el evento.

            REGLAS DE NEGOCIO:
            - Cada participante indicado debe corresponder a un personaje existente.
            - Un personaje cuyo estado actual sea Muerto no puede participar en un nuevo evento.
            - La validacion de todos los participantes se realiza antes de almacenar el evento.

            RESPUESTAS:
            - 201 Created: el evento fue creado correctamente.
            - 400 Bad Request: uno o mas participantes no existen o alguno de ellos se encuentra muerto.

            La informacion se almacena en memoria y puede volver a su estado inicial al reiniciar la aplicacion.
            """)
            .Produces<Evento>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);





            // PUT /eventos/{id}
            app.MapPut("/eventos/{id}", (int id, CrearEventoDTO DTO, EventoService service) =>
            {
                try
                {
                    var evento = service.Modificar(id, DTO);

                    if (evento is null)
                    {
                        return Results.NotFound(new
                        {
                            error = $"No existe Evento con Id {id}"
                        });
                    }

                    return Results.Ok(evento);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            })
            .WithTags("Eventos")
            .WithSummary("Modificar un evento existente")
            .WithDescription("""
            Modifica los datos de un evento registrado en el catalogo galactico.

            El identificador del evento se recibe mediante la URL. Los demas datos se reciben
            en el cuerpo de la solicitud mediante CrearEventoDTO.

            REGLAS DE NEGOCIO:
            - El evento indicado debe existir.
            - Cada participante indicado debe corresponder a un personaje existente.
            - Un personaje cuyo estado actual sea Muerto no puede participar en el evento.
            - Las validaciones de los participantes se realizan antes de modificar los datos.
            - Si alguna validacion falla, el evento no se modifica.

            RESPUESTAS:
            - 200 OK: el evento fue modificado correctamente.
            - 400 Bad Request: uno o mas participantes no existen o alguno se encuentra muerto.
            - 404 Not Found: no existe un evento con el identificador indicado.

            La informacion se almacena en memoria y puede volver a su estado inicial
            al reiniciar la aplicacion.
            """)
            .Produces<Evento>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);






            // GET /eventos/{id}/mvp
            app.MapGet("/eventos/{id}/mvp", (int id, EventoService service) =>
            {
                try
                {
                    var mvp = service.ObtenerMVP(id);

                    if (mvp is null)
                    {
                        return Results.NotFound(new
                        {
                            error = $"No existe Evento con Id {id}"
                        });
                    }

                    return Results.Ok(mvp);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            })
            .WithTags("Eventos")
            .WithSummary("Obtener el MVP de un evento")
            .WithDescription("""
            Obtiene el personaje considerado MVP de un evento previamente simulado.

            El identificador del evento se recibe mediante la URL.

            REGLAS DE NEGOCIO:
            - El evento debe existir.
            - El evento debe haber sido simulado previamente y tener un resultado registrado.
            - Todos los participantes del evento deben tener una Card asociada.
            - Los personajes muertos y los participantes neutrales no pueden ser seleccionados como MVP.
            - Entre los participantes vivos de las facciones involucradas se determina primero la faccion con mayor cantidad de participantes.
            - Dentro de la faccion seleccionada, el MVP es el participante con mayor poder segun su Card.

            RESPUESTAS:
            - 200 OK: se devuelve el personaje seleccionado como MVP.
            - 400 Bad Request: el evento aun no ha sido simulado o algun participante no tiene Card.
            - 404 Not Found: no existe el evento indicado.

            El MVP se obtiene a partir de los datos actuales almacenados en memoria.
            """)
            .Produces<Personaje>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);






            // POST /eventos/{id}/simular
            app.MapPost("/eventos/{id}/simular", (int id, EventoService service) =>
            {
                try
                {
                    var resultado = service.Simular(id);
                    return Results.Ok(resultado);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new
                    {
                        error = ex.Message
                    });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            })
            .WithTags("Eventos")
            .WithSummary("Simular un evento")
            .WithDescription("""
            Ejecuta la simulacion de un evento registrado en el catalogo galactico.

            La simulacion utiliza las Cards de los personajes participantes para calcular
            el poder de cada faccion involucrada.

            REGLAS DE VALIDACION:
            - El evento debe existir.
            - El evento debe tener participantes.
            - Debe existir al menos un participante Rebelde y uno Imperial.
            - Todos los personajes participantes deben existir.
            - Todos los participantes deben tener una Card asociada.
            - Los participantes neutrales no intervienen en el combate ni aportan poder.

            CALCULO DE PODER:
            - Se suma el poder de las Cards de los participantes de cada faccion.
            - A cada faccion se le aplica un factor aleatorio entre 0.90 y 1.10.
            - El poder final se obtiene multiplicando el poder base por el factor aleatorio.
            - La faccion con mayor poder final es declarada ganadora.

            ACTUALIZACION DEL ESTADO:
            - Los participantes de la faccion perdedora pasan al estado Muerto.
            - Los participantes de la faccion ganadora mantienen su estado actual.
            - Los participantes neutrales no participan en el combate y no mueren.
            - El resultado de la simulacion se guarda en el evento.

            RESPUESTAS:
            - 200 OK: la simulacion fue ejecutada correctamente y devuelve los poderes,
              factores, ganador y criterio utilizado.
            - 400 Bad Request: el evento no cumple alguna de las condiciones necesarias
              para realizar la simulacion.
            - 404 Not Found: no existe un evento con el identificador indicado.

            Las validaciones se realizan antes de guardar el resultado de la simulacion.
            """)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);




        }


    }
}
