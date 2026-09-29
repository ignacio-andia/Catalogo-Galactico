using CatalogoGalactico.Models;
using CatalogoGalactico.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using static CatalogoGalactico.Models.Personaje;

namespace CatalogoGalactico.EndPoints
{
    public static class PersonajeEndpoints
    {
        public static void MapPersonajeEndpoints(this WebApplication app)
        {
            app.MapGet("/personajes", (PersonajeService service) =>
            {
                var personajes = service.MostrarTodos();

                return Results.Ok(personajes);
            });

            app.MapGet("/personajes/{id}", (int id, PersonajeService service) =>
            {
                var personaje = service.obtenerPorID(id);

                if(personaje is null)
                {
                    return Results.NotFound(new
                    {
                        error = $"no existe Personaje con Id {id}"
                    });
                }

                return Results.Ok(personaje);
            }
            );

            app.MapPost("/personajes", (CrearPersonajeDTO DTO, PersonajeService service) =>
            {
                if (string.IsNullOrWhiteSpace(DTO.Nombre))
                    return Results.BadRequest("El nombre no puede estar vacío.");

                var personaje = service.Crear(DTO);
                return Results.Created($"/personajes/{personaje.Id}", personaje);
            }
            );


            app.MapPut("/personajes/{id}", (int id, CrearPersonajeDTO DTO, PersonajeService service) =>
            {
                var personaje = service.Modificar(id, DTO);

                if(personaje is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(personaje);
            });

            app.MapDelete("/personajes/{id}", (int id, PersonajeService service)=>
            {
                bool eliminado = service.Eliminar(id);
                if (eliminado)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.NotFound();
                }
            });


            app.MapGet("/personajes/ranking", (string? por, PersonajeService service) =>
            {
                if (por != "poder")
                {
                    return Results.BadRequest(new
                    {
                        error = "Parametro 'por' debe ser 'poder'."
                    });
                }

                var ranking = service.RankingPorPoder();
                return Results.Ok(ranking);

            });

            app.MapGet("personsajes/{id}/eventos", (int id, EventoService service) =>
            {
                try
                {
                    var eventos = service.ObtenerEventosPorPersonaje(id);
                    return Results.Ok(eventos);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new
                    {
                        error = ex.Message
                    });
                }
            });


        }
    }
}
