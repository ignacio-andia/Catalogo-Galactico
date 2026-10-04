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
            });


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
            });


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
            });


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
            });


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
            });


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
            });




        }


    }
}
